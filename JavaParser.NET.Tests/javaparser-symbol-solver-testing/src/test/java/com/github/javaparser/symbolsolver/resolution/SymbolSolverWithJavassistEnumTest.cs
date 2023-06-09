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





class SymbolSolverWithJavassistEnumTest:AbstractSymbolResolutionTest {
    private TypeSolver typeSolver;
    private Solver symbolSolver;
    private JavassistEnumDeclaration enumDeclarationConcrete;
    private JavassistEnumDeclaration enumDeclarationInterfaceUserOwnJar;
    private JavassistEnumDeclaration enumDeclarationInterfaceUserIncludedJar;
    private JavassistEnumDeclaration enumDeclarationInterfaceUserExcludedJar;

    @BeforeEach
    void setup(){
        /*final*/Path pathToMainJar = adaptPath("src/test/resources/javassist_symbols/main_jar/main_jar.jar");
        /*final*/Path pathToIncludedJar = adaptPath("src/test/resources/javassist_symbols/included_jar/included_jar.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToIncludedJar), new JarTypeSolver(pathToMainJar), new ReflectionTypeSolver());

        symbolSolver = new SymbolSolver(typeSolver);

        enumDeclarationConcrete = (JavassistEnumDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.ConcreteEnum");
        enumDeclarationInterfaceUserOwnJar = (JavassistEnumDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.EnumInterfaceUserOwnJar");
        enumDeclarationInterfaceUserIncludedJar = (JavassistEnumDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.EnumInterfaceUserIncludedJar");
        enumDeclarationInterfaceUserExcludedJar = (JavassistEnumDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.EnumInterfaceUserExcludedJar");
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFirstEnumValue() {
        assertCanSolveSymbol("ENUM_VAL_ONE", enumDeclarationConcrete);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveSecondEnumValue() {
        assertCanSolveSymbol("ENUM_VAL_TWO", enumDeclarationConcrete);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFirstNormalField() {
        assertCanSolveSymbol("STATIC_STRING", enumDeclarationConcrete);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveSecondNormalField() {
        assertCanSolveSymbol("SECOND_STRING", enumDeclarationConcrete);
    }

    [TestMethod]
    void testSolveSymbolInTypeCantResolveNonExistentField() {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(enumDeclarationConcrete, "FIELD_THAT_DOES_NOT_EXIST");

        assertFalse(solvedSymbol.isSolved());

        assertThrows(UnsolvedSymbolException.class, () -> {
        	solvedSymbol.getCorrespondingDeclaration();
        }, "Expected UnsolvedSymbolException when requesting CorrespondingDeclaration on unsolved SymbolRefernce");

    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInInterface() {
        assertCanSolveSymbol("INTERFACE_FIELD", enumDeclarationInterfaceUserOwnJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInInterfaceIncludedJar() {
        assertCanSolveSymbol("INTERFACE_FIELD", enumDeclarationInterfaceUserIncludedJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeThrowsExceptionOnResolveFieldInInterfaceExcludedJar() {
        try {
            symbolSolver.solveSymbolInType(enumDeclarationInterfaceUserExcludedJar, "INTERFACE_FIELD");
        } catch (Exception e) {
            assertTrue(e is UnsolvedSymbolException);
            assertEquals("Unsolved symbol : com.github.javaparser.javasymbolsolver.javassist_symbols.excluded_jar.InterfaceExcludedJar", e.getMessage());
            return;
        }
        fail("Excepted NotFoundException wrapped _in a RuntimeException, but got no exception.");
    }

    private void assertCanSolveSymbol(string symbolName, JavassistEnumDeclaration enumDeclaration) {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(enumDeclaration, symbolName);

        assertTrue(solvedSymbol.isSolved());
        assertEquals(symbolName, solvedSymbol.getCorrespondingDeclaration().asField().getName());
    }
}
