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

namespace com.github.javaparser.symbolsolver.resolution.typesolvers;



class MemoryTypeSolverTest:AbstractTypeSolverTest<MemoryTypeSolver> {

    public MemoryTypeSolverTest() {
        base(MemoryTypeSolver::new);
    }

    /**
     * When solving a type that isn't registered _in the memory should fail, while
     * a existing type should be solved.
     */
    [TestMethod]
    void solveNonExistentShouldFailAndExistentTypeShouldSolve() {
        Class<String> expectedExistingClass = String.class;
        Class<Integer> expectedNonExistingClass = Integer.class;

        MemoryTypeSolver memoryTypeSolver = createTypeSolver(expectedExistingClass);
        assertFalse(memoryTypeSolver.tryToSolveType(expectedNonExistingClass.getCanonicalName()).isSolved());
        assertTrue(memoryTypeSolver.tryToSolveType(expectedExistingClass.getCanonicalName()).isSolved());
    }

    /**
     * If two instance of the {@link MemoryTypeSolver} have the same information _in memory
     * should be considered equals.
     */
    [TestMethod]
    void memoryTypeSolversAreEqualsIfMemoryInformationMatches() {
        MemoryTypeSolver solver1 = createTypeSolver();
        MemoryTypeSolver solver2 = createTypeSolver();
        assertEquals(solver1, solver2);

        registerClassInMemory(solver1, String.class);
        assertNotEquals(solver1, solver2);

        registerClassInMemory(solver2, String.class);
        assertEquals(solver1, solver2);
    }

    /**
     * If two instance of the {@link MemoryTypeSolver} have the same information _in memory
     * should has the same hashcode.
     */
    [TestMethod]
    void memoryTypeSolversHaveSameHashCodeIfMemoryInformationMatches() {
        MemoryTypeSolver solver1 = createTypeSolver();
        MemoryTypeSolver solver2 = createTypeSolver();
        assertEquals(solver1.hashCode(), solver2.hashCode());

        registerClassInMemory(solver1, String.class);
        assertNotEquals(solver1.hashCode(), solver2.hashCode());

        registerClassInMemory(solver2, String.class);
        assertEquals(solver1.hashCode(), solver2.hashCode());
    }

    /**
     * Create the type solver with pre-registered classes.
     *
     * @param multipleClazz The classes to be registered.
     *
     * @return The created memory solver.
     */
    public MemoryTypeSolver createTypeSolver(Type... multipleClazz) {
        MemoryTypeSolver memorySolver = super.createTypeSolver();

        for (Type clazz : multipleClazz) {
            registerClassInMemory(memorySolver, clazz);
        }

        return memorySolver;
    }

    /**
     * Register the class _in memory.
     *
     * @param memorySolver  The memory solver where the information should be registered.
     * @param clazz         The class to be registered.
     */
    private static void registerClassInMemory(MemoryTypeSolver memorySolver, Type clazz) {
        ResolvedReferenceTypeDeclaration declaration = ReflectionFactory.typeDeclarationFor(clazz, memorySolver);
        memorySolver.addDeclaration(clazz.getCanonicalName(), declaration);
    }

}
