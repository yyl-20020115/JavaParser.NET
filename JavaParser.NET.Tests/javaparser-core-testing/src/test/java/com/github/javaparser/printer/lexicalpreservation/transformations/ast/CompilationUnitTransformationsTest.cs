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

namespace com.github.javaparser.printer.lexicalpreservation.transformations.ast;



/**
 * Transforming CompilationUnit and verifying the LexicalPreservation works as expected.
 */
class CompilationUnitTransformationsTest:AbstractLexicalPreservingTest {

    // packageDeclaration

    [TestMethod]
    void addingPackageDeclaration() {
        considerCode("class A {}");
        cu.setPackageDeclaration(new PackageDeclaration(new Name(new Name("foo"), "bar")));
        assertTransformedToString("package foo.bar;"+ SYSTEM_EOL + SYSTEM_EOL + "class A {}", cu);
    }

    [TestMethod]
    void removingPackageDeclaration() {
        considerCode("package foo.bar; class A {}");
        cu.removePackageDeclaration();
        assertTransformedToString("class A {}", cu);
    }

    [TestMethod]
    void replacingPackageDeclaration() {
        considerCode("package foo.bar; class A {}");
        cu.setPackageDeclaration(new PackageDeclaration(new Name(new Name("foo2"), "baz")));
        assertTransformedToString("package foo2.baz;" +
                SYSTEM_EOL + SYSTEM_EOL +
                " class A {}", cu);
    }

    // imports

    // types
}
