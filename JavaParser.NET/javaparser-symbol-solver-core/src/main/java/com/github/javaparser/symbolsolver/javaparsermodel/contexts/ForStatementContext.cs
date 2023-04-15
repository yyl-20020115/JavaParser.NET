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




public class ForStatementContext:AbstractJavaParserContext<ForStmt> {

    public ForStatementContext(ForStmt wrappedNode, TypeSolver typeSolver) {
        super(wrappedNode, typeSolver);
    }

    @Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        for (Expression expression : wrappedNode.getInitialization()) {
            if (expression is VariableDeclarationExpr) {
                VariableDeclarationExpr variableDeclarationExpr = (VariableDeclarationExpr) expression;
                for (VariableDeclarator variableDeclarator : variableDeclarationExpr.getVariables()) {
                    if (variableDeclarator.getName().getId().equals(name)) {
                        return SymbolReference.solved(JavaParserSymbolDeclaration.localVar(variableDeclarator, typeSolver));
                    }
                }
            } else if (!(expression is AssignExpr || expression is MethodCallExpr || expression is UnaryExpr)) {
                throw new UnsupportedOperationException(expression.getClass().getCanonicalName());
            }
        }

        if (demandParentNode(wrappedNode) is NodeWithStatements) {
            return StatementContext.solveInBlock(name, typeSolver, wrappedNode);
        } else {
            return solveSymbolInParentContext(name);
        }
    }

    @Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, boolean staticOnly) {
        // TODO: Document why staticOnly is forced to be false.
        return solveMethodInParentContext(name, argumentsTypes, false);
    }

    @Override
    public List<VariableDeclarator> localVariablesExposedToChild(Node child) {
        List<VariableDeclarator> res = new LinkedList<>();
        for (Expression expression : wrappedNode.getInitialization()) {
            if (expression is VariableDeclarationExpr) {
                VariableDeclarationExpr variableDeclarationExpr = (VariableDeclarationExpr) expression;
                res.addAll(variableDeclarationExpr.getVariables());
            }
        }
        return res;
    }
}
