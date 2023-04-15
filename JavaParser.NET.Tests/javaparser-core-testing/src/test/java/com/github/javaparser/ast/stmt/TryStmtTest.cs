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



class TryStmtTest {
    [TestMethod]
    void simpleTest() {
        TryStmt tryStmt = parse9("try(Reader x = new FileReader()){}");
        assertInstanceOf(VariableDeclarationExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void multipleTest() {
        TryStmt tryStmt = parse9("try(Reader x = new FileReader(); Reader x = new FileReader()){}");
        assertInstanceOf(VariableDeclarationExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void modifiersTest() {
        TryStmt tryStmt = parse9("try(/*final*/@A Reader x = new FileReader()){}");
        assertInstanceOf(VariableDeclarationExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void simpleVariable() {
        TryStmt tryStmt = parse9("try(a){}");
        assertInstanceOf(NameExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void twoSimpleVariables() {
        TryStmt tryStmt = parse9("try(a;b){}");
        assertInstanceOf(NameExpr.class, tryStmt.getResources().get(0));
        assertInstanceOf(NameExpr.class, tryStmt.getResources().get(1));
    }

    [TestMethod]
    void complexVariable() {
        TryStmt tryStmt = parse9("try(a.b.c){}");
        assertInstanceOf(FieldAccessExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void superAccess() {
        TryStmt tryStmt = parse9("try(super.a){}");
        assertInstanceOf(FieldAccessExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void outerClassAccess() {
        TryStmt tryStmt = parse9("try(X.this.a){}");
        assertInstanceOf(FieldAccessExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void varTestJava10() {
        TryStmt tryStmt = parse10("try(var x = new FileReader()){}");
        assertInstanceOf(VariableDeclarationExpr.class, tryStmt.getResources().get(0));
    }

    [TestMethod]
    void varTestJava11() {
        TryStmt tryStmt = parse11("try(var x = new FileReader()){}");
        assertInstanceOf(VariableDeclarationExpr.class, tryStmt.getResources().get(0));
    }

    private <T> T parse9(string code) {
        JavaParser parser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_9));
        ParseResult<Statement> result = parser.parse(ParseStart.STATEMENT, provider(code));
        assertTrue(result.isSuccessful(), result.toString());
        return (T) result.getResult().get();
    }

    private <T> T parse10(string code) {
        JavaParser parser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_10));
        ParseResult<Statement> result = parser.parse(ParseStart.STATEMENT, provider(code));
        assertTrue(result.isSuccessful(), result.toString());
        return (T) result.getResult().get();
    }

    private <T> T parse11(string code) {
        JavaParser parser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_11));
        ParseResult<Statement> result = parser.parse(ParseStart.STATEMENT, provider(code));
        assertTrue(result.isSuccessful(), result.toString());
        return (T) result.getResult().get();
    }
}
