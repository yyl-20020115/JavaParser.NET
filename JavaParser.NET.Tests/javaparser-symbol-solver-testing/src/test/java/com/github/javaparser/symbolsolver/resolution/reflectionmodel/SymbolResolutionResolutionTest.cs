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

namespace com.github.javaparser.symbolsolver.resolution.reflectionmodel;



class SymbolResolutionResolutionTest:AbstractResolutionTest {

    [TestMethod]
    void getTypeOfField() {
        CompilationUnit cu = parseSample("ReflectionFieldOfItself");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MyClass");
        VariableDeclarator field = Navigator.demandField(clazz, "PUBLIC");

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType ref = JavaParserFacade.get(typeSolver).getType(field);
        assertEquals("int", ref.describe());
    }

    [TestMethod]
    void getTypeOfFieldAccess() {
        CompilationUnit cu = parseSample("ReflectionFieldOfItself");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MyClass");
        VariableDeclarator field = Navigator.demandField(clazz, "PUBLIC");

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType ref = JavaParserFacade.get(typeSolver).getType(field.getInitializer().get());
        assertEquals("int", ref.describe());
    }

    [TestMethod]
    void conditionalExpressionExample() {
        CompilationUnit cu = parseSample("JreConditionalExpression");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MyClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "foo1");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        Expression expression = returnStmt.getExpression().get();

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType ref = JavaParserFacade.get(typeSolver).getType(expression);
        assertEquals("java.lang.String", ref.describe());
    }

    [TestMethod]
    void conditionalExpressionExampleFollowUp1() {
        CompilationUnit cu = parseSample("JreConditionalExpression");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MyClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "foo1");
        MethodCallExpr expression = Navigator.findMethodCall(method, "next").get();

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType ref = JavaParserFacade.get(typeSolver).getType(expression);
        assertEquals("java.lang.String", ref.describe());
    }

}
