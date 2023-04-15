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



public class Issue2210Test:AbstractResolutionTest {

    @BeforeEach
    void setup() {
    }

    [TestMethod]
    void test2210Issue() {
        // Source code
        string sourceCode = 
                "class A {" +
                " public void m() {\n" +
                "   java.util.Arrays.asList(1, 2, 3).forEach(System._out::println);" +
                " }\n" +
                "}";
        // Setup symbol solver
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(new ReflectionTypeSolver())));
        // Setup parser
        JavaParser parser = new JavaParser(configuration);
        CompilationUnit cu = parser.parse(sourceCode).getResult().get();
        // Test
        MethodReferenceExpr expr = Navigator.demandNodeOfGivenClass(cu, MethodReferenceExpr.class);
        ResolvedType type = expr.calculateResolvedType();
        assertEquals("java.util.function.Consumer<? super T>", type.describe());
    }

}
