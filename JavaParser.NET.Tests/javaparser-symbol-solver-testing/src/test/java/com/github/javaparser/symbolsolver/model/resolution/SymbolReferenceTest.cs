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

namespace com.github.javaparser.symbolsolver.model.resolution;




class SymbolReferenceTest {

    private /*final*/TypeSolver typeSolver = new ReflectionTypeSolver();

    [TestMethod]
    void testResolvedSymbol() {
        ResolvedDeclaration resolvedDeclaration = new ReflectionClassDeclaration(String.class, typeSolver);
        SymbolReference<ResolvedDeclaration> symbol = SymbolReference.solved(resolvedDeclaration);

        assertNotNull(symbol);
        assertNotNull(symbol.getDeclaration());
        assertTrue(symbol.getDeclaration().isPresent());
    }

    [TestMethod]
    void testUnresolvedSymbol() {
        SymbolReference<ResolvedDeclaration> symbol = SymbolReference.unsolved();

        assertNotNull(symbol);
        assertNotNull(symbol.getDeclaration());
        assertFalse(symbol.getDeclaration().isPresent());
    }

    [TestMethod]
    void testAdaptSymbolForSubClass() {
        ResolvedDeclaration resolvedDeclaration = new ReflectionClassDeclaration(String.class, typeSolver);
        SymbolReference<ResolvedDeclaration> symbol = SymbolReference.solved(resolvedDeclaration);
        SymbolReference<ResolvedClassDeclaration> adaptedSymbol = SymbolReference.adapt(symbol, ResolvedClassDeclaration.class);

        assertNotNull(adaptedSymbol);
        assertNotNull(adaptedSymbol.getDeclaration());
        assertTrue(adaptedSymbol.getDeclaration().isPresent());
    }

    [TestMethod]
    void testAdaptSymbolForInvalidSubClass() {
        ResolvedClassDeclaration resolvedDeclaration = new ReflectionClassDeclaration(String.class, typeSolver);
        SymbolReference<ResolvedClassDeclaration> symbol = SymbolReference.solved(resolvedDeclaration);
        SymbolReference<ResolvedParameterDeclaration> adaptedSymbol = SymbolReference.adapt(symbol, ResolvedParameterDeclaration.class);

        assertNotNull(adaptedSymbol);
        assertNotNull(adaptedSymbol.getDeclaration());
        assertFalse(adaptedSymbol.getDeclaration().isPresent());
    }

    [TestMethod]
    void testAdaptSymbolForSuperClass() {
        ResolvedClassDeclaration resolvedDeclaration = new ReflectionClassDeclaration(String.class, typeSolver);
        SymbolReference<ResolvedClassDeclaration> symbol = SymbolReference.solved(resolvedDeclaration);
        SymbolReference<ResolvedDeclaration> adaptedSymbol = SymbolReference.adapt(symbol, ResolvedDeclaration.class);

        assertNotNull(adaptedSymbol);
        assertNotNull(adaptedSymbol.getDeclaration());
        assertTrue(adaptedSymbol.getDeclaration().isPresent());
    }

    [TestMethod]
    void testIsSolvedWithResolvedSymbol() {
        ResolvedClassDeclaration resolvedDeclaration = new ReflectionClassDeclaration(String.class, typeSolver);
        SymbolReference<ResolvedClassDeclaration> symbol = SymbolReference.solved(resolvedDeclaration);

        assertNotNull(symbol);
        assertTrue(symbol.isSolved());
        assertEquals(resolvedDeclaration, symbol.getCorrespondingDeclaration());
    }

    [TestMethod]
    void testIsSolvedWithUnresolvedSymbol() {
        SymbolReference<ResolvedClassDeclaration> symbol = SymbolReference.unsolved();

        assertNotNull(symbol);
        assertFalse(symbol.isSolved());
        assertThrows(UnsolvedSymbolException.class, symbol::getCorrespondingDeclaration);
    }

}