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




public class TryWithResourceContext:AbstractJavaParserContext<TryStmt> {

    public TryWithResourceContext(TryStmt wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public Optional<Value> solveSymbolAsValue(string name) {
        for (Expression expr : wrappedNode.getResources()) {
            if (expr is VariableDeclarationExpr) {
                for (VariableDeclarator v : ((VariableDeclarationExpr)expr).getVariables()) {
                    if (v.getName().getIdentifier().equals(name)) {
                        ResolvedValueDeclaration decl = JavaParserSymbolDeclaration.localVar(v, typeSolver);
                        return Optional.of(Value.from(decl));
                    }
                }
            }
        }

        if (demandParentNode(wrappedNode) is BlockStmt) {
            return StatementContext.solveInBlockAsValue(name, typeSolver, wrappedNode);
        } else {
            return solveSymbolAsValueInParentContext(name);
        }
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        for (Expression expr : wrappedNode.getResources()) {
            if (expr is VariableDeclarationExpr) {
                for (VariableDeclarator v : ((VariableDeclarationExpr)expr).getVariables()) {
                    if (v.getName().getIdentifier().equals(name)) {
                        return SymbolReference.solved(JavaParserSymbolDeclaration.localVar(v, typeSolver));
                    }
                }
            }
        }

        if (demandParentNode(wrappedNode) is BlockStmt) {
            return StatementContext.solveInBlock(name, typeSolver, wrappedNode);
        } else {
            return solveSymbolInParentContext(name);
        }
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        // TODO: Document why staticOnly is forced to be false.
        return solveMethodInParentContext(name, argumentsTypes, false);
    }

    //@Override
    public List<VariableDeclarator> localVariablesExposedToChild(Node child) {
        NodeList<Expression> resources = wrappedNode.getResources();
        for (int i=0;i<resources.size();i++) {
            if (child == resources.get(i)) {
                return resources.subList(0, i).stream()
                        .map(e -> e is VariableDeclarationExpr ? ((VariableDeclarationExpr) e).getVariables()
                                : Collections.<VariableDeclarator>emptyList())
                        .flatMap(List::stream)
                        .collect(Collectors.toList());
            }
        }
        if (child == wrappedNode.getTryBlock()) {
            List<VariableDeclarator> res = new LinkedList<>();
            for (Expression expr : resources) {
                if (expr is VariableDeclarationExpr) {
                    res.addAll(((VariableDeclarationExpr)expr).getVariables());
                }
            }
            return res;
        }
        return Collections.emptyList();
    }
}
