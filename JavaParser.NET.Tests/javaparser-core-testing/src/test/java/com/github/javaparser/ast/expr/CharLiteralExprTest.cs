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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.ast.expr;



class CharLiteralExprTest {
    @Test
    void parseSimpleChar() {
        CharLiteralExpr c = parseExpression("'a'");
        assertEquals("a", c.getValue());
    }

    @Test
    void parseSimpleEscape() {
        CharLiteralExpr c = parseExpression("'\\t'");
        assertEquals("\\t", c.getValue());
    }

    @Test
    void parseUnicode() {
        CharLiteralExpr c = parseExpression("'Ω'");
        assertEquals("Ω", c.getValue());
    }

    @Test
    void parseNumericEscape() {
        CharLiteralExpr c = parseExpression("'\\177'");
        assertEquals("\\177", c.getValue());
    }

    @Test
    void parseUnicodeEscape() {
        CharLiteralExpr c = parseExpression("'\\u03a9'");
        assertEquals("\\u03a9", c.getValue());
    }

    @Test
    void parseUnicodeEscapedEscape() {
        JavaParser javaParser = new JavaParser(new ParserConfiguration()
                .setPreprocessUnicodeEscapes(true));

        CharLiteralExpr c = javaParser.parse(ParseStart.EXPRESSION, provider("'\\u005c''")).getResult().get().asCharLiteralExpr();
        assertEquals("\\'", c.getValue());
    }
}
