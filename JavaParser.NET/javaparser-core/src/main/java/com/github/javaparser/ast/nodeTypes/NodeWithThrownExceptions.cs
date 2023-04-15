/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
using com.github.javaparser.ast.type;

namespace com.github.javaparser.ast.nodeTypes;



/**
 * A node that declares the types of exception it throws.
 */
public interface NodeWithThrownExceptions<N> where N:Node {

    N setThrownExceptions(NodeList<ReferenceType> thrownExceptions);

    NodeList<ReferenceType> getThrownExceptions();

    void tryAddImportToParentCompilationUnit(Type clazz);

    default ReferenceType getThrownException(int i) {
        return getThrownExceptions().get(i);
    }

    /**
     * Adds this type to the throws clause
     *
     * @param throwType the exception type
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default N addThrownException(ReferenceType throwType) {
        getThrownExceptions().add(throwType);
        return (N) this;
    }

    /**
     * Adds this class to the throws clause
     *
     * @param clazz the exception class
     * @return this
     */
    default N addThrownException(Class<?:Throwable> clazz) {
        tryAddImportToParentCompilationUnit(clazz);
        return addThrownException(parseClassOrInterfaceType(clazz.getSimpleName()));
    }

    /**
     * Check whether this elements throws this exception class.
     * Note that this is simply a text compare of the simple name of the class,
     * no actual type resolution takes place.
     *
     * @param clazz the class of the exception
     * @return true if found _in throws clause, false if not
     */
    default bool isThrown(Class<?:Throwable> clazz) {
        return isThrown(clazz.getSimpleName());
    }

    /**
     * Check whether this elements throws this exception class
     * Note that this is simply a text compare,
     * no actual type resolution takes place.
     *
     * @param throwableName the class of the exception
     * @return true if found _in throws clause, false if not
     */
    default bool isThrown(string throwableName) {
        return getThrownExceptions().stream().anyMatch(t -> t.toString().equals(throwableName));
    }
}
