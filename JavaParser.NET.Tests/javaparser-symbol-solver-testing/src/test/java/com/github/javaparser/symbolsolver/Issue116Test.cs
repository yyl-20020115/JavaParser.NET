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

namespace com.github.javaparser.symbolsolver;



class Issue116Test extends AbstractResolutionTest {

    @Test
    void arrayTypeIsNotPartOfTheTree() {
        CompilationUnit cu = parseSample("Issue116");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "JavaTest");
        MethodDeclaration methodDeclaration = Navigator.demandMethod(clazz, "foo");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        com.github.javaparser.ast.type.Type typeNode = methodDeclaration.getParameters().get(0).getType();
        ResolvedType type = javaParserFacade.convert(typeNode, typeNode);
        assertEquals("java.lang.String[]", type.describe());

        ExpressionStmt expressionStmt = (ExpressionStmt) methodDeclaration.getBody().get().getStatements().get(0);
        Expression argRef = expressionStmt.getExpression();
        assertEquals("java.lang.String[]", javaParserFacade.getType(argRef).describe());
    }
}
