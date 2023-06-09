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

namespace com.github.javaparser.symbolsolver.resolution;



class LambdaResolutionTest:AbstractResolutionTest {

    [TestMethod]
    void lambdaMapParameter() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaMap");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodCallExpr methodCallExpr = (MethodCallExpr) returnStmt.getExpression().get();
        Expression expression = methodCallExpr.getArguments().get(0);

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.util.function.Function<? super java.lang.String, ?:java.lang.String>", type.describe());
    }

    [TestMethod]
    void personsStream() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaMap");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();
        expression = Navigator.findMethodCall(expression, "stream").get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.util.stream.Stream<java.lang.String>", type.describe());
    }

    [TestMethod]
    void lambdaMap() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration m1 = Navigator.demandMethod(clazz, "lambdaMap");
        MethodDeclaration m2 = Navigator.demandMethod(clazz, "lambdaMap2");
        ReturnStmt returnStmt1 = Navigator.demandReturnStmt(m1);
        ReturnStmt returnStmt2 = Navigator.demandReturnStmt(m2);
        Expression e1 = returnStmt1.getExpression().get();
        Expression e2 = returnStmt2.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type1 = javaParserFacade.getType(e1);
        ResolvedType type2 = javaParserFacade.getType(e2);
        assertEquals("java.util.stream.Stream<java.lang.String>", type1.describe());
        assertEquals("java.util.stream.Stream<java.util.stream.IntStream>", type2.describe());
    }

    [TestMethod]
    void lambdaReduce() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "reduce");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expr = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type1 = javaParserFacade.getType(expr);
        assertEquals("java.util.Optional<java.lang.Integer>", type1.describe());
    }

    [TestMethod]
    void lambdaPrint() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "print");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expr = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type1 = javaParserFacade.getType(expr);
        assertEquals("void", type1.describe());
    }

    [TestMethod]
    void lambdaBifunc() {
        CompilationUnit cu = parseSample("Lambda");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "bifunc");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expr = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type1 = javaParserFacade.getType(expr);
        assertEquals("double", type1.describe());
    }

    [TestMethod]
    void lambdaCollectParam() {
        CompilationUnit cu = parseSample("LambdaCollect");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaMap");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodCallExpr methodCallExpr = (MethodCallExpr) returnStmt.getExpression().get();
        // Collectors.toList()
        Expression expression = methodCallExpr.getArguments().get(0);

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.util.stream.Collector<T, ?:java.lang.Object, java.util.List<T>>", type.describe());
    }

    [TestMethod]
    void lambdaCollect() {
        CompilationUnit cu = parseSample("LambdaCollect");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaMap");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.util.List<java.lang.String>", type.describe());
    }

    [TestMethod]
    void lambdaBlockExplicitReturn() {
        CompilationUnit cu = parseSample("LambdaMulti");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaSingleReturn");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.lang.String", type.describe());
    }

    [TestMethod]
    void lambdaBlockMultiLineReturn() {
        CompilationUnit cu = parseSample("LambdaMulti");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "multiLineReturn");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals("java.lang.String", type.describe());
    }

    [TestMethod]
    void typeOfVoidLambda() {
        CompilationUnit cu = parseSample("LambdaVoid");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaEmpty");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();
        LambdaExpr lambdaExpr = Navigator.demandNodeOfGivenClass(expression, LambdaExpr.class);

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(lambdaExpr);
        assertEquals("java.util.function.Consumer<? super java.lang.String>", type.describe());
    }


}
