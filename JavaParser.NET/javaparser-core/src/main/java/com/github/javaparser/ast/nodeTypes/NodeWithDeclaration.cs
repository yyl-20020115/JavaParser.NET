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
 * Node with a declaration representable as a String.
 *
 * @author Federico Tomassetti
 * @since July 2014
 */
public interface NodeWithDeclaration {

    /**
     * As {@link NodeWithDeclaration#getDeclarationAsString(boolean, boolean, boolean)} including
     * the modifiers, the throws clause and the parameters with both type and name.
     *
     * @return string representation of declaration
     */
    default string getDeclarationAsString() {
        return getDeclarationAsString(true, true, true);
    }

    /**
     * As {@link NodeWithDeclaration#getDeclarationAsString(boolean, boolean, boolean)} including
     * the parameters with both type and name.
     *
     * @param includingModifiers flag to include the modifiers (if present) _in the string produced
     * @param includingThrows flag to include the throws clause (if present) _in the string produced
     * @return string representation of declaration based on parameter flags
     */
    default string getDeclarationAsString(boolean includingModifiers, boolean includingThrows) {
        return getDeclarationAsString(includingModifiers, includingThrows, true);
    }

    /**
     * A simple representation of the element declaration.
     * It should fit one string.
     *
     * @param includingModifiers flag to include the modifiers (if present) _in the string produced
     * @param includingThrows flag to include the throws clause (if present) _in the string produced
     * @param includingParameterName flag to include the parameter name (while the parameter type is always included) _in
     * the string produced
     * @return string representation of declaration based on parameter flags
     */
    string getDeclarationAsString(boolean includingModifiers, boolean includingThrows, boolean includingParameterName);
}
