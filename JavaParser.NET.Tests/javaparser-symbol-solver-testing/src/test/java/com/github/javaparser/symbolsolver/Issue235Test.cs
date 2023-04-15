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



class Issue235Test:AbstractResolutionTest{

    static Collection<String> data() {
        return Arrays.asList(
                "new_Bar_Baz_direct",
                "new_Bar_Baz",
                "new_Bar",
                "new_Foo_Bar"
        );
    }

    @ParameterizedTest
    @MethodSource("data")
    void issue235(string method) {
        CompilationUnit cu = parseSample("Issue235");
        ClassOrInterfaceDeclaration cls = Navigator.demandClassOrInterface(cu, "Foo");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        MethodDeclaration m = Navigator.demandMethod(cls, method);
        ExpressionStmt stmt = (ExpressionStmt) m.getBody().get().getStatements().get(0);
        ObjectCreationExpr expression = (ObjectCreationExpr) stmt.getExpression();
        Assertions.assertNotNull(javaParserFacade.convertToUsage(expression.getType()));
    }
}
