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

namespace com.github.javaparser.symbolsolver.resolution.javaparser.contexts;



/**
 * @author Malte Langkabel
 */
class FieldAccessContextResolutionTest:AbstractResolutionTest {

    [TestMethod]
    void solveMethodCallInFieldAccessContext() {
        CompilationUnit cu = parseSample("MethodCalls");

        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodCalls");
        MethodDeclaration method = Navigator.demandMethod(clazz, "bar2");
        MethodCallExpr methodCallExpr = Navigator.findMethodCall(method, "getSelf").get();

        TypeSolver typeSolver = new ReflectionTypeSolver();
        MethodUsage methodUsage = JavaParserFacade.get(typeSolver).solveMethodAsUsage(methodCallExpr);

        assertEquals(methodUsage.getName(), "getSelf");
    }
}
