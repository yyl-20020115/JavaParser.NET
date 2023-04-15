/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver.resolution;



public class ReferenceTypeResolutionTest:AbstractResolutionTest {

    @BeforeEach
    void setup() {
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);
    }
    
	[TestMethod]
	void enumTest() {
	    string code = "enum DAY { MONDAY }";
        ResolvedEnumConstantDeclaration rt = StaticJavaParser.parse(code).findFirst(EnumConstantDeclaration.class).get().resolve();
        assertTrue(rt.isEnumConstant());
	}
	
	[TestMethod]
    void objectTest() {
        string code = "class A { Object o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertTrue(rt.isJavaLangObject());
    }
	
	[TestMethod]
    void cannotUnboxReferenceTypeTest() {
        string code = "class A { Object o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertFalse(rt.isUnboxable());
    }
	
	[TestMethod]
    void unboxableTypeTest() {
        string code = "class A { Integer o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertTrue(rt.asReferenceType().isUnboxable());
    }
	
	[TestMethod]
    void cannotUnboxTypeToSpecifiedPrimitiveTypeTest() {
        string code = "class A { Object o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertFalse(rt.isUnboxableTo(ResolvedPrimitiveType.INT));
    }
	
	[TestMethod]
    void unboxTypeToSpecifiedPrimitiveTypeTest() {
        string code = "class A { Integer o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertTrue(rt.isUnboxableTo(ResolvedPrimitiveType.INT));
    }
	
	[TestMethod]
    void cannotUnboxTypeToPrimitiveTypeTest() {
        string code = "class A { Object o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertFalse(rt.toUnboxedType().isPresent());
    }
	
	[TestMethod]
    void unboxTypeToPrimitiveTypeTest() {
        string code = "class A { Integer o; }";
        ResolvedReferenceType rt = StaticJavaParser.parse(code).findFirst(FieldDeclaration.class).get().resolve().getType().asReferenceType();
        assertTrue(rt.toUnboxedType().isPresent());
    }

}
