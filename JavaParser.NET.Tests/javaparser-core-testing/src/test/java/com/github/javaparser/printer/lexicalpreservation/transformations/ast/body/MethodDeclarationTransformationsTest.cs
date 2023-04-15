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

namespace com.github.javaparser.printer.lexicalpreservation.transformations.ast.body;



/**
 * Transforming MethodDeclaration and verifying the LexicalPreservation works as expected.
 */
class MethodDeclarationTransformationsTest:AbstractLexicalPreservingTest {

    protected MethodDeclaration consider(string code) {
        considerCode("class A { " + code + " }");
        return cu.getType(0).getMembers().get(0).asMethodDeclaration();
    }

    // Name

    [TestMethod]
    void settingName() {
        MethodDeclaration it = consider("void A(){}");
        it.setName("B");
        assertTransformedToString("void B(){}", it);
    }

    // JavaDoc

    @Disabled
    [TestMethod]
    void removingDuplicateJavaDocComment() {
        // Arrange
        considerCode("public class MyClass {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /**" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void oneMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /**" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void anotherMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                "}" +
                SYSTEM_EOL);

        MethodDeclaration methodDeclaration = cu.findAll(MethodDeclaration.class).get(1);

        // Act
        methodDeclaration.removeComment();

        // Assert
        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("public class MyClass {\n" +
                "\n" +
                "  /**\n" +
                "   * Comment A\n" +
                "   */\n" +
                "  public void oneMethod() {\n" +
                "  }\n" +
                "\n" +
                "  public void anotherMethod() {\n" +
                "  }\n" +
                "}\n", result);
    }

    @Disabled
    [TestMethod]
    void replacingDuplicateJavaDocComment() {
        // Arrange
        considerCode("public class MyClass {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /**" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void oneMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /**" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void anotherMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                "}" +
                SYSTEM_EOL);

        MethodDeclaration methodDeclaration = cu.findAll(MethodDeclaration.class).get(1);

        // Act
        Javadoc javadoc = new Javadoc(JavadocDescription.parseText("Change Javadoc"));
        methodDeclaration.setJavadocComment("", javadoc);

        // Assert
        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("public class MyClass {\n" +
                "\n" +
                "  /**\n" +
                "   * Comment A\n" +
                "   */\n" +
                "  public void oneMethod() {\n" +
                "  }\n" +
                "\n" +
                "  /**\n" +
                "   * Change Javadoc\n" +
                "   */\n" +
                "  public void anotherMethod() {\n" +
                "  }\n" +
                "}\n", result);
    }

    // Comments

    @Disabled
    [TestMethod]
    void removingDuplicateComment() {
        // Arrange
        considerCode("public class MyClass {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /*" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void oneMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                SYSTEM_EOL +
                "  /*" + SYSTEM_EOL +
                "   * Comment A" + SYSTEM_EOL +
                "   */" + SYSTEM_EOL +
                "  public void anotherMethod() {" + SYSTEM_EOL +
                "  }" + SYSTEM_EOL +
                "}" +
                SYSTEM_EOL);

        MethodDeclaration methodDeclaration = cu.findAll(MethodDeclaration.class).get(1);

        // Act
        methodDeclaration.removeComment();

        // Assert
        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("public class MyClass {\n" +
                "\n" +
                "  /*\n" +
                "   * Comment A\n" +
                "   */\n" +
                "  public void oneMethod() {\n" +
                "  }\n" +
                "\n" +
                "  public void anotherMethod() {\n" +
                "  }\n" +
                "}\n", result);
    }

    // Modifiers

    [TestMethod]
    void addingModifiers() {
        MethodDeclaration it = consider("void A(){}");
        it.setModifiers(createModifierList(PUBLIC));
        assertTransformedToString("public void A(){}", it);
    }

    [TestMethod]
    void removingModifiers() {
        MethodDeclaration it = consider("public void A(){}");
        it.setModifiers(new NodeList<>());
        assertTransformedToString("void A(){}", it);
    }

    [TestMethod]
    void removingModifiersWithExistingAnnotationsShort() {
        MethodDeclaration it = consider("@Override public void A(){}");
        it.setModifiers(new NodeList<>());
        assertTransformedToString("@Override void A(){}", it);
    }

    [TestMethod]
    void removingPublicModifierFromPublicStaticMethod() {
        MethodDeclaration it = consider("public static void a(){}");
        it.removeModifier(Modifier.Keyword.PUBLIC);
        assertTransformedToString("static void a(){}", it);
    }

    [TestMethod]
    void removingModifiersWithExistingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  [TestMethod]" + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).setModifiers(new NodeList<>());

        string result = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol("class X {\n" +
                "  [TestMethod]\n" +
                "  void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }
    
    [TestMethod]
    void removingModifiersWithExistingAnnotations_withVariableNumberOfSeparator() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  [TestMethod]" + SYSTEM_EOL +
                        "  public      void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).setModifiers(new NodeList<>());

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol(
        		"class X {\n" +
                "  [TestMethod]\n" +
                "  void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }

    [TestMethod]
    void replacingModifiers() {
        MethodDeclaration it = consider("public void A(){}");
        it.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("protected void A(){}", it);
    }

    [TestMethod]
    void replacingModifiersWithExistingAnnotationsShort() {
        MethodDeclaration it = consider("@Override public void A(){}");
        it.setModifiers(createModifierList(PROTECTED));
        assertTransformedToString("@Override protected void A(){}", it);
    }

    [TestMethod]
    void replacingModifiersWithExistingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  [TestMethod]" + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).setModifiers(createModifierList(PROTECTED));

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("class X {\n" +
                "  [TestMethod]\n" +
                "  protected void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }

    // Parameters

    [TestMethod]
    void addingParameters() {
        MethodDeclaration it = consider("void foo(){}");
        it.addParameter(PrimitiveType.doubleType(), "d");
        assertTransformedToString("void foo(double d){}", it);
    }

    [TestMethod]
    void removingOnlyParameter() {
        MethodDeclaration it = consider("public void foo(double d){}");
        it.getParameters().remove(0);
        assertTransformedToString("public void foo(){}", it);
    }

    [TestMethod]
    void removingFirstParameterOfMany() {
        MethodDeclaration it = consider("public void foo(double d, float f){}");
        it.getParameters().remove(0);
        assertTransformedToString("public void foo(float f){}", it);
    }

    [TestMethod]
    void removingLastParameterOfMany() {
        MethodDeclaration it = consider("public void foo(double d, float f){}");
        it.getParameters().remove(1);
        assertTransformedToString("public void foo(double d){}", it);
    }

    [TestMethod]
    void replacingOnlyParameter() {
        MethodDeclaration it = consider("public void foo(float f){}");
        it.getParameters().set(0, new Parameter(new ArrayType(PrimitiveType.intType()), new SimpleName("foo")));
        assertTransformedToString("public void foo(int[] foo){}", it);
    }

    // ThrownExceptions

    // Body

    // Annotations
    [TestMethod]
    void addingToExistingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  [TestMethod]" + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).addSingleMemberAnnotation(
                "org.junit.Ignore",
                new StringLiteralExpr("flaky test"));

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("class X {\n" +
                "  [TestMethod]\n" +
                "  @org.junit.Ignore(\"flaky test\")\n" +
                "  public void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }

    [TestMethod]
    void addingAnnotationsNoModifiers() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).addMarkerAnnotation("Test");
        cu.getType(0).getMethods().get(0).addMarkerAnnotation("Override");

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("class X {\n" +
                "  [TestMethod]\n" +
                "  @Override\n" +
                "  void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }

    [TestMethod]
    void replacingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  @Override" + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).setAnnotations(new NodeList<>(new MarkerAnnotationExpr("Test")));

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol(
                "class X {\n" +
                        "  [TestMethod]\n" +
                        "  public void testCase() {\n" +
                        "  }\n" +
                        "}\n", result);
    }

    [TestMethod]
    void addingAnnotationsShort() {
        MethodDeclaration it = consider("void testMethod(){}");
        it.addMarkerAnnotation("Override");
        assertTransformedToString(
                "@Override" + SYSTEM_EOL +
                        "void testMethod(){}", it);
    }

    // This test case was disabled because we cannot resolve this case for now
    // because indentation before the removed annotation is not part
    // of difference elements (see removingAnnotationsWithSpaces too)
    @Disabled
    [TestMethod]
    void removingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  @Override" + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).getAnnotationByName("Override").get().remove();

        string result = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol(
                "class X {\n" +
                        "  public void testCase() {\n" +
                        "  }\n" +
                        "}\n", result);
    }

    @Disabled
    [TestMethod]
    void removingAnnotationsWithSpaces() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  @Override " + SYSTEM_EOL +
                        "  public void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).getAnnotationByName("Override").get().remove();

        string result = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol(
                "class X {\n" +
                        "  public void testCase() {\n" +
                        "  }\n" +
                        "}\n", result);
    }

    [TestMethod]
    public void addingModifiersWithExistingAnnotationsShort() {
        MethodDeclaration it = consider("@Override void A(){}");
        it.setModifiers(NodeList.nodeList(Modifier.publicModifier(), Modifier.finalModifier()));
        assertTransformedToString("@Override public /*final*/void A(){}", it);
    }

    [TestMethod]
    public void addingModifiersWithExistingAnnotations() {
        considerCode(
                "class X {" + SYSTEM_EOL +
                        "  [TestMethod]" + SYSTEM_EOL +
                        "  void testCase() {" + SYSTEM_EOL +
                        "  }" + SYSTEM_EOL +
                        "}" + SYSTEM_EOL
        );

        cu.getType(0).getMethods().get(0).addModifier(Modifier.finalModifier().getKeyword(), Modifier.publicModifier().getKeyword());

        string result = LexicalPreservingPrinter.print(cu.findCompilationUnit().get());
        assertEqualsStringIgnoringEol("class X {\n" +
                "  [TestMethod]\n" +
                "  /*final*/public void testCase() {\n" +
                "  }\n" +
                "}\n", result);
    }

    [TestMethod]
    public void parseAndPrintAnonymousClassExpression() {
        Expression expression = parseExpression("new Object() {" + SYSTEM_EOL +
                "}");
         string expected = "new Object() {" + SYSTEM_EOL +
                "}";
        assertTransformedToString(expected, expression);
    }

    [TestMethod]
    public void parseAndPrintAnonymousClassStatement() {
        Statement statement = parseStatement("Object anonymous = new Object() {" + SYSTEM_EOL +
                "};");
        string expected = "Object anonymous = new Object() {" + SYSTEM_EOL +
                "};";
        assertTransformedToString(expected, statement);
    }

    [TestMethod]
    public void replaceBodyShouldNotBreakAnonymousClasses() {
        MethodDeclaration it = consider("public void method() { }");
        it.getBody().ifPresent(body -> {
            Statement statement = parseStatement("Object anonymous = new Object() {" + SYSTEM_EOL +
                    "};");
            NodeList<Statement> statements = new NodeList<>();
            statements.add(statement);
            body.setStatements(statements);
        });

        string expected = "public void method() {" + SYSTEM_EOL +
                "    Object anonymous = new Object() {" + SYSTEM_EOL +
                "    };" + SYSTEM_EOL +
                "}";
        assertTransformedToString(expected, it);
    }

}
