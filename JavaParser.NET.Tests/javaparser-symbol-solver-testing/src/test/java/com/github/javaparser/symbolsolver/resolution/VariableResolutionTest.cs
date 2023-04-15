/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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



public class VariableResolutionTest:AbstractResolutionTest {

	[TestMethod]
	void variableResolutionNoBlockStmt() {
		// Test without nested block statement

		CompilationUnit cu = parseSample("VariableResolutionInVariousScopes");
		ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "VariableResolutionInVariousScopes");

		MethodDeclaration method = Navigator.demandMethod(clazz, "noBlock");
		MethodCallExpr callExpr = method.findFirst(MethodCallExpr.class).get();
		MethodUsage methodUsage = JavaParserFacade.get(new ReflectionTypeSolver()).solveMethodAsUsage(callExpr);

		assertTrue(methodUsage.declaringType().getQualifiedName().equals("java.lang.String"));
	}

	[TestMethod]
	void variableResolutionWithBlockStmt() {
		// Test without nested block statement

		CompilationUnit cu = parseSample("VariableResolutionInVariousScopes");
		ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "VariableResolutionInVariousScopes");

		MethodDeclaration method = Navigator.demandMethod(clazz, "withBlock");
		MethodCallExpr callExpr = method.findFirst(MethodCallExpr.class).get();
		MethodUsage methodUsage = JavaParserFacade.get(new ReflectionTypeSolver()).solveMethodAsUsage(callExpr);

		assertTrue(methodUsage.declaringType().getQualifiedName().equals("java.lang.String"));
	}
}
