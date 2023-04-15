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

namespace com.github.javaparser.printer.lexicalpreservation;




public class Issue2620Test:AbstractLexicalPreservingTest {

    [TestMethod]
    public void testWithCr() {
        doTest(LineSeparator.CR);
    }

    [TestMethod]
    public void testWithLf() {
        doTest(LineSeparator.LF);
    }

    [TestMethod]
    public void testWithCrLf() {
        doTest(LineSeparator.CRLF);
    }


    /*
     * This test case must prevent an UnsupportedOperation Removed throwed by LexicalPreservation when we try to replace an expression
     */
    public void doTest(LineSeparator eol) {

        considerCode("" +
                "    public class Foo { //comment" + eol +
                "        private string a;" + eol +
                "        private string b;" + eol +
                "        private string c;" + eol +
                "        private string d;" + eol +
                "    }");

        // Note: Expect the platform's EOL character when printing
        // FIXME: Indentation is bad here.
        string expected = "" +
                "    public class Foo { //comment" + eol +
                "        private string newField;" + eol +
                "        " + eol +
                "        private string a;" + eol +
                "        private string b;" + eol +
                "        private string c;" + eol +
                "        private string d;" + eol +
                "    }";


        // create a new field declaration
        VariableDeclarator variable = new VariableDeclarator(new ClassOrInterfaceType("String"), "newField");
        FieldDeclaration fd = new FieldDeclaration(new NodeList(Modifier.privateModifier()), variable);
        Optional<ClassOrInterfaceDeclaration> cd = cu.findFirst(ClassOrInterfaceDeclaration.class);

        // add the new variable
        cd.get().getMembers().addFirst(fd);

        // should be printed like this
//        System._out.println("\n\nOriginal:\n" + original);
//        System._out.println("\n\nExpected:\n" + expected);

        // but the result is
        /*final*/string actual = LexicalPreservingPrinter.print(cu);
//        System._out.println("\n\nActual:\n" + actual);

        LineSeparator detectedLineSeparator = LineSeparator.detect(actual);

        assertFalse(detectedLineSeparator.equals(LineSeparator.MIXED));
        assertEquals(eol.asEscapedString(), detectedLineSeparator.asEscapedString());

        assertEquals(normaliseNewlines(expected), normaliseNewlines(actual));

        // Commented _out until #2661 is fixed (re: EOL characters of injected code)
        assertEqualsStringIgnoringEol(escapeNewlines(expected), escapeNewlines(actual));
        assertEquals(expected, actual, "Failed due to EOL differences.");
    }

    private string escapeNewlines(string input) {
        return input
                .replaceAll("\\r", "\\\\r")
                .replaceAll("\\n", "\\\\n");
    }

    private string normaliseNewlines(string input) {
        return input.replaceAll("\\r\\n|\\r|\\n", "\\\\n");
    }
}
