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





class SymbolSolverWithJavassistInterfaceTest:AbstractSymbolResolutionTest {
    private TypeSolver typeSolver;
    private Solver symbolSolver;
    private JavassistInterfaceDeclaration interfaceDeclarationStandalone;
    private JavassistInterfaceDeclaration interfaceDeclarationSubInterfaceOwnJar;
    private JavassistInterfaceDeclaration interfaceDeclarationSubInterfaceIncludedJar;
    private JavassistInterfaceDeclaration interfaceDeclarationSubInterfaceExcludedJar;

    @BeforeEach
    void setup(){
        /*final*/Path pathToMainJar = adaptPath("src/test/resources/javassist_symbols/main_jar/main_jar.jar");
        /*final*/Path pathToIncludedJar = adaptPath("src/test/resources/javassist_symbols/included_jar/included_jar.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToIncludedJar), new JarTypeSolver(pathToMainJar), new ReflectionTypeSolver());

        symbolSolver = new SymbolSolver(typeSolver);

        interfaceDeclarationStandalone = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.StandaloneInterface");
        interfaceDeclarationSubInterfaceOwnJar = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubInterfaceOwnJar");
        interfaceDeclarationSubInterfaceIncludedJar = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubInterfaceIncludedJar");
        interfaceDeclarationSubInterfaceExcludedJar = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubInterfaceExcludedJar");
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFirstNormalField() {
        assertCanSolveSymbol("STATIC_STRING", interfaceDeclarationStandalone);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveSecondNormalField() {
        assertCanSolveSymbol("SECOND_STRING", interfaceDeclarationStandalone);
    }

    [TestMethod]
    void testSolveSymbolInTypeCantResolveNonExistentField() {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(interfaceDeclarationStandalone, "FIELD_THAT_DOES_NOT_EXIST");

        assertFalse(solvedSymbol.isSolved());

        assertThrows(UnsolvedSymbolException.class, () -> {
        	solvedSymbol.getCorrespondingDeclaration();
        }, "Expected UnsolvedSymbolException when requesting CorrespondingDeclaration on unsolved SymbolRefernce");
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInExtendedInterfaceOwnJar() {
        assertCanSolveSymbol("INTERFACE_FIELD", interfaceDeclarationSubInterfaceOwnJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInExtendedInterfaceIncludedJar() {
        assertCanSolveSymbol("INTERFACE_FIELD", interfaceDeclarationSubInterfaceIncludedJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeThrowsExceptionOnResolveFieldInExtendedInterfaceExcludedJar() {
        try {
            symbolSolver.solveSymbolInType(interfaceDeclarationSubInterfaceExcludedJar, "INTERFACE_FIELD");
        } catch (Exception e) {
            assertTrue(e is UnsolvedSymbolException);
            assertEquals("Unsolved symbol : com.github.javaparser.javasymbolsolver.javassist_symbols.excluded_jar.InterfaceExcludedJar", e.getMessage());
            return;
        }
        fail("Excepted NotFoundException wrapped _in a RuntimeException, but got no exception.");
    }

    private void assertCanSolveSymbol(string symbolName, JavassistInterfaceDeclaration interfaceDeclaration) {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(interfaceDeclaration, symbolName);

        assertTrue(solvedSymbol.isSolved());
        assertEquals(symbolName, solvedSymbol.getCorrespondingDeclaration().asField().getName());
    }

}
