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

namespace com.github.javaparser.symbolsolver.resolution.javaparser.declarations;



class JavaParserTypeParameterResolutionTest:AbstractResolutionTest {

    private void testGenericArguments(string containingMethodName) {
        CompilationUnit cu = parseSample("GenericMethodArguments");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        ClassOrInterfaceDeclaration classDecl = Navigator.demandClass(cu, "GenericMethodArguments");
        MethodDeclaration containingMethod = Navigator.demandMethod(classDecl, containingMethodName);
        MethodCallExpr bar = Navigator.findMethodCall(containingMethod, "apply").get();

        assertTrue(javaParserFacade.solve(bar).isSolved());
    }

    [TestMethod]
    void genericMethodWithGenericClassBasedArgument() {
        testGenericArguments("useCase1");
    }

    [TestMethod]
    void genericMethodWithGenericClassArgument() {
        testGenericArguments("useCase2");
    }

    [TestMethod]
    void declaredOnMethodPositiveCase() {
        CompilationUnit cu = parseSample("MethodTypeParameter");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        ClassOrInterfaceDeclaration classDecl = Navigator.demandClass(cu, "Foo");
        MethodDeclaration methodDecl = Navigator.demandMethod(classDecl, "usage");
        MethodCallExpr callToFoo = (MethodCallExpr) Navigator.demandReturnStmt(methodDecl).getExpression().get();
        ResolvedMethodDeclaration methodDeclaration = javaParserFacade.solve(callToFoo).getCorrespondingDeclaration();
        for (ResolvedTypeParameterDeclaration tp : methodDeclaration.getTypeParameters()) {
            assertTrue(tp is JavaParserTypeParameter);
            assertEquals("C", tp.getName());
            assertEquals(true, tp.declaredOnMethod());
            assertEquals(false, tp.declaredOnType());
        }
    }

    [TestMethod]
    void declaredOnMethodNegativeCase() {
        CompilationUnit cu = parseSample("ClassTypeParameter");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        ClassOrInterfaceDeclaration classDecl = Navigator.demandClass(cu, "Foo");
        MethodDeclaration methodDecl = Navigator.demandMethod(classDecl, "usage");
        MethodCallExpr callToFoo = (MethodCallExpr) Navigator.demandReturnStmt(methodDecl).getExpression().get();
        ResolvedMethodDeclaration methodDeclaration = javaParserFacade.solve(callToFoo).getCorrespondingDeclaration();
        ResolvedReferenceTypeDeclaration typeDeclaration = methodDeclaration.declaringType();
        assertEquals(2, typeDeclaration.getTypeParameters().size());
        assertTrue(typeDeclaration.getTypeParameters().get(0) is JavaParserTypeParameter);
        assertEquals("A", typeDeclaration.getTypeParameters().get(0).getName());
        assertEquals(false, typeDeclaration.getTypeParameters().get(0).declaredOnMethod());
        assertEquals(true, typeDeclaration.getTypeParameters().get(0).declaredOnType());
        assertTrue(typeDeclaration.getTypeParameters().get(1) is JavaParserTypeParameter);
        assertEquals("B", typeDeclaration.getTypeParameters().get(1).getName());
        assertEquals(false, typeDeclaration.getTypeParameters().get(1).declaredOnMethod());
        assertEquals(true, typeDeclaration.getTypeParameters().get(1).declaredOnType());

    }

}
