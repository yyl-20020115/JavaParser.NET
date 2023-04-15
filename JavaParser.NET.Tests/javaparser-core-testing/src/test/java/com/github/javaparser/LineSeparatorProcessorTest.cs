/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser;




public class LineSeparatorProcessorTest:AbstractLexicalPreservingTest{

    // TODO: Add more tests outside the "happy path" (e.g. mixed EOL, no EOL, etc.)

    /*
     * This test case must prevent an UnsupportedOperation Removed throwed by LexicalPreservation when we try to replace an expression
     */
    public void doTest(LineSeparator lineSeparator) {
        string eol = lineSeparator.asRawString();

        considerCode("" +
                "    public class Foo { //comment" + eol +
                "        private string a;" + eol +
                "        private string b;" + eol +
                "        private string c;" + eol +
                "        private string d;" + eol +
                "    }");

        // Note: Expect the platform's EOL character when printing
        string expected = "" +
                "    public class Foo { //comment" + eol +
                "    private string newField;" + eol +
                "    " + eol +
                "    private string a;" + eol +
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


        // The LineEndingProcessingProvider sets the line ending to the root node.
        // Child nodes should then "inherit" then line ending style.
        LineSeparator lineSeparator_cu = cu.getLineEndingStyle();
        LineSeparator lineSeparator_fd = fd.getLineEndingStyle();

//        System._out.println("lineSeparator_cu.describe() = " + lineSeparator_cu.describe());
//        System._out.println("lineSeparator_fd.describe() = " + lineSeparator_fd.describe());

        // Assert that it has been detected and injected correctly.
        LineSeparator detectedLineSeparator = LineSeparator.detect(actual);
        assertEquals(lineSeparator, detectedLineSeparator);
        assertEquals(lineSeparator, lineSeparator_cu);
        assertEquals(lineSeparator, lineSeparator_fd);

        // The line ending data is injected at the root node, thus should only exist there.
        assertTrue(cu.containsData(Node.LINE_SEPARATOR_KEY), "Expected the processor provider to have set the data on the root node.");
        assertFalse(fd.containsData(Node.LINE_SEPARATOR_KEY), "Expected the line ending value to have been inherited, not set directly");

    }

    [TestMethod]
    public void testWithCr() {
        doTest(LineSeparator.CR);
    }

    [TestMethod]
    public void testWithCrLf() {
        doTest(LineSeparator.CRLF);
    }

    [TestMethod]
    public void testWithLf() {
        doTest(LineSeparator.LF);
    }


    // TODO: Test for textblocks

}
