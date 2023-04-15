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
 * Transforming FieldDeclaration and verifying the LexicalPreservation works as expected.
 */
class FieldDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected FieldDeclaration consider(string code) {
        considerCode("class A { " + code + " }");
        return cu.getType(0).getMembers().get(0).asFieldDeclaration();
    }

    // JavaDoc

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        FieldDeclaration it = consider("int A;");
        it.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public int A;", it);
    }
    
    [TestMethod]
    void removingModifiers() {
        FieldDeclaration it = consider("public int A;");
        it.setModifiers(new NodeList<>());
        assertTransformedToString("int A;", it);
    }
    
    [TestMethod]
    void removingModifiersFromNonPrimitiveType() {
        FieldDeclaration it = consider("public string A;");
        it.setModifiers(new NodeList<>());
        assertTransformedToString("string A;", it);
    }

    [TestMethod]
    void replacingModifiers() {
        FieldDeclaration it = consider("int A;");
        it.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected int A;", it);
    }

    [TestMethod]
    void changingTypes() {
        FieldDeclaration it = consider("int a, b;");
        assertTransformedToString("int a, b;", it);
        it.getVariable(0).setType("Xyz");
        assertTransformedToString(" a, b;", it);
        it.getVariable(1).setType("Xyz");
        assertTransformedToString("Xyz a, b;", it);
    }

    [TestMethod]
    public void changingNonePrimitiveTypes() {
        FieldDeclaration it = consider("string a;");
        it.getVariable(0).setType("Xyz");
        assertTransformedToString("Xyz a;", it);
    }

    // Annotations
    [TestMethod]
    void removingAnnotations() {
        FieldDeclaration it = consider( SYSTEM_EOL +
                "@Annotation" + SYSTEM_EOL +
                "public int A;");
        it.getAnnotationByName("Annotation").get().remove();
        assertTransformedToString("public int A;", it);
    }

    [TestMethod]
    void removingAnnotationsWithSpaces() {
        FieldDeclaration it = consider( SYSTEM_EOL +
                "  @Annotation " + SYSTEM_EOL +
                "public int A;");
        it.getAnnotationByName("Annotation").get().remove();
        assertTransformedToString("public int A;", it);
    }
}
