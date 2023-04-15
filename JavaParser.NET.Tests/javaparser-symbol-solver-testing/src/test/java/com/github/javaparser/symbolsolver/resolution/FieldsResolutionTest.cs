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




class FieldsResolutionTest:AbstractResolutionTest {

    @AfterEach
    void resetConfiguration() {
        StaticJavaParser.setConfiguration(new ParserConfiguration());
    }

    [TestMethod]
    void accessClassFieldThroughThis() {
        CompilationUnit cu = parseSample("AccessClassMemberThroughThis");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "AccessClassMemberThroughThis");
        MethodDeclaration method = Navigator.demandMethod(clazz, "getLabel2");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        Expression expression = returnStmt.getExpression().get();

        ResolvedType ref = JavaParserFacade.get(new ReflectionTypeSolver()).getType(expression);
        assertEquals("java.lang.String", ref.describe());
    }

    [TestMethod]
    void accessClassFieldThroughThisWithCompetingSymbolInParentContext() {
        CompilationUnit cu = parseSample("AccessClassMemberThroughThis");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "AccessClassMemberThroughThis");
        MethodDeclaration method = Navigator.demandMethod(clazz, "setLabel");
        ExpressionStmt expressionStmt = (ExpressionStmt) method.getBody().get().getStatements().get(0);
        AssignExpr assignExpr = (AssignExpr) expressionStmt.getExpression();
        FieldAccessExpr fieldAccessExpr = (FieldAccessExpr) assignExpr.getTarget();

        Path src = adaptPath("src/test/resources");
        CombinedTypeSolver typeSolver = new CombinedTypeSolver(new JavaParserTypeSolver(src, new LeanParserConfiguration()), new ReflectionTypeSolver());
        Solver symbolSolver = new SymbolSolver(typeSolver);
        SymbolReference<?:ResolvedValueDeclaration> ref = symbolSolver.solveSymbol(fieldAccessExpr.getName().getId(), fieldAccessExpr);

        assertTrue(ref.isSolved());
        assertTrue(ref.getCorrespondingDeclaration().isField());
    }

    [TestMethod]
    void accessEnumFieldThroughThis() {
        CompilationUnit cu = parseSample("AccessEnumMemberThroughThis");
        EnumDeclaration enumDecl = Navigator.demandEnum(cu, "AccessEnumMemberThroughThis");
        MethodDeclaration method = Navigator.demandMethod(enumDecl, "getLabel");
        SimpleName expression = Navigator.findSimpleName(method, "label").get();

        SymbolReference ref = JavaParserFacade.get(new ReflectionTypeSolver()).solve(expression);
        assertTrue(ref.isSolved());
        assertEquals("label", ref.getCorrespondingDeclaration().getName());
    }

    [TestMethod]
    void accessEnumMethodThroughThis() {
        CompilationUnit cu = parseSample("AccessEnumMemberThroughThis");
        EnumDeclaration enumDecl = Navigator.demandEnum(cu, "AccessEnumMemberThroughThis");
        MethodDeclaration method = Navigator.demandMethod(enumDecl, "getLabel2");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        Expression expression = returnStmt.getExpression().get();

        ResolvedType ref = JavaParserFacade.get(new ReflectionTypeSolver()).getType(expression);
        assertEquals("java.lang.String", ref.describe());
    }

    [TestMethod]
    void accessClassFieldThroughSuper() {
        CompilationUnit cu = parseSample("AccessThroughSuper");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "AccessThroughSuper.SubClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "fieldTest");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        Expression expression = returnStmt.getExpression().get();

        ResolvedType ref = JavaParserFacade.get(new ReflectionTypeSolver()).getType(expression);
        assertEquals("java.lang.String", ref.describe());
    }

    [TestMethod]
    void resolveClassFieldThroughThis() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get field access expression
        CompilationUnit cu = parseSample("AccessClassMemberThroughThis");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "AccessClassMemberThroughThis");
        MethodDeclaration method = Navigator.demandMethod(clazz, "getLabel2");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        FieldAccessExpr expression = returnStmt.getExpression().get().asFieldAccessExpr();

        // resolve field access expression
        ResolvedValueDeclaration resolvedValueDeclaration = expression.resolve();

        // get expected field declaration
        VariableDeclarator variableDeclarator = Navigator.demandField(clazz, "label");

        // check that the expected field declaration equals the resolved field declaration
        assertEquals(variableDeclarator, ((JavaParserFieldDeclaration) resolvedValueDeclaration).getVariableDeclarator());
    }

    [TestMethod]
    void resolveClassFieldThroughSuper() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get field access expression
        CompilationUnit cu = parseSample("AccessThroughSuper");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "AccessThroughSuper.SubClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "fieldTest");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        FieldAccessExpr expression = returnStmt.getExpression().get().asFieldAccessExpr();

        // resolve field access expression
        ResolvedValueDeclaration resolvedValueDeclaration = expression.resolve();

        // get expected field declaration
        clazz = Navigator.demandClass(cu, "AccessThroughSuper.SuperClass");
        VariableDeclarator variableDeclarator = Navigator.demandField(clazz, "field");

        // check that the expected field declaration equals the resolved field declaration
        assertEquals(variableDeclarator, ((JavaParserFieldDeclaration) resolvedValueDeclaration).getVariableDeclarator());
    }

    [TestMethod]
    void resolveClassFieldOfClassExtendingUnknownClass1() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get field access expression
        CompilationUnit cu = parseSample("ClassExtendingUnknownClass");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassExtendingUnknownClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "getFoo");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        NameExpr expression = returnStmt.getExpression().get().asNameExpr();

        // resolve field access expression
        ResolvedValueDeclaration resolvedValueDeclaration = expression.resolve();

        // get expected field declaration
        VariableDeclarator variableDeclarator = Navigator.demandField(clazz, "foo");

        // check that the expected field declaration equals the resolved field declaration
        assertEquals(variableDeclarator, ((JavaParserFieldDeclaration) resolvedValueDeclaration).getVariableDeclarator());
    }

    [TestMethod]
    void resolveClassFieldOfClassExtendingUnknownClass2() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get field access expression
        CompilationUnit cu = parseSample("ClassExtendingUnknownClass");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassExtendingUnknownClass");
        MethodDeclaration method = Navigator.demandMethod(clazz, "getFoo2");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        FieldAccessExpr expression = returnStmt.getExpression().get().asFieldAccessExpr();

        // resolve field access expression
        ResolvedValueDeclaration resolvedValueDeclaration = expression.resolve();

        // get expected field declaration
        VariableDeclarator variableDeclarator = Navigator.demandField(clazz, "foo");

        // check that the expected field declaration equals the resolved field declaration
        assertEquals(variableDeclarator, ((JavaParserFieldDeclaration) resolvedValueDeclaration).getVariableDeclarator());
    }

    [TestMethod]
    void resolveInheritedFieldFromInterface() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get field access expression
        CompilationUnit cu = parseSample("ReflectionTypeSolverFieldFromInterfaceResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Test");
        MethodDeclaration method = Navigator.demandMethod(clazz, "foo");
        ReturnStmt returnStmt = (ReturnStmt) method.getBody().get().getStatements().get(0);
        Expression expression = returnStmt.getExpression().get();

        ResolvedType ref = JavaParserFacade.get(new ReflectionTypeSolver()).getType(expression);
        assertEquals("int", ref.describe());
    }
}
