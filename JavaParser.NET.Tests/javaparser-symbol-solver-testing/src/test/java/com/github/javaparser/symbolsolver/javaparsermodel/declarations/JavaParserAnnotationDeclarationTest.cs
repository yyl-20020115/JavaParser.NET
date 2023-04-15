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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;




class JavaParserAnnotationDeclarationTest:AbstractResolutionTest {

	private /*final*/TypeSolver typeSolver = new ReflectionTypeSolver();
	private /*final*/JavaParser javaParser = createParserWithResolver(typeSolver);

	[TestMethod]
	void getAllFields_shouldReturnASingleField() {
		string sourceCode = "@interface Foo { int a = 0; }";

		ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
		assertTrue(result.getResult().isPresent());
		CompilationUnit cu = result.getResult().get();

		Optional<AnnotationDeclaration> annotation = cu.findFirst(AnnotationDeclaration.class);
		assertTrue(annotation.isPresent());

		List<ResolvedFieldDeclaration> fields = annotation.get().resolve().getAllFields();
		assertEquals(1, fields.size());
		assertEquals("a", fields.get(0).getName());
	}

	[TestMethod]
	void getAllFields_shouldReturnMultipleVariablesDeclaration() {
		string sourceCode = "@interface Foo { int a = 0, b = 1; }";

		ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
		assertTrue(result.getResult().isPresent());
		CompilationUnit cu = result.getResult().get();

		Optional<AnnotationDeclaration> annotation = cu.findFirst(AnnotationDeclaration.class);
		assertTrue(annotation.isPresent());

		List<ResolvedFieldDeclaration> fields = annotation.get().resolve().getAllFields();
		assertEquals(2, fields.size());
		assertEquals("a", fields.get(0).getName());
		assertEquals("b", fields.get(1).getName());
	}

	[TestMethod]
	void testForIssue3094() {
		string sourceCode = "@interface Foo { int a = 0; int b = a; }";
		ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
		assertTrue(result.getResult().isPresent());
		CompilationUnit cu = result.getResult().get();

		Optional<NameExpr> nameExpr = cu.findFirst(NameExpr.class);
		assertTrue(nameExpr.isPresent());
		assertDoesNotThrow(nameExpr.get()::resolve);
	}

	[TestMethod]
	void internalTypes_shouldFindAllInnerTypeDeclaration() {
		string sourceCode = "@interface Foo { class A {} interface B {} @interface C {} enum D {} }";

		ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
		assertTrue(result.getResult().isPresent());
		CompilationUnit cu = result.getResult().get();

		Optional<AnnotationDeclaration> annotation = cu.findFirst(AnnotationDeclaration.class);
		assertTrue(annotation.isPresent());
		assertEquals(4, annotation.get().resolve().internalTypes().size());
	}
	
	[TestMethod]
    void isAnnotationNotInheritable() {
        string sourceCode = "@interface Foo {}";

        ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
        assertTrue(result.getResult().isPresent());
        CompilationUnit cu = result.getResult().get();

        Optional<AnnotationDeclaration> annotation = cu.findFirst(AnnotationDeclaration.class);
        assertTrue(annotation.isPresent());

        assertFalse(annotation.get().resolve().isInheritable());
    }
	
	[TestMethod]
    void isAnnotationInheritable() {
        string sourceCode = "import java.lang.annotation.Inherited;\n" + 
                "    @Inherited\n" + 
                "    @interface Foo {}";

        ParseResult<CompilationUnit> result = javaParser.parse(sourceCode);
        assertTrue(result.getResult().isPresent());
        CompilationUnit cu = result.getResult().get();

        Optional<AnnotationDeclaration> annotation = cu.findFirst(AnnotationDeclaration.class);
        assertTrue(annotation.isPresent());

        assertTrue(annotation.get().resolve().isInheritable());
    }
	
}
