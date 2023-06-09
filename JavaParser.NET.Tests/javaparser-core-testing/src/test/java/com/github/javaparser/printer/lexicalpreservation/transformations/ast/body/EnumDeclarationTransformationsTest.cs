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
 * Transforming EnumDeclaration and verifying the LexicalPreservation works as expected.
 */
class EnumDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected EnumDeclaration consider(string code) {
        considerCode(code);
        return cu.getType(0).asEnumDeclaration();
    }

    // Name

    [TestMethod]
    void settingName() {
        EnumDeclaration cid = consider("enum A { E1, E2 }");
        cid.setName("B");
        assertTransformedToString("enum B { E1, E2 }", cid);
    }

    // implementedTypes

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        EnumDeclaration ed = consider("enum A { E1, E2 }");
        ed.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public enum A { E1, E2 }", ed);
    }

    [TestMethod]
    void removingModifiers() {
        EnumDeclaration ed = consider("public enum A { E1, E2 }");
        ed.setModifiers(new NodeList<>());
        assertTransformedToString("enum A { E1, E2 }", ed);
    }

    [TestMethod]
    void replacingModifiers() {
        EnumDeclaration ed = consider("public enum A { E1, E2 }");
        ed.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected enum A { E1, E2 }", ed);
    }

    [TestMethod]
    void addingConstants() {
        EnumDeclaration ed = consider("enum A {" + SYSTEM_EOL +
                " E1" + SYSTEM_EOL +
                "}");
        ed.getEntries().addLast(new EnumConstantDeclaration("E2"));
        assertTransformedToString("enum A {" + SYSTEM_EOL +
                " E1," + SYSTEM_EOL +
                " E2" + SYSTEM_EOL +
                "}", ed);
    }

    // members

    // Annotations

    // Javadoc

}
