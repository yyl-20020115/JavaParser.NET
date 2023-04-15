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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver.resolution.javaparser.contexts;




/**
 * @author Malte Langkabel
 */
class LambdaExprContextResolutionTest extends AbstractResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
    }

    @Test
    void solveParameterOfLambdaInMethodCallExpr() {
        CompilationUnit cu = parseSample("Lambda");

        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "lambdaMap");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodCallExpr methodCallExpr = (MethodCallExpr) returnStmt.getExpression().get();
        LambdaExpr lambdaExpr = (LambdaExpr) methodCallExpr.getArguments().get(0);

        Context context = new LambdaExprContext(lambdaExpr, typeSolver);

        Optional<Value> ref = context.solveSymbolAsValue("p");
        assertTrue(ref.isPresent());
        assertEquals("? super java.lang.String", ref.get().getType().describe());
    }

    @Test
    void solveParameterOfLambdaInFieldDecl() {
        CompilationUnit cu = parseSample("Lambda");

        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        VariableDeclarator field = Navigator.demandField(clazz, "functional");
        LambdaExpr lambdaExpr = (LambdaExpr) field.getInitializer().get();

        Path src = Paths.get("src/test/resources");
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new ReflectionTypeSolver());
        combinedTypeSolver.add(new JavaParserTypeSolver(adaptPath(src), new LeanParserConfiguration()));

        Context context = new LambdaExprContext(lambdaExpr, combinedTypeSolver);

        Optional<Value> ref = context.solveSymbolAsValue("p");
        assertTrue(ref.isPresent());
        assertEquals("java.lang.String", ref.get().getType().describe());
    }

    @Test
    void solveParameterOfLambdaInVarDecl() {
        CompilationUnit cu = parseSample("Lambda");

        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testFunctionalVar");
        VariableDeclarator varDecl = Navigator.demandVariableDeclaration(method, "a").get();
        LambdaExpr lambdaExpr = (LambdaExpr) varDecl.getInitializer().get();

        Path src = adaptPath("src/test/resources");
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new ReflectionTypeSolver());
        combinedTypeSolver.add(new JavaParserTypeSolver(src, new LeanParserConfiguration()));

        Context context = new LambdaExprContext(lambdaExpr, combinedTypeSolver);

        Optional<Value> ref = context.solveSymbolAsValue("p");
        assertTrue(ref.isPresent());
        assertEquals("java.lang.String", ref.get().getType().describe());
    }

    @Test
    void solveParameterOfLambdaInCast() {
        CompilationUnit cu = parseSample("Lambda");

        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Agenda");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testCast");
        VariableDeclarator varDecl = Navigator.demandVariableDeclaration(method, "a").get();
        LambdaExpr lambdaExpr = ((CastExpr) varDecl.getInitializer().get()).getExpression().asLambdaExpr();

        Path src = adaptPath("src/test/resources");
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new ReflectionTypeSolver());
        combinedTypeSolver.add(new JavaParserTypeSolver(src, new LeanParserConfiguration()));

        Context context = new LambdaExprContext(lambdaExpr, combinedTypeSolver);

        Optional<Value> ref = context.solveSymbolAsValue("p");
        assertTrue(ref.isPresent());
        assertEquals("java.lang.String", ref.get().getType().describe());
    }
}
