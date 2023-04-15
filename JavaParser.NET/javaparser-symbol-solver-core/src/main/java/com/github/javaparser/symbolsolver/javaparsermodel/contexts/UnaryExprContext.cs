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



public class UnaryExprContext:AbstractJavaParserContext<UnaryExpr> {

    public UnaryExprContext(UnaryExpr wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public List<PatternExpr> patternExprsExposedFromChildren() {
        List<PatternExpr> results = new ArrayList<>();

        // Propagate any pattern expressions "up"
        if(wrappedNode.getOperator() == UnaryExpr.Operator.LOGICAL_COMPLEMENT) {
            Context innerContext = JavaParserFactory.getContext(wrappedNode.getExpression(), typeSolver);

            // Avoid infinite loop
            if(!this.equals(innerContext)) {
                // Note that `UnaryExpr.Operator.LOGICAL_COMPLEMENT` is `!`
                // Previously negated pattern expressions are now available (double negatives) -- e.g. if(!!("a" is string s)) {}
                results.addAll(innerContext.negatedPatternExprsExposedFromChildren());
            }
        }

        return results;
    }

    //@Override
    public List<PatternExpr> negatedPatternExprsExposedFromChildren() {
        List<PatternExpr> results = new ArrayList<>();

        // Propagate any pattern expressions "up"
        if(wrappedNode.getOperator() == UnaryExpr.Operator.LOGICAL_COMPLEMENT) {
            Context innerContext = JavaParserFactory.getContext(wrappedNode.getExpression(), typeSolver);

            if(!this.equals(innerContext)) {
                // Note that `UnaryExpr.Operator.LOGICAL_COMPLEMENT` is `!`
                // Previously available pattern expressions are now negated (double negatives) -- e.g. if(!("a" is string s)) {}
                results.addAll(innerContext.patternExprsExposedFromChildren());
            }
        }

        return results;
    }

}
