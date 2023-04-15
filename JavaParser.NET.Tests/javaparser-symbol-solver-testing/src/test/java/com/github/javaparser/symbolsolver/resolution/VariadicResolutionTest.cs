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




class VariadicResolutionTest:AbstractResolutionTest {

    [TestMethod]
    void issue7() {
        CompilationUnit cu = parseSample("Generics_issue7");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "SomeCollection");

        MethodDeclaration method = Navigator.demandMethod(clazz, "foo3");

        ReturnStmt stmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        Expression expression = stmt.getExpression().get();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        ResolvedType type = javaParserFacade.getType(expression);
        assertEquals(true, type.isReferenceType());
        assertEquals(List.class.getCanonicalName(), type.asReferenceType().getQualifiedName());
        assertEquals("java.util.List<java.lang.Long>", type.describe());
    }

    [TestMethod]
    void methodCallWithReferenceTypeAsVaridicArgumentIsSolved() {
        CompilationUnit cu = parseSample("MethodCalls");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodCalls");

        MethodDeclaration method = Navigator.demandMethod(clazz, "variadicMethod");
        MethodCallExpr callExpr = Navigator.findMethodCall(method, "variadicMethod").get();

        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        MethodUsage callee = javaParserFacade.solveMethodAsUsage(callExpr);
        assertEquals("variadicMethod", callee.getName());
    }

    [TestMethod]
    void resolveVariadicMethodWithGenericArgument() {
        CompilationUnit cu = parseSample("MethodCalls");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodCalls");

        MethodDeclaration method = Navigator.demandMethod(clazz, "genericMethodTest");
        MethodCallExpr callExpr = Navigator.findMethodCall(method, "variadicWithGenericArg").get();

        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        MethodUsage callee = javaParserFacade.solveMethodAsUsage(callExpr);
        assertEquals("variadicWithGenericArg", callee.getName());
    }

    [TestMethod]
    void selectMostSpecificVariadic() {
        CompilationUnit cu = parseSample("MethodCalls");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodCalls");

        MethodDeclaration method = Navigator.demandMethod(clazz, "variadicTest");
        List<MethodCallExpr> calls = method.findAll(MethodCallExpr.class);

        Path src = adaptPath("src/test/resources");
        TypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(), new JavaParserTypeSolver(src, new LeanParserConfiguration()));

        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        MethodUsage call1 = javaParserFacade.solveMethodAsUsage(calls.get(0)); // foobar();
        MethodUsage call2 = javaParserFacade.solveMethodAsUsage(calls.get(1)); // foobar("a");
        MethodUsage call3 = javaParserFacade.solveMethodAsUsage(calls.get(2)); // foobar("a", "a");
        MethodUsage call4 = javaParserFacade.solveMethodAsUsage(calls.get(3)); // foobar(varArg);
        assertEquals("void", call1.returnType().describe()); // foobar();
        assertEquals("int", call2.returnType().describe()); // foobar("a");
        assertEquals("void", call3.returnType().describe()); // foobar("a", "a");
        assertEquals("void", call4.returnType().describe()); // foobar(varArg);

        assertThrows(RuntimeException.class, () -> {
            MethodUsage call5 = javaParserFacade.solveMethodAsUsage(calls.get(4));
        });
    }

    [TestMethod]
    void getDeclaredConstructorTest() {
        CompilationUnit cu = parseSample("MethodCalls");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodCalls");

        MethodDeclaration method = Navigator.demandMethod(clazz, "getDeclaredConstructorTest");
        List<MethodCallExpr> calls = method.findAll(MethodCallExpr.class);

        JavaParserFacade javaParserFacade = JavaParserFacade.get(new ReflectionTypeSolver());
        MethodUsage call1 = javaParserFacade.solveMethodAsUsage(calls.get(1));
        MethodUsage call2 = javaParserFacade.solveMethodAsUsage(calls.get(2));
        MethodUsage call3 = javaParserFacade.solveMethodAsUsage(calls.get(3));
        MethodUsage call4 = javaParserFacade.solveMethodAsUsage(calls.get(4));
        assertEquals("java.lang.reflect.Constructor", call1.returnType().asReferenceType().getQualifiedName());
        assertEquals("java.lang.reflect.Constructor", call2.returnType().asReferenceType().getQualifiedName());
        assertEquals("java.lang.reflect.Constructor", call3.returnType().asReferenceType().getQualifiedName());
        assertEquals("java.lang.reflect.Constructor", call4.returnType().asReferenceType().getQualifiedName());
    }
}
