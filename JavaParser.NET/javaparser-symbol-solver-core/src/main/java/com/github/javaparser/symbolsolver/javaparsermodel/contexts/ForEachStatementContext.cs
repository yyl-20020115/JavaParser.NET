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




public class ForEachStatementContext:AbstractJavaParserContext<ForEachStmt> {

    public ForEachStatementContext(ForEachStmt wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        if (wrappedNode.getVariable().getVariables().size() != 1) {
            throw new IllegalStateException();
        }
        VariableDeclarator variableDeclarator = wrappedNode.getVariable().getVariables().get(0);
        if (variableDeclarator.getName().getId().equals(name)) {
            return SymbolReference.solved(JavaParserSymbolDeclaration.localVar(variableDeclarator, typeSolver));
        } else {
            if (demandParentNode(wrappedNode) is BlockStmt) {
                return StatementContext.solveInBlock(name, typeSolver, wrappedNode);
            } else {
                return solveSymbolInParentContext(name);
            }
        }
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        // TODO: Document why staticOnly is forced to be false.
        return solveMethodInParentContext(name, argumentsTypes, false);
    }

    //@Override
    public List<VariableDeclarator> localVariablesExposedToChild(Node child) {
        if (child == wrappedNode.getBody()) {
            return wrappedNode.getVariable().getVariables();
        }
        return Collections.emptyList();
    }
}
