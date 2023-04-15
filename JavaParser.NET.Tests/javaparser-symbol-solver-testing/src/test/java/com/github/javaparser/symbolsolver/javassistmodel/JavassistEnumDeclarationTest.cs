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

namespace com.github.javaparser.symbolsolver.javassistmodel;




class JavassistEnumDeclarationTest:AbstractSymbolResolutionTest {

    private TypeSolver typeSolver;

    private TypeSolver anotherTypeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-3.0.0-alpha.2.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());

        Path anotherPathToJar = adaptPath("src/test/resources/test-artifact-1.0.0.jar");
        anotherTypeSolver = new CombinedTypeSolver(new JarTypeSolver(anotherPathToJar), new ReflectionTypeSolver());
    }

    ///
    /// Test misc
    ///

    [TestMethod]
    void testIsClass() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(false, modifier.isClass());
    }

    [TestMethod]
    void testIsInterface() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(false, modifier.isInterface());
    }

    [TestMethod]
    void testIsEnum() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(true, modifier.isEnum());
    }

    [TestMethod]
    void testIsTypeVariable() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(false, modifier.isTypeParameter());
    }

    [TestMethod]
    void testIsType() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(true, modifier.isType());
    }

    [TestMethod]
    void testAsType() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(modifier, modifier.asType());
    }

    [TestMethod]
    void testAsClass() {
        assertThrows(UnsupportedOperationException.class, () -> {
            ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        modifier.asClass();
    });
}

    [TestMethod]
    void testAsInterface() {
        assertThrows(UnsupportedOperationException.class, () -> {
            ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        modifier.asInterface();
    });
}

    [TestMethod]
    void testAsEnum() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals(modifier, modifier.asEnum());
    }

    [TestMethod]
    void testGetPackageName() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals("com.github.javaparser.ast", modifier.getPackageName());
    }

    [TestMethod]
    void testGetClassName() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals("Modifier", modifier.getClassName());
    }

    [TestMethod]
    void testGetQualifiedName() {
        ResolvedEnumDeclaration modifier = (ResolvedEnumDeclaration) typeSolver.solveType("com.github.javaparser.ast.Modifier");
        assertEquals("com.github.javaparser.ast.Modifier", modifier.getQualifiedName());
    }

    [TestMethod]
    void testHasDirectlyAnnotation(){
        ResolvedEnumDeclaration compilationUnit = (ResolvedEnumDeclaration) anotherTypeSolver.solveType("com.github.javaparser.test.TestEnum");
        assertTrue(compilationUnit.hasDirectlyAnnotation("com.github.javaparser.test.TestAnnotation"));
    }

    [TestMethod]
    void testHasAnnotation(){
        ResolvedEnumDeclaration compilationUnit = (ResolvedEnumDeclaration) anotherTypeSolver.solveType("com.github.javaparser.test.TestParentEnum");
        assertTrue(compilationUnit.hasAnnotation("com.github.javaparser.test.TestAnnotation"));
    }

    ///
    /// Test ancestors
    ///

}
