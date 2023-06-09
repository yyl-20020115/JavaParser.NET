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




class EnumDeclarationBuildersTest {
    private /*final*/CompilationUnit cu = new CompilationUnit();

    [TestMethod]
    void testAddImplements() {
        EnumDeclaration testEnum = cu.addEnum("test");
        testEnum.addImplementedType(Function.class);
        assertEquals(1, cu.getImports().size());
        assertEquals("import " + Function.class.getName() + ";" + SYSTEM_EOL,
                cu.getImport(0).toString());
        assertEquals(1, testEnum.getImplementedTypes().size());
        assertEquals(Function.class.getSimpleName(), testEnum.getImplementedTypes(0).getNameAsString());
    }

    [TestMethod]
    void testAddEnumConstant() {
        EnumDeclaration testEnum = cu.addEnum("test");
        testEnum.addEnumConstant("MY_ENUM_CONSTANT");
        assertEquals(1, testEnum.getEntries().size());
        assertEquals("MY_ENUM_CONSTANT", testEnum.getEntry(0).getNameAsString());
    }
}
