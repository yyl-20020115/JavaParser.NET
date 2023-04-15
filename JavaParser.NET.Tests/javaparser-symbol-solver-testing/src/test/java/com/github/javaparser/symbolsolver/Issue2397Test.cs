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



public class Issue2397Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void testProvided1() {
        string sourceCode = "static /*final*/class ConstantFuture<T> implements Future<T> {\n" +
                "        private /*final*/T value;\n" +
                "      \n" +
                "        @Override\n" +
                "        public T get() {\n" +
                "            return value;\n" +
                "        }\n" +
                "}";
        testIssue(sourceCode);
    }

    [TestMethod]
    public void testProvided2() {
        string sourceCode = "class A {\n" +
                "  public static <T> T[] toArray(/*final*/T... items) {\n" +
                "    return items;\n" +
                "  }\n" +
                "}";
        testIssue(sourceCode);
    }

    public void testIssue(string sourceCode) {
        TypeSolver solver = new ReflectionTypeSolver();
        ParserConfiguration parserConfiguration = new ParserConfiguration();
        parserConfiguration.setSymbolResolver(new JavaSymbolSolver(solver));
        JavaParser parser = new JavaParser(parserConfiguration);

        ParseResult<CompilationUnit> cu = parser.parse(sourceCode);
        cu.ifSuccessful( c -> c.accept(new VoidVisitorAdapter<Void>() {
            @Override
            public void visit(ClassOrInterfaceType classOrInterfaceType, Void arg) {
                super.visit(classOrInterfaceType, arg);

                ResolvedType resolved = classOrInterfaceType.resolve();
                assertTrue(resolved.isTypeVariable());
            }
        }, null));
    }

}
