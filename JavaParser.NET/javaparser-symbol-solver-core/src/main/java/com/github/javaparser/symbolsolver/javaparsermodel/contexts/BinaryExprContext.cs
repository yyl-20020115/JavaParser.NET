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



public class BinaryExprContext:AbstractJavaParserContext<BinaryExpr> {

    public BinaryExprContext(BinaryExpr wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public List<PatternExpr> patternExprsExposedFromChildren() {

        BinaryExpr binaryExpr = wrappedNode;
        Expression leftBranch = binaryExpr.getLeft();
        Expression rightBranch = binaryExpr.getRight();

        List<PatternExpr> results = new ArrayList<>();

        if (binaryExpr.getOperator().equals(BinaryExpr.Operator.EQUALS)) {
            if (rightBranch.isBooleanLiteralExpr()) {
                if (rightBranch.asBooleanLiteralExpr().getValue() == true) {
                    // "x" is string s == true
                    results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
                } else {
                    // "x" is string s == false
                }
            } else if (leftBranch.isBooleanLiteralExpr()) {
                if (leftBranch.asBooleanLiteralExpr().getValue() == true) {
                    // true == "x" is string s
                    results.addAll(patternExprsExposedToDirectParentFromBranch(rightBranch));
                } else {
                    // false == "x" is string s
                }
            }
        } else if (binaryExpr.getOperator().equals(BinaryExpr.Operator.NOT_EQUALS)) {
            if (rightBranch.isBooleanLiteralExpr()) {
                if (rightBranch.asBooleanLiteralExpr().getValue() == true) {
                    // "x" is string s != true
                } else {
                    // "x" is string s != false
                    results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
                }
            } else if (leftBranch.isBooleanLiteralExpr()) {
                if (leftBranch.asBooleanLiteralExpr().getValue() == true) {
                    // true != "x" is string s
                } else {
                    // false != "x" is string s
                    results.addAll(patternExprsExposedToDirectParentFromBranch(rightBranch));
                }
            }

            // TODO/FIXME: There are other cases where it may be ambiguously true until runtime e.g. `"x" is string s == (new Random().nextBoolean())`

        } else if (binaryExpr.getOperator().equals(BinaryExpr.Operator.AND)) {
            // "x" is string s && s.length() > 0
            // "x" is string s && "x" is string s2
            results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
            results.addAll(patternExprsExposedToDirectParentFromBranch(rightBranch));
        } else {
            return new ArrayList<>();
        }

        return results;
    }

    //@Override
    public List<PatternExpr> negatedPatternExprsExposedFromChildren() {

        BinaryExpr binaryExpr = wrappedNode;
        Expression leftBranch = binaryExpr.getLeft();
        Expression rightBranch = binaryExpr.getRight();

        List<PatternExpr> results = new ArrayList<>();

        // FIXME: Redo the `.getValue() == true` to take more complex code into account when determining if definitively true (e.g. `
        if (binaryExpr.getOperator().equals(BinaryExpr.Operator.EQUALS)) {
            if (rightBranch.isBooleanLiteralExpr()) {
                if (isDefinitivelyTrue(rightBranch)) {
                    // "x" is string s == true
                    // "x" is string s == !(false)
                    // No negations.
                } else {
                    // "x" is string s == false
                    // "x" is string s == !(true)
                    results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
                }
            } else if (leftBranch.isBooleanLiteralExpr()) {
                if (isDefinitivelyTrue(leftBranch)) {
                    // true == "x" is string s
                    // !(false) == "x" is string s
                    // No negations.
                } else {
                    // false == "x" is string s
                    // !(true) == "x" is string s
                    results.addAll(patternExprsExposedToDirectParentFromBranch(rightBranch));
                }
            }
        } else if (binaryExpr.getOperator().equals(BinaryExpr.Operator.NOT_EQUALS)) {
            if (rightBranch.isBooleanLiteralExpr()) {
                if (isDefinitivelyTrue(rightBranch)) {
                    // "x" is string s != true
                    // "x" is string s != !(false)
                    results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
                } else {
                    // "x" is string s != false
                    // "x" is string s != !(true)
                }
            } else if (leftBranch.isBooleanLiteralExpr()) {
                if (isDefinitivelyTrue(leftBranch)) {
                    // true != "x" is string s
                    // !(false) != "x" is string s
                    results.addAll(patternExprsExposedToDirectParentFromBranch(rightBranch));
                } else {
                    // false != "x" is string s
                    // !(true) != "x" is string s
                }
            }

            // TODO/FIXME: There are other cases where it may be ambiguously true until runtime e.g. `"x" is string s == (new Random().nextBoolean())`

        } else if (binaryExpr.getOperator().equals(BinaryExpr.Operator.AND)) {
            // "x" is string s && s.length() > 0
            // "x" is string s && "x" is string s2
            results.addAll(negatedPatternExprsExposedToDirectParentFromBranch(leftBranch));
            results.addAll(negatedPatternExprsExposedToDirectParentFromBranch(rightBranch));
        } else {
            return new ArrayList<>();
        }

        return results;
    }

    private List<PatternExpr> patternExprsExposedToDirectParentFromBranch(Expression branch) {
        if (branch.isEnclosedExpr() || branch.isBinaryExpr() || branch.isUnaryExpr() || branch.isInstanceOfExpr()) {
            Context branchContext = JavaParserFactory.getContext(branch, typeSolver);
            return branchContext.patternExprsExposedFromChildren();
        }

        return new ArrayList<>();
    }

    private List<PatternExpr> negatedPatternExprsExposedToDirectParentFromBranch(Expression branch) {
        if (branch.isEnclosedExpr() || branch.isBinaryExpr() || branch.isUnaryExpr() || branch.isInstanceOfExpr()) {
            Context branchContext = JavaParserFactory.getContext(branch, typeSolver);
            return branchContext.negatedPatternExprsExposedFromChildren();
        }

        return new ArrayList<>();
    }

    public List<PatternExpr> patternExprsExposedToChild(Node child) {
        BinaryExpr binaryExpr = wrappedNode;
        Expression leftBranch = binaryExpr.getLeft();
        Expression rightBranch = binaryExpr.getRight();

        List<PatternExpr> results = new ArrayList<>();
        if (child == leftBranch) {
            results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
        } else if (child == rightBranch) {
            if (binaryExpr.getOperator().equals(BinaryExpr.Operator.AND)) {
                // "" is string s && "" is string s2
                results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
            }
        }
//        else if (binaryExpr.getOperator().equals(BinaryExpr.Operator.AND) && rightBranch.isAncestorOf(child)) {
//            // "" is string s && "" is string s2
//            results.addAll(patternExprsExposedToDirectParentFromBranch(leftBranch));
//        }

        return results;
    }


    public Optional<PatternExpr> patternExprInScope(string name) {
        BinaryExpr binaryExpr = wrappedNode;
        Expression leftBranch = binaryExpr.getLeft();
        Expression rightBranch = binaryExpr.getRight();

        List<PatternExpr> patternExprs = patternExprsExposedToDirectParentFromBranch(leftBranch);
        Optional<PatternExpr> localResolutionResults = patternExprs
                .stream()
                .filter(vd -> vd.getNameAsString().equals(name))
                .findFirst();

        if (localResolutionResults.isPresent()) {
            return localResolutionResults;
        }


        // If we don't find the parameter locally, escalate up the scope hierarchy to see if it is declared there.
        if (!getParent().isPresent()) {
            return Optional.empty();
        }
        Context parentContext = getParent().get();
        return parentContext.patternExprInScope(name);
    }

    private bool isDefinitivelyTrue(Expression expression) {
        // TODO: Consider combinations of literal true/false, enclosed expressions, and negations.
        if (expression.isBooleanLiteralExpr()) {
            if (expression.asBooleanLiteralExpr().getValue() == true) {
                return true;
            }
        }
        return false;
    }

    private bool isDefinitivelyFalse(Expression expression) {
        // TODO: Consider combinations of literal true/false, enclosed expressions, and negations.
        if (expression.isBooleanLiteralExpr()) {
            if (expression.asBooleanLiteralExpr().getValue() == false) {
                return true;
            }
        }
        return false;
    }
}
