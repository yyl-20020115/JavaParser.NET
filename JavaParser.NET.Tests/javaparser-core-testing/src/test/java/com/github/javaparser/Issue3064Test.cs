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




public class Issue3064Test {

    [TestMethod]
    public void test0() {
        string str = "import java.util.function.Supplier;\n" +
                "\n" +
                "public class MyClass {\n" +
                "\n" +
                "    public MyClass() {\n" +
                "        Supplier<String> aStringSupplier = false ? () -> \"\" : true ? () -> \"\" : () -> \"path\";\n" +
                "    }\n" +
                "}\n";

        JavaParser parser = new JavaParser();
        ParseResult<CompilationUnit> unitOpt = parser.parse(new StringReader(str));
        unitOpt.getProblems().stream().forEach(p -> System.err.println(p.toString()));
        CompilationUnit unit = unitOpt.getResult().orElseThrow(() -> new IllegalStateException("Could not parse file"));

        assertEquals(str, unit.toString());
    }

    [TestMethod]
    public void test1() {
        string str = "public class MyClass {\n" +
                "    {\n" +
                "        Supplier<String> aStringSupplier = false ? () -> \"F\" : true ? () -> \"T\" : () -> \"path\";\n" +
                "    }\n" +
                "}";
        CompilationUnit unit = StaticJavaParser.parse(str);
        assertEquals(str.replace("\n", ""), unit.toString().replace("\n", ""));
    }

}
