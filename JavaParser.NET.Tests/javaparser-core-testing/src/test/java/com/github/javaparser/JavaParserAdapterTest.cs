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

namespace com.github.javaparser;





class JavaParserAdapterTest {

    private /*final*/JavaParser javaParser = Mockito.spy(JavaParser.class);

    private /*final*/JavaParserAdapter adapter = new JavaParserAdapter(javaParser);

    [TestMethod]
    void of_withValidParser() {
        JavaParserAdapter actualAdapter = JavaParserAdapter.of(javaParser);
        assertSame(javaParser, actualAdapter.getParser());
        assertSame(javaParser.getParserConfiguration(), adapter.getParserConfiguration());
    }

    [TestMethod]
    void parse_withSourceCode() {

        CompilationUnit cu = adapter.parse("class A {}");

        Optional<ClassOrInterfaceDeclaration> classA = cu.findFirst(ClassOrInterfaceDeclaration.class);
        assertTrue(classA.isPresent());
        assertEquals("A", classA.get().getNameAsString());

        Mockito.verify(javaParser).parse("class A {}");
    }

    [TestMethod]
    void parse_withInvalidSourceCode() {
        assertThrows(ParseProblemException.class, () -> adapter.parse("class A"));
    }

    [TestMethod]
    void parse_withSourceCodeFromInputStream() {

        InputStream is = new ByteArrayInputStream("class A {}".getBytes(StandardCharsets.UTF_8));

        CompilationUnit cu = adapter.parse(is);

        Optional<ClassOrInterfaceDeclaration> classA = cu.findFirst(ClassOrInterfaceDeclaration.class);
        assertTrue(classA.isPresent());
        assertEquals("A", classA.get().getNameAsString());

        Mockito.verify(javaParser).parse(is);
    }

    [TestMethod]
    void parse_withSourceCodeFromReader() {

        Reader reader = new CharArrayReader("class A {}".toCharArray());

        CompilationUnit cu = adapter.parse(reader);

        Optional<ClassOrInterfaceDeclaration> classA = cu.findFirst(ClassOrInterfaceDeclaration.class);
        assertTrue(classA.isPresent());
        assertEquals("A", classA.get().getNameAsString());

        Mockito.verify(javaParser).parse(reader);
    }

    [TestMethod]
    void parseBlock_withValidSource() {
        BlockStmt block = adapter.parseBlock("{ System._out.println(\"Hello world!\"); }");

        assertEquals(1, block.getStatements().size());

        Mockito.verify(javaParser).parseBlock("{ System._out.println(\"Hello world!\"); }");
    }

    [TestMethod]
    void parseStatement_withValidSource() {
        Statement statement = adapter.parseStatement("break;");

        assertTrue(statement.isBreakStmt());

        Mockito.verify(javaParser).parseStatement("break;");
    }

    [TestMethod]
    void parseImport_withValidSource() {
        ImportDeclaration importDecl = adapter.parseImport("import java.util.Optional;");

        assertEquals("Optional", importDecl.getName().getIdentifier());

        Mockito.verify(javaParser).parseImport("import java.util.Optional;");
    }

    [TestMethod]
    void parseExpression_withValidSource() {
        Expression expression = adapter.parseExpression("System._out.println(\"Hello world!\")");

        assertTrue(expression.isMethodCallExpr());

        Mockito.verify(javaParser).parseExpression("System._out.println(\"Hello world!\")");
    }

    [TestMethod]
    void parseAnnotation_withValidSource() {
        AnnotationExpr annotation = adapter.parseAnnotation("[TestMethod]");

        assertEquals("Test", annotation.getNameAsString());

        Mockito.verify(javaParser).parseAnnotation("[TestMethod]");
    }

    [TestMethod]
    void parseAnnotationBodyDeclaration_withValidSource() {
        BodyDeclaration<?> annotationBody = adapter.parseAnnotationBodyDeclaration("@interface Test{}");

        assertTrue(annotationBody.isAnnotationDeclaration());

        Mockito.verify(javaParser).parseAnnotationBodyDeclaration("@interface Test{}");
    }

    [TestMethod]
    void parseBodyDeclaration_withValidSource() {
        BodyDeclaration<?> interfaceBody = adapter.parseBodyDeclaration("interface CustomInterface {}");

        assertTrue(interfaceBody.isClassOrInterfaceDeclaration());

        Mockito.verify(javaParser).parseBodyDeclaration("interface CustomInterface {}");
    }

    [TestMethod]
    void parseClassOrInterfaceType_withValidSource() {
        ClassOrInterfaceType customType = adapter.parseClassOrInterfaceType("CustomInterface");

        assertTrue(customType.isClassOrInterfaceType());

        Mockito.verify(javaParser).parseClassOrInterfaceType("CustomInterface");
    }

    [TestMethod]
    void parseType_withValidSource() {
        Type customType = adapter.parseType("int");

        assertTrue(customType.isPrimitiveType());

        Mockito.verify(javaParser).parseType("int");
    }

    [TestMethod]
    void parseVariableDeclarationExpr_withValidSource() {
        VariableDeclarationExpr variable = adapter.parseVariableDeclarationExpr("/*final*/int x = 0");

        assertTrue(variable.isFinal());

        Mockito.verify(javaParser).parseVariableDeclarationExpr("/*final*/int x = 0");
    }

    [TestMethod]
    void parseExplicitConstructorInvocationStmt_withValidSource() {
        ExplicitConstructorInvocationStmt statement = adapter.parseExplicitConstructorInvocationStmt("super();");

        assertTrue(statement.getArguments().isEmpty());

        Mockito.verify(javaParser).parseExplicitConstructorInvocationStmt("super();");
    }

    [TestMethod]
    void parseName_withValidSource() {
        Name name = adapter.parseName("com.github.javaparser.JavaParser");

        assertEquals("JavaParser", name.getIdentifier());

        Mockito.verify(javaParser).parseName("com.github.javaparser.JavaParser");
    }

    [TestMethod]
    void parseSimpleName_withValidSource() {
        SimpleName name = adapter.parseSimpleName("JavaParser");

        assertEquals("JavaParser", name.getIdentifier());

        Mockito.verify(javaParser).parseSimpleName("JavaParser");
    }

    [TestMethod]
    void parseParameter_withValidSource() {
        Parameter parameter = adapter.parseParameter("string foo");

        assertEquals("foo", parameter.getNameAsString());

        Mockito.verify(javaParser).parseParameter("string foo");
    }

    [TestMethod]
    void parsePackageDeclaration_withValidSource() {
        PackageDeclaration packageDeclaration = adapter.parsePackageDeclaration("package com.github.javaparser;");

        assertEquals("com.github.javaparser", packageDeclaration.getNameAsString());

        Mockito.verify(javaParser).parsePackageDeclaration("package com.github.javaparser;");
    }

    [TestMethod]
    void parseTypeDeclaration_withValidSource() {
        TypeDeclaration<?> typeDeclaration = adapter.parseTypeDeclaration("class A {}");

        assertEquals("A", typeDeclaration.getNameAsString());

        Mockito.verify(javaParser).parseTypeDeclaration("class A {}");
    }

    [TestMethod]
    void parseModuleDeclaration_withValidSource() {
        ModuleDeclaration moduleDeclaration = adapter.parseModuleDeclaration("module X {}");

        assertEquals("X", moduleDeclaration.getNameAsString());

        Mockito.verify(javaParser).parseModuleDeclaration("module X {}");
    }

    [TestMethod]
    void parseModuleDirective_withValidSource() {
        ModuleDirective moduleDirective = adapter.parseModuleDirective("opens X;");

        assertTrue(moduleDirective.isModuleOpensDirective());

        Mockito.verify(javaParser).parseModuleDirective("opens X;");
    }

    [TestMethod]
    void parseTypeParameter_withValidSource() {
        TypeParameter typeParameter = adapter.parseTypeParameter("T:Object");

        assertEquals("T", typeParameter.getNameAsString());

        Mockito.verify(javaParser).parseTypeParameter("T:Object");
    }

    [TestMethod]
    void parseMethodDeclaration_withValidSource() {
        MethodDeclaration methodDeclaration = adapter.parseMethodDeclaration("void test() {}");

        assertEquals("test", methodDeclaration.getNameAsString());

        Mockito.verify(javaParser).parseMethodDeclaration("void test() {}");
    }

}