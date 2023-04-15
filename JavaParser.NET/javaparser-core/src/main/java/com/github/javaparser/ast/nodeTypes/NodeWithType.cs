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
namespace com.github.javaparser.ast.nodeTypes;



/**
 * A node with a type.
 * <p>
 * The main reason for this interface is to permit users to manipulate homogeneously all nodes with getType/setType
 * methods
 *
 * @since 2.3.1
 */
public interface NodeWithType<N:Node, T:Type> {

    /**
     * Gets the type
     *
     * @return the type
     */
    T getType();

    /**
     * Sets the type
     *
     * @param type the type
     * @return this
     */
    N setType(T type);

    void tryAddImportToParentCompilationUnit(Type clazz);

    /**
     * Sets this type to this class and try to import it to the {@link CompilationUnit} if needed
     *
     * @param typeClass the type
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default N setType(Type typeClass) {
        tryAddImportToParentCompilationUnit(typeClass);
        return setType((T) parseType(typeClass.getSimpleName()));
    }

    //@SuppressWarnings("unchecked")
    default N setType(/*final*/string typeString) {
        assertNonEmpty(typeString);
        return setType((T) parseType(typeString));
    }

    default string getTypeAsString() {
        return getType().asString();
    }
}
