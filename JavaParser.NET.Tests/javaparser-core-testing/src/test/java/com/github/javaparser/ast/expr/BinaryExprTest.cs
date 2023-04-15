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

namespace com.github.javaparser.ast.expr;



class BinaryExprTest {

    [TestMethod]
    void convertOperator() {
        assertEquals(AssignExpr.Operator.PLUS, BinaryExpr.Operator.PLUS.toAssignOperator().get());
    }

    /**
     * Evaluation takes place left to right, with && taking precedence over ||
     *
     * true || false && false || false
     * true ||      (1)       || false
     * (        2           ) || false
     * (             3               )
     *
     * true || false && false || false
     * true ||    (false)     || false
     * (     true           ) || false
     * (           true              )
     */
    @Nested
    class LogicalOperatorPrecedence {

        [TestMethod]
        public void logicalAndOr() {
            Expression expression = StaticJavaParser.parseExpression("true || false && false || false");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "(true || (false && false)) || false";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }

        [TestMethod]
        public void logicalOrEvaluationLeftToRight() {
            Expression expression = StaticJavaParser.parseExpression("false || true || false || true || false || true");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "((((false || true) || false) || true) || false) || true";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }

        [TestMethod]
        public void logicalAndEvaluationLeftToRight() {
            Expression expression = StaticJavaParser.parseExpression("false && true && false && true && false && true");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "((((false && true) && false) && true) && false) && true";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }

        [TestMethod]
        public void andTakesPrecedenceOverOr() {
            Expression expression = StaticJavaParser.parseExpression("true || false && false");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "true || (false && false)";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }

        [TestMethod]
        public void andTakesPrecedenceOverOrThenLeftToRight() {
            Expression expression = StaticJavaParser.parseExpression("true || false && false || true");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "(true || (false && false)) || true";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }


        [TestMethod]
        public void example() {
            Expression expression = StaticJavaParser.parseExpression("year % 4 == 0 && year % 100 != 0 || year % 400 == 0");
            Expression bracketedExpression = applyBrackets(expression);

            string expected = "((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)";
            string actual = bracketedExpression.toString();

            assertEquals(expected, actual);
        }


    }


    private Expression applyBrackets(Expression expression) {
        expression.findAll(BinaryExpr.class)
                .stream()
                .filter(binaryExpr -> binaryExpr.getOperator() == BinaryExpr.Operator.AND || binaryExpr.getOperator() == BinaryExpr.Operator.OR)
                .forEach(binaryExpr -> {
                    if(!binaryExpr.getLeft().isBooleanLiteralExpr()) {
                        binaryExpr.setLeft(new EnclosedExpr(binaryExpr.getLeft()));
                    }
                    if(!binaryExpr.getRight().isBooleanLiteralExpr()) {
                        binaryExpr.setRight(new EnclosedExpr(binaryExpr.getRight()));
                    }
                });

        return expression;
    }
}
