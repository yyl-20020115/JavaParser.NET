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




public class AnnotationSpaceTest:AbstractLexicalPreservingTest {
    /** Tests that inserted annotations on types are followed by a space. */
    [TestMethod]
    public void test() {
        considerCode("public class Foo {\n" +
                        "    void myMethod(string param);\n" +
                        "}");
        // Insert the annotation onto the string parameter type.
        Optional<ClassOrInterfaceType> type = cu.findFirst(ClassOrInterfaceType.class);
        type.get().addAnnotation(new MarkerAnnotationExpr("Nullable"));
        string result = LexicalPreservingPrinter.print(cu);
        // Verify that there's a space between the annotation and the string type.
        assertTrue(result.contains("@Nullable String"));
    }
}
