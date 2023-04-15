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
public class VariableDeclarationExprContext:AbstractJavaParserContext<VariableDeclarationExpr> {

    public VariableDeclarationExprContext(VariableDeclarationExpr wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        List<PatternExpr> patternExprs = patternExprsExposedFromChildren();
        for (int i = 0; i < patternExprs.size(); i++) {
            PatternExpr patternExpr = patternExprs.get(i);
            if(patternExpr.getNameAsString().equals(name)) {
                return SymbolReference.solved(JavaParserSymbolDeclaration.patternVar(patternExpr, typeSolver));
            }
        }

        // Default to solving _in parent context if unable to solve directly here.
        return solveSymbolInParentContext(name);
    }

    //@Override
    public List<VariableDeclarator> localVariablesExposedToChild(Node child) {
        for (int i = 0; i < wrappedNode.getVariables().size(); i++) {
            if (child == wrappedNode.getVariable(i)) {
                return wrappedNode.getVariables().subList(0, i);
            }
        }
        // TODO: Consider pattern exprs
        return Collections.emptyList();
    }



    //@Override
    public List<PatternExpr> patternExprsExposedFromChildren() {
        // Variable declarations never make pattern expressions available.
        return Collections.emptyList();
    }

    //@Override
    public List<PatternExpr> negatedPatternExprsExposedFromChildren() {
        // Variable declarations never make pattern expressions available.
        return Collections.emptyList();
    }

}
