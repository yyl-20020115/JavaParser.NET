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



class YieldStmtTest {
    [TestMethod]
    void yield() {
        YieldStmt statement = parseStatement("yield 12*12;").asYieldStmt();
        assertEquals(BinaryExpr.class, statement.getExpression().getClass());
    }

    [TestMethod]
    void yield2() {
        YieldStmt statement;
        statement = parseStatement("yield (2 + 2);").asYieldStmt();
        assertEquals(EnclosedExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield ((2 + 2) * 3);").asYieldStmt();
        assertEquals(EnclosedExpr.class, statement.getExpression().getClass());
    }

    [TestMethod]
    void yieldMethodCall() {
        YieldStmt statement;
        statement = parseStatement("yield a();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield a(5, arg);").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield a.b();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield a.b(5, arg);").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield this.b();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield this.b(5, arg);").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield Clazz.this.b();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield Clazz.this.b(5, arg);").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
    }

    [TestMethod]
    void yieldAssignment() {
        YieldStmt statement = parseStatement("yield (x = 5);").asYieldStmt();
        assertEquals(EnclosedExpr.class, statement.getExpression().getClass());
    }

    [TestMethod]
    void yieldConditional() {
        YieldStmt statement = parseStatement("yield x ? 5 : 6;").asYieldStmt();
        assertEquals(ConditionalExpr.class, statement.getExpression().getClass());
    }

    [TestMethod]
    void threadYieldShouldNotBreak() {
        parseStatement("Thread.yield();").asExpressionStmt().getExpression().asMethodCallExpr();
    }

    [TestMethod]
    void keywordShouldNotInterfereWithIdentifiers() {
        CompilationUnit compilationUnit = parseCompilationUnit(JAVA_12, "class yield { yield yield(yield yield){yield();} }");
        assertEqualsStringIgnoringEol("class yield {\n" +
                "\n" +
                "    yield yield(yield yield) {\n" +
                "        yield();\n" +
                "    }\n" +
                "}\n", compilationUnit.toString());
    }

    [TestMethod]
    void keywordShouldNotInterfereWithIdentifiers2() {
        CompilationUnit compilationUnit = parseCompilationUnit("enum X { yield, }");
        assertEqualsStringIgnoringEol("enum X {\n" +
                "\n" +
                "    yield\n" +
                "}\n", compilationUnit.toString());
    }

    [TestMethod]
    void keywordShouldNotInterfereWithIdentifiers3() {
        YieldStmt statement;
        statement = parseStatement("yield yield;").asYieldStmt();
        assertEquals(NameExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield Clazz.yield();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
        statement = parseStatement("yield yield.yield();").asYieldStmt();
        assertEquals(MethodCallExpr.class, statement.getExpression().getClass());
    }
}
