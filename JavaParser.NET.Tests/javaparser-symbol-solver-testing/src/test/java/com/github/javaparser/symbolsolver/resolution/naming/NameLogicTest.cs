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

namespace com.github.javaparser.symbolsolver.resolution.naming;



class NameLogicTest:AbstractNameLogicTest {


    private void assertNameInCodeIsSyntactically(string code, string name, NameCategory nameCategory, ParseStart parseStart) {
        Node nameNode = getNameInCode(code, name, parseStart);
        assertEquals(nameCategory, NameLogic.syntacticClassificationAccordingToContext(nameNode));
    }

    [TestMethod]
    void requiresModuleName() {
        assertNameInCodeIsSyntactically("module com.mydeveloperplanet.jpmshello {\n" +
                "    requires java.base;\n" +
                "    requires java.xml;\n" +
                "    requires com.mydeveloperplanet.jpmshi;\n" +
                "}\n", "java.xml", NameCategory.MODULE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void exportsModuleName() {
        assertNameInCodeIsSyntactically("module my.module{\n" +
                "  exports my.packag to other.module, another.module;\n" +
                "}", "other.module", NameCategory.MODULE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void opensModuleName() {
        assertNameInCodeIsSyntactically("module client.modul{\n" +
                "    opens some.client.packag to framework.modul;\n" +
                "    requires framework.modul2;\n" +
                "}", "framework.modul", NameCategory.MODULE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void exportsPackageName() {
        assertNameInCodeIsSyntactically("module common.widget{\n" +
                "  exports com.logicbig;\n" +
                "}", "com.logicbig", NameCategory.PACKAGE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void opensPackageName() {
        assertNameInCodeIsSyntactically("module foo {\n" +
                "    opens com.example.bar;\n" +
                "}", "com.example.bar", NameCategory.PACKAGE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void packageNameInPackageName() {
        assertNameInCodeIsSyntactically("module foo {\n" +
                "    opens com.example.bar;\n" +
                "}", "com.example", NameCategory.PACKAGE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void usesTypeName() {
        assertNameInCodeIsSyntactically("module modi.mod {\n" +
                "    uses modi.api;\n" +
                "}", "modi.api", NameCategory.TYPE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void providesTypeName() {
        assertNameInCodeIsSyntactically("module foo {\n" +
                "    provides com.modi.api.query.Query with ModuleQuery;\n" +
                "}", "com.modi.api.query.Query", NameCategory.TYPE_NAME, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void singleTypeImportTypeName() {
        assertNameInCodeIsSyntactically("import a.b.c;", "a.b.c",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void singleStaticTypeImportTypeName() {
        assertNameInCodeIsSyntactically("import static a.B.c;", "a.B",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void singleStaticImportOnDemandTypeName() {
        assertNameInCodeIsSyntactically("import static a.B.*;", "a.B",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void constructorDeclarationTypeName() {
        assertNameInCodeIsSyntactically("A() { }", "A",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void annotationTypeName() {
        assertNameInCodeIsSyntactically("@Anno class A {} ", "Anno",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classLiteralTypeName() {
        assertNameInCodeIsSyntactically("Class<?> c = String.class;", "String",
                NameCategory.TYPE_NAME, ParseStart.STATEMENT);
    }

    [TestMethod]
    void thisExprTypeName() {
        assertNameInCodeIsSyntactically("Object o = String.this;", "String",
                NameCategory.TYPE_NAME, ParseStart.STATEMENT);
    }

    [TestMethod]
    void qualifiedSuperFieldAccessTypeName() {
        assertNameInCodeIsSyntactically("Object o = MyClass.super.myField;", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.STATEMENT);
    }

    [TestMethod]
    void qualifiedSuperCallTypeName() {
        assertNameInCodeIsSyntactically("Object o = MyClass.super.myCall();", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.STATEMENT);
    }

    [TestMethod]
    void qualifiedSuperMethodReferenceTypeName() {
        assertNameInCodeIsSyntactically("Object o = MyClass.super::myMethod;", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.STATEMENT);
    }

    [TestMethod]
    void extendsClauseTypeName() {
        assertNameInCodeIsSyntactically("class Foo:bar.MyClass { }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void implementsClauseTypeName() {
        assertNameInCodeIsSyntactically("class Foo implements bar.MyClass { }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void returnTypeTypeName() {
        assertNameInCodeIsSyntactically("class Foo { bar.MyClass myMethod() {} }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void qualifiedAnnotationMemberTypeTypeName() {
        assertNameInCodeIsSyntactically("@interface MyAnno { bar.MyClass myMember(); }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void unqualifiedAnnotationMemberTypeTypeName() {
        assertNameInCodeIsSyntactically("@interface MyAnno { MyClass myMember(); }", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void throwClauseMethodTypeName() {
        assertNameInCodeIsSyntactically("class Foo { void myMethod() throws bar.MyClass {} }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void qualifiedThrowClauseConstructorTypeName() {
        assertNameInCodeIsSyntactically("class Foo { Foo() throws bar.MyClass {} }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void unualifiedThrowClauseConstructorTypeName() {
        assertNameInCodeIsSyntactically("class Foo { Foo() throws MyClass {} }", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void qualifiedFieldTypeTypeName() {
        assertNameInCodeIsSyntactically("class Foo { bar.MyClass myField; }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void fieldTypeTypeNameSecondAttempt() {
        assertNameInCodeIsSyntactically("public class JavaParserInterfaceDeclaration:AbstractTypeDeclaration implements InterfaceDeclaration {\n" +
                        "private TypeSolver typeSolver; }", "TypeSolver",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void unqualifiedFieldTypeTypeName() {
        assertNameInCodeIsSyntactically("class Foo { MyClass myField; }", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void qualifiedFormalParameterOfMethodTypeName() {
        assertNameInCodeIsSyntactically("class Foo { void myMethod(bar.MyClass param) {} }", "bar.MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void unqualifiedFormalParameterOfMethodTypeName() {
        assertNameInCodeIsSyntactically("class Foo { void myMethod(MyClass param) {} }", "MyClass",
                NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void receiverParameterOfMethodTypeName() {
        assertNameInCodeIsSyntactically("void myMethod(Foo this) {}", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void variableDeclarationTypeTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { Foo myVar; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void exceptionParameterTypeTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { try { } catch(Foo e) { } }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void explicitParameterTypeInConstructorCallTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { new Call<Foo>(); }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void explicitParameterTypeInMethodCallTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { new Call().<Foo>myMethod(); }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void instantiationCallTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { new Foo(); }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void instantiationCallOfAnonymousTypeTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { new Foo() { void method() { } } ; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void arrayCreationExpressionTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { new Foo[0]; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void castTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { Object o = (Foo)someField; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void instanceOfTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { if (myValue is Foo) { }; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void instanceOfPatternTypeName() {
        // Note: Requires JDK14
        assertNameInCodeIsSyntactically("void myMethod() { if (myValue is Foo f) { }; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void methodReferenceTypeName() {
        assertNameInCodeIsSyntactically("void myMethod() { Object o = Foo::myMethod; }", "Foo",
                NameCategory.TYPE_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void qualifiedConstructorSuperClassInvocationExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { anExpression.super(); } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void qualifiedClassInstanceCreationExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { anExpression.new MyClass(); } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void arrayReferenceExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { anExpression[0]; } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void postfixExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { anExpression++; } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void leftHandAssignmentExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { anExpression = 2; } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void variableAccessInTryWithResourceExpressionName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { try (anExpression) { }; } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void variableAccessInTryWithResourceWothTypeExpressionName() {
        assertNameInCodeIsSyntactically("class Bar {  Bar() { try (Object o = anExpression) { }; } } ", "anExpression",
                NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void methodInvocationMethodName() {
        assertNameInCodeIsSyntactically("class Bar {  Bar() { myMethod(); } } ", "myMethod",
                NameCategory.METHOD_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void leftOfQualifiedTypeNamePackageOrTypeName() {
        assertNameInCodeIsSyntactically("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified.path.to",
                NameCategory.PACKAGE_OR_TYPE_NAME, ParseStart.COMPILATION_UNIT);
        assertNameInCodeIsSyntactically("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified.path",
                NameCategory.PACKAGE_OR_TYPE_NAME, ParseStart.COMPILATION_UNIT);
        assertNameInCodeIsSyntactically("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified",
                NameCategory.PACKAGE_OR_TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void typeImportOnDemandPackageOrTypeName() {
        assertNameInCodeIsSyntactically("import a.B.*;", "a.B",
                NameCategory.PACKAGE_OR_TYPE_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void leftOfExpressionNameAmbiguousName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a.b.c",
                NameCategory.AMBIGUOUS_NAME, ParseStart.COMPILATION_UNIT);
        assertNameInCodeIsSyntactically("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a.b",
                NameCategory.AMBIGUOUS_NAME, ParseStart.COMPILATION_UNIT);
        assertNameInCodeIsSyntactically("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a",
                NameCategory.AMBIGUOUS_NAME, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void leftOfMethodCallAmbiguousName() {
        assertNameInCodeIsSyntactically("class Bar { Bar() { a.b.c.aMethod(); } } ", "a.b.c",
                NameCategory.AMBIGUOUS_NAME, ParseStart.COMPILATION_UNIT);
    }



    private void assertNameInCodeHasRole(string code, string name, NameRole nameRole, ParseStart parseStart) {
        Node nameNode = getNameInCode(code, name, parseStart);
        assertEquals(nameRole, NameLogic.classifyRole(nameNode));
    }

    private void assertIsSimpleName(string code, string name, ParseStart parseStart) {
        Node nameNode = getNameInCode(code, name, parseStart);
        assertTrue(NameLogic.isSimpleName(nameNode));
    }

    private void assertIsQualifiedName(string code, string name, ParseStart parseStart) {
        Node nameNode = getNameInCode(code, name, parseStart);
        assertTrue(NameLogic.isQualifiedName(nameNode));
    }

    [TestMethod]
    void identifyNamesInSimpleExamples() {
        string code = "package a.b.c; class A { void foo(int param) { return a.b.c.D.e; } }";
        CompilationUnit cu = StaticJavaParser.parse(code);

        assertEquals(false, NameLogic.isAName(cu));
        assertEquals(false, NameLogic.isAName(cu.getPackageDeclaration().get()));

        Name packageName = cu.getPackageDeclaration().get().getName();
        assertEquals(true, NameLogic.isAName(packageName));
        assertEquals(true, NameLogic.isAName(packageName.getQualifier().get()));
        assertEquals(true, NameLogic.isAName(packageName.getQualifier().get().getQualifier().get()));

        ClassOrInterfaceDeclaration classA = cu.getType(0).asClassOrInterfaceDeclaration();
        assertEquals(false, NameLogic.isAName(classA));
        assertEquals(true, NameLogic.isAName(classA.getName()));

        MethodDeclaration methodFoo = classA.getMethods().get(0);
        assertEquals(false, NameLogic.isAName(methodFoo));
        assertEquals(true, NameLogic.isAName(methodFoo.getName()));
        assertEquals(false, NameLogic.isAName(methodFoo.getParameter(0)));
        assertEquals(true, NameLogic.isAName(methodFoo.getParameter(0).getName()));
        assertEquals(false, NameLogic.isAName(methodFoo.getParameter(0).getType()));
        assertEquals(false, NameLogic.isAName(methodFoo.getType()));

        ReturnStmt returnStmt = methodFoo.getBody().get().getStatements().get(0).asReturnStmt();
        assertEquals(false, NameLogic.isAName(returnStmt));
        assertEquals(true, NameLogic.isAName(returnStmt.getExpression().get()));
        FieldAccessExpr fieldAccessExpr = returnStmt.getExpression().get().asFieldAccessExpr();
        assertEquals(true, NameLogic.isAName(fieldAccessExpr
                .getScope())); // a.b.c.D
        assertEquals(true, NameLogic.isAName(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope())); // a.b.c
        assertEquals(true, NameLogic.isAName(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope())); // a.b
        assertEquals(true, NameLogic.isAName(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope())); // a
    }

    [TestMethod]
    void identifyNameRolesInSimpleExamples() {
        string code = "package a.b.c; class A { void foo(int param) { return a.b.c.D.e; } }";
        CompilationUnit cu = StaticJavaParser.parse(code);

        Name packageName = cu.getPackageDeclaration().get().getName();
        assertEquals(DECLARATION, NameLogic.classifyRole(packageName));
        assertEquals(DECLARATION, NameLogic.classifyRole(packageName.getQualifier().get()));
        assertEquals(DECLARATION, NameLogic.classifyRole(packageName.getQualifier().get().getQualifier().get()));

        ClassOrInterfaceDeclaration classA = cu.getType(0).asClassOrInterfaceDeclaration();
        assertEquals(DECLARATION, NameLogic.classifyRole(classA.getName()));

        MethodDeclaration methodFoo = classA.getMethods().get(0);
        assertEquals(DECLARATION, NameLogic.classifyRole(methodFoo.getName()));
        assertEquals(DECLARATION, NameLogic.classifyRole(methodFoo.getParameter(0).getName()));

        ReturnStmt returnStmt = methodFoo.getBody().get().getStatements().get(0).asReturnStmt();
        assertEquals(REFERENCE, NameLogic.classifyRole(returnStmt.getExpression().get())); // a.b.c.D.e
        FieldAccessExpr fieldAccessExpr = returnStmt.getExpression().get().asFieldAccessExpr();
        assertEquals(REFERENCE, NameLogic.classifyRole(fieldAccessExpr
                .getScope())); // a.b.c.D
        assertEquals(REFERENCE, NameLogic.classifyRole(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope())); // a.b.c
        assertEquals(REFERENCE, NameLogic.classifyRole(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope())); // a.b
        assertEquals(REFERENCE, NameLogic.classifyRole(fieldAccessExpr
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope().asFieldAccessExpr()
                .getScope())); // a
    }

    [TestMethod]
    void nameAsStringModuleName() {
        ModuleDeclaration md = parse("module com.mydeveloperplanet.jpmshello {\n" +
                "    requires java.base;\n" +
                "    requires java.xml;\n" +
                "    requires com.mydeveloperplanet.jpmshi;\n" +
                "}\n", ParseStart.MODULE_DECLARATION);
        assertEquals("com.mydeveloperplanet.jpmshello", NameLogic.nameAsString(md.getName()));
    }

    [TestMethod]
    void nameAsStringClassName() {
        CompilationUnit cu = parse("class Foo:bar.MyClass { }", ParseStart.COMPILATION_UNIT);
        assertEquals("Foo", NameLogic.nameAsString(cu.getType(0).getName()));
    }

    [TestMethod]
    void qualifiedModuleName() {
        assertIsQualifiedName("module com.mydeveloperplanet.jpmshello {\n" +
                "    requires java.base;\n" +
                "    requires java.xml;\n" +
                "    requires com.mydeveloperplanet.jpmshi;\n" +
                "}\n", "com.mydeveloperplanet.jpmshello", ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void simpleNameUnqualifiedAnnotationMemberTypeTypeName() {
        assertIsSimpleName("@interface MyAnno { MyClass myMember(); }", "MyClass",
                ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleModuleName() {
        assertNameInCodeHasRole("module com.mydeveloperplanet.jpmshello {\n" +
                "    requires java.base;\n" +
                "    requires java.xml;\n" +
                "    requires com.mydeveloperplanet.jpmshi;\n" +
                "}\n", "com.mydeveloperplanet.jpmshello", DECLARATION, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleRequiresModuleName() {
        assertNameInCodeHasRole("module com.mydeveloperplanet.jpmshello {\n" +
                "    requires java.base;\n" +
                "    requires java.xml;\n" +
                "    requires com.mydeveloperplanet.jpmshi;\n" +
                "}\n", "java.xml", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleExportsModuleName() {
        assertNameInCodeHasRole("module my.module{\n" +
                "  exports my.packag to other.module, another.module;\n" +
                "}", "other.module", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleOpensModuleName() {
        assertNameInCodeHasRole("module client.modul{\n" +
                "    opens some.client.packag to framework.modul;\n" +
                "    requires framework.modul2;\n" +
                "}", "framework.modul", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleExportsPackageName() {
        assertNameInCodeHasRole("module common.widget{\n" +
                "  exports com.logicbig;\n" +
                "}", "com.logicbig", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleOpensPackageName() {
        assertNameInCodeHasRole("module foo {\n" +
                "    opens com.example.bar;\n" +
                "}", "com.example.bar", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRolePackageNameInPackageName() {
        assertNameInCodeHasRole("module foo {\n" +
                "    opens com.example.bar;\n" +
                "}", "com.example", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleUsesTypeName() {
        assertNameInCodeHasRole("module modi.mod {\n" +
                "    uses modi.api;\n" +
                "}", "modi.api", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleProvidesTypeName() {
        assertNameInCodeHasRole("module foo {\n" +
                "    provides com.modi.api.query.Query with ModuleQuery;\n" +
                "}", "com.modi.api.query.Query", REFERENCE, ParseStart.MODULE_DECLARATION);
    }

    [TestMethod]
    void classifyRoleSingleTypeImportTypeName() {
        assertNameInCodeHasRole("import a.b.c;", "a.b.c",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleSingleStaticTypeImportTypeName() {
        assertNameInCodeHasRole("import static a.B.c;", "a.B",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleSingleStaticImportOnDemandTypeName() {
        assertNameInCodeHasRole("import static a.B.*;", "a.B",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleConstructorDeclarationTypeName() {
        assertNameInCodeHasRole("A() { }", "A",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleAnnotationTypeName() {
        assertNameInCodeHasRole("@Anno class A {} ", "Anno",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleClassName() {
        assertNameInCodeHasRole("@Anno class A {} ", "A",
                DECLARATION, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleClassLiteralTypeName() {
        assertNameInCodeHasRole("Class<?> c = String.class;", "String",
                REFERENCE, ParseStart.STATEMENT);
    }

    [TestMethod]
    void classifyRoleThisExprTypeName() {
        assertNameInCodeHasRole("Object o = String.this;", "String",
                REFERENCE, ParseStart.STATEMENT);
    }

    [TestMethod]
    void classifyRoleQualifiedSuperFieldAccessTypeName() {
        assertNameInCodeHasRole("Object o = MyClass.super.myField;", "MyClass",
                REFERENCE, ParseStart.STATEMENT);
    }

    [TestMethod]
    void classifyRoleQualifiedSuperCallTypeName() {
        assertNameInCodeHasRole("Object o = MyClass.super.myCall();", "MyClass",
                REFERENCE, ParseStart.STATEMENT);
    }

    [TestMethod]
    void classifyRoleQualifiedSuperMethodReferenceTypeName() {
        assertNameInCodeHasRole("Object o = MyClass.super::myMethod;", "MyClass",
                REFERENCE, ParseStart.STATEMENT);
    }

    [TestMethod]
    void classifyRoleExtendsClauseTypeName() {
        assertNameInCodeHasRole("class Foo:bar.MyClass { }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleImplementsClauseTypeName() {
        assertNameInCodeHasRole("class Foo implements bar.MyClass { }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleReturnTypeTypeName() {
        assertNameInCodeHasRole("class Foo { bar.MyClass myMethod() {} }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleQualifiedAnnotationMemberTypeTypeName() {
        assertNameInCodeHasRole("@interface MyAnno { bar.MyClass myMember(); }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleAnnotationName() {
        assertNameInCodeHasRole("@interface MyAnno { bar.MyClass myMember(); }", "MyAnno",
                DECLARATION, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleUnqualifiedAnnotationMemberTypeTypeName() {
        assertNameInCodeHasRole("@interface MyAnno { MyClass myMember(); }", "MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleThrowClauseMethodTypeName() {
        assertNameInCodeHasRole("class Foo { void myMethod() throws bar.MyClass {} }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleQualifiedThrowClauseConstructorTypeName() {
        assertNameInCodeHasRole("class Foo { Foo() throws bar.MyClass {} }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleUnualifiedThrowClauseConstructorTypeName() {
        assertNameInCodeHasRole("class Foo { Foo() throws MyClass {} }", "MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleQualifiedFieldTypeTypeName() {
        assertNameInCodeHasRole("class Foo { bar.MyClass myField; }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleFieldTypeTypeNameSecondAttempt() {
        assertNameInCodeHasRole("public class JavaParserInterfaceDeclaration:AbstractTypeDeclaration implements InterfaceDeclaration {\n" +
                        "private TypeSolver typeSolver; }", "TypeSolver",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleUnqualifiedFieldTypeTypeName() {
        assertNameInCodeHasRole("class Foo { MyClass myField; }", "MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleFieldName() {
        assertNameInCodeHasRole("class Foo { MyClass myField; }", "myField",
                DECLARATION, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleQualifiedFormalParameterOfMethodTypeName() {
        assertNameInCodeHasRole("class Foo { void myMethod(bar.MyClass param) {} }", "bar.MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleUnqualifiedFormalParameterOfMethodTypeName() {
        assertNameInCodeHasRole("class Foo { void myMethod(MyClass param) {} }", "MyClass",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleMethodName() {
        assertNameInCodeHasRole("class Foo { void myMethod(MyClass param) {} }", "myMethod",
                DECLARATION, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleReceiverParameterOfMethodTypeName() {
        assertNameInCodeHasRole("void myMethod(Foo this) {}", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleVariableDeclarationTypeTypeName() {
        assertNameInCodeHasRole("void myMethod() { Foo myVar; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleExceptionParameterTypeTypeName() {
        assertNameInCodeHasRole("void myMethod() { try { } catch(Foo e) { } }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleExceptionParameterName() {
        assertNameInCodeHasRole("void myMethod() { try { } catch(Foo e) { } }", "e",
                DECLARATION, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleExplicitParameterTypeInConstructorCallTypeName() {
        assertNameInCodeHasRole("void myMethod() { new Call<Foo>(); }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleExplicitParameterTypeInMethodCallTypeName() {
        assertNameInCodeHasRole("void myMethod() { new Call().<Foo>myMethod(); }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleInstantiationCallTypeName() {
        assertNameInCodeHasRole("void myMethod() { new Foo(); }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleInstantiationCallOfAnonymousTypeTypeName() {
        assertNameInCodeHasRole("void myMethod() { new Foo() { void method() { } } ; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleArrayCreationExpressionTypeName() {
        assertNameInCodeHasRole("void myMethod() { new Foo[0]; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleCastTypeName() {
        assertNameInCodeHasRole("void myMethod() { Object o = (Foo)someField; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleInstanceOfTypeName() {
        assertNameInCodeHasRole("void myMethod() { if (myValue is Foo) { }; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleInstanceOfPatternTypeName() {
        // Note: Requires JDK14
        assertNameInCodeHasRole("void myMethod() { if (myValue is Foo f) { }; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleMethodReferenceTypeName() {
        assertNameInCodeHasRole("void myMethod() { Object o = Foo::myMethod; }", "Foo",
                REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleQualifiedConstructorSuperClassInvocationExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { anExpression.super(); } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleQualifiedClassInstanceCreationExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { anExpression.new MyClass(); } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleArrayReferenceExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { anExpression[0]; } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRolePostfixExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { anExpression++; } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleLeftHandAssignmentExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { anExpression = 2; } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleVariableAccessInTryWithResourceExpressionName() {
        assertNameInCodeHasRole("class Bar { Bar() { try (anExpression) { }; } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleVariableAccessInTryWithResourceWithTypeExpressionName() {
        assertNameInCodeHasRole("class Bar {  Bar() { try (Object o = anExpression) { }; } } ", "anExpression",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyTryWithResourceName() {
        assertNameInCodeHasRole("class Bar {  Bar() { try (Object o = anExpression) { }; } } ", "o",
                DECLARATION, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleMethodInvocationMethodName() {
        assertNameInCodeHasRole("class Bar {  Bar() { myMethod(); } } ", "myMethod",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleLeftOfQualifiedTypeNamePackageOrTypeName() {
        assertNameInCodeHasRole("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified.path.to",
                REFERENCE, ParseStart.COMPILATION_UNIT);
        assertNameInCodeHasRole("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified.path",
                REFERENCE, ParseStart.COMPILATION_UNIT);
        assertNameInCodeHasRole("class Bar {  Bar() { new myQualified.path.to.TypeName(); } } ", "myQualified",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleTypeImportOnDemandPackageOrTypeName() {
        assertNameInCodeHasRole("import a.B.*;", "a.B",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleLeftOfExpressionNameAmbiguousName() {
        assertNameInCodeHasRole("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a.b.c",
                REFERENCE, ParseStart.COMPILATION_UNIT);
        assertNameInCodeHasRole("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a.b",
                REFERENCE, ParseStart.COMPILATION_UNIT);
        assertNameInCodeHasRole("class Bar { Bar() { a.b.c.anExpression[0]; } } ", "a",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void classifyRoleLeftOfMethodCallAmbiguousName() {
        assertNameInCodeHasRole("class Bar { Bar() { a.b.c.aMethod(); } } ", "a.b.c",
                REFERENCE, ParseStart.COMPILATION_UNIT);
    }

    [TestMethod]
    void defaultValueTypeName() {
        assertNameInCodeIsSyntactically("@RequestForEnhancement(\n" +
                        "    id       = 2868724,\n" +
                        "    synopsis = \"Provide time-travel functionality\",\n" +
                        "    engineer = \"Mr. Peabody\",\n" +
                        "    date     = anExpression" +
                        ")\n" +
                        "public static void travelThroughTime(Date destination) {  }",
                "anExpression", NameCategory.AMBIGUOUS_NAME, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleDefaultValueTypeName() {
        assertNameInCodeHasRole("@RequestForEnhancement(\n" +
                        "    id       = 2868724,\n" +
                        "    synopsis = \"Provide time-travel functionality\",\n" +
                        "    engineer = \"Mr. Peabody\",\n" +
                        "    date     = anExpression" +
                        ")\n" +
                        "public static void travelThroughTime(Date destination) {  }",
                "anExpression", REFERENCE, ParseStart.CLASS_BODY);
    }

    [TestMethod]
    void classifyRoleDefaultValueDeclaration() {
        assertNameInCodeHasRole("@RequestForEnhancement(\n" +
                        "    id       = 2868724,\n" +
                        "    synopsis = \"Provide time-travel functionality\",\n" +
                        "    engineer = \"Mr. Peabody\",\n" +
                        "    date     = anExpression" +
                        ")\n" +
                        "public static void travelThroughTime(Date destination) {  }",
                "date", DECLARATION, ParseStart.CLASS_BODY);
    }

}
