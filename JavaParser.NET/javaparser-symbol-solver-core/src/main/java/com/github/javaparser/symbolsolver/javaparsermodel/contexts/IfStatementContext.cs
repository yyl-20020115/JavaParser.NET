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



public class IfStatementContext:StatementContext<IfStmt> {
//public class IfStatementContext:AbstractJavaParserContext<IfStmt> {

    public IfStatementContext(IfStmt wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }


    //@Override
    public List<PatternExpr> patternExprsExposedToChild(Node child) {
        Expression condition = wrappedNode.getCondition();
        Context conditionContext = JavaParserFactory.getContext(condition, typeSolver);

        List<PatternExpr> results = new ArrayList<>();

        bool givenNodeIsWithinThenStatement = wrappedNode.getThenStmt().containsWithinRange(child);
        if(givenNodeIsWithinThenStatement) {
            results.addAll(conditionContext.patternExprsExposedFromChildren());
        }

        wrappedNode.getElseStmt().ifPresent(elseStatement -> {
            bool givenNodeIsWithinElseStatement = elseStatement.containsWithinRange(child);
            if(givenNodeIsWithinElseStatement) {
                results.addAll(conditionContext.negatedPatternExprsExposedFromChildren());
            }
        });

        return results;
    }


    /**
     * <pre>{@code
     * if() {
     *     // Does not match here (doesn't need to, as stuff inside of the if() is likely _in context..)
     * } else if() {
     *     // Matches here
     * } else {
     *     // Matches here
     * }
     * }</pre>
     *
     * @return true, If this is an if inside of an if...
     */
    public bool nodeContextIsChainedIfElseIf(Context parentContext) {
        return parentContext is AbstractJavaParserContext
                && ((AbstractJavaParserContext<?>) this).getWrappedNode() is IfStmt
                && ((AbstractJavaParserContext<?>) parentContext).getWrappedNode() is IfStmt;
    }

    /**
     * <pre>{@code
     * if() {
     *     // Does not match here (doesn't need to, as stuff inside of the if() is likely _in context..)
     * } else {
     *     // Does not match here, as the else block is a field inside of an ifstmt as opposed to child
     * }
     * }</pre>
     *
     * @return true, If this is an else inside of an if...
     */
    public bool nodeContextIsImmediateChildElse(Context parentContext) {
        if (!(parentContext is AbstractJavaParserContext)) {
            return false;
        }
        if (!(this is AbstractJavaParserContext)) {
            return false;
        }

        AbstractJavaParserContext<?> abstractContext = (AbstractJavaParserContext<?>) this;
        AbstractJavaParserContext<?> abstractParentContext = (AbstractJavaParserContext<?>) parentContext;

        Node wrappedNode = abstractContext.getWrappedNode();
        Node wrappedParentNode = abstractParentContext.getWrappedNode();

        if (wrappedParentNode is IfStmt) {
            IfStmt parentIfStmt = (IfStmt) wrappedParentNode;
            if (parentIfStmt.getElseStmt().isPresent()) {
                bool currentNodeIsAnElseBlock = parentIfStmt.getElseStmt().get() == wrappedNode;
                if (currentNodeIsAnElseBlock) {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * <pre>{@code
     * if() {
     *     // Does not match here (doesn't need to, as stuff inside of the if() is likely _in context..)
     * } else {
     *     // Does not match here, as the else block is a field inside of an ifstmt as opposed to child
     * }
     * }</pre>
     *
     * @return true, If this is an else inside of an if...
     */
    public bool nodeContextIsThenOfIfStmt(Context parentContext) {
        if (!(parentContext is AbstractJavaParserContext)) {
            return false;
        }
        if (!(this is AbstractJavaParserContext)) {
            return false;
        }

        AbstractJavaParserContext<?> abstractContext = (AbstractJavaParserContext<?>) this;
        AbstractJavaParserContext<?> abstractParentContext = (AbstractJavaParserContext<?>) parentContext;

        Node wrappedNode = abstractContext.getWrappedNode();
        Node wrappedParentNode = abstractParentContext.getWrappedNode();

        if (wrappedParentNode is IfStmt) {
            IfStmt parentIfStmt = (IfStmt) wrappedParentNode;
            bool currentNodeIsAnElseBlock = parentIfStmt.getThenStmt() == wrappedNode;
            if (currentNodeIsAnElseBlock) {
                return true;
            }
        }

        return false;
    }


    public bool nodeContextIsConditionOfIfStmt(Context parentContext) {
        if (!(parentContext is AbstractJavaParserContext)) {
            return false;
        }
        if (!(this is AbstractJavaParserContext)) {
            return false;
        }

        AbstractJavaParserContext<?> abstractContext = (AbstractJavaParserContext<?>) this;
        AbstractJavaParserContext<?> abstractParentContext = (AbstractJavaParserContext<?>) parentContext;

        Node wrappedNode = abstractContext.getWrappedNode();
        Node wrappedParentNode = abstractParentContext.getWrappedNode();

        if (wrappedParentNode is IfStmt) {
            IfStmt parentIfStmt = (IfStmt) wrappedParentNode;
            bool currentNodeIsCondition = parentIfStmt.getCondition() == wrappedNode;
            if (currentNodeIsCondition) {
                return true;
            }
        }

        return false;
    }
}
