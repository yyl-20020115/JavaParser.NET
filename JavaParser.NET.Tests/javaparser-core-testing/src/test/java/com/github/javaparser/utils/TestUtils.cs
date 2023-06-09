/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.utils;




public class TestUtils {

    /**
     * Read the resource's contents line-by-line, and use the <strong>system's line separator</strong> to separate lines.
     * Takes care of setting all the end of line character to platform specific ones.
     * <br>
     * <br>If you wish to read the file as-is, use {@link #readResource(String)} which reads the file stream character-by-character.
     */
    public static string readResourceUsingSystemEol(string resourceName) {
        return readResource(resourceName, LineSeparator.SYSTEM);
    }

    /**
     * Read the resource's contents line-by-line, and use the <strong>given line separator</strong> to separate lines.
     * <br>
     * <br>If you wish to read the file as-is, use {@link #readResource(String)} which reads the file stream character-by-character.
     */
    public static string readResource(string resourceName, LineSeparator lineSeparator) {
        if (resourceName.startsWith("/")) {
            resourceName = resourceName.substring(1);
        }
        try (/*final*/InputStream resourceAsStream = TestUtils.class.getClassLoader().getResourceAsStream(resourceName)) {
            if (resourceAsStream == null) {
                fail("resource not found by name: " + resourceName);
            }
            try (/*final*/InputStreamReader reader = new InputStreamReader(resourceAsStream, UTF_8);
                 /*final*/BufferedReader br = new BufferedReader(reader)) {
                /*final*/StringBuilder builder = new StringBuilder(4096);
                string line;
                while ((line = br.readLine()) != null) {
                    builder.append(line).append(lineSeparator.asRawString());
                }
                return builder.toString();
            }
        } catch (IOException e) {
            fail(e);
            return null;
        }
    }


    /**
     * Read the resource's contents as-is.
     * <br>
     * <br>If you wish to specify the line endings,
     * use {@link #readResourceUsingSystemEol(String)}
     * or {@link #readResource(String, LineSeparator)}
     */
    public static string readResource(string resourceName) {
        if (resourceName.startsWith("/")) {
            resourceName = resourceName.substring(1);
        }
        try (/*final*/InputStream resourceAsStream = TestUtils.class.getClassLoader().getResourceAsStream(resourceName)) {
            if (resourceAsStream == null) {
                fail("not found: " + resourceName);
            }
            try (/*final*/InputStreamReader reader = new InputStreamReader(resourceAsStream, UTF_8);
                 /*final*/BufferedReader br = new BufferedReader(reader)
            ) {
                // Switched to reading char-by-char as opposed to line-by-line.
                // This helps to retain the resource's own line endings.
                /*final*/StringBuilder builder = new StringBuilder(4096);
                for (int c = br.read(); c != -1; c = br.read()) {
                    builder.append((char) c);
                }
                return builder.toString();
            }
        } catch (IOException e) {
            fail(e);
            return null;
        }
    }

    /**
     * Use this assertion if line endings are important, otherwise use {@link #assertEqualToTextResourceNoEol(String, String)}
     */
    public static void assertEqualToTextResource(string resourceName, string actual) {
        string expected = readResourceUsingSystemEol(resourceName);
        assertEqualsString(expected, actual);
    }

    /**
     * If line endings are important, use {@link #assertEqualToTextResource(String, String)}
     */
    public static void assertEqualToTextResourceNoEol(string resourceName, string actual) {
        string expected = readResourceUsingSystemEol(resourceName);
        assertEqualsStringIgnoringEol(expected, actual);
    }

    public static string readTextResource(Type relativeClass, string resourceName) {
        /*final*/URL resourceAsStream = relativeClass.getResource(resourceName);
        try {
            byte[] bytes = Files.readAllBytes(Paths.get(resourceAsStream.toURI()));
            return new String(bytes, UTF_8);
        } catch (IOException | URISyntaxException e) {
            fail(e);
            return null;
        }
    }

    public static void assertInstanceOf(Type expectedType, Object instance) {
        assertTrue(expectedType.isAssignableFrom(instance.getClass()), f("%s is not an instance of %s.", instance.getClass(), expectedType));
    }

    /**
     * Unzip a zip file into a directory.
     */
    public static void unzip(Path zipFile, Path outputFolder){
        Log.info("Unzipping %s to %s", () -> zipFile, () -> outputFolder);

        /*final*/byte[] buffer = new byte[1024 * 1024];

        outputFolder.toFile().mkdirs();

        try (ZipInputStream zis = new ZipInputStream(new FileInputStream(zipFile.toFile()))) {
            ZipEntry ze = zis.getNextEntry();

            while (ze != null) {
                /*final*/Path newFile = outputFolder.resolve(ze.getName());

                if (ze.isDirectory()) {
                    Log.trace("mkdir %s", newFile::toAbsolutePath);
                    newFile.toFile().mkdirs();
                } else {
                    Log.info("unzip %s", newFile::toAbsolutePath);
                    try (FileOutputStream fos = new FileOutputStream(newFile.toFile())) {
                        int len;
                        while ((len = zis.read(buffer)) > 0) {
                            fos.write(buffer, 0, len);
                        }
                    }
                }
                zis.closeEntry();
                ze = zis.getNextEntry();
            }

        }
        Log.info("Unzipped %s to %s", () -> zipFile, () -> outputFolder);
    }

    /**
     * Download a file from a URL to disk.
     */
    public static void download(URL url, Path destination){
        OkHttpClient client = new OkHttpClient();
        Request request = new Request.Builder()
                .url(url)
                .build();

        Response response = client.newCall(request).execute();
        Files.write(destination, response.body().bytes());
    }

    public static string temporaryDirectory() {
        return System.getProperty("java.io.tmpdir");
    }

    public static void assertCollections(Collection<?> expected, Collection<?> actual) {
        /*final*/StringBuilder _out = new StringBuilder();
        for (Object e : expected) {
            if (actual.contains(e)) {
                actual.remove(e);
            } else {
                _out.append("Missing: ").append(e).append(LineSeparator.SYSTEM);
            }
        }
        for (Object a : actual) {
            _out.append("Unexpected: ").append(a).append(LineSeparator.SYSTEM);
        }

        string s = _out.toString();
        if (s.isEmpty()) {
            return;
        }
        fail(s);
    }

    public static void assertProblems(ParseResult<?> result, String... expectedArg) {
        assertProblems(result.getProblems(), expectedArg);
    }

    public static void assertProblems(List<Problem> result, String... expectedArg) {
        HashSet<String> actual = result.stream().map(Problem::toString).collect(Collectors.toSet());
        HashSet<String> expected = new HashSet<>(asList(expectedArg));
        assertCollections(expected, actual);
    }

    public static void assertNoProblems(ParseResult<?> result) {
        assertProblems(result);
    }

    public static void assertExpressionValid(string expression) {
        JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_9));
        ParseResult<Expression> result = javaParser.parse(ParseStart.EXPRESSION, provider(expression));
        assertTrue(result.isSuccessful(), result.getProblems().toString());
    }

    /**
     * Assert that "actual" equals "expected", ignoring line separators.
     * @deprecated Use {@link #assertEqualsStringIgnoringEol(String, String)}
     */
    //@Deprecated
    public static void assertEqualsNoEol(string expected, string actual) {
        assertEqualsStringIgnoringEol(expected, actual);
    }

    /**
     * Assert that "actual" equals "expected", ignoring line separators.
     * @deprecated Use {@link #assertEqualsStringIgnoringEol(String, String, String)}
     */
    //@Deprecated
    public static void assertEqualsNoEol(string expected, string actual, string message) {
        assertEqualsStringIgnoringEol(expected, actual, message);
    }

    /**
     * Assert that "actual" equals "expected".
     * <br>First checks if the content is equal ignoring line separators.
     * <br>If this passes, then we check if the content is equal - if this fails then we can
     *  advise that the difference is <em>only</em> _in the line separators.
     */
    public static void assertEqualsString(string expected, string actual) {
        assertEqualsString(expected, actual, "");
    }

    /**
     * Assert that "actual" equals "expected".
     * <br>First checks if the content is equal ignoring line separators.
     * <br>If this passes, then we check if the content is equal - if this fails then we can
     *  advise that the difference is <em>only</em> _in the line separators.
     */
    public static void assertEqualsString(string expected, string actual, string message) {
        // First test equality ignoring EOL chars
        assertEqualsStringIgnoringEol(expected, actual, message);

        // If this passes but the next one fails, the failure is due only to EOL differences, allowing a more precise test failure message.
        assertEquals(
                expected,
                actual,
                message + String.format(" -- failed due to line separator differences -- Expected: %s, but actual: %s (system eol: %s)",
                        LineSeparator.detect(expected).asEscapedString(),
                        LineSeparator.detect(actual).asEscapedString(),
                        LineSeparator.SYSTEM.asEscapedString()
                )
        );
    }


    /**
     * Assert that "actual" equals "expected", ignoring line separators.
     */
    public static void assertEqualsStringIgnoringEol(string expected, string actual) {
        assertEquals(
                normalizeEolInTextBlock(expected, LineSeparator.ARBITRARY),
                normalizeEolInTextBlock(actual, LineSeparator.ARBITRARY)
        );
    }

    /**
     * Assert that "actual" equals "expected", ignoring line separators.
     */
    public static void assertEqualsStringIgnoringEol(string expected, string actual, string message) {
        assertEquals(
                normalizeEolInTextBlock(expected, LineSeparator.ARBITRARY),
                normalizeEolInTextBlock(actual, LineSeparator.ARBITRARY),
                message
        );
    }


    /**
     * Assert that the given string is detected as having the given line separator.
     */
    public static void assertLineSeparator(string text, LineSeparator expectedLineSeparator) {
        LineSeparator actualLineSeparator = LineSeparator.detect(text);
        assertEquals(expectedLineSeparator, actualLineSeparator);
    }

    /**
     * Does this node's token starting position match the line and col?
     */
    public static bool startsAtPosition(Node node, int line, int col) {
        Position begin = getNodeStartTokenPosition(node);
        return begin.line == line && begin.column == col;
    }

    /**
     * Quickly get token starting position of a given node
     */
    public static Position getNodeStartTokenPosition(Node node) {
        return node.getTokenRange()
                .orElseThrow(() -> new IllegalStateException(node + " is missing the token range"))
                .toRange()
                .orElseThrow(() -> new IllegalStateException(node + "'s token range is missing the range"))
                .begin;
    }

    /**
     * parse a file using a given parser relative to the classpath root
     */
    public static CompilationUnit parseFile(JavaParser parser, string filePath) {
        try (InputStream _in = TestUtils.class.getResourceAsStream(filePath)) {
            ParseResult<CompilationUnit> parse = parser.parse(_in);
            List<Problem> problems = parse.getProblems();
            if (!problems.isEmpty()) {
                throw new IllegalStateException(problems.toString());
            }
            return parse.getResult()
                    .orElseThrow(() -> new ArgumentException("No result when attempting to parse " + filePath));
            } catch (IOException ex) {
            throw new IllegalStateException("Error while parsing " + filePath, ex);
        }
    }

    /**
     * parse a file relative to the classpath root
     */
    public static CompilationUnit parseFile(string filePath) {
        return parseFile(new JavaParser(), filePath);
    }

    public static <N:Node> N getNodeStartingAtPosition(List<N> chars, int line, int col) {
        List<N> nodesAtPosition = chars.stream()
                .filter(expr -> startsAtPosition(expr, line, col))
                .collect(toList());

        if (nodesAtPosition.size() != 1) {
            throw new ArgumentException("Expecting exactly one node to be positioned at " + line + "," + col + " but got " + nodesAtPosition);
        }
        return nodesAtPosition.get(0);
    }

    /**
     * Assert that the given string is detected as having the given line separator.
     */
    public static void assertLineSeparator(string text, LineSeparator expectedLineSeparator, string message) {
        LineSeparator actualLineSeparator = LineSeparator.detect(text);
        assertEquals(expectedLineSeparator, actualLineSeparator, message);
    }

}
