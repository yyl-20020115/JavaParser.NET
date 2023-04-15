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
 * Transforming AnnotationMemberDeclaration and verifying the LexicalPreservation works as expected.
 */
class AnnotationMemberDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected AnnotationMemberDeclaration consider(string code) {
        considerCode("@interface AD { " + code + " }");
        return cu.getAnnotationDeclarationByName("AD").get().getMember(0).asAnnotationMemberDeclaration();
    }

    // Name

    [TestMethod]
    void changingName() {
        AnnotationMemberDeclaration md = consider("int foo();");
        md.setName("bar");
        assertTransformedToString("int bar();", md);
    }

    // Type

    [TestMethod]
    void changingType() {
        AnnotationMemberDeclaration md = consider("int foo();");
        md.setType("String");
        assertTransformedToString("string foo();", md);
    }

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        AnnotationMemberDeclaration md = consider("int foo();");
        md.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public int foo();", md);
    }

    [TestMethod]
    void removingModifiers() {
        AnnotationMemberDeclaration md = consider("public int foo();");
        md.setModifiers(new NodeList<>());
        assertTransformedToString("int foo();", md);
    }

    [TestMethod]
    void replacingModifiers() {
        AnnotationMemberDeclaration md = consider("public int foo();");
        md.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected int foo();", md);
    }

    // Default value

    [TestMethod]
    void addingDefaultValue() {
        AnnotationMemberDeclaration md = consider("int foo();");
        md.setDefaultValue(new IntegerLiteralExpr("10"));
        assertTransformedToString("int foo() default 10;", md);
    }

    [TestMethod]
    void removingDefaultValue() {
        AnnotationMemberDeclaration md = consider("int foo() default 10;");
        assertTrue(md.getDefaultValue().get().remove());
        assertTransformedToString("int foo();", md);
    }

    [TestMethod]
    void replacingDefaultValue() {
        AnnotationMemberDeclaration md = consider("int foo() default 10;");
        md.setDefaultValue(new IntegerLiteralExpr("11"));
        assertTransformedToString("int foo() default 11;", md);
    }

    // Annotations

    [TestMethod]
    void addingAnnotation() {
        AnnotationMemberDeclaration it = consider("int foo();");
        it.addAnnotation("myAnno");
        assertTransformedToString("@myAnno" + SYSTEM_EOL + "int foo();", it);
    }

    [TestMethod]
    void addingTwoAnnotations() {
        AnnotationMemberDeclaration it = consider("int foo();");
        it.addAnnotation("myAnno");
        it.addAnnotation("myAnno2");
        assertTransformedToString("@myAnno" + SYSTEM_EOL + "@myAnno2" + SYSTEM_EOL + "int foo();", it);
    }

    [TestMethod]
    void removingAnnotationOnSomeLine() {
        AnnotationMemberDeclaration it = consider("@myAnno int foo();");
        it.getAnnotations().remove(0);
        assertTransformedToString("int foo();", it);
    }

    [TestMethod]
    void removingAnnotationOnPrevLine() {
        AnnotationMemberDeclaration it = consider("@myAnno" + SYSTEM_EOL + "int foo();");
        it.getAnnotations().remove(0);
        assertTransformedToString("int foo();", it);
    }

    [TestMethod]
    void replacingAnnotation() {
        AnnotationMemberDeclaration it = consider("@myAnno int foo();");
        it.getAnnotations().set(0, new NormalAnnotationExpr(new Name("myOtherAnno"), new NodeList<>()));
        assertTransformedToString("@myOtherAnno int foo();", it);
    }

    // Javadoc

    [TestMethod]
    void addingJavadoc() {
        AnnotationMemberDeclaration it = consider("int foo();");
        it.setJavadocComment("Cool this annotation!");
        assertTransformedToString("@interface AD { /**Cool this annotation!*/" + SYSTEM_EOL +
                "int foo(); }", it.getParentNode().get());
    }

    [TestMethod]
    void removingJavadoc() {
        AnnotationMemberDeclaration it = consider("/**Cool this annotation!*/ int foo();");
        assertTrue(it.getJavadocComment().get().remove());
        assertTransformedToString("@interface AD { int foo(); }", it.getParentNode().get());
    }

    [TestMethod]
    void replacingJavadoc() {
        AnnotationMemberDeclaration it = consider("/**Cool this annotation!*/ int foo();");
        it.setJavadocComment("Super extra cool this annotation!!!");
        assertTransformedToString("@interface AD { /**Super extra cool this annotation!!!*/ int foo(); }", it.getParentNode().get());
    }

}
