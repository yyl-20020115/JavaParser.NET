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



class ConcreteSyntaxModelTest {

    private string print(Node node) {
        return ConcreteSyntaxModel.genericPrettyPrint(node);
    }

    [TestMethod]
    void printSimpleClassExpr() {
        ClassExpr expr = parseExpression("Foo.class");
        assertEquals("Foo.class", print(expr));
    }

    [TestMethod]
    void printArrayClassExpr() {
        ClassExpr expr = parseExpression("Foo[].class");
        assertEquals("Foo[].class", print(expr));
    }

    [TestMethod]
    void printGenericClassExpr() {
        ClassExpr expr = parseExpression("Foo<String>.class");
        assertEquals("Foo<String>.class", print(expr));
    }

    [TestMethod]
    void printSimplestClass() {
        Node node = parse("class A {}");
        assertEquals("class A {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }

    [TestMethod]
    void printAClassWithField() {
        Node node = parse("class A { int a; }");
        assertEquals("class A {" + SYSTEM_EOL
                + SYSTEM_EOL +
                "    int a;" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }

    [TestMethod]
    void printParameters() {
        Node node = parseBodyDeclaration("int x(int y, int z) {}");
        assertEquals("int x(int y, int z) {" + SYSTEM_EOL + "}", print(node));
    }

    [TestMethod]
    void printReceiverParameter() {
        Node node = parseBodyDeclaration("int x(X A.B.this, int y, int z) {}");
        assertEquals("int x(X A.B.this, int y, int z) {" + SYSTEM_EOL + "}", print(node));
    }

    [TestMethod]
    void printAnEmptyInterface() {
        Node node = parse("interface A {}");
        assertEquals("interface A {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }

    [TestMethod]
    void printAnEmptyInterfaceWithModifier() {
        Node node = parse("public interface A {}");
        assertEquals("public interface A {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }
}
