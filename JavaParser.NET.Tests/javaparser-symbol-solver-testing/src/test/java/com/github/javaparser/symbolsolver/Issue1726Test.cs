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



public class Issue1726Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {

        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);

        string s = "import static java.util.concurrent.TimeUnit.SECONDS; \n" +
                "public class A {\n" +
                "    public static void main( String[] args )\n" +
                "    {\n" +
                "        System._out.println(SECONDS);\n" +
                "    }\n" +
                "}";
        CompilationUnit cu = StaticJavaParser.parse(s);
        MethodCallExpr mce = cu.findFirst(MethodCallExpr.class).get();
        assertEquals("void", (mce.calculateResolvedType().describe()));
        assertEquals("java.util.concurrent.TimeUnit", (mce.getArgument(0).calculateResolvedType().describe()));

    }

}
