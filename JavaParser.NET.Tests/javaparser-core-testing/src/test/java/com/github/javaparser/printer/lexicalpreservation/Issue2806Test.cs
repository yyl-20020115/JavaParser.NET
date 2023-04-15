
/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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



class Issue2806Test:AbstractLexicalPreservingTest{

    private JavaParser javaParser;

    [TestMethod]
    void importIsAddedOnTheSameLine() {
        considerCode("import java.lang.IllegalArgumentException;\n" +
                "\n" +
                "public class A {\n" +
                "}");
        string junit5 = "import java.lang.IllegalArgumentException;\n" +
                "import java.nio.file.Paths;\n" +
                "\n" +
                "public class A {\n" +
                "}";
        ImportDeclaration importDeclaration = new ImportDeclaration("java.nio.file.Paths", false, false);
        CompilationUnit compilationUnit = cu.addImport(importDeclaration);
        string _out = LexicalPreservingPrinter.print(compilationUnit);
        assertThat(_out, equalTo(junit5));
    }

}
