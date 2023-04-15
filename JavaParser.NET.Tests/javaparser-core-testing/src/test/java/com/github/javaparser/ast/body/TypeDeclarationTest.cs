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

namespace com.github.javaparser.ast.body;



class TypeDeclarationTest {
    [TestMethod]
    void qualifiedNameOfClassInDefaultPackage() {
        assertFQN("X", parseCompilationUnit("class X{ }"));
    }

    [TestMethod]
    void qualifiedNameOfClassInAPackage() {
        assertFQN("a.b.c.X", parseCompilationUnit("package a.b.c; class X{}"));
    }

    [TestMethod]
    void qualifiedNameOfInterfaceInAPackage() {
        assertFQN("a.b.c.X", parseCompilationUnit("package a.b.c; interface X{}"));
    }

    [TestMethod]
    void qualifiedNameOfEnumInAPackage() {
        assertFQN("a.b.c.X", parseCompilationUnit("package a.b.c; enum X{}"));
    }

    [TestMethod]
    void qualifiedNameOfAnnotationInAPackage() {
        assertFQN("a.b.c.X", parseCompilationUnit("package a.b.c; @interface X{}"));
    }

    [TestMethod]
    void qualifiedNameOfNestedClassInAPackage() {
        assertFQN("a.b.c.Outer,a.b.c.Outer.Nested", parseCompilationUnit("package a.b.c; class Outer{ class Nested {} }"));
    }

    [TestMethod]
    void qualifiedNameOfAnonymousClassCantBeQueried() {
        assertFQN("X", parseCompilationUnit("class X{ int aaa() {new Object(){};} }"));
    }

    [TestMethod]
    void qualifiedNameOfLocalClassIsEmpty() {
        assertFQN("X,?", parseCompilationUnit("class X{ int aaa() {class Local {}} }"));
    }

    [TestMethod]
    void qualifiedNameOfDetachedClassIsEmpty() {
        assertFQN("?", parseBodyDeclaration("class X{}"));
    }

    void assertFQN(string fqn, Node node) {
        assertEquals(fqn, node.findAll(TypeDeclaration.class).stream()
                .map(td -> (TypeDeclaration<?>) td)
                .map(td -> td.getFullyQualifiedName().orElse("?"))
                .collect(joining(",")));
    }
}
