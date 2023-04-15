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

namespace com.github.javaparser.ast.visitor;



class NoCommentEqualsVisitorTest {

    [TestMethod]
    void testEquals() {
        CompilationUnit p1 = parse("class X { }");
        CompilationUnit p2 = parse("class X { }");
        assertTrue(NoCommentEqualsVisitor.equals(p1, p2));
    }

    [TestMethod]
    void testEqualsWithDifferentComments() {
        CompilationUnit p1 = parse("/* a */ class X { /** b */} //c");
        CompilationUnit p2 = parse("/* b */ class X { }  //c");
        assertTrue(NoCommentEqualsVisitor.equals(p1, p2));
    }

    [TestMethod]
    void testNotEquals() {
        CompilationUnit p1 = parse("class X { }");
        CompilationUnit p2 = parse("class Y { }");
        assertFalse(NoCommentEqualsVisitor.equals(p1, p2));
    }
}
