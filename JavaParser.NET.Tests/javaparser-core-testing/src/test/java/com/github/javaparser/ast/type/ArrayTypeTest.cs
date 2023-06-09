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

namespace com.github.javaparser.ast.type;



class ArrayTypeTest {
    [TestMethod]
    void getFieldDeclarationWithArrays() {
        FieldDeclaration fieldDeclaration = parseBodyDeclaration("@C int @A[] @B[] a @X[] @Y[];").asFieldDeclaration();

        ArrayType arrayType1 = fieldDeclaration.getVariable(0).getType().asArrayType();
        ArrayType arrayType2 = arrayType1.getComponentType().asArrayType();
        ArrayType arrayType3 = arrayType2.getComponentType().asArrayType();
        ArrayType arrayType4 = arrayType3.getComponentType().asArrayType();
        PrimitiveType elementType = arrayType4.getComponentType().asPrimitiveType();

        assertThat(arrayType1.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("X")));
        assertThat(arrayType2.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("Y")));
        assertThat(arrayType3.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("A")));
        assertThat(arrayType4.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("B")));

        assertThat(elementType.getType()).isEqualTo(PrimitiveType.Primitive.INT);
        assertThat(fieldDeclaration.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("C")));

        assertThat(arrayType1.getParentNode().get().getParentNode().get()).isSameAs(fieldDeclaration);
    }

    [TestMethod]
    void getVariableDeclarationWithArrays() {
        ExpressionStmt variableDeclarationStatement = parseStatement("@C int @A[] @B[] a @X[] @Y[];").asExpressionStmt();
        VariableDeclarationExpr variableDeclarationExpr = variableDeclarationStatement.getExpression().asVariableDeclarationExpr();

        ArrayType arrayType1 = variableDeclarationExpr.getVariable(0).getType().asArrayType();
        ArrayType arrayType2 = arrayType1.getComponentType().asArrayType();
        ArrayType arrayType3 = arrayType2.getComponentType().asArrayType();
        ArrayType arrayType4 = arrayType3.getComponentType().asArrayType();
        PrimitiveType elementType = arrayType4.getComponentType().asPrimitiveType();

        assertThat(arrayType1.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("X")));
        assertThat(arrayType2.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("Y")));
        assertThat(arrayType3.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("A")));
        assertThat(arrayType4.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("B")));

        assertThat(elementType.getType()).isEqualTo(PrimitiveType.Primitive.INT);
        assertThat(variableDeclarationExpr.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("C")));

        assertThat(arrayType1.getParentNode().get().getParentNode().get()).isSameAs(variableDeclarationExpr);
    }

    [TestMethod]
    void getMethodDeclarationWithArrays() {
        MethodDeclaration methodDeclaration = parseBodyDeclaration("@C int @A[] a() @B[] {}").asMethodDeclaration();

        ArrayType arrayType1 = methodDeclaration.getType().asArrayType();
        ArrayType arrayType2 = arrayType1.getComponentType().asArrayType();
        Type elementType = arrayType2.getComponentType();
        assertThat(elementType).isInstanceOf(PrimitiveType.class);

        assertThat(arrayType1.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("B")));
        assertThat(arrayType2.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("A")));
        assertThat(methodDeclaration.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("C")));

        assertThat(methodDeclaration.getType().getParentNode().get()).isSameAs(methodDeclaration);
    }

    [TestMethod]
    void getParameterWithArrays() {
        MethodDeclaration methodDeclaration = parseBodyDeclaration("void a(@C int @A[] a @B[]) {}").asMethodDeclaration();

        Parameter parameter = methodDeclaration.getParameter(0);

        ArrayType outerArrayType = parameter.getType().asArrayType();

        ArrayType innerArrayType = outerArrayType.getComponentType().asArrayType();
        PrimitiveType elementType = innerArrayType.getComponentType().asPrimitiveType();

        assertThat(elementType).isInstanceOf(PrimitiveType.class);
        assertThat(outerArrayType.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("B")));
        assertThat(innerArrayType.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("A")));
        assertThat(parameter.getAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("C")));

        assertThat(parameter.getType().getParentNode().get()).isSameAs(parameter);
    }

    [TestMethod]
    void setVariableDeclarationWithArrays() {
        ExpressionStmt variableDeclarationStatement = parseStatement("@C int @A[] @B[] a @X[] @Y[];").asExpressionStmt();
        VariableDeclarationExpr variableDeclarationExpr = variableDeclarationStatement.getExpression().asVariableDeclarationExpr();

        variableDeclarationExpr.getVariable(0).setType(new ArrayType(new ArrayType(PrimitiveType.intType())));
        assertEquals("@C" + SYSTEM_EOL + "int[][] a;", variableDeclarationStatement.toString());
    }

    [TestMethod]
    void setFieldDeclarationWithArrays() {
        FieldDeclaration fieldDeclaration = parseBodyDeclaration("int[][] a[][];").asFieldDeclaration();
        fieldDeclaration.getVariable(0).setType(new ArrayType(new ArrayType(parseClassOrInterfaceType("Blob"))));

        assertEquals("Blob[][] a;", fieldDeclaration.toString());
    }

    [TestMethod]
    void setMethodDeclarationWithArrays() {
        MethodDeclaration method = parseBodyDeclaration("int[][] a()[][] {}").asMethodDeclaration();
        method.setType(new ArrayType(new ArrayType(parseClassOrInterfaceType("Blob"))));

        assertEquals("Blob[][] a() {" + SYSTEM_EOL + "}", method.toString());
    }

    [TestMethod]
    void fieldDeclarationWithArraysHasCorrectOrigins() {
        FieldDeclaration fieldDeclaration = parseBodyDeclaration("int[] a[];").asFieldDeclaration();

        Type outerType = fieldDeclaration.getVariables().get(0).getType();
        assertEquals(ArrayType.Origin.NAME, outerType.asArrayType().getOrigin());
        assertEquals(ArrayType.Origin.TYPE, outerType.asArrayType().getComponentType().asArrayType().getOrigin());
    }

    [TestMethod]
    void methodDeclarationWithArraysHasCorrectOrigins() {
        MethodDeclaration method = (MethodDeclaration) parseBodyDeclaration("int[] a()[] {}");

        Type outerType = method.getType();
        assertEquals(ArrayType.Origin.NAME, outerType.asArrayType().getOrigin());
        assertEquals(ArrayType.Origin.TYPE, outerType.asArrayType().getComponentType().asArrayType().getOrigin());
    }

    [TestMethod]
    void setParameterWithArrays() {
        MethodDeclaration method = parseBodyDeclaration("void a(int[][] a[][]) {}").asMethodDeclaration();
        method.getParameter(0).setType(new ArrayType(new ArrayType(parseClassOrInterfaceType("Blob"))));

        assertEquals("void a(Blob[][] a) {" + SYSTEM_EOL + "}", method.toString());
    }

    [TestMethod]
    void getArrayCreationType() {
        ArrayCreationExpr expr = parseExpression("new int[]");
        ArrayType outerType = expr.createdType().asArrayType();
        Type innerType = outerType.getComponentType();
        assertThat(innerType).isEqualTo(expr.getElementType());
    }

    [TestMethod]
    void ellipsisCanHaveAnnotationsToo() {
        Parameter p = parseParameter("int[]@X...a[]");

        assertThat(p.getVarArgsAnnotations()).containsExactly(new MarkerAnnotationExpr(parseName("X")));
        assertEquals("int[][]@X ... a", p.toString());
        assertEquals("int[][]@X... a", ConcreteSyntaxModel.genericPrettyPrint(p));
    }
    
    [TestMethod]
    void arrayLevel() {
        FieldDeclaration fd1 = parseBodyDeclaration("int[] a;").asFieldDeclaration();
        assertEquals(1, fd1.getVariable(0).getType().getArrayLevel());
        FieldDeclaration fd2 = parseBodyDeclaration("int[][] a;").asFieldDeclaration();
        assertEquals(2, fd2.getVariable(0).getType().getArrayLevel());
    }
    
    [TestMethod]
	void range() {
		Type type = parseType("Long[][]");
		assertEquals(8, type.getRange().get().end.column);
	}
}
