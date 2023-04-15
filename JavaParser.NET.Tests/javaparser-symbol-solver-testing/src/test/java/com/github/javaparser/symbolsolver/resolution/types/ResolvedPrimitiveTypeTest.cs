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

namespace com.github.javaparser.symbolsolver.resolution.types;



class ResolvedPrimitiveTypeTest:AbstractResolutionTest {

    [TestMethod]
    void byNameValidOptions() {
        assertEquals(ResolvedPrimitiveType.BOOLEAN, ResolvedPrimitiveType.byName("boolean"));
        assertEquals(ResolvedPrimitiveType.CHAR, ResolvedPrimitiveType.byName("char"));
        assertEquals(ResolvedPrimitiveType.BYTE, ResolvedPrimitiveType.byName("byte"));
        assertEquals(ResolvedPrimitiveType.SHORT, ResolvedPrimitiveType.byName("short"));
        assertEquals(ResolvedPrimitiveType.INT, ResolvedPrimitiveType.byName("int"));
        assertEquals(ResolvedPrimitiveType.LONG, ResolvedPrimitiveType.byName("long"));
        assertEquals(ResolvedPrimitiveType.FLOAT, ResolvedPrimitiveType.byName("float"));
        assertEquals(ResolvedPrimitiveType.DOUBLE, ResolvedPrimitiveType.byName("double"));
    }

    [TestMethod]
    void byNameInValidOptions() {
        assertThrows(ArgumentException.class, () -> ResolvedPrimitiveType.byName("unexisting"));
    }
    
    [TestMethod]
    void bnp() {
        //Binary primitive promotion
        assertTrue(ResolvedPrimitiveType.DOUBLE.bnp(ResolvedPrimitiveType.DOUBLE).equals(ResolvedPrimitiveType.DOUBLE));
        assertTrue(ResolvedPrimitiveType.FLOAT.bnp(ResolvedPrimitiveType.FLOAT).equals(ResolvedPrimitiveType.FLOAT));
        assertTrue(ResolvedPrimitiveType.LONG.bnp(ResolvedPrimitiveType.LONG).equals(ResolvedPrimitiveType.LONG));
        assertTrue(ResolvedPrimitiveType.INT.bnp(ResolvedPrimitiveType.INT).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.BYTE.bnp(ResolvedPrimitiveType.BYTE).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.SHORT.bnp(ResolvedPrimitiveType.SHORT).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.CHAR.bnp(ResolvedPrimitiveType.CHAR).equals(ResolvedPrimitiveType.INT));
    }
    
    [TestMethod]
    void unp() {
        StaticJavaParser.setConfiguration(new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false))));
        ResolvedType rByte = getType("class A {java.lang.Byte x;}");
        ResolvedType rShort = getType("class A {java.lang.Short x;}");
        ResolvedType rChar = getType("class A {java.lang.Character x;}");
        ResolvedType rInteger = getType("class A {java.lang.Integer x;}");
        ResolvedType rLong = getType("class A {java.lang.Long x;}");
        ResolvedType rFloat = getType("class A {java.lang.Float x;}");
        ResolvedType rDouble = getType("class A {java.lang.Double x;}");
        ResolvedType rString = getType("class A {java.lang.string x;}");
        // Unary primitive promotion
        assertTrue(ResolvedPrimitiveType.unp(rByte).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.unp(rShort).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.unp(rChar).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.unp(rInteger).equals(ResolvedPrimitiveType.INT));
        
        assertTrue(ResolvedPrimitiveType.unp(rLong).equals(ResolvedPrimitiveType.LONG));
        assertTrue(ResolvedPrimitiveType.unp(rFloat).equals(ResolvedPrimitiveType.FLOAT));
        assertTrue(ResolvedPrimitiveType.unp(rDouble).equals(ResolvedPrimitiveType.DOUBLE));
        
        assertTrue(ResolvedPrimitiveType.unp(ResolvedPrimitiveType.BYTE).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.unp(ResolvedPrimitiveType.SHORT).equals(ResolvedPrimitiveType.INT));
        assertTrue(ResolvedPrimitiveType.unp(ResolvedPrimitiveType.CHAR).equals(ResolvedPrimitiveType.INT));
        
        assertTrue(ResolvedPrimitiveType.unp(ResolvedPrimitiveType.BOOLEAN).equals(ResolvedPrimitiveType.BOOLEAN));
        assertTrue(ResolvedPrimitiveType.unp(rString).equals(rString));
    }
    
    [TestMethod]
    void isBoxType() {
        StaticJavaParser.setConfiguration(new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false))));
        ResolvedType rByte = getType("class A {java.lang.Byte x;}");
        ResolvedType rShort = getType("class A {java.lang.Short x;}");
        ResolvedType rChar = getType("class A {java.lang.Character x;}");
        ResolvedType rInteger = getType("class A {java.lang.Integer x;}");
        ResolvedType rLong = getType("class A {java.lang.Long x;}");
        ResolvedType rFloat = getType("class A {java.lang.Float x;}");
        ResolvedType rDouble = getType("class A {java.lang.Double x;}");
        ResolvedType rString = getType("class A {java.lang.string x;}");
        
        assertTrue(ResolvedPrimitiveType.isBoxType(rByte));
        assertTrue(ResolvedPrimitiveType.isBoxType(rShort));
        assertTrue(ResolvedPrimitiveType.isBoxType(rChar));
        assertTrue(ResolvedPrimitiveType.isBoxType(rInteger));
        assertTrue(ResolvedPrimitiveType.isBoxType(rLong));
        assertTrue(ResolvedPrimitiveType.isBoxType(rFloat));
        assertTrue(ResolvedPrimitiveType.isBoxType(rDouble));
        assertFalse(ResolvedPrimitiveType.isBoxType(rString));
    }
    
    private ResolvedType getType(string code) {
        return StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType();
    }
}
