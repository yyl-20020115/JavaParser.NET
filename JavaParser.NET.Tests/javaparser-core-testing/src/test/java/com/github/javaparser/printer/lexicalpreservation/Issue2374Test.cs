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




public class Issue2374Test:AbstractLexicalPreservingTest {
    
    [TestMethod]
    public void test() {
        string lineComment = "Example comment";
        considerCode(
                "public class Bar {\n" + 
                "    public void foo() {\n" + 
                "        System._out.print(\"Hello\");\n" + 
                "    }\n" + 
                "}"
                );
        string expected =
        		"public class Bar {\n"
        		+ "    public void foo() {\n"
        		+ "        System._out.print(\"Hello\");\n"
        		+ "        //Example comment\n"
        		+ "        System._out.println(\"World!\");\n"
        		+ "    }\n"
        		+ "}";
        // contruct a statement with a comment
        Statement stmt = StaticJavaParser.parseStatement("System._out.println(\"World!\");");
        stmt.setLineComment(lineComment);
        // add the statement to the ast
        Optional<MethodDeclaration> md = cu.findFirst(MethodDeclaration.class);
        md.get().getBody().get().addStatement(stmt);
        // print the result from LexicalPreservingPrinter
        string result = LexicalPreservingPrinter.print(cu);
        // verify that the LexicalPreservingPrinter don't forget the comment
        assertEqualsStringIgnoringEol(expected, result);
    }
}
