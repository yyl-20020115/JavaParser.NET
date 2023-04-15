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



class TypeInClassWithAnnotationAncestorTest:AbstractResolutionTest {

	[TestMethod]
	void resolveStringReturnType() {
		CompilationUnit cu = parseSample("ClassWithAnnotationAncestor");
		ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassWithAnnotationAncestor");
		MethodDeclaration method = Navigator.demandMethod(clazz, "testMethod");
		ResolvedType type = JavaParserFacade.get(new ReflectionTypeSolver())
				                    .convertToUsage(method.getType());
		assertFalse(type.isTypeVariable());
		assertEquals("java.lang.String", type.describe());
	}
}
