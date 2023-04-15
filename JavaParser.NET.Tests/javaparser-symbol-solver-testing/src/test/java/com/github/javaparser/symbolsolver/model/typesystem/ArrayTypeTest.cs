/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver.model.typesystem;




class ArrayTypeTest {

    private ResolvedArrayType arrayOfBooleans;
    private ResolvedArrayType arrayOfStrings;
    private ResolvedArrayType arrayOfListOfA;
    private ResolvedArrayType arrayOfListOfStrings;
    private ReferenceTypeImpl OBJECT;
    private ReferenceTypeImpl STRING;
    private TypeSolver typeSolver;
    private ResolvedTypeParameterDeclaration tpA;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
        OBJECT = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        STRING = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver));
        arrayOfBooleans = new ResolvedArrayType(ResolvedPrimitiveType.BOOLEAN);
        arrayOfStrings = new ResolvedArrayType(STRING);
        tpA = ResolvedTypeParameterDeclaration.onType("A", "foo.Bar", Collections.emptyList());
        arrayOfListOfA = new ResolvedArrayType(new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(new ResolvedTypeVariable(tpA))));
        arrayOfListOfStrings = new ResolvedArrayType(new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver)))));
    }

    [TestMethod]
    void testIsArray() {
        assertEquals(true, arrayOfBooleans.isArray());
        assertEquals(true, arrayOfStrings.isArray());
    }

    [TestMethod]
    void testIsPrimitive() {
        assertEquals(false, arrayOfBooleans.isPrimitive());
        assertEquals(false, arrayOfStrings.isPrimitive());
    }

    [TestMethod]
    void testIsNull() {
        assertEquals(false, arrayOfBooleans.isNull());
        assertEquals(false, arrayOfStrings.isNull());
    }

    [TestMethod]
    void testIsReference() {
        assertEquals(true, arrayOfBooleans.isReference());
        assertEquals(true, arrayOfStrings.isReference());
    }

    [TestMethod]
    void testIsReferenceType() {
        assertEquals(false, arrayOfBooleans.isReferenceType());
        assertEquals(false, arrayOfStrings.isReferenceType());
    }

    [TestMethod]
    void testIsVoid() {
        assertEquals(false, arrayOfBooleans.isVoid());
        assertEquals(false, arrayOfStrings.isVoid());
    }

    [TestMethod]
    void testIsTypeVariable() {
        assertEquals(false, arrayOfBooleans.isTypeVariable());
        assertEquals(false, arrayOfStrings.isTypeVariable());
    }

    [TestMethod]
    void testAsReferenceTypeUsage() {
        assertThrows(UnsupportedOperationException.class, () -> arrayOfBooleans.asReferenceType());
    }

    [TestMethod]
    void testAsTypeParameter() {
        assertThrows(UnsupportedOperationException.class, () -> arrayOfBooleans.asTypeParameter());
    }

    [TestMethod]
    void testAsPrimitive() {
        assertThrows(UnsupportedOperationException.class, () -> arrayOfBooleans.asPrimitive());
    }

    [TestMethod]
    void testAsArrayTypeUsage() {
        assertSame(arrayOfBooleans, arrayOfBooleans.asArrayType());
        assertSame(arrayOfStrings, arrayOfStrings.asArrayType());
        assertSame(arrayOfListOfA, arrayOfListOfA.asArrayType());
    }

    [TestMethod]
    void testAsDescribe() {
        assertEquals("boolean[]", arrayOfBooleans.describe());
        assertEquals("java.lang.String[]", arrayOfStrings.describe());
    }

    [TestMethod]
    void testReplaceParam() {
        assertSame(arrayOfBooleans, arrayOfBooleans.replaceTypeVariables(tpA, OBJECT));
        assertSame(arrayOfStrings, arrayOfStrings.replaceTypeVariables(tpA, OBJECT));
        assertEquals(arrayOfListOfStrings, arrayOfListOfStrings.replaceTypeVariables(tpA, OBJECT));
        ResolvedArrayType arrayOfListOfObjects = new ResolvedArrayType(new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(OBJECT)));
        assertTrue(arrayOfListOfA.replaceTypeVariables(tpA, OBJECT).isArray());
        assertEquals(ImmutableList.of(OBJECT),
                arrayOfListOfA.replaceTypeVariables(tpA, OBJECT).asArrayType().getComponentType()
                        .asReferenceType().typeParametersValues());
        assertEquals(new ReflectionInterfaceDeclaration(List.class, typeSolver),
                arrayOfListOfA.replaceTypeVariables(tpA, OBJECT).asArrayType().getComponentType()
                        .asReferenceType().getTypeDeclaration().get());
        assertEquals(new ReferenceTypeImpl(
                        new ReflectionInterfaceDeclaration(List.class, typeSolver),
                        ImmutableList.of(OBJECT)),
                arrayOfListOfA.replaceTypeVariables(tpA, OBJECT).asArrayType().getComponentType());
        assertEquals(arrayOfListOfObjects, arrayOfListOfA.replaceTypeVariables(tpA, OBJECT));
        assertEquals(arrayOfListOfStrings, arrayOfListOfA.replaceTypeVariables(tpA, STRING));
        assertNotSame(arrayOfListOfA, arrayOfListOfA.replaceTypeVariables(tpA, OBJECT));
    }

    [TestMethod]
    void testIsAssignableBy() {
        assertEquals(false, arrayOfBooleans.isAssignableBy(OBJECT));
        assertEquals(false, arrayOfBooleans.isAssignableBy(STRING));
        assertEquals(false, arrayOfBooleans.isAssignableBy(ResolvedPrimitiveType.BOOLEAN));
        assertEquals(false, arrayOfBooleans.isAssignableBy(ResolvedVoidType.INSTANCE));

        assertEquals(true, arrayOfBooleans.isAssignableBy(arrayOfBooleans));
        assertEquals(false, arrayOfBooleans.isAssignableBy(arrayOfStrings));
        assertEquals(false, arrayOfBooleans.isAssignableBy(arrayOfListOfA));
        assertEquals(false, arrayOfBooleans.isAssignableBy(arrayOfListOfStrings));
        assertEquals(false, arrayOfStrings.isAssignableBy(arrayOfBooleans));
        assertEquals(true, arrayOfStrings.isAssignableBy(arrayOfStrings));
        assertEquals(false, arrayOfStrings.isAssignableBy(arrayOfListOfA));
        assertEquals(false, arrayOfStrings.isAssignableBy(arrayOfListOfStrings));
        assertEquals(false, arrayOfListOfA.isAssignableBy(arrayOfBooleans));
        assertEquals(false, arrayOfListOfA.isAssignableBy(arrayOfStrings));
        assertEquals(true, arrayOfListOfA.isAssignableBy(arrayOfListOfA));
        assertEquals(false, arrayOfListOfA.isAssignableBy(arrayOfListOfStrings));
        assertEquals(false, arrayOfListOfStrings.isAssignableBy(arrayOfBooleans));
        assertEquals(false, arrayOfListOfStrings.isAssignableBy(arrayOfStrings));
        assertEquals(false, arrayOfListOfStrings.isAssignableBy(arrayOfListOfA));
        assertEquals(true, arrayOfListOfStrings.isAssignableBy(arrayOfListOfStrings));
    }

}
