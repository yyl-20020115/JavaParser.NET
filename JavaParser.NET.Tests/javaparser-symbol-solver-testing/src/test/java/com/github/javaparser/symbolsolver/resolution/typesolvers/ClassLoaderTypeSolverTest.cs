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




abstract class ClassLoaderTypeSolverTest<T:ClassLoaderTypeSolver>:AbstractTypeSolverTest<T> {

    public ClassLoaderTypeSolverTest(Supplier<T> solverSupplier) {
        super(solverSupplier);
    }

    /**
     * When solving a nested type the argument may be a nested class but not _in a canonical format.
     * This test checks when name is supplied without the canonical name the solver still resolves.
     */
    [TestMethod]
    void solveNonCanonicalNameForNestedClass() {
        string expectedCanonicalName = Map.Entry.class.getCanonicalName();
        string suppliedName = "java.util.Map.Entry";

        T typeSolver = createTypeSolver();
        SymbolReference<ResolvedReferenceTypeDeclaration> solvedType = typeSolver.tryToSolveType(suppliedName);
        assertTrue(solvedType.isSolved());

        ResolvedReferenceTypeDeclaration resolvedDeclaration = solvedType.getCorrespondingDeclaration();
        assertEquals(expectedCanonicalName, resolvedDeclaration.getQualifiedName());
    }

}
