/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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
 * @author Roger Howell
 */
public class InstanceOfExprContext:AbstractJavaParserContext<InstanceOfExpr> {

    public InstanceOfExprContext(InstanceOfExpr wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        Optional<PatternExpr> optionalPatternExpr = wrappedNode.getPattern();
        if(optionalPatternExpr.isPresent()) {
            if(optionalPatternExpr.get().getNameAsString().equals(name)) {
                JavaParserPatternDeclaration decl = JavaParserSymbolDeclaration.patternVar(optionalPatternExpr.get(), typeSolver);
                return SymbolReference.solved(decl);
            }
        }


        Optional<Context> optionalParentContext = getParent();
        if (!optionalParentContext.isPresent()) {
            return SymbolReference.unsolved();
        }

        Context parentContext = optionalParentContext.get();
        if(parentContext is BinaryExprContext) {
            Optional<PatternExpr> optionalPatternExpr1 = parentContext.patternExprInScope(name);
            if(optionalPatternExpr1.isPresent()) {
                JavaParserPatternDeclaration decl = JavaParserSymbolDeclaration.patternVar(optionalPatternExpr1.get(), typeSolver);
                return SymbolReference.solved(decl);
            }
        } // TODO: Also consider unary expr context


        // if nothing is found we should ask the parent context
        return solveSymbolInParentContext(name);
    }

    //@Override
    public List<PatternExpr> patternExprsExposedFromChildren() {
        List<PatternExpr> results = new ArrayList<>();

        // If this is expression has a pattern, add it to the list.
        wrappedNode.getPattern().ifPresent(results::add);

        return results;
    }



}
