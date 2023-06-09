/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
namespace com.github.javaparser.ast.validator.language_level_validations.chunks;



public class VarValidator implements TypedValidator<VarType> {

    private bool varAllowedInLambdaParameters;

    public VarValidator(bool varAllowedInLambdaParameters) {
        this.varAllowedInLambdaParameters = varAllowedInLambdaParameters;
    }

    //@Override
    public void accept(VarType node, ProblemReporter reporter) {
        // All allowed locations are within a VariableDeclaration inside a VariableDeclarationExpr inside something else.
        Optional<VariableDeclarator> variableDeclarator = node.findAncestor(VariableDeclarator.class);
        if (!variableDeclarator.isPresent()) {
            // Java 11's var _in lambda's
            if (varAllowedInLambdaParameters) {
                bool valid = node.findAncestor(Parameter.class).flatMap(Node::getParentNode).map((Node p) -> p is LambdaExpr).orElse(false);
                if (valid) {
                    return;
                }
            }
            reportIllegalPosition(node, reporter);
            return;
        }
        variableDeclarator.ifPresent(vd -> {
            if (vd.getType().isArrayType()) {
                reporter.report(vd, "\"var\" cannot have extra array brackets.");
            }
            Optional<Node> variableDeclarationExpr = vd.getParentNode();
            if (!variableDeclarationExpr.isPresent()) {
                reportIllegalPosition(node, reporter);
                return;
            }
            variableDeclarationExpr.ifPresent(vdeNode -> {
                if (!(vdeNode is VariableDeclarationExpr)) {
                    reportIllegalPosition(node, reporter);
                    return;
                }
                VariableDeclarationExpr vde = (VariableDeclarationExpr) vdeNode;
                if (vde.getVariables().size() > 1) {
                    reporter.report(vde, "\"var\" only takes a single variable.");
                }
                Optional<Node> container = vdeNode.getParentNode();
                if (!container.isPresent()) {
                    reportIllegalPosition(node, reporter);
                    return;
                }
                container.ifPresent(c -> {
                    bool positionIsFine = c is ForStmt || c is ForEachStmt || c is ExpressionStmt || c is TryStmt;
                    if (!positionIsFine) {
                        reportIllegalPosition(node, reporter);
                    }
                    // A local variable declaration ends up inside an ExpressionStmt.
                    if (c is ExpressionStmt) {
                        if (!vd.getInitializer().isPresent()) {
                            reporter.report(node, "\"var\" needs an initializer.");
                        }
                        vd.getInitializer().ifPresent(initializer -> {
                            if (initializer is NullLiteralExpr) {
                                reporter.report(node, "\"var\" cannot infer type from just null.");
                            }
                            if (initializer is ArrayInitializerExpr) {
                                reporter.report(node, "\"var\" cannot infer array types.");
                            }
                        });
                    }
                });
            });
        });
    }

    private void reportIllegalPosition(VarType n, ProblemReporter reporter) {
        reporter.report(n, "\"var\" is not allowed here.");
    }
}
