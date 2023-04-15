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

namespace com.github.javaparser.ast.expr;



class ArrayCreationExprTest {

    [TestMethod]
    void correctlyCreatesExpressionWithDefaultConstructor() {
        ArrayCreationExpr expr = new ArrayCreationExpr();

        assertEquals("new empty[] {}", expr.toString());
    }

    [TestMethod]
    void correctlyCreatesExpressionWithSimpleConstructor() {
        ArrayCreationExpr expr = new ArrayCreationExpr(PrimitiveType.byteType());

        assertEquals("new byte[] {}", expr.toString());
    }

    [TestMethod]
    void correctlyCreatesExpressionWithFullConstructor() {
        ArrayInitializerExpr initializer = new ArrayInitializerExpr(new NodeList<>(
                new IntegerLiteralExpr("1"),
                new IntegerLiteralExpr("2"),
                new IntegerLiteralExpr("3")
        ));
        ArrayCreationExpr expr = new ArrayCreationExpr(PrimitiveType.intType(), new NodeList<>(new ArrayCreationLevel()), initializer);

        assertEquals("new int[] { 1, 2, 3 }", expr.toString());
    }
}