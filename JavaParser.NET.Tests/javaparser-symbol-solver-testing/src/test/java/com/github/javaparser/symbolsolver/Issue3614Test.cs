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



public class Issue3614Test:AbstractResolutionTest {

    [TestMethod]
    void test() {
        string code = "package java./*aaaaa*/util;\n"
                + "class Foo {\n"
                + "  public void test() {\n"
                + "        ArrayList list = new ArrayList();\n"
                + "    }"
                + "}";

        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(new ReflectionTypeSolver())));
        StaticJavaParser.setConfiguration(configuration);

        CompilationUnit cu = StaticJavaParser.parse(code);

        VariableDeclarationExpr vde = cu.findFirst(VariableDeclarationExpr.class).get();
        string resolvedType = vde.calculateResolvedType().describe();
        assertEquals("java.util.ArrayList", resolvedType);
    }
}
