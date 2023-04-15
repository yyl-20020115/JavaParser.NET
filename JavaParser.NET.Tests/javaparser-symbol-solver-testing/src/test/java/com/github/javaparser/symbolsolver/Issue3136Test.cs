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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;




public class Issue3136Test extends AbstractResolutionTest {

    @Test
    void test() {
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        String s =
                "public class Program {\n" +
                        "\n" +
                        "    public class InnerClass {\n" +
                        "    }\n" +
                        "\n" +
                        "    {\n" +
                        "        this.new InnerClass();\n" +
                        "    }\n" +
                        "}";

        CompilationUnit cu = StaticJavaParser.parse(s);
        List<ThisExpr> exprs = cu.findAll(ThisExpr.class);
        exprs.forEach(expr-> {
            assertEquals("Program",expr.calculateResolvedType().describe());
            assertEquals("Program",expr.resolve().getQualifiedName());
        });
    }
}
