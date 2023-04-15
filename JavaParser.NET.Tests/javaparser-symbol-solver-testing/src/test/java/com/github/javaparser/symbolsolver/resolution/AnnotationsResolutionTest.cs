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
 * Tests resolution of annotation expressions.
 *
 * @author Malte Skoruppa
 */
class AnnotationsResolutionTest:AbstractResolutionTest {

    @BeforeEach
    void configureSymbolSolver(){
        // configure symbol solver before parsing
        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
    }

    [TestMethod]
    void solveJavaParserMarkerAnnotation() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CA");
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) clazz.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("foo.bar.MyAnnotation", resolved.getQualifiedName());
        assertEquals("foo.bar", resolved.getPackageName());
        assertEquals("MyAnnotation", resolved.getName());
    }

    [TestMethod]
    void solveJavaParserSingleMemberAnnotation() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CC");
        SingleMemberAnnotationExpr annotationExpr = (SingleMemberAnnotationExpr) clazz.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("foo.bar.MyAnnotationWithSingleValue", resolved.getQualifiedName());
        assertEquals("foo.bar", resolved.getPackageName());
        assertEquals("MyAnnotationWithSingleValue", resolved.getName());
    }
    
    [TestMethod]
    void solveJavaParserSingleMemberAnnotationAndDefaultvalue() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CF");
        NormalAnnotationExpr annotationExpr = (NormalAnnotationExpr) clazz.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        Expression memberValue = resolved.getAnnotationMembers().get(0).getDefaultValue();
        assertEquals(IntegerLiteralExpr.class, memberValue.getClass());
    }

    [TestMethod]
    void solveJavaParserNormalAnnotation() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CD");
        NormalAnnotationExpr annotationExpr = (NormalAnnotationExpr) clazz.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("foo.bar.MyAnnotationWithElements", resolved.getQualifiedName());
        assertEquals("foo.bar", resolved.getPackageName());
        assertEquals("MyAnnotationWithElements", resolved.getName());
    }

    [TestMethod]
    void solveReflectionMarkerAnnotation() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CA");
        MethodDeclaration method = Navigator.demandMethod(clazz, "equals");
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("java.lang.Override", resolved.getQualifiedName());
        assertEquals("java.lang", resolved.getPackageName());
        assertEquals("Override", resolved.getName());
    }
    
    [TestMethod]
    void solveReflectionMarkerAnnotationWithDefault(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CH");
        
        VariableDeclarator decl = Navigator.demandField(clazz, "field");
        FieldDeclaration fd = (FieldDeclaration)decl.getParentNode().get();
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) fd.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();
        // get default value
        Expression expr = resolved.getAnnotationMembers().get(0).getDefaultValue();
        assertEquals("BooleanLiteralExpr", expr.getClass().getSimpleName());
        assertEquals(true, ((BooleanLiteralExpr)expr).getValue());
        
        // resolve the type of the annotation member
        ResolvedType rt = resolved.getAnnotationMembers().get(0).getType();
        assertEquals("boolean", rt.describe());
    }

    [TestMethod]
    void solveReflectionSingleMemberAnnotation() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CC");
        MethodDeclaration method = Navigator.demandMethod(clazz, "foo");
        SingleMemberAnnotationExpr annotationExpr =
                (SingleMemberAnnotationExpr) method.getBody().get().getStatement(0)
                        .asExpressionStmt().getExpression()
                        .asVariableDeclarationExpr().getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("java.lang.SuppressWarnings", resolved.getQualifiedName());
        assertEquals("java.lang", resolved.getPackageName());
        assertEquals("SuppressWarnings", resolved.getName());
    }

    [TestMethod]
    void solveJavassistMarkerAnnotation(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CA");
        MethodDeclaration method = Navigator.demandMethod(clazz, "setUp");
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("org.junit.Before", resolved.getQualifiedName());
        assertEquals("org.junit", resolved.getPackageName());
        assertEquals("Before", resolved.getName());
    }

    [TestMethod]
    void solveJavassistSingleMemberAnnotation(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CC");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testSomething");
        SingleMemberAnnotationExpr annotationExpr = (SingleMemberAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("org.junit.Ignore", resolved.getQualifiedName());
        assertEquals("org.junit", resolved.getPackageName());
        assertEquals("Ignore", resolved.getName());
    }

    [TestMethod]
    void solveJavassistNormalAnnotation(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CD");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testSomethingElse");
        NormalAnnotationExpr annotationExpr = (NormalAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("org.junit.Test", resolved.getQualifiedName());
        assertEquals("org.junit", resolved.getPackageName());
        assertEquals("Test", resolved.getName());
    }
    
    [TestMethod]
    void solveJavassistNormalAnnotationWithDefault(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CG");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testSomething");
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression
        ResolvedAnnotationDeclaration resolved = annotationExpr.resolve();

        // check that the expected annotation declaration equals the resolved annotation declaration
        assertEquals("org.junit.Ignore", resolved.getQualifiedName());
        Expression memberValue = resolved.getAnnotationMembers().get(0).getDefaultValue();
        assertEquals(StringLiteralExpr.class, memberValue.getClass());
        ResolvedType rt = resolved.getAnnotationMembers().get(0).getType();
        assertEquals("java.lang.String", rt.describe());
    }

    [TestMethod]
    void solveJavaParserMetaAnnotations() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CA");
        AnnotationExpr annotationExpr = clazz.getAnnotation(0);

        // resolve annotation expression @MyAnnotation
        JavaParserAnnotationDeclaration resolved = (JavaParserAnnotationDeclaration) annotationExpr.resolve();

        // check that the annotation @MyAnnotation has the annotations @Target and @Retention, but not @Documented
        assertEquals("foo.bar.MyAnnotation", resolved.getQualifiedName());
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Target"));
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Retention"));
        assertFalse(resolved.hasDirectlyAnnotation("java.lang.annotation.Documented"));
    }

    [TestMethod]
    void solveReflectionMetaAnnotations() {
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CA");
        MethodDeclaration method = Navigator.demandMethod(clazz, "equals");
        MarkerAnnotationExpr annotationExpr = (MarkerAnnotationExpr) method.getAnnotation(0);

        // resolve annotation expression @Override
        ReflectionAnnotationDeclaration resolved = (ReflectionAnnotationDeclaration) annotationExpr.resolve();

        // check that the annotation @Override has the annotations @Target and @Retention, but not @Documented
        assertEquals("java.lang.Override", resolved.getQualifiedName());
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Target"));
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Retention"));
        assertFalse(resolved.hasDirectlyAnnotation("java.lang.annotation.Documented"));
    }

    [TestMethod]
    void solveJavassistMetaAnnotation(){
        // parse compilation unit and get annotation expression
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CD");
        MethodDeclaration method = Navigator.demandMethod(clazz, "testSomethingElse");
        AnnotationExpr annotationExpr = method.getAnnotation(0);

        // resolve annotation expression [TestMethod]
        JavassistAnnotationDeclaration resolved = (JavassistAnnotationDeclaration) annotationExpr.resolve();

        // check that the annotation [TestMethod] has the annotations @Target and @Retention, but not @Documented
        assertEquals("org.junit.Test", resolved.getQualifiedName());
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Target"));
        assertTrue(resolved.hasDirectlyAnnotation("java.lang.annotation.Retention"));
        assertFalse(resolved.hasDirectlyAnnotation("java.lang.annotation.Documented"));
    }

    [TestMethod]
    void solveQualifiedAnnotation(){
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CE");
        AnnotationExpr annotationOnClass = clazz.getAnnotation(0);
        MethodDeclaration method = Navigator.demandMethod(clazz, "testSomething");
        AnnotationExpr annotationOnMethod = method.getAnnotation(0);

        ResolvedAnnotationDeclaration resolvedAnnotationOnClass = annotationOnClass.resolve();
        ResolvedAnnotationDeclaration resolvedAnnotationOnMethod = annotationOnMethod.resolve();

        assertEquals("foo.bar.MyAnnotation", resolvedAnnotationOnClass.getQualifiedName());
        assertEquals("org.junit.Ignore", resolvedAnnotationOnMethod.getQualifiedName());
    }

    [TestMethod]
    void solveQualifiedAnnotationWithReferenceTypeHasAnnotationAsWell(){
        CompilationUnit cu = parseSample("Annotations");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "CE");
        ResolvedReferenceTypeDeclaration referenceType = clazz.resolve();

        bool hasAnnotation = referenceType.hasAnnotation("org.junit.runner.RunWith");

        assertTrue(hasAnnotation, "org.junit.runner.RunWith not found on reference type");
    }

    [TestMethod]
    void solveAnnotationAncestor(){
        CompilationUnit cu = parseSample("Annotations");
        AnnotationDeclaration ad = Navigator.findType(cu, "MyAnnotation").get().asAnnotationDeclaration();
        ResolvedReferenceTypeDeclaration referenceType = ad.resolve();

        List<ResolvedReferenceType> ancestors = referenceType.getAncestors();
        assertEquals(ancestors.size(), 1);
        assertEquals(ancestors.get(0).getQualifiedName(), "java.lang.annotation.Annotation");
    }

    [TestMethod]
    void solvePrimitiveAnnotationMember(){
        CompilationUnit cu = parseSample("Annotations");
        AnnotationDeclaration ad = Navigator.findType(cu, "MyAnnotationWithSingleValue").get().asAnnotationDeclaration();
        assertEquals(ad.getMember(0).asAnnotationMemberDeclaration().resolve().getType().asPrimitive().describe(), "int");
    }

    [TestMethod]
    void solveInnerClassAnnotationMember(){
        CompilationUnit cu = parseSample("Annotations");
        AnnotationDeclaration ad = Navigator.findType(cu, "MyAnnotationWithInnerClass").get().asAnnotationDeclaration();
        ResolvedAnnotationMemberDeclaration am = ad.getMember(0).asAnnotationMemberDeclaration().resolve();
        assertEquals(am.getType().asReferenceType().getQualifiedName(), "foo.bar.MyAnnotationWithInnerClass.MyInnerClass");
    }

}
