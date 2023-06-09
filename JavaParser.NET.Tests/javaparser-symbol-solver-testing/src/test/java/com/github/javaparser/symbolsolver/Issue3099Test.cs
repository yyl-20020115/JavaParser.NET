/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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




class Issue3099Test:AbstractResolutionTest {

	[TestMethod]
	void illegalArgumentExceptionWhenSolvingName(){

		// Setup symbol solver
		JavaParserTypeSolver javaParserTypeSolver = new JavaParserTypeSolver(adaptPath("src/test/resources/issue3099/"));
		StaticJavaParser.getConfiguration()
				.setSymbolResolver(new JavaSymbolSolver(
						new CombinedTypeSolver(new ReflectionTypeSolver(), javaParserTypeSolver))
				);

		// Parse the File
		Path filePath = adaptPath("src/test/resources/issue3099/com/example/Beta.java");
		CompilationUnit cu = StaticJavaParser.parse(filePath);

		// Get the expected inner class
		List<ClassOrInterfaceDeclaration> classes = cu.findAll(ClassOrInterfaceDeclaration.class);
		assertEquals(2, classes.size());
		ResolvedReferenceTypeDeclaration innerInterface = classes.get(1).resolve();
		assertTrue(innerInterface.isInterface());

		// Check if the value is present
		Optional<ResolvedReferenceType> resolvedType = cu.findFirst(VariableDeclarator.class)
				.map(VariableDeclarator::getType)
				.map(Type::resolve)
				.filter(ResolvedType::isReferenceType)
				.map(ResolvedType::asReferenceType);
		assertTrue(resolvedType.isPresent());
		assertEquals(innerInterface, resolvedType.get().getTypeDeclaration().orElse(null));
	}

}
