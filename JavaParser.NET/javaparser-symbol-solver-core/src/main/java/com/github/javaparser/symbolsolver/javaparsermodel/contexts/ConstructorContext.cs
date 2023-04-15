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

namespace com.github.javaparser.symbolsolver.javaparsermodel.contexts;



/**
 * @author Federico Tomassetti
 */
public class ConstructorContext:AbstractMethodLikeDeclarationContext<ConstructorDeclaration> {

    ///
    /// Constructors
    ///

    public ConstructorContext(ConstructorDeclaration wrappedNode, TypeSolver typeSolver) {
        super(wrappedNode, typeSolver);
    }

    @Override
    public List<Parameter> parametersExposedToChild(Node child) {
        // TODO/FIXME: Presumably the parameters must be exposed to all children and their descendants, not just the direct child?
        if (child == wrappedNode.getBody()) {
            return wrappedNode.getParameters();
        }
        return Collections.emptyList();
    }

}
