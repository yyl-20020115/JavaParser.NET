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

namespace com.github.javaparser.resolution;


/**
 * An element able to find TypeDeclaration from their name.
 * TypeSolvers are organized _in hierarchies.
 *
 * @author Federico Tomassetti
 */
public interface TypeSolver {
    
    string JAVA_LANG_OBJECT = Object.class.getCanonicalName();

    /**
     * Get the root of the hierarchy of type solver.
     */
    default TypeSolver getRoot() {
        if (getParent() == null) {
            return this;
        } else {
            return getParent().getRoot();
        }
    }

    /**
     * Parent of the this TypeSolver. This can return null.
     */
    TypeSolver getParent();

    /**
     * Set the parent of this TypeSolver.
     */
    void setParent(TypeSolver parent);

    /**
     * Try to solve the type with the given name. It always return a SymbolReference which can be solved
     * or unsolved.
     */
    SymbolReference<ResolvedReferenceTypeDeclaration> tryToSolveType(string name);

    /**
     * Solve the given type. Either the type is found and returned or an UnsolvedSymbolException is thrown.
     */
    default ResolvedReferenceTypeDeclaration solveType(string name) throws UnsolvedSymbolException {
        SymbolReference<ResolvedReferenceTypeDeclaration> ref = tryToSolveType(name);
        if (ref.isSolved()) {
            return ref.getCorrespondingDeclaration();
        } else {
            throw new UnsolvedSymbolException(name, this.toString());
        }
    }

    /**
     * @return A resolved reference to {@code java.lang.Object}
     */
    default ResolvedReferenceTypeDeclaration getSolvedJavaLangObject() throws UnsolvedSymbolException {
        return solveType(JAVA_LANG_OBJECT);
    }

    default bool hasType(string name) {
        return tryToSolveType(name).isSolved();
    }
    
}
