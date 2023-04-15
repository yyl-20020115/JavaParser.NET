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



class NodeWithAnnotationsBuildersTest {
    private CompilationUnit cu = new CompilationUnit();
    private ClassOrInterfaceDeclaration testClass = cu.addClass("testClass");

    @interface hey {

    }

    [TestMethod]
    void testAddAnnotation() {
        NormalAnnotationExpr annotation = testClass.addAndGetAnnotation(hey.class);
        assertEquals("import com.github.javaparser.builders.NodeWithAnnotationsBuildersTest.hey;", cu.getImport(0).toString().trim());
        assertEquals(1, testClass.getAnnotations().size());
        assertEquals(annotation, testClass.getAnnotation(0));
        assertEquals(NormalAnnotationExpr.class, testClass.getAnnotation(0).getClass());
    }

    [TestMethod]
    void testAddMarkerAnnotation() {
        testClass.addMarkerAnnotation("test");
        assertEquals(1, testClass.getAnnotations().size());
    }

    [TestMethod]
    void testAddSingleMemberAnnotation() {
        testClass.addSingleMemberAnnotation("test", "value");
        assertEquals(1, testClass.getAnnotations().size());
        assertEquals("value", testClass.getAnnotation(0).asSingleMemberAnnotationExpr().getMemberValue().toString());
    }

    [TestMethod]
    void testAddSingleMemberAnnotation2() {
        testClass.addSingleMemberAnnotation(hey.class, new NameExpr(new SimpleName("value")));
        assertEquals(1, testClass.getAnnotations().size());
        assertEquals("value", testClass.getAnnotation(0).asSingleMemberAnnotationExpr().getMemberValue().toString());
    }

    [TestMethod]
    void testIsAnnotationPresent() {
        testClass.addMarkerAnnotation(hey.class);
        assertTrue(testClass.isAnnotationPresent(hey.class));
    }

    [TestMethod]
    void testGetAnnotationByName() {
        NormalAnnotationExpr annotation = testClass.addAndGetAnnotation(hey.class);
        assertEquals(annotation, testClass.getAnnotationByName("hey").get());
    }

    [TestMethod]
    void testGetAnnotationByClass() {
        NormalAnnotationExpr annotation = testClass.addAndGetAnnotation(hey.class);
        assertEquals(annotation, testClass.getAnnotationByClass(hey.class).get());
    }
}
