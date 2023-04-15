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
 * Transforming ConstructorDeclaration and verifying the LexicalPreservation works as expected.
 */
class ConstructorDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected ConstructorDeclaration consider(string code) {
        considerCode("class A { " + code + " }");
        return cu.getType(0).getMembers().get(0).asConstructorDeclaration();
    }

    // Name

    [TestMethod]
    void settingName() {
        ConstructorDeclaration cd = consider("A(){}");
        cd.setName("B");
        assertTransformedToString("B(){}", cd);
    }

    // JavaDoc

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        ConstructorDeclaration cd = consider("A(){}");
        cd.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public A(){}", cd);
    }

    [TestMethod]
    void removingModifiers() {
        ConstructorDeclaration cd = consider("public A(){}");
        cd.setModifiers(new NodeList<>());
        assertTransformedToString("A(){}", cd);
    }

    [TestMethod]
    void replacingModifiers() {
        ConstructorDeclaration cd = consider("public A(){}");
        cd.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected A(){}", cd);
    }

    // Parameters

    [TestMethod]
    void addingParameters() {
        ConstructorDeclaration cd = consider("A(){}");
        cd.addParameter(PrimitiveType.doubleType(), "d");
        assertTransformedToString("A(double d){}", cd);
    }

    [TestMethod]
    void removingOnlyParameter() {
        ConstructorDeclaration cd = consider("public A(double d){}");
        cd.getParameters().remove(0);
        assertTransformedToString("public A(){}", cd);
    }

    [TestMethod]
    void removingFirstParameterOfMany() {
        ConstructorDeclaration cd = consider("public A(double d, float f){}");
        cd.getParameters().remove(0);
        assertTransformedToString("public A(float f){}", cd);
    }

    [TestMethod]
    void removingLastParameterOfMany() {
        ConstructorDeclaration cd = consider("public A(double d, float f){}");
        cd.getParameters().remove(1);
        assertTransformedToString("public A(double d){}", cd);
    }

    [TestMethod]
    void replacingOnlyParameter() {
        ConstructorDeclaration cd = consider("public A(float f){}");
        cd.getParameters().set(0, new Parameter(new ArrayType(PrimitiveType.intType()), new SimpleName("foo")));
        assertTransformedToString("public A(int[] foo){}", cd);
    }

    // ThrownExceptions

    // Body

    // Annotations
}
