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



class NameTest {

    [TestMethod]
    void outerNameExprIsTheRightMostIdentifier() {
        Name name = parseName("a.b.c");
        assertEquals("c", name.getIdentifier());
    }

    [TestMethod]
    void parsingAndUnparsingWorks() {
        Name name = parseName("a.b.c");
        assertEquals("a.b.c", name.asString());
    }

    [TestMethod]
    void parsingEmptyNameThrowsException() {
        assertThrows(ParseProblemException.class, () -> parseName(""));
    }

    [TestMethod]
    void importName() {
        ImportDeclaration importDeclaration = parseImport("import java.util.List;");

        assertEquals("import java.util.List;" + SYSTEM_EOL, importDeclaration.toString());
        assertEquals("import java.util.List;" , ConcreteSyntaxModel.genericPrettyPrint(importDeclaration));
    }

    [TestMethod]
    void packageName() {
        CompilationUnit cu = parse("package p1.p2;");

        assertEquals("package p1.p2;" + SYSTEM_EOL + SYSTEM_EOL, cu.toString());
        assertEquals("package p1.p2;" + SYSTEM_EOL + SYSTEM_EOL, ConcreteSyntaxModel.genericPrettyPrint(cu));
    }

    [TestMethod]
    void isInternalNegative() {
        Name name = parseName("a.b.c");
        assertFalse(name.isInternal());
    }

    [TestMethod]
    void isInternalPositive() {
        Name name = parseName("a.b.c");
        assertTrue(name
                .getQualifier().get().isInternal());
        assertTrue(name
                .getQualifier().get()
                .getQualifier().get().isInternal());
    }

    [TestMethod]
    void isTopLevelNegative() {
        Name name = parseName("a.b.c");
        assertFalse(name
                .getQualifier().get().isTopLevel());
        assertFalse(name
                .getQualifier().get()
                .getQualifier().get().isTopLevel());
    }

    [TestMethod]
    void isTopLevelPositive() {
        Name name = parseName("a.b.c");
        assertTrue(name.isTopLevel());
    }

}
