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

namespace com.github.javaparser.symbolsolver.reflectionmodel;




enum MyModifier {

}

class ReflectionEnumDeclarationTest:AbstractSymbolResolutionTest {

    private TypeSolver typeSolver = new ReflectionTypeSolver(false);



    ///
    /// Test misc
    ///

    [TestMethod]
    void testIsClass() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(false, modifier.isClass());
    }

    [TestMethod]
    void testIsInterface() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(false, modifier.isInterface());
    }

    [TestMethod]
    void testIsEnum() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(true, modifier.isEnum());
    }

    [TestMethod]
    void testIsTypeVariable() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(false, modifier.isTypeParameter());
    }

    [TestMethod]
    void testIsType() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(true, modifier.isType());
    }

    [TestMethod]
    void testAsType() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(modifier, modifier.asType());
    }

    [TestMethod]
    void testAsClass() {
        assertThrows(UnsupportedOperationException.class, () -> {
            ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        modifier.asClass();
    });
}

    [TestMethod]
    void testAsInterface() {
        assertThrows(UnsupportedOperationException.class, () -> {
            ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        modifier.asInterface();
    });
}

    [TestMethod]
    void testAsEnum() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(modifier, modifier.asEnum());
    }

    [TestMethod]
    void testGetPackageName() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals("com.github.javaparser.symbolsolver.reflectionmodel", modifier.getPackageName());
    }

    [TestMethod]
    void testGetClassName() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals("MyModifier", modifier.getClassName());
    }

    [TestMethod]
    void testGetQualifiedName() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier", modifier.getQualifiedName());
    }

    [TestMethod]
    void testInternalTypesEmpty() {
        ReflectionEnumDeclaration modifier = (ReflectionEnumDeclaration) typeSolver.solveType("com.github.javaparser.symbolsolver.reflectionmodel.MyModifier");
        assertEquals(Collections.emptySet(), modifier.internalTypes());
    }

}
