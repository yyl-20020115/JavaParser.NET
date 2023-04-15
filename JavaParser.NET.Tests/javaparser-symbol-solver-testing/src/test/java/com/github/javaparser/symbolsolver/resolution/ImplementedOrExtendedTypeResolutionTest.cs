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

namespace com.github.javaparser.symbolsolver.resolution;




/**
 * Tests resolution of implemented/extended types
 *
 * @author Takeshi D. Itoh
 */
class ImplementedOrExtendedTypeResolutionTest:AbstractResolutionTest {

    @AfterEach
    void unConfigureSymbolSolver() {
        // unconfigure symbol solver so as not to potentially disturb tests _in other classes
        StaticJavaParser.getConfiguration().setSymbolResolver(null);
    }

    [TestMethod]
    void solveImplementedTypes() {
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        CompilationUnit cu = parseSample("ImplementedOrExtendedTypeResolution/ImplementedOrExtendedTypeResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "InterfaceTest");
        assertEquals(clazz.getFieldByName("field_i1").get().resolve().getType().describe(), "I1");
        assertEquals(clazz.getFieldByName("field_i2").get().resolve().getType().describe(), "I2.I2_1");
        assertEquals(clazz.getFieldByName("field_i3").get().resolve().getType().describe(), "I3.I3_1.I3_1_1");
    }

    [TestMethod]
    void solveExtendedType1() {
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        CompilationUnit cu = parseSample("ImplementedOrExtendedTypeResolution/ImplementedOrExtendedTypeResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassTest1");
        assertEquals(clazz.getFieldByName("field_c1").get().resolve().getType().describe(), "C1");
    }

    [TestMethod]
    void solveExtendedType2() {
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        CompilationUnit cu = parseSample("ImplementedOrExtendedTypeResolution/ImplementedOrExtendedTypeResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassTest2");
        assertEquals(clazz.getFieldByName("field_c2").get().resolve().getType().describe(), "C2.C2_1");
    }

    [TestMethod]
    void solveExtendedType3() {
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        CompilationUnit cu = parseSample("ImplementedOrExtendedTypeResolution/ImplementedOrExtendedTypeResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ClassTest3");
        assertEquals(clazz.getFieldByName("field_c3").get().resolve().getType().describe(), "C3.C3_1.C3_1_1");
    }

    [TestMethod]
    void solveImplementedTypeWithSameName(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(
            new JavaParserTypeSolver(adaptPath("src/test/resources/ImplementedOrExtendedTypeResolution/pkg"))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/ImplementedOrExtendedTypeResolution/pkg/main/A.java"));
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "A");
        string actual = clazz.getFieldByName("field_a").get().resolve().getType().describe();
        assertEquals("main.A", actual);
        assertNotEquals("another.A", actual);
    }

}
