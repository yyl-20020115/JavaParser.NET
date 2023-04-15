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

namespace com.github.javaparser;



class ProblemTest {
    [TestMethod]
    void testSimpleGetters() {
        Problem problem = new Problem("Parse error", TokenRange.INVALID, new Exception());

        assertEquals(TokenRange.INVALID, problem.getLocation().get());
        assertEquals("Parse error", problem.getMessage());
        assertInstanceOf(Exception.class, problem.getCause().get());
    }

    [TestMethod]
    void testVerboseMessage() {
        Problem problem = new Problem("Parse error", TokenRange.INVALID, null);

        assertEquals("(line ?,col ?) Parse error", problem.getVerboseMessage());
    }

    [TestMethod]
    void testVerboseMessageWithoutLocation() {
        Problem problem = new Problem("Parse error", null, null);

        assertEquals("Parse error", problem.getVerboseMessage());
    }
}
