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

namespace com.github.javaparser.printer.lexicalpreservation.transformations.ast.body;



/**
 * Transforming Statement and verifying the LexicalPreservation works as expected.
 */
class StatementTransformationsTest:AbstractLexicalPreservingTest {

    Statement consider(string code) {
        Statement statement = parseStatement(code);
        LexicalPreservingPrinter.setup(statement);
        return statement;
    }

    [TestMethod]
    void ifStmtTransformation() {
        Statement stmt = consider("if (a) {} else {}");
        stmt.asIfStmt().setCondition(new NameExpr("b"));
        assertTransformedToString("if (b) {} else {}", stmt);
    }

    [TestMethod]
    void switchEntryCsmHasTrailingUnindent() {
        Statement stmt = consider("switch (a) { case 1: a; a; }");
        NodeList<Statement> statements = stmt.asSwitchStmt().getEntry(0).getStatements();
        statements.set(1, statements.get(1).clone()); // clone() to force replacement
        assertTransformedToString("switch (a) { case 1: a; a; }", stmt);
    }

}
