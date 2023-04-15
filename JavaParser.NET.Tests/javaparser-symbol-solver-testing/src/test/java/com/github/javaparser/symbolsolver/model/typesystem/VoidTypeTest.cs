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




class VoidTypeTest {

    private ResolvedArrayType arrayOfBooleans;
    private ResolvedArrayType arrayOfListOfA;
    private ReferenceTypeImpl OBJECT;
    private ReferenceTypeImpl STRING;
    private TypeSolver typeSolver;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
        OBJECT = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        STRING = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver));
        arrayOfBooleans = new ResolvedArrayType(ResolvedPrimitiveType.BOOLEAN);
        arrayOfListOfA = new ResolvedArrayType(new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(new ResolvedTypeVariable(ResolvedTypeParameterDeclaration.onType("A", "foo.Bar", Collections.emptyList())))));
    }

    [TestMethod]
    void testIsArray() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isArray());
    }

    [TestMethod]
    void testIsPrimitive() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isPrimitive());
    }

    [TestMethod]
    void testIsNull() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isNull());
    }

    [TestMethod]
    void testIsReference() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isReference());
    }

    [TestMethod]
    void testIsReferenceType() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isReferenceType());
    }

    [TestMethod]
    void testIsVoid() {
        assertEquals(true, ResolvedVoidType.INSTANCE.isVoid());
    }

    [TestMethod]
    void testIsTypeVariable() {
        assertEquals(false, ResolvedVoidType.INSTANCE.isTypeVariable());
    }

    [TestMethod]
    void testAsReferenceTypeUsage() {
        assertThrows(UnsupportedOperationException.class, () -> ResolvedVoidType.INSTANCE.asReferenceType());
    }

    [TestMethod]
    void testAsTypeParameter() {
        assertThrows(UnsupportedOperationException.class, () -> ResolvedVoidType.INSTANCE.asTypeParameter());
    }

    [TestMethod]
    void testAsArrayTypeUsage() {
        assertThrows(UnsupportedOperationException.class, () -> ResolvedVoidType.INSTANCE.asArrayType());
    }

    [TestMethod]
    void testAsDescribe() {
        assertEquals("void", ResolvedVoidType.INSTANCE.describe());
    }

    [TestMethod]
    void testIsAssignableBy() {
        assertFalse(ResolvedVoidType.INSTANCE.isAssignableBy(NullType.INSTANCE));
        assertFalse(ResolvedVoidType.INSTANCE.isAssignableBy(OBJECT));
        assertFalse(ResolvedVoidType.INSTANCE.isAssignableBy(STRING));
        assertFalse(ResolvedVoidType.INSTANCE.isAssignableBy(ResolvedPrimitiveType.BOOLEAN));
        assertFalse(ResolvedVoidType.INSTANCE.isAssignableBy(ResolvedVoidType.INSTANCE));
    }

}
