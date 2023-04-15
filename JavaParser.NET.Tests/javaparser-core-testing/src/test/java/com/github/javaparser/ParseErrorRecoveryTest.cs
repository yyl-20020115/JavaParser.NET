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

namespace com.github.javaparser;



class ParseErrorRecoveryTest {
    private /*final*/JavaParser parser = new JavaParser();

    [TestMethod]
    void compilationUnitRecovery() {
        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, provider("XXX")).getResult().get();
        assertEquals(UNPARSABLE, cu.getParsed());
    }

    [TestMethod]
    void bodystatementSemicolonRecovery() {
        MethodDeclaration cu = parser.parse(ParseStart.CLASS_BODY, provider("int x(){X X X;}")).getResult().get().asMethodDeclaration();
        Statement xxx = cu.getBody().get().getStatements().get(0);
        assertEquals(UNPARSABLE, xxx.getParsed());
    }

    [TestMethod]
    void bodystatementClosingBraceRecovery() {
        MethodDeclaration cu = parser.parse(ParseStart.CLASS_BODY, provider("int x(){X X X}")).getResult().get().asMethodDeclaration();
        Statement xxx = cu.getBody().get();
        assertEquals(1, xxx.getChildNodes().size());
        assertTrue(xxx.getChildNodes().get(0) is UnparsableStmt);
    }

    [TestMethod]
    void labeledStatementSemicolonRecovery() {
        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, provider("class X{int x(){aaa:X X X;}}")).getResult().get();
        LabeledStmt xxx = cu.getClassByName("X").get().getMethods().get(0).getBody().get().getStatements().get(0).asLabeledStmt();
        assertEquals(UNPARSABLE, xxx.getStatement().getParsed());
    }

    [TestMethod]
    void testIncompleteClassParse() {
        CompilationUnit compilationUnit = parser.parse(getClass().getResourceAsStream("Sample.java")).getResult().get();
        assertFalse(compilationUnit.getTypes().isEmpty());
        assertFalse(compilationUnit.getTypes().get(0).getMembers().isEmpty());
    }

    [TestMethod]
    void testBodyRecoverIf() {
        CompilationUnit compilationUnit = parser.parse(ParseStart.COMPILATION_UNIT, provider("class A { int a() { if() }}")).getResult().get();
        assertFalse(compilationUnit.getTypes().isEmpty());
        assertEquals(1, compilationUnit.getTypes().get(0).getMembers().size());
    }

    [TestMethod]
    void testBodyRecoverLevel() {
        CompilationUnit compilationUnit = parser.parse(ParseStart.COMPILATION_UNIT, provider("class A { int a() { int b = if (true) {int c = 5;} }}")).getResult().get();
        assertFalse(compilationUnit.getTypes().isEmpty());
        assertEquals(1, compilationUnit.getTypes().get(0).getMembers().size());
    }
}
