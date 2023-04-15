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


public class Issue2481Test {

    [TestMethod]
    public void test() {
        TypeSolver solver = new ReflectionTypeSolver();
        ParserConfiguration parserConfiguration = new ParserConfiguration();
        parserConfiguration.setSymbolResolver(new JavaSymbolSolver(solver));
        JavaParser parser = new JavaParser(parserConfiguration);
        ParseResult<CompilationUnit> cu = parser.parse("class A<T> { T t; }");
        cu.ifSuccessful( c -> {
            c.accept(new VoidVisitorAdapter<Void>() {
                @Override
                public void visit(ClassOrInterfaceType n, Void arg) {
                    super.visit(n, arg);
                    n.resolve();
                }
            }, null);
        });
    }

}
