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




public class Issue2065Test:AbstractResolutionTest {

    [TestMethod]
    void test() {
        string code = "import java.util.stream.Stream;\n" +
                "\n" +
                "public class A {\n" +
                "    public void test(){\n" +
                "        Stream.of(1,2).reduce((a, b) -> Math.max(a, b));\n" +
                "    }\n" +
                "}";

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(code);
        List<MethodCallExpr> exprs = cu.findAll(MethodCallExpr.class);
        for (MethodCallExpr expr : exprs) {
            if (expr.getNameAsString().contentEquals("max")) {
                assertEquals("java.lang.Math.max(int, int)", expr.resolve().getQualifiedSignature());
            }
        }
    }

}
