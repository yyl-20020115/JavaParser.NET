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





class SymbolSolverWithJavassistClassTest:AbstractSymbolResolutionTest {
    private TypeSolver typeSolver;
    private Solver symbolSolver;
    private JavassistClassDeclaration classDeclarationConcreteClass;
    private JavassistClassDeclaration classDeclarationSubClassOwnJar;
    private JavassistClassDeclaration classDeclarationInterfaceUserOwnJar;
    private JavassistClassDeclaration classDeclarationSubClassIncludedJar;
    private JavassistClassDeclaration classDeclarationInterfaceUserIncludedJar;
    private JavassistClassDeclaration classDeclarationSubClassExcludedJar;
    private JavassistClassDeclaration classDeclarationInterfaceUserExcludedJar;

    @BeforeEach
    void setup(){
        /*final*/Path pathToMainJar = adaptPath("src/test/resources/javassist_symbols/main_jar/main_jar.jar");
        /*final*/Path pathToIncludedJar = adaptPath("src/test/resources/javassist_symbols/included_jar/included_jar.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToIncludedJar), new JarTypeSolver(pathToMainJar), new ReflectionTypeSolver());

        symbolSolver = new SymbolSolver(typeSolver);

        classDeclarationConcreteClass = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.ConcreteClass");
        classDeclarationSubClassOwnJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubClassOwnJar");
        classDeclarationSubClassIncludedJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubClassIncludedJar");
        classDeclarationSubClassExcludedJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.SubClassExcludedJar");
        classDeclarationInterfaceUserOwnJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.InterfaceUserOwnJar");
        classDeclarationInterfaceUserIncludedJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.InterfaceUserIncludedJar");
        classDeclarationInterfaceUserExcludedJar = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.javasymbolsolver.javassist_symbols.main_jar.InterfaceUserExcludedJar");
    }

    [TestMethod]
    void testSolveSymbolInTypeCanSolveFirstOwnField() {
        assertCanSolveSymbol("STATIC_STRING", classDeclarationConcreteClass);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanSolveSecondOwnField() {
        assertCanSolveSymbol("SECOND_STRING", classDeclarationConcreteClass);
    }

    [TestMethod]
    void testSolveSymbolInTypeCantResolveNonExistentField() {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(classDeclarationConcreteClass, "FIELD_THAT_DOES_NOT_EXIST");

        assertFalse(solvedSymbol.isSolved());

        assertThrows(UnsolvedSymbolException.class, () -> {
        	solvedSymbol.getCorrespondingDeclaration();
        }, "Expected UnsolvedSymbolException when requesting CorrespondingDeclaration on unsolved SymbolRefernce");

    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInSuper() {
        assertCanSolveSymbol("SUPER_FIELD", classDeclarationSubClassOwnJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInSuperIncludedJar() {
        assertCanSolveSymbol("SUPER_FIELD", classDeclarationSubClassIncludedJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeThrowsExceptionOnResolveFieldInSuperExcludedJar() {
        try {
            symbolSolver.solveSymbolInType(classDeclarationSubClassExcludedJar, "SUPER_FIELD");
        } catch (Exception e) {
            assertTrue(e is UnsolvedSymbolException);
            assertEquals("Unsolved symbol : com.github.javaparser.javasymbolsolver.javassist_symbols.excluded_jar.SuperClassExcludedJar", e.getMessage());
            return;
        }
        fail("Excepted NotFoundException wrapped _in a RuntimeException, but got no exception.");
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInInterface() {
        assertCanSolveSymbol("INTERFACE_FIELD", classDeclarationInterfaceUserOwnJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeCanResolveFieldInInterfaceIncludedJar() {
        assertCanSolveSymbol("INTERFACE_FIELD", classDeclarationInterfaceUserIncludedJar);
    }

    [TestMethod]
    void testSolveSymbolInTypeThrowsExceptionOnResolveFieldInInterfaceExcludedJar() {
        try {
            symbolSolver.solveSymbolInType(classDeclarationInterfaceUserExcludedJar, "INTERFACE_FIELD");
        } catch (Exception e) {
            assertTrue(e is UnsolvedSymbolException);
            assertEquals("Unsolved symbol : com.github.javaparser.javasymbolsolver.javassist_symbols.excluded_jar.InterfaceExcludedJar", e.getMessage());
            return;
        }
        fail("Excepted NotFoundException wrapped _in a RuntimeException, but got no exception.");
    }

    private void assertCanSolveSymbol(string symbolName, JavassistClassDeclaration classDeclaration) {
        SymbolReference<?:ResolvedValueDeclaration> solvedSymbol = symbolSolver.solveSymbolInType(classDeclaration, symbolName);

        assertTrue(solvedSymbol.isSolved());
        assertEquals(symbolName, solvedSymbol.getCorrespondingDeclaration().asField().getName());
    }
}
