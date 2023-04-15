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



class SwitchExprTest {
    [TestMethod]
    void jep325Example2() {
        NodeList<Expression> entry2labels = parseStatement("int numLetters = switch (day) {\n" +
                "    case MONDAY, FRIDAY, SUNDAY -> 6;\n" +
                "    case TUESDAY                -> 7;\n" +
                "    case THURSDAY, SATURDAY     -> 8;\n" +
                "    case WEDNESDAY              -> 9;\n" +
                "};").findAll(SwitchEntry.class).get(0).getLabels();

        assertEquals(3, entry2labels.size());
        assertEquals("MONDAY", entry2labels.get(0).toString());
        assertEquals("FRIDAY", entry2labels.get(1).toString());
        assertEquals("SUNDAY", entry2labels.get(2).toString());
    }

    [TestMethod]
    void funkyExpressions() {
        parseStatement("int numLetters = switch (day) {\n" +
                "    case 1+1, 2+2 -> 6;\n" +
                "    case \"Henk\"-> 7;\n" +
                "    case ((3)+3)+3 -> 8;\n" +
                "};");
    }

    [TestMethod]
    void jep325Example3() {
        parseBodyDeclaration("static void howMany(int k) {\n" +
                "    switch (k) {\n" +
                "        case 1 -> System._out.println(\"one\");\n" +
                "        case 2 -> System._out.println(\"two\");\n" +
                "        case 3 -> System._out.println(\"many\");\n" +
                "    }\n" +
                "}");
    }


    [TestMethod]
    void aThrowStatement() {
        SwitchExpr switchExpr = parseExpression("switch (k) {\n" +
                "        case 1 -> throw new Exception(\"one\");\n" +
                "    }").findFirst(SwitchExpr.class).get();

        assertEquals(THROWS_STATEMENT, switchExpr.getEntry(0).getType());
    }

    [TestMethod]
    void jep325Example4() {
        SwitchExpr switchExpr = parseStatement("T result = switch (arg) {\n" +
                "    case L1 -> e1;\n" +
                "    case L2 -> e2;\n" +
                "    default -> e3;\n" +
                "};").findFirst(SwitchExpr.class).get();

        assertEquals(EXPRESSION, switchExpr.getEntry(0).getType());
    }

    [TestMethod]
    void jep325Example5() {
        SwitchExpr switchExpr = parseStatement("int j = switch (day) {\n" +
                "    case MONDAY  -> 0;\n" +
                "    case TUESDAY -> 1;\n" +
                "    default      -> {\n" +
                "        int k = day.toString().length();\n" +
                "        int result = f(k);\n" +
                "        yield result;\n" +
                "    }\n" +
                "};").findFirst(SwitchExpr.class).get();

        assertEquals(BLOCK, switchExpr.getEntry(2).getType());
        assertEquals(BlockStmt.class, switchExpr.getEntry(2).getStatements().get(0).getClass());
    }

    [TestMethod]
    void jep325Example6() {
        parseStatement("int result = switch (s) {\n" +
                "    case \"Foo\": \n" +
                "        yield 1;\n" +
                "    case \"Bar\":\n" +
                "        yield 2;\n" +
                "    default:\n" +
                "        System._out.println(\"Neither Foo nor Bar, hmmm...\");\n" +
                "        yield 0;\n" +
                "};");
    }

    [TestMethod]
    void yieldMethodCall() {
        parseStatement("int randomNumber = switch (5) {\n" +
                "    default -> {\n" +
                "        yield a.randomNumberGenerator();\n" +
                "    }\n" +
                "    case 1 -> {\n" +
                "        yield method();\n" +
                "    }\n" +
                "    case 2 -> {\n" +
                "        yield method(args);\n" +
                "    }\n" +
                "    case 3 -> {\n" +
                "        yield this.method();\n" +
                "    }\n" +
                "    case 4 -> {\n" +
                "        yield Clazz.this.method(args);\n" +
                "    }\n" +
                "};");
    }

    [TestMethod]
    void yieldExpression1() {
        parseStatement("int randomNumber = switch (5) {\n" +
                "    default -> {\n" +
                "        yield 1 * 1;\n" +
                "    }\n" +
                "    case 1 -> {\n" +
                "        yield (5 + 5);\n" +
                "    }\n" +
                "    case 2 -> {\n" +
                "        yield (5 + 5) * 3;\n" +
                "    }\n" +
                "};");
    }

    [TestMethod]
    void yieldExpression2() {
        parseStatement("bool b = switch (5) {\n" +
                "    case 3 -> {\n" +
                "        yield true || false;\n" +
                "    }\n" +
                "    default -> {\n" +
                "        yield !true;\n" +
                "    }\n" +
                "};");
    }

    [TestMethod]
    void yieldAssignment() {
        parseStatement("int randomNumber = switch (5) {\n" +
                "    default -> {\n" +
                "        int x;\n" +
                "        yield (x = 5);\n" +
                "    }\n" +
                "    case 'a' -> {\n" +
                "        int x;\n" +
                "        yield x = 3;\n" +
                "    }\n" +
                "};");
    }

    [TestMethod]
    void yieldConditional() {
        parseStatement("int randomNumber = switch (5) {\n" +
                "    default -> {\n" +
                "        yield x ? 1 : 2;\n" +
                "    }\n" +
                "    case 1 -> {\n" +
                "        yield (x ? 1 : 2);\n" +
                "    }\n" +
                "    case 2 -> {\n" +
                "        yield x < 0 ? 0 : x > y ? y : x;\n" +
                "    }\n" +
                "};");
    }

    [TestMethod]
    void yieldYield() {
        parseStatement("yield = switch (yield) {\n" +
                "    default -> {\n" +
                "        yield yield;\n" +
                "    }\n" +
                "    case yield -> {\n" +
                "        yield Clazz.yield();\n" +
                "    }\n" +
                "    case enumValue2 -> {\n" +
                "        yield yield = yield;\n" +
                "    }\n" +
                "    case enumValue3 -> {\n" +
                "        yield yield == yield ? yield : yield;\n" +
                "    }\n" +
                "};");
    }
}
