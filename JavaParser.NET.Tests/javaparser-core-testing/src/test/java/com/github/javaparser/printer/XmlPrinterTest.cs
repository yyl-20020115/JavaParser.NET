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

namespace com.github.javaparser.printer;



class XmlPrinterTest {
    [TestMethod]
    void testWithType() {
        Expression expression = parseExpression("1+1");
        XmlPrinter xmlOutput = new XmlPrinter(true);

        string output = xmlOutput.output(expression);

        assertEquals("<root type='BinaryExpr' operator='PLUS'><left type='IntegerLiteralExpr' value='1'></left><right type='IntegerLiteralExpr' value='1'></right></root>", output);
    }

    [TestMethod]
    void testWithoutType() {
        Expression expression = parseExpression("1+1");

        XmlPrinter xmlOutput = new XmlPrinter(false);

        string output = xmlOutput.output(expression);

        assertEquals("<root operator='PLUS'><left value='1'></left><right value='1'></right></root>", output);
    }

    [TestMethod]
    void testList() {
        Expression expression = parseExpression("a(1,2)");

        XmlPrinter xmlOutput = new XmlPrinter(true);

        string output = xmlOutput.output(expression);

        assertEquals("<root type='MethodCallExpr'><name type='SimpleName' identifier='a'></name><arguments><argument type='IntegerLiteralExpr' value='1'></argument><argument type='IntegerLiteralExpr' value='2'></argument></arguments></root>", output);
    }
}
