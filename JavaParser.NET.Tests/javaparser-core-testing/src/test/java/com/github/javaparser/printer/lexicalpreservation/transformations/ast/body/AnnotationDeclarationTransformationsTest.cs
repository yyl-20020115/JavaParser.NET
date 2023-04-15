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
 * Transforming AnnotationDeclaration and verifying the LexicalPreservation works as expected.
 */
class AnnotationDeclarationTransformationsTest extends AbstractLexicalPreservingTest {

    [TestMethod]
    void unchangedExamples(){
        assertUnchanged("AnnotationDeclaration_Example1");
        assertUnchanged("AnnotationDeclaration_Example3");
        assertUnchanged("AnnotationDeclaration_Example9");
    }

    // name

    [TestMethod]
    void changingName(){
        considerExample("AnnotationDeclaration_Example1_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setName("NewName");
        assertTransformed("AnnotationDeclaration_Example1", cu);
    }

    // modifiers

    [TestMethod]
    void addingModifiers(){
        considerExample("AnnotationDeclaration_Example1_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setModifiers(createModifierList(PUBLIC));
        assertTransformed("AnnotationDeclaration_Example2", cu);
    }

    [TestMethod]
    void removingModifiers(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setModifiers(new NodeList<>());
        assertTransformed("AnnotationDeclaration_Example3", cu);
    }

    [TestMethod]
    void replacingModifiers(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setModifiers(createModifierList(PROTECTED));
        assertTransformed("AnnotationDeclaration_Example4", cu);
    }

    // members

    [TestMethod]
    void addingMember(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().addMember(new AnnotationMemberDeclaration(new NodeList<>(), PrimitiveType.intType(), "foo", null));
        assertTransformed("AnnotationDeclaration_Example5", cu);
    }

    [TestMethod]
    void removingMember(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().getMember(2).remove();
        assertTransformed("AnnotationDeclaration_Example6", cu);
    }

    [TestMethod]
    void replacingMember(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setMember(2, new AnnotationMemberDeclaration(new NodeList<>(), PrimitiveType.intType(), "foo", null));
        assertTransformed("AnnotationDeclaration_Example7", cu);
    }

    // javadoc

    [TestMethod]
    void addingJavadoc(){
        considerExample("AnnotationDeclaration_Example3_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setJavadocComment("Cool this annotation!");
        assertTransformed("AnnotationDeclaration_Example8", cu);
    }

    [TestMethod]
    void removingJavadoc(){
        considerExample("AnnotationDeclaration_Example9_original");
        bool removed = cu.getAnnotationDeclarationByName("ClassPreamble").get().getJavadocComment().get().remove();
        assertTrue(removed);
        assertTransformed("AnnotationDeclaration_Example9", cu);
    }

    [TestMethod]
    void replacingJavadoc(){
        considerExample("AnnotationDeclaration_Example9_original");
        cu.getAnnotationDeclarationByName("ClassPreamble").get().setJavadocComment("Super extra cool this annotation!!!");
        assertTransformed("AnnotationDeclaration_Example10", cu);
    }

}
