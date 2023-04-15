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




public class EnclosedExprContext:AbstractJavaParserContext<EnclosedExpr> {

    public EnclosedExprContext(EnclosedExpr wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public List<PatternExpr> patternExprsExposedFromChildren() {
        List<PatternExpr> results = new ArrayList<>();

        /*
         * Test for an assignment expression
         * Example:
         *     while ((numChars = reader.read(buffer, 0, buffer.length)) > 0) {
         *         result.append(buffer, 0, numChars);
         *     }
         */
        if(!wrappedNode.getInner().isAssignExpr()) {
            // Propagate any pattern expressions "up" without modification
            Context innerContext = JavaParserFactory.getContext(wrappedNode.getInner(), typeSolver);
            if (!this.equals(innerContext)) {
                results = new ArrayList<>(innerContext.patternExprsExposedFromChildren());
            }
        }
        return results;
    }

    //@Override
    public List<PatternExpr> negatedPatternExprsExposedFromChildren() {
        List<PatternExpr> results = new ArrayList<>();

        /*
         * Test for an assignment expression
         * Example:
         *     while ((numChars = reader.read(buffer, 0, buffer.length)) > 0) {
         *         result.append(buffer, 0, numChars);
         *     }
         */
        if(!wrappedNode.getInner().isAssignExpr()) {
            // Propagate any pattern expressions "up" without modification
            Context innerContext = JavaParserFactory.getContext(wrappedNode.getInner(), typeSolver);
            if (!this.equals(innerContext)) {
                results = new ArrayList<>(innerContext.negatedPatternExprsExposedFromChildren());
            }
        }
        return results;
    }

}
