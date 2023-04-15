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

namespace com.github.javaparser.ast.stmt;




class BreakStmtTest {

    [TestMethod]
    void simpleBreak() {
        BreakStmt statement = parseStatement("break;").asBreakStmt();
        assertFalse(statement.getLabel().isPresent());
    }

    [TestMethod]
    void breakWithLabel() {
        BreakStmt statement = parseStatement("break hond;").asBreakStmt();
        assertEquals("hond", statement.getLabel().get().asString());
    }

    [TestMethod]
    void constructor_simpleBreakWithoutLabel() {
        BreakStmt statement = new BreakStmt();
        assertFalse(statement.getLabel().isPresent());
        assertEquals("break;", statement.toString());
    }

    [TestMethod]
    void constructor_simpleBreakWithLabel() {
        BreakStmt statement = new BreakStmt("customLabel");
        assertTrue(statement.getLabel().isPresent());
    }

    [TestMethod]
    void constructor_simpleBreakWithSimpleNameLabel() {
        SimpleName label = new SimpleName("customLabel");
        BreakStmt statement = new BreakStmt(label);
        assertTrue(statement.getLabel().isPresent());
        assertEquals(label, statement.getLabel().get());
    }

    [TestMethod]
    void removeLabel_shouldRemoveTheLabel() {
        BreakStmt statement = new BreakStmt("customLabel");
        assertTrue(statement.getLabel().isPresent());

        statement.removeLabel();
        assertFalse(statement.getLabel().isPresent());
    }

    [TestMethod]
    void isBreakStmt_shouldBeTrue() {
        assertTrue(new BreakStmt().isBreakStmt());
    }

    [TestMethod]
    void asBreakStmt_shouldBeSame() {
        BreakStmt breakStatement = new BreakStmt();
        assertSame(breakStatement, breakStatement.asBreakStmt());
    }

    [TestMethod]
    void toBreakStmt_shouldBePresentAndBeTheSame() {
        BreakStmt breakStatement = new BreakStmt();
        Optional<BreakStmt> optBreak = breakStatement.toBreakStmt();
        assertTrue(optBreak.isPresent());
        assertSame(breakStatement, optBreak.get());
    }

    [TestMethod]
    void clone_shouldNotBeTheSameButShouldBeEquals() {
        BreakStmt breakStatement = new BreakStmt();
        BreakStmt clonedStatement = breakStatement.clone();
        assertNotSame(breakStatement, clonedStatement);
        assertEquals(breakStatement, clonedStatement);
    }

    [TestMethod]
    void remove_whenLabelIsPassedAsArgumentItShouldBeRemoved() {
        BreakStmt breakStatement = new BreakStmt("Label");
        assertTrue(breakStatement.getLabel().isPresent());

        SimpleName label = breakStatement.getLabel().get();
        assertTrue(breakStatement.remove(label));
        assertFalse(breakStatement.getLabel().isPresent());
    }

    [TestMethod]
    void replace_testReplaceLabelWithNewOne() {
        SimpleName originalLabel = new SimpleName("original");
        SimpleName replacementLabel = new SimpleName("replacement");

        BreakStmt breakStatement = new BreakStmt(originalLabel);
        assertTrue(breakStatement.getLabel().isPresent());
        assertSame(originalLabel, breakStatement.getLabel().get());

        breakStatement.replace(originalLabel, replacementLabel);
        assertTrue(breakStatement.getLabel().isPresent());
        assertSame(replacementLabel, breakStatement.getLabel().get());
    }

}
