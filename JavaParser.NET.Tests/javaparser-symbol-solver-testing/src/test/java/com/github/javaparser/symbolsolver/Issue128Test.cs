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




class Issue128Test:AbstractResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup(){
        Path srcDir = adaptPath("src/test/resources/issue128");
        typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(), new JavaParserTypeSolver(srcDir, new LeanParserConfiguration()));
    }

    [TestMethod]
    void verifyJavaTestClassIsSolved() {
        typeSolver.solveType("foo.JavaTest");
    }

    [TestMethod]
    void loopOnStaticallyImportedType() {
        CompilationUnit cu = parseSampleWithStandardExtension("issue128/foo/Issue128");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "JavaTest");
        ExpressionStmt expressionStmt = (ExpressionStmt)clazz.getMethodsByName("test").get(0).getBody().get().getStatement(0);
        MethodCallExpr methodCallExpr = (MethodCallExpr) expressionStmt.getExpression();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);

        assertEquals(false, javaParserFacade.solve(methodCallExpr).isSolved());
    }
}
