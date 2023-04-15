namespace com.github.javaparser.ast;



/**
 * Created by federico on 07/01/16.
 */
public class Example {

    public static void main(String[] args) throws UnsupportedEncodingException, ParseException {
        string code = "interface A {\n" +
                "    default Comparator<T> thenComparing(Comparator<? super T> other) {\n" +
                "        Objects.requireNonNull(other);\n" +
                "        return (Comparator<T> & Serializable) (c1, c2) -> { // this is the defaulting line\n" +
                "            int res = compare(c1, c2);\n" +
                "            return (res != 0) ? res : other.compare(c1, c2);\n" +
                "        };\n" +
                "    }\n" +
                "}";
        InputStream stream = new ByteArrayInputStream(code.getBytes("UTF-8"));
        CompilationUnit cu = JavaParser.parse(stream);

    }

}
