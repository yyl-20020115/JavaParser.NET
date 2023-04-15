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

namespace com.github.javaparser.symbolsolver;



public class Issue2823Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {
        /*final*/Path testRoot = adaptPath("src/test/resources/issue2823");
        TypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(testRoot);
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver(reflectionTypeSolver, javaParserTypeSolver);
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(combinedTypeSolver));
        StaticJavaParser.setConfiguration(configuration);

        string src = "import java.util.Optional;\n"
                + "public class TestClass {\n"
                + "    public Long getValue() {\n"
                + "        Optional<ClassA> classA = Optional.of(new ClassA());\n"
                + "        return classA.map(a -> a.obj)\n"
                + "                .map(b -> b.value)\n"
                + "                .orElse(null);\n"
                + "    }\n"
                + "}";

        CompilationUnit cu = StaticJavaParser.parse(src);

        // verify there is no exception thrown when we try to resolve all field access expressions
        cu.findAll(FieldAccessExpr.class).forEach(fd -> fd.resolve());

    }
}
