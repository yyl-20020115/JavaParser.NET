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

namespace com.github.javaparser.builders;




class NodeWithParametersBuildersTest {
    private /*final*/CompilationUnit cu = new CompilationUnit();

    [TestMethod]
    void testAddParameter() {
        MethodDeclaration addMethod = cu.addClass("test").addMethod("foo", PUBLIC);
        addMethod.addParameter(int.class, "yay");
        Parameter myNewParam = addMethod.addAndGetParameter(List.class, "myList");
        assertEquals(1, cu.getImports().size());
        assertEquals("import " + List.class.getName() + ";" + SYSTEM_EOL, cu.getImport(0).toString());
        assertEquals(2, addMethod.getParameters().size());
        assertEquals("yay", addMethod.getParameter(0).getNameAsString());
        assertEquals("List", addMethod.getParameter(1).getType().toString());
        assertEquals(myNewParam, addMethod.getParameter(1));
    }

    [TestMethod]
    void testGetParamByName() {
        MethodDeclaration addMethod = cu.addClass("test").addMethod("foo", PUBLIC);
        Parameter addAndGetParameter = addMethod.addAndGetParameter(int.class, "yay");
        assertEquals(addAndGetParameter, addMethod.getParameterByName("yay").get());
    }

    [TestMethod]
    void testGetParamByType() {
        MethodDeclaration addMethod = cu.addClass("test").addMethod("foo", PUBLIC);
        Parameter addAndGetParameter = addMethod.addAndGetParameter(int.class, "yay");
        assertEquals(addAndGetParameter, addMethod.getParameterByType("int").get());
        assertEquals(addAndGetParameter, addMethod.getParameterByType(int.class).get());
    }

}
