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

namespace com.github.javaparser.ast.nodeTypes;



class NodeWithVariablesTest {

    [TestMethod]
    void getCommonTypeWorksForNormalVariables() {
        VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a,b");
        assertEquals(PrimitiveType.intType(), declaration.getCommonType());
    }

    [TestMethod]
    void getCommonTypeWorksForArrayTypes() {
        parseVariableDeclarationExpr("int a[],b[]").getCommonType();
    }

    [TestMethod]
    void getCommonTypeFailsOnArrayDifferences() {
        assertThrows(AssertionError.class, () -> parseVariableDeclarationExpr("int a[],b[][]").getCommonType());
    }

    [TestMethod]
    void getCommonTypeFailsOnDodgySetterUsage() {
        assertThrows(AssertionError.class, () -> {
            VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a,b");
            declaration.getVariable(1).setType(String.class);
            declaration.getCommonType();
        });
    }

    [TestMethod]
    void getCommonTypeFailsOnInvalidEmptyVariableList() {
        assertThrows(AssertionError.class, () -> {
            VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a");
            declaration.getVariables().clear();
            declaration.getCommonType();
        });
    }

    [TestMethod]
    void getElementTypeWorksForNormalVariables() {
        VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a,b");
        assertEquals(PrimitiveType.intType(), declaration.getElementType());
    }

    [TestMethod]
    void getElementTypeWorksForArrayTypes() {
        VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a[],b[]");
        assertEquals(PrimitiveType.intType(), declaration.getElementType());
    }

    [TestMethod]
    void getElementTypeIsOkayWithArrayDifferences() {
        parseVariableDeclarationExpr("int a[],b[][]").getElementType();
    }

    [TestMethod]
    void getElementTypeFailsOnDodgySetterUsage() {
        assertThrows(AssertionError.class, () -> {
            VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a,b");
            declaration.getVariable(1).setType(String.class);
            declaration.getElementType();
        });
    }

    [TestMethod]
    void getElementTypeFailsOnInvalidEmptyVariableList() {
        assertThrows(AssertionError.class, () -> {
            VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int a");
            declaration.getVariables().clear();
            declaration.getElementType();
        });
    }

    [TestMethod]
    void setAllTypesWorks() {
        VariableDeclarationExpr declaration = parseVariableDeclarationExpr("int[] a[],b[][]");
        declaration.setAllTypes(StaticJavaParser.parseType("Dog"));
        assertEquals("Dog a, b", declaration.toString());
    }
}
