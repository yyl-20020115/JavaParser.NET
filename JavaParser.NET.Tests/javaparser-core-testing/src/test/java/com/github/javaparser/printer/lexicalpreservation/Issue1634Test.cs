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

namespace com.github.javaparser.printer.lexicalpreservation;



public class Issue1634Test:AbstractLexicalPreservingTest {

    [TestMethod]
    public void testWithLexicalPreservationEnabled() {

        considerCode("package com.wangym.test;\nclass A{ }");
        
        string expected =
                "package com.wangym.test;\n"
                + "import lombok.Data;\n"
                + "\n"
                + "class A{ }";

        NodeList<ImportDeclaration> imports = cu.getImports();
        string str = "lombok.Data";
        imports.add(new ImportDeclaration(str, false, false));

        assertEquals(expected, LexicalPreservingPrinter.print(cu));
    }
}
