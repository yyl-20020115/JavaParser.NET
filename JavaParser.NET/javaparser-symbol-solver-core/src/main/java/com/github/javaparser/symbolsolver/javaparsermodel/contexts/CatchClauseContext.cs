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
 * @author Fred Lefévère-Laoide
 */
public class CatchClauseContext:AbstractJavaParserContext<CatchClause> {

    public CatchClauseContext(CatchClause wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public /*final*/SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        SymbolDeclarator sb = JavaParserFactory.getSymbolDeclarator(wrappedNode.getParameter(), typeSolver);
        SymbolReference<?:ResolvedValueDeclaration> symbolReference = AbstractJavaParserContext.solveWith(sb, name);
        if (symbolReference.isSolved()) {
            return symbolReference;
        }

        // if nothing is found we should ask the parent context
        return solveSymbolInParentContext(name);
    }

    //@Override
    public /*final*/Optional<Value> solveSymbolAsValue(string name) {
        SymbolDeclarator sb = JavaParserFactory.getSymbolDeclarator(wrappedNode.getParameter(), typeSolver);
        Optional<Value> symbolReference = solveWithAsValue(sb, name);
        if (symbolReference.isPresent()) {
            // Perform parameter type substitution as needed
            return symbolReference;
        }

        // if nothing is found we should ask the parent context
        return solveSymbolAsValueInParentContext(name);
    }

    //@Override
    public /*final*/SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        // TODO: Document why staticOnly is forced to be false.
        return solveMethodInParentContext(name, argumentsTypes, false);
    }

    //@Override
    public List<VariableDeclarator> localVariablesExposedToChild(Node child) {
        return Collections.emptyList();
    }

    //@Override
    public List<Parameter> parametersExposedToChild(Node child) {
        // TODO/FIXME: Presumably the parameters must be exposed to all children and their descendants, not just the direct child?
        if (child == getWrappedNode().getBody()) {
            return Collections.singletonList(getWrappedNode().getParameter());
        }
        return Collections.emptyList();
    }
}
