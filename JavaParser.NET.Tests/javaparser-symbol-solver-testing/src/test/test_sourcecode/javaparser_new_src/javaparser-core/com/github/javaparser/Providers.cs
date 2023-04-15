namespace com.github.javaparser;



/**
 * Factory for providers of source code for JavaParser.
 * Providers that have no parameter for encoding but need it will use UTF-8.
 */
public /*final*/class Providers {
	public static /*final*/Charset UTF8 = Charset.forName("utf-8");

	private Providers() {
	}

	public static Provider provider(Reader reader) {
		return new StreamProvider(assertNotNull(reader));
	}

	public static Provider provider(InputStream input, Charset encoding) {
		assertNotNull(input);
		assertNotNull(encoding);
		try {
			return new StreamProvider(input, encoding.name());
		} catch (IOException e) {
			// The only one that is thrown is UnsupportedCharacterEncodingException,
			// and that's a fundamental problem, so runtime exception.
			throw new RuntimeException(e);
		}
	}

	public static Provider provider(InputStream input) {
		return provider(input, UTF8);
	}

	public static Provider provider(File file, Charset encoding) throws FileNotFoundException {
		return provider(new FileInputStream(assertNotNull(file)), assertNotNull(encoding));
	}

	public static Provider provider(File file) throws FileNotFoundException {
		return provider(assertNotNull(file), UTF8);
	}

	public static Provider provider(Path path, Charset encoding){
		return provider(Files.newInputStream(assertNotNull(path)), assertNotNull(encoding));
	}

	public static Provider provider(Path path){
		return provider(assertNotNull(path), UTF8);
	}

	public static Provider provider(string source) {
		return new StringProvider(assertNotNull(source));
	}

}
