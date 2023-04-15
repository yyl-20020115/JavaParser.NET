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
 * Transforming ClassOrInterfaceDeclaration and verifying the LexicalPreservation works as expected.
 */
class ClassOrInterfaceDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected ClassOrInterfaceDeclaration consider(string code) {
        considerCode(code);
        return cu.getType(0).asClassOrInterfaceDeclaration();
    }

    // Name

    [TestMethod]
    void settingName() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.setName("B");
        assertTransformedToString("class B {}", cid);
    }

    // isInterface

    [TestMethod]
    void classToInterface() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.setInterface(true);
        assertTransformedToString("interface A {}", cid);
    }

    [TestMethod]
    void interfaceToClass() {
        ClassOrInterfaceDeclaration cid = consider("interface A {}");
        cid.setInterface(false);
        assertTransformedToString("class A {}", cid);
    }

    // typeParameters

    [TestMethod]
    void addingTypeParameterWhenThereAreNone() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.addTypeParameter("T");
        assertTransformedToString("class A<T> {}", cid);
    }

    [TestMethod]
    void addingTypeParameterAsFirstWhenThereAreSome() {
        ClassOrInterfaceDeclaration cid = consider("class A<U> {}");
        cid.getTypeParameters().addFirst(new TypeParameter("T", new NodeList<>()));
        assertTransformedToString("class A<T, U> {}", cid);
    }

    [TestMethod]
    void addingTypeParameterAsLastWhenThereAreSome() {
        ClassOrInterfaceDeclaration cid = consider("class A<U> {}");
        cid.addTypeParameter("T");
        assertTransformedToString("class A<U, T> {}", cid);
    }

    // extendedTypes

    [TestMethod]
    void addingExtendedTypes() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.addExtendedType("Foo");
        assertTransformedToString("class A:Foo {}", cid);
    }

    [TestMethod]
    void removingExtendedTypes() {
        ClassOrInterfaceDeclaration cid = consider("public class A:Foo {}");
        cid.getExtendedTypes().remove(0);
        assertTransformedToString("public class A {}", cid);
    }

    [TestMethod]
    void replacingExtendedTypes() {
        ClassOrInterfaceDeclaration cid = consider("public class A:Foo {}");
        cid.getExtendedTypes().set(0, parseClassOrInterfaceType("Bar"));
        assertTransformedToString("public class A:Bar {}", cid);
    }

    // implementedTypes

    [TestMethod]
    void addingImplementedTypes() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.addImplementedType("Foo");
        assertTransformedToString("class A implements Foo {}", cid);
    }

    [TestMethod]
    void removingImplementedTypes() {
        ClassOrInterfaceDeclaration cid = consider("public class A implements Foo {}");
        cid.getImplementedTypes().remove(0);
        assertTransformedToString("public class A {}", cid);
    }

    [TestMethod]
    void replacingImplementedTypes() {
        ClassOrInterfaceDeclaration cid = consider("public class A implements Foo {}");
        cid.getImplementedTypes().set(0, parseClassOrInterfaceType("Bar"));
        assertTransformedToString("public class A implements Bar {}", cid);
    }

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public class A {}", cid);
    }

    [TestMethod]
    void removingModifiers() {
        ClassOrInterfaceDeclaration cid = consider("public class A {}");
        cid.setModifiers(new NodeList<>());
        assertTransformedToString("class A {}", cid);
    }

    [TestMethod]
    void replacingModifiers() {
        ClassOrInterfaceDeclaration cid = consider("public class A {}");
        cid.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected class A {}", cid);
    }

    // members

    [TestMethod]
    void addingField() {
        ClassOrInterfaceDeclaration cid = consider("class A {}");
        cid.addField("int", "foo");
        assertTransformedToString("class A {" + SYSTEM_EOL + "    int foo;" + SYSTEM_EOL + "}", cid);
    }

    [TestMethod]
    void removingField() {
        ClassOrInterfaceDeclaration cid = consider("public class A { int foo; }");
        cid.getMembers().remove(0);
        assertTransformedToString("public class A { }", cid);
    }

    [TestMethod]
    void replacingFieldWithAnotherField() {
        ClassOrInterfaceDeclaration cid = consider("public class A {float f;}");
        cid.getMembers().set(0, new FieldDeclaration(new NodeList<>(), new VariableDeclarator(PrimitiveType.intType(), "bar")));
        assertTransformedToString("public class A {int bar;}", cid);
    }

    // Annotations
    [TestMethod]
    void removingAnnotations() {
        ClassOrInterfaceDeclaration cid = consider(
                "@Value" + SYSTEM_EOL +
                "public class A {}");
        cid.getAnnotationByName("Value").get().remove();
        assertTransformedToString("public class A {}", cid);
    }

    [TestMethod]
    void removingAnnotationsWithSpaces() {
        ClassOrInterfaceDeclaration cid = consider(
                  "   @Value " + SYSTEM_EOL +
                        "public class A {}");
        cid.getAnnotationByName("Value").get().remove();
        assertTransformedToString("public class A {}", cid);
    }

    // Javadoc

}
