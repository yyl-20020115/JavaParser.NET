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




class Issue300Test:AbstractResolutionTest {

    [TestMethod]
    void fieldAccessIssue(){
        Path pathToSourceFile = adaptPath("src/test/resources/issue300/Issue300.java");
        CompilationUnit cu = parse(pathToSourceFile);

        /*final*/FieldAccessExpr fieldAccess = Navigator.demandNodeOfGivenClass(cu, FieldAccessExpr.class);
        assertNotNull(fieldAccess);

        TypeSolver typeSolver = new CombinedTypeSolver(
                new ReflectionTypeSolver(),
                new JavaParserTypeSolver(adaptPath("src/test/resources/issue300"), new LeanParserConfiguration()));
        /*final*/JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        /*final*/SymbolReference<?:ResolvedValueDeclaration> ref = javaParserFacade.solve(fieldAccess);
        assertEquals(ResolvedPrimitiveType.INT, ref.getCorrespondingDeclaration().getType().asPrimitive());
    }
}

