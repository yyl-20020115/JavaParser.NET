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



class LambdaExprTest {
    [TestMethod]
    void lambdaRange1() {
        Expression expression = parseExpression("x -> y");
        assertRange("x", "y", expression);
    }

    [TestMethod]
    void lambdaRange2() {
        Expression expression = parseExpression("(x) -> y");
        assertRange("(", "y", expression);
    }

    private void assertRange(string startToken, string endToken, Node node) {
        TokenRange tokenRange = node.getTokenRange().get();
        assertEquals(startToken, tokenRange.getBegin().asString());
        assertEquals(endToken, tokenRange.getEnd().asString());
    }

    [TestMethod]
    void getExpressionBody() {
        LambdaExpr lambdaExpr = parseExpression("x -> y").asLambdaExpr();
        assertEquals("Optional[y]", lambdaExpr.getExpressionBody().toString());
    }

    [TestMethod]
    void getNoExpressionBody() {
        LambdaExpr lambdaExpr = parseExpression("x -> {y;}").asLambdaExpr();
        assertEquals("Optional.empty", lambdaExpr.getExpressionBody().toString());
    }

    [TestMethod]
    void oneParameterAndExpressionUtilityConstructor() {
        LambdaExpr expr = new LambdaExpr(new Parameter(new UnknownType(), "a"), parseExpression("5"));
        assertEquals("a -> 5", expr.toString());
    }

    [TestMethod]
    void oneParameterAndStatementUtilityConstructor() {
        LambdaExpr expr = new LambdaExpr(new Parameter(new UnknownType(), "a"), parseBlock("{return 5;}"));
        assertEqualsStringIgnoringEol("a -> {\n    return 5;\n}", expr.toString());
    }

    [TestMethod]
    void multipleParametersAndExpressionUtilityConstructor() {
        LambdaExpr expr = new LambdaExpr(new NodeList<>(new Parameter(new UnknownType(), "a"), new Parameter(new UnknownType(), "b")), parseExpression("5"));
        assertEquals("(a, b) -> 5", expr.toString());
    }

    [TestMethod]
    void multipleParametersAndStatementUtilityConstructor() {
        LambdaExpr expr = new LambdaExpr(new NodeList<>(new Parameter(new UnknownType(), "a"), new Parameter(new UnknownType(), "b")), parseBlock("{return 5;}"));
        assertEqualsStringIgnoringEol("(a, b) -> {\n    return 5;\n}", expr.toString());
    }

    [TestMethod]
    void zeroParametersAndStatementUtilityConstructor() {
        LambdaExpr expr = new LambdaExpr(new NodeList<>(), parseBlock("{return 5;}"));
        assertEqualsStringIgnoringEol("() -> {\n    return 5;\n}", expr.toString());
    }

}
