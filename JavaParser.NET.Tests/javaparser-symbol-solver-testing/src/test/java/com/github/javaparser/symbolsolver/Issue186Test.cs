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

namespace com.github.javaparser.symbolsolver;




class Issue186Test:AbstractResolutionTest {

    [TestMethod]
    void lambdaFlatMapIssue() {
        CompilationUnit cu = parseSample("Issue186");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "JavaTest");
        MethodDeclaration methodDeclaration = Navigator.demandMethod(clazz, "foo");
        MethodCallExpr methodCallExpr = Navigator.findMethodCall(methodDeclaration, "flatMap").get();
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        assertEquals("java.util.stream.Stream<java.lang.String>", javaParserFacade.getType(methodCallExpr).describe());

    }

    [TestMethod]
    void lambdaPrimitivesIssue() {
        CompilationUnit cu = parseSample("Issue186");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "JavaTest");
        MethodDeclaration methodDeclaration = Navigator.demandMethod(clazz, "bar");
        List<LambdaExpr> lambdas = methodDeclaration.findAll(LambdaExpr.class);
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        assertEquals("java.util.function.Predicate<? super java.lang.String>", javaParserFacade.getType(lambdas.get(0)).describe());
        assertEquals("java.util.function.Function<? super java.lang.String, ?:java.lang.Integer>", javaParserFacade.getType(lambdas.get(1)).describe());
        assertEquals("java.util.function.Predicate<? super java.lang.Integer>", javaParserFacade.getType(lambdas.get(2)).describe());

    }

}
