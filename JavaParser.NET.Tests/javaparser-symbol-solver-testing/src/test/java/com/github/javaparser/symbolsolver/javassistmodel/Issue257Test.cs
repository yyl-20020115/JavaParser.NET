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

namespace com.github.javaparser.symbolsolver.javassistmodel;




class Issue257Test:AbstractSymbolResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/issue257/issue257.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());
    }

    [TestMethod]
    void verifyBCanBeSolved() {
        typeSolver.solveType("net.testbug.B");
    }

    [TestMethod]
    void issue257(){
        Path pathToSourceFile = adaptPath("src/test/resources/issue257/A.java.txt");
        CompilationUnit cu = parse(pathToSourceFile);
        Statement statement = cu.getClassByName("A").get().getMethodsByName("run").get(0).getBody().get().getStatement(0);
        ExpressionStmt expressionStmt = (ExpressionStmt)statement;
        Expression expression = expressionStmt.getExpression();
        JavaParserFacade.get(typeSolver).getType(expression);
    }

}
