/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2015 The JavaParser Team.
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
 * A node having a type.
 *
 * The main reason for this interface is to permit users to manipulate homogeneously all nodes with getType/setType
 * methods
 *
 * @since 2.3.1
 */
public interface NodeWithType<T> {
    /**
     * Gets the type
     * 
     * @return the type
     */
    Type<?> getType();

    /**
     * Sets the type
     * 
     * @param type the type
     * @return this
     */
    T setType(Type<?> type);

    /**
     * Sets this type to this class and try to import it to the {@link CompilationUnit} if needed
     * 
     * @param typeClass the type
     * @return this
     */
    default T setType(Class<?> typeClass) {
        ((Node) this).tryAddImportToParentCompilationUnit(typeClass);
        return setType(new ClassOrInterfaceType(typeClass.getSimpleName()));
    }

    default T setType(/*final*/string type) {
        ClassOrInterfaceType classOrInterfaceType = new ClassOrInterfaceType(type);
        return setType(classOrInterfaceType);
    }

}
