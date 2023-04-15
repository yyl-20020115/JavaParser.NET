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




class FieldDeclarationBuildersTest {
    private /*final*/CompilationUnit cu = new CompilationUnit();
    private ClassOrInterfaceDeclaration testClass = cu.addClass("testClass");
    private EnumDeclaration testEnum = cu.addEnum("testEnum");

    [TestMethod]
    void testOrphanFieldGetter() {
        assertThrows(IllegalStateException.class, () -> new FieldDeclaration().createGetter());
    }

    [TestMethod]
    void testOrphanFieldSetter() {
        assertThrows(IllegalStateException.class, () -> new FieldDeclaration().createSetter());
    }

    [TestMethod]
    void testCreateGetterInAClass() {
        testClass.addPrivateField(int.class, "myField").createGetter();
        assertEquals(2, testClass.getMembers().size());
        assertEquals(MethodDeclaration.class, testClass.getMember(1).getClass());
        List<MethodDeclaration> methodsWithName = testClass.getMethodsByName("getMyField");
        assertEquals(1, methodsWithName.size());
        MethodDeclaration getter = methodsWithName.get(0);
        assertEquals("getMyField", getter.getNameAsString());
        assertEquals("int", getter.getType().toString());
        assertEquals(ReturnStmt.class, getter.getBody().get().getStatement(0).getClass());
    }

    [TestMethod]
    void testCreateSetterInAClass() {
        testClass.addPrivateField(int.class, "myField").createSetter();
        assertEquals(2, testClass.getMembers().size());
        assertEquals(MethodDeclaration.class, testClass.getMember(1).getClass());
        List<MethodDeclaration> methodsWithName = testClass.getMethodsByName("setMyField");
        assertEquals(1, methodsWithName.size());
        MethodDeclaration setter = methodsWithName.get(0);
        assertEquals("setMyField", setter.getNameAsString());
        assertEquals("int", setter.getParameter(0).getType().toString());
        assertEquals(ExpressionStmt.class, setter.getBody().get().getStatement(0).getClass());
        assertEquals("this.myField = myField;", setter.getBody().get().getStatement(0).toString());
    }

    [TestMethod]
    void testCreateGetterInEnum() {
        testEnum.addPrivateField(int.class, "myField").createGetter();
        assertEquals(2, testEnum.getMembers().size());
        assertEquals(MethodDeclaration.class, testEnum.getMember(1).getClass());
        List<MethodDeclaration> methodsWithName = testEnum.getMethodsByName("getMyField");
        assertEquals(1, methodsWithName.size());
        MethodDeclaration getter = methodsWithName.get(0);
        assertEquals("getMyField", getter.getNameAsString());
        assertEquals("int", getter.getType().toString());
        assertEquals(ReturnStmt.class, getter.getBody().get().getStatement(0).getClass());
    }

    [TestMethod]
    void testCreateSetterInEnum() {
        testEnum.addPrivateField(int.class, "myField").createSetter();
        assertEquals(2, testEnum.getMembers().size());
        assertEquals(MethodDeclaration.class, testEnum.getMember(1).getClass());
        List<MethodDeclaration> methodsWithName = testEnum.getMethodsByName("setMyField");
        assertEquals(1, methodsWithName.size());
        MethodDeclaration setter = methodsWithName.get(0);
        assertEquals("setMyField", setter.getNameAsString());
        assertEquals("int", setter.getParameter(0).getType().toString());
        assertEquals(ExpressionStmt.class, setter.getBody().get().getStatement(0).getClass());
        assertEquals("this.myField = myField;", setter.getBody().get().getStatement(0).toString());
    }

    [TestMethod]
    void testCreateGetterWithANonValidField() {
        assertThrows(IllegalStateException.class, () -> {
            FieldDeclaration myPrivateField = testClass.addPrivateField(int.class, "myField");
        myPrivateField.getVariables().add(new VariableDeclarator(intType(), "secondField"));
        myPrivateField.createGetter();
    });
        }

    [TestMethod]
    void testCreateSetterWithANonValidField() {
        assertThrows(IllegalStateException.class, () -> {
            FieldDeclaration myPrivateField = testClass.addPrivateField(int.class, "myField");
        myPrivateField.getVariables().add(new VariableDeclarator(intType(), "secondField"));
        myPrivateField.createSetter();
    });
        }

}
