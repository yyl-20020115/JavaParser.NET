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




public class Issue3387Test:AbstractLexicalPreservingTest {

    [TestMethod]
    public void test3387() {
        considerCode(new StringJoiner("\n")
                .add("class A {")
                .add("")
                .add("\tpublic void setTheNumber(int number) {")
                .add("\t\tnumber = number;")
                .add("\t}")
                .add("")
                .add("}").toString());
        
        string expected = "class A {\n" + 
                "\n" + 
                "\t/**\n" + 
                "\t * Change Javadoc\n" + 
                "\t */\n" + 
                "\tpublic void setTheNumber(int number) {\n" + 
                "\t\tnumber = number;\n" + 
                "\t}\n" + 
                "\n" + 
                "}";

            MethodDeclaration md = cu.findFirst(MethodDeclaration.class).get();
            // create new javadoc comment
            Javadoc javadoc = new Javadoc(JavadocDescription.parseText("Change Javadoc"));
            md.setJavadocComment("\t", javadoc);
            System._out.println(LexicalPreservingPrinter.print(cu));
            assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }


}
