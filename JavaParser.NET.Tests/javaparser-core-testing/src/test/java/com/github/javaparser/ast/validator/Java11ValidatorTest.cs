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

namespace com.github.javaparser.ast.validator;



class Java11ValidatorTest {
    public static /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_11));

    [TestMethod]
    void varAllowedInLocalVariableDeclaration() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("x((var x, var y) -> x+y);"));
        assertNoProblems(result);
    }

    [TestMethod]
    void switchExpressionNotAllowed() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("int a = switch(x){};"));
        assertProblems(result, "(line 1,col 9) Switch expressions are not supported.");
    }

    [TestMethod]
    void multiLabelCaseNotAllowed() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("switch(x){case 3,4,5: ;}"));
        assertProblems(result, "(line 1,col 11) Only one label allowed _in a switch-case.");
    }
}
