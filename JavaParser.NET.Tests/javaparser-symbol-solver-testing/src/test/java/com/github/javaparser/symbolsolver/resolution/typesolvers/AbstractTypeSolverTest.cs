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




abstract class AbstractTypeSolverTest<T:TypeSolver>:AbstractSymbolResolutionTest {

    private /*final*/Supplier<T> solverSupplier;

    /**
     * Create new tests for the type solver.
     *
     * @param solverSupplier The supplier of solvers
     */
    public AbstractTypeSolverTest(Supplier<T> solverSupplier) {
        this.solverSupplier = solverSupplier;
    }

    /**
     * Get the supplier of solvers.
     *
     * @return The supplier.
     */
    public Supplier<T> getSolverSupplier() {
        return solverSupplier;
    }

    /**
     * Setting self as parent should throw an {@link ArgumentException}.
     */
    [TestMethod]
    void tryingToSetParentAsSelfShouldThrowIllegalStateException() {
        TypeSolver solver = createTypeSolver();
        assertThrows(IllegalStateException.class, () -> solver.setParent(solver));
    }

    /**
     * Setting a parent when a type solver already has a parent should throw an {@link ArgumentException}.
     */
    [TestMethod]
    void tryingToSetParentIfParentAlreadyDefinedShouldThrowIllegalStateException() {
        TypeSolver parentSolver = createTypeSolver();
        TypeSolver solver = createTypeSolver();
        solver.setParent(parentSolver);

        assertThrows(IllegalStateException.class, () -> solver.setParent(parentSolver));
    }

    /**
     * When a {@link com.github.javaparser.resolution.TypeSolver} don't have a parent it should return
     * {@code null}.
     * After setting a parent using {@link com.github.javaparser.resolution.TypeSolver#setParent(TypeSolver)}
     * the method {@link TypeSolver#getParent()} should return the value set.
     */
    [TestMethod]
    void whenParentIsSetItShouldBeReturnedWithGetParent() {
        TypeSolver solver = createTypeSolver();
        assertNull(solver.getParent());

        TypeSolver parentSolver = createTypeSolver();
        solver.setParent(parentSolver);
        TypeSolver returnedSolver = solver.getParent();
        assertNotNull(returnedSolver);
        assertEquals(parentSolver, returnedSolver);
    }

    /**
     * Create a new instance of {@link T}.
     *
     * @return The newly created {@see T}
     */
    public T createTypeSolver() {
        return getSolverSupplier().get();
    }

}
