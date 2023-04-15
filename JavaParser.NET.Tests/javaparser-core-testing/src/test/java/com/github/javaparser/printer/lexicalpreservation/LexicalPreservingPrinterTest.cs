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

namespace com.github.javaparser.printer.lexicalpreservation;




class LexicalPreservingPrinterTest:AbstractLexicalPreservingTest {
    private NodeText getTextForNode(Node node) {
        return node.getData(NODE_TEXT_DATA);
    }

    //
    // Tests on TextNode definition
    //

    [TestMethod]
    void checkNodeTextCreatedForSimplestClass() {
        considerCode("class A {}");

        // CU
        assertEquals(1, getTextForNode(cu).numberOfElements());
        assertTrue(getTextForNode(cu).getTextElement(0) is ChildTextElement);
        assertEquals(cu.getClassByName("A").get(),
                ((ChildTextElement) getTextForNode(cu).getTextElement(0)).getChild());

        // Class
        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        assertEquals(7, getTextForNode(classA).numberOfElements());
        assertEquals("class", getTextForNode(classA).getTextElement(0).expand());
        assertEquals(" ", getTextForNode(classA).getTextElement(1).expand());
        assertEquals("A", getTextForNode(classA).getTextElement(2).expand());
        assertEquals(" ", getTextForNode(classA).getTextElement(3).expand());
        assertEquals("{", getTextForNode(classA).getTextElement(4).expand());
        assertEquals("}", getTextForNode(classA).getTextElement(5).expand());
        assertEquals("", getTextForNode(classA).getTextElement(6).expand());
        assertTrue(getTextForNode(classA).getTextElement(6) is TokenTextElement);
        assertEquals(GeneratedJavaParserConstants.EOF,
                ((TokenTextElement) getTextForNode(classA).getTextElement(6)).getTokenKind());
    }

    [TestMethod]
    void checkNodeTextCreatedForField() {
        string code = "class A {int i;}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        FieldDeclaration fd = classA.getFieldByName("i").get();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(fd);
        assertEquals(Arrays.asList("int", " ", "i", ";"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedForVariableDeclarator() {
        string code = "class A {int i;}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        FieldDeclaration fd = classA.getFieldByName("i").get();
        VariableDeclarator vd = fd.getVariables().get(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(vd);
        assertEquals(Arrays.asList("i"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedForMethod() {
        string code = "class A {void foo(int p1, float p2) { }}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        MethodDeclaration md = classA.getMethodsByName("foo").get(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(md);
        assertEquals(Arrays.asList("void", " ", "foo", "(", "int p1", ",", " ", "float p2", ")", " ", "{ }"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedForMethodParameter() {
        string code = "class A {void foo(int p1, float p2) { }}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        MethodDeclaration md = classA.getMethodsByName("foo").get(0);
        Parameter p1 = md.getParameterByName("p1").get();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(p1);
        assertEquals(Arrays.asList("int", " ", "p1"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedForPrimitiveType() {
        string code = "class A {void foo(int p1, float p2) { }}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        MethodDeclaration md = classA.getMethodsByName("foo").get(0);
        Parameter p1 = md.getParameterByName("p1").get();
        Type t = p1.getType();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(t);
        assertEquals(Arrays.asList("int"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedForSimpleImport() {
        string code = "import a.b.c.D;";
        considerCode(code);

        ImportDeclaration imp = (ImportDeclaration) cu.getChildNodes().get(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(imp);
        assertEquals(Arrays.asList("import", " ", "a.b.c.D", ";", ""),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void addedImportShouldBePrependedWithEOL() {
        considerCode("import a.A;" + SYSTEM_EOL + "class X{}");

        cu.addImport("a.B");

        assertEqualsStringIgnoringEol("import a.A;\nimport a.B;\nclass X{}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void checkNodeTextCreatedGenericType() {
        string code = "class A {ParseResult<T> result;}";
        considerCode(code);

        FieldDeclaration field = cu.getClassByName("A").get().getFieldByName("result").get();
        Node t = field.getCommonType();
        Node t2 = field.getVariable(0).getType();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(field);
        assertEquals(Arrays.asList("ParseResult", "<", "T", ">", " ", "result", ";"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedAnnotationDeclaration() {
        string code = "public @interface ClassPreamble { string author(); }";
        considerCode(code);

        AnnotationDeclaration ad = cu.getAnnotationDeclarationByName("ClassPreamble").get();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(ad);
        assertEquals(
                Arrays.asList("public", " ", "@", "interface", " ", "ClassPreamble", " ", "{", " ", "string author();",
                        " ", "}", ""),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedAnnotationMemberDeclaration() {
        string code = "public @interface ClassPreamble { string author(); }";
        considerCode(code);

        AnnotationDeclaration ad = cu.getAnnotationDeclarationByName("ClassPreamble").get();
        AnnotationMemberDeclaration md = (AnnotationMemberDeclaration) ad.getMember(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(md);
        assertEquals(Arrays.asList("String", " ", "author", "(", ")", ";"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedAnnotationMemberDeclarationWithArrayType() {
        string code = "public @interface ClassPreamble { String[] author(); }";
        considerCode(code);

        AnnotationDeclaration ad = cu.getAnnotationDeclarationByName("ClassPreamble").get();
        AnnotationMemberDeclaration md = (AnnotationMemberDeclaration) ad.getMember(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(md);
        assertEquals(Arrays.asList("String[]", " ", "author", "(", ")", ";"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedAnnotationMemberDeclarationArrayType() {
        string code = "public @interface ClassPreamble { String[] author(); }";
        considerCode(code);

        AnnotationDeclaration ad = cu.getAnnotationDeclarationByName("ClassPreamble").get();
        AnnotationMemberDeclaration md = ad.getMember(0).asAnnotationMemberDeclaration();
        Type type = md.getType();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(type);
        assertEquals(Arrays.asList("String", "[", "]"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedAnnotationMemberDeclarationWithComment(){
        considerExample("AnnotationDeclaration_Example3_original");

        AnnotationMemberDeclaration md = cu.getAnnotationDeclarationByName("ClassPreamble").get().getMember(5)
                .asAnnotationMemberDeclaration();
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(md);
        assertEquals(Arrays.asList("String[]", " ", "reviewers", "(", ")", ";"),
                nodeText.getElements().stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedArrayCreationLevelWithoutExpression() {
        considerExpression("new int[]");

        ArrayCreationExpr arrayCreationExpr = expression.asArrayCreationExpr();
        ArrayCreationLevel arrayCreationLevel = arrayCreationExpr.getLevels().get(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(arrayCreationLevel);
        assertEquals(Arrays.asList("[", "]"),
                nodeText.getElements().stream().map(TextElement::expand).filter(e -> !e.isEmpty())
                        .collect(Collectors.toList()));
    }

    [TestMethod]
    void checkNodeTextCreatedArrayCreationLevelWith() {
        considerExpression("new int[123]");

        ArrayCreationExpr arrayCreationExpr = expression.asArrayCreationExpr();
        ArrayCreationLevel arrayCreationLevel = arrayCreationExpr.getLevels().get(0);
        NodeText nodeText = LexicalPreservingPrinter.getOrCreateNodeText(arrayCreationLevel);
        assertEquals(Arrays.asList("[", "123", "]"),
                nodeText.getElements().stream().map(TextElement::expand).filter(e -> !e.isEmpty())
                        .collect(Collectors.toList()));
    }

    //
    // Tests on findIndentation
    //

    [TestMethod]
    void findIndentationForAnnotationMemberDeclarationWithoutComment(){
        considerExample("AnnotationDeclaration_Example3_original");
        Node node = cu.getAnnotationDeclarationByName("ClassPreamble").get().getMember(4);
        List<TextElement> indentation = LexicalPreservingPrinter.findIndentation(node);
        assertEquals(Arrays.asList(" ", " ", " "),
                indentation.stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    [TestMethod]
    void findIndentationForAnnotationMemberDeclarationWithComment(){
        considerExample("AnnotationDeclaration_Example3_original");
        Node node = cu.getAnnotationDeclarationByName("ClassPreamble").get().getMember(5);
        List<TextElement> indentation = LexicalPreservingPrinter.findIndentation(node);
        assertEquals(Arrays.asList(" ", " ", " "),
                indentation.stream().map(TextElement::expand).collect(Collectors.toList()));
    }

    //
    // Tests on printing
    //

    [TestMethod]
    void printASuperSimpleCUWithoutChanges() {
        string code = "class A {}";
        considerCode(code);

        assertEquals(code, LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printASuperSimpleClassWithAFieldAdded() {
        string code = "class A {}";
        considerCode(code);

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        classA.addField("int", "myField");
        assertEquals("class A {" + SYSTEM_EOL + "    int myField;" + SYSTEM_EOL + "}", LexicalPreservingPrinter.print(classA));
    }

    [TestMethod]
    void printASuperSimpleClassWithoutChanges() {
        string code = "class A {}";
        considerCode(code);

        assertEquals(code, LexicalPreservingPrinter.print(cu.getClassByName("A").get()));
    }

    [TestMethod]
    void printASimpleCUWithoutChanges() {
        string code = "class /*a comment*/ A {\t\t" + SYSTEM_EOL + " int f;" + SYSTEM_EOL + SYSTEM_EOL + SYSTEM_EOL
                + "         void foo(int p  ) { return  'z'  \t; }}";
        considerCode(code);

        assertEquals(code, LexicalPreservingPrinter.print(cu));
        assertEquals(code, LexicalPreservingPrinter.print(cu.getClassByName("A").get()));
        assertEquals("void foo(int p  ) { return  'z'  \t; }",
                LexicalPreservingPrinter.print(cu.getClassByName("A").get().getMethodsByName("foo").get(0)));
    }

    [TestMethod]
    void printASimpleClassRemovingAField() {
        string code = "class /*a comment*/ A {\t\t" + SYSTEM_EOL +
                " int f;" + SYSTEM_EOL + SYSTEM_EOL + SYSTEM_EOL +
                "         void foo(int p  ) { return  'z'  \t; }}";
        considerCode(code);

        ClassOrInterfaceDeclaration c = cu.getClassByName("A").get();
        c.getMembers().remove(0);
        // This rendering is probably caused by the concret syntax model
        assertEquals("class /*a comment*/ A {\t\t" + SYSTEM_EOL +
        		SYSTEM_EOL +
                "         void foo(int p  ) { return  'z'  \t; }}", LexicalPreservingPrinter.print(c));
    }

    [TestMethod]
    void printASimpleClassRemovingAMethod() {
        string code = "class /*a comment*/ A {\t\t" + SYSTEM_EOL +
                " int f;" + SYSTEM_EOL + SYSTEM_EOL + SYSTEM_EOL +
                "         void foo(int p  ) { return  'z'  \t; }" + SYSTEM_EOL +
                " int g;}";
        considerCode(code);

        ClassOrInterfaceDeclaration c = cu.getClassByName("A").get();
        c.getMembers().remove(1);
        assertEquals("class /*a comment*/ A {\t\t" + SYSTEM_EOL +
                " int f;" + SYSTEM_EOL + SYSTEM_EOL + SYSTEM_EOL +
                " int g;}", LexicalPreservingPrinter.print(c));
    }

    [TestMethod]
    void printASimpleMethodAddingAParameterToAMethodWithZeroParameters() {
        string code = "class A { void foo() {} }";
        considerCode(code);

        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        m.addParameter("float", "p1");
        assertEquals("void foo(float p1) {}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodAddingAParameterToAMethodWithOneParameter() {
        string code = "class A { void foo(char p1) {} }";
        considerCode(code);

        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        m.addParameter("float", "p2");
        assertEquals("void foo(char p1, float p2) {}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodRemovingAParameterToAMethodWithOneParameter() {
        string code = "class A { void foo(float p1) {} }";
        considerCode(code);

        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        m.getParameters().remove(0);
        assertEquals("void foo() {}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodRemovingParameterOneFromMethodWithTwoParameters() {
        string code = "class A { void foo(char p1, int p2) {} }";
        considerCode(code);

        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        m.getParameters().remove(0);
        assertEquals("void foo(int p2) {}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodRemovingParameterTwoFromMethodWithTwoParameters() {
        string code = "class A { void foo(char p1, int p2) {} }";
        considerCode(code);

        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        m.getParameters().remove(1);
        assertEquals("void foo(char p1) {}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodAddingAStatement() {
        string code = "class A { void foo(char p1, int p2) {} }";
        considerCode(code);

        Statement s = new ExpressionStmt(new BinaryExpr(
                new IntegerLiteralExpr("10"), new IntegerLiteralExpr("2"), BinaryExpr.Operator.PLUS));
        NodeList<Statement> stmts = cu.getClassByName("A").get().getMethodsByName("foo").get(0).getBody().get()
                .getStatements();
        stmts.add(s);
        MethodDeclaration m = cu.getClassByName("A").get().getMethodsByName("foo").get(0);
        assertEquals("void foo(char p1, int p2) {" + SYSTEM_EOL +
                "    10 + 2;" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(m));
    }

    [TestMethod]
    void printASimpleMethodRemovingAStatementCRLF() {
        printASimpleMethodRemovingAStatement("\r\n");
    }

    [TestMethod]
    void printASimpleMethodRemovingAStatementLF() {
        printASimpleMethodRemovingAStatement("\n");
    }

    [TestMethod]
    void printASimpleMethodRemovingAStatementCR() {
        printASimpleMethodRemovingAStatement("\r");
    }

    private void printASimpleMethodRemovingAStatement(string eol) {
        considerCode("class A {" + eol
                + "\t" + "foo(int a, int b) {" + eol
                + "\t\t" + "int result = a * b;" + eol
                + "\t\t" + "return a * b;" + eol
                + "\t" + "}" + eol
                + "}");

        ExpressionStmt stmt = cu.findAll(ExpressionStmt.class).get(0);
        stmt.remove();

        assertEquals("class A {" + eol
                + "\t" + "foo(int a, int b) {" + eol
                + "\t\t" + "return a * b;" + eol
                + "\t" + "}" + eol
                + "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printASimpleImport() {
        string code = "import a.b.c.D;";
        considerCode(code);

        ImportDeclaration imp = (ImportDeclaration) cu.getChildNodes().get(0);
        assertEquals("import a.b.c.D;", LexicalPreservingPrinter.print(imp));
    }

    [TestMethod]
    void printAnotherImport() {
        string code = "import com.github.javaparser.ast.CompilationUnit;";
        considerCode(code);

        ImportDeclaration imp = (ImportDeclaration) cu.getChildNodes().get(0);
        assertEquals("import com.github.javaparser.ast.CompilationUnit;", LexicalPreservingPrinter.print(imp));
    }

    [TestMethod]
    void printAStaticImport() {
        string code = "import static com.github.javaparser.ParseStart.*;";
        considerCode(code);

        ImportDeclaration imp = (ImportDeclaration) cu.getChildNodes().get(0);
        assertEquals("import static com.github.javaparser.ParseStart.*;", LexicalPreservingPrinter.print(imp));
    }

    [TestMethod]
    void checkAnnidatedTypeParametersPrinting() {
        string code = "class A { private /*final*/Stack<Iterator<Triple>> its = new Stack<Iterator<Triple>>(); }";
        considerCode(code);
        assertEquals("class A { private /*final*/Stack<Iterator<Triple>> its = new Stack<Iterator<Triple>>(); }",
                LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printASingleCatch() {
        string code = "class A {{try { doit(); } catch (Exception e) {}}}";
        considerCode(code);

        assertEquals("class A {{try { doit(); } catch (Exception e) {}}}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printAMultiCatch() {
        string code = "class A {{try { doit(); } catch (Exception | AssertionError e) {}}}";
        considerCode(code);

        assertEquals("class A {{try { doit(); } catch (Exception | AssertionError e) {}}}",
                LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printASingleCatchType() {
        string code = "class A {{try { doit(); } catch (Exception e) {}}}";
        considerCode(code);
        InitializerDeclaration initializerDeclaration = (InitializerDeclaration) cu.getType(0).getMembers().get(0);
        TryStmt tryStmt = (TryStmt) initializerDeclaration.getBody().getStatements().get(0);
        CatchClause catchClause = tryStmt.getCatchClauses().get(0);
        Type catchType = catchClause.getParameter().getType();

        assertEquals("Exception", LexicalPreservingPrinter.print(catchType));
    }

    [TestMethod]
    void printUnionType() {
        string code = "class A {{try { doit(); } catch (Exception | AssertionError e) {}}}";
        considerCode(code);
        InitializerDeclaration initializerDeclaration = (InitializerDeclaration) cu.getType(0).getMembers().get(0);
        TryStmt tryStmt = (TryStmt) initializerDeclaration.getBody().getStatements().get(0);
        CatchClause catchClause = tryStmt.getCatchClauses().get(0);
        UnionType unionType = (UnionType) catchClause.getParameter().getType();

        assertEquals("Exception | AssertionError", LexicalPreservingPrinter.print(unionType));
    }

    [TestMethod]
    void printParameterHavingUnionType() {
        string code = "class A {{try { doit(); } catch (Exception | AssertionError e) {}}}";
        considerCode(code);
        InitializerDeclaration initializerDeclaration = (InitializerDeclaration) cu.getType(0).getMembers().get(0);
        TryStmt tryStmt = (TryStmt) initializerDeclaration.getBody().getStatements().get(0);
        CatchClause catchClause = tryStmt.getCatchClauses().get(0);
        Parameter parameter = catchClause.getParameter();

        assertEquals("Exception | AssertionError e", LexicalPreservingPrinter.print(parameter));
    }

    [TestMethod]
    void printLambaWithUntypedParams() {
        string code = "class A {Function<String,String> f = a -> a;}";
        considerCode(code);

        assertEquals("class A {Function<String,String> f = a -> a;}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printAModuleInfoSpecificKeywordUsedAsIdentifier1() {
        considerCode("class module { }");

        cu.getClassByName("module").get().setName("xyz");

        assertEquals("class xyz { }", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printAModuleInfoSpecificKeywordUsedAsIdentifier2() {
        considerCode("class xyz { }");

        cu.getClassByName("xyz").get().setName("module");

        assertEquals("class module { }", LexicalPreservingPrinter.print(cu));
    }

    // Issue 823: setPackageDeclaration on CU starting with a comment
    [TestMethod]
    void reactToSetPackageDeclarationOnCuStartingWithComment() {
        considerCode("// Hey, this is a comment\n" +
                "\n" +
                "\n" +
                "// Another one\n" +
                "\n" +
                "class A {}");
        cu.setPackageDeclaration("org.javaparser.lexicalpreservation.examples");
    }

    [TestMethod]
    void printLambdaIntersectionTypeAssignment() {
        string code = "class A {" + SYSTEM_EOL +
                "  void f() {" + SYSTEM_EOL +
                "    Runnable r = (Runnable & Serializable) (() -> {});" + SYSTEM_EOL +
                "    r = (Runnable & Serializable)() -> {};" + SYSTEM_EOL +
                "    r = (Runnable & I)() -> {};" + SYSTEM_EOL +
                "  }}";
        considerCode(code);

        assertEquals(code, LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void printLambdaIntersectionTypeReturn() {
        string code = "class A {" + SYSTEM_EOL
                + "  Object f() {" + SYSTEM_EOL
                + "    return (Comparator<Map.Entry<K, V>> & Serializable)(c1, c2) -> c1.getKey().compareTo(c2.getKey()); "
                + SYSTEM_EOL
                + "}}";
        considerCode(code);

        assertEquals(code, LexicalPreservingPrinter.print(cu));
    }

    // See issue #855
    [TestMethod]
    void handleOverrideAnnotation() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> {
                            if (member is MethodDeclaration) {
                                MethodDeclaration methodDeclaration = (MethodDeclaration) member;
                                if (!methodDeclaration.getAnnotationByName("Override").isPresent()) {
                                    methodDeclaration.addAnnotation("Override");
                                }
                            }
                        }));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void preserveSpaceAsIsForASimpleClassWithMoreFormatting(){
        considerExample("ASimpleClassWithMoreFormatting");
        assertEquals(readExample("ASimpleClassWithMoreFormatting"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void renameASimpleClassWithMoreFormatting(){
        considerExample("ASimpleClassWithMoreFormatting");

        cu.getClassByName("ASimpleClass").get()
                .setName("MyRenamedClass");
        assertEquals(readExample("ASimpleClassWithMoreFormatting_step1"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void theLexicalPreservationStringForAnAddedMethodShouldBeIndented(){
        considerExample("ASimpleClassWithMoreFormatting");

        cu.getClassByName("ASimpleClass").get()
                .setName("MyRenamedClass");
        MethodDeclaration setter = cu
                .getClassByName("MyRenamedClass").get()
                .addMethod("setAField", PUBLIC);
        assertEquals("public void setAField() {" + SYSTEM_EOL +
                "    }", LexicalPreservingPrinter.print(setter));
    }

    [TestMethod]
    void addMethodToASimpleClassWithMoreFormatting(){
        considerExample("ASimpleClassWithMoreFormatting");

        cu.getClassByName("ASimpleClass").get()
                .setName("MyRenamedClass");
        MethodDeclaration setter = cu
                .getClassByName("MyRenamedClass").get()
                .addMethod("setAField", PUBLIC);
        TestUtils.assertEqualsStringIgnoringEol(readExample("ASimpleClassWithMoreFormatting_step2"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void addingParameterToAnAddedMethodInASimpleClassWithMoreFormatting(){
        considerExample("ASimpleClassWithMoreFormatting");

        cu.getClassByName("ASimpleClass").get()
                .setName("MyRenamedClass");
        MethodDeclaration setter = cu
                .getClassByName("MyRenamedClass").get()
                .addMethod("setAField", PUBLIC);
        setter.addParameter("boolean", "aField");
        TestUtils.assertEqualsStringIgnoringEol(readExample("ASimpleClassWithMoreFormatting_step3"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void findIndentationOfEmptyMethod(){
        considerExample("ASimpleClassWithMoreFormatting_step3");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        assertEquals(4, LexicalPreservingPrinter.findIndentation(setter).size());
        assertEquals(4, LexicalPreservingPrinter.findIndentation(setter.getBody().get()).size());
    }

    [TestMethod]
    void findIndentationOfMethodWithStatements(){
        considerExample("ASimpleClassWithMoreFormatting_step4");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        assertEquals(4, LexicalPreservingPrinter.findIndentation(setter).size());
        assertEquals(4, LexicalPreservingPrinter.findIndentation(setter.getBody().get()).size());
        assertEquals(8, LexicalPreservingPrinter.findIndentation(setter.getBody().get().getStatement(0)).size());
    }

    [TestMethod]
    void addingStatementToAnAddedMethodInASimpleClassWithMoreFormatting(){
        considerExample("ASimpleClassWithMoreFormatting");

        cu.getClassByName("ASimpleClass").get()
                .setName("MyRenamedClass");
        MethodDeclaration setter = cu
                .getClassByName("MyRenamedClass").get()
                .addMethod("setAField", PUBLIC);
        setter.addParameter("boolean", "aField");
        setter.getBody().get().getStatements().add(new ExpressionStmt(
                new AssignExpr(
                        new FieldAccessExpr(new ThisExpr(), "aField"),
                        new NameExpr("aField"),
                        AssignExpr.Operator.ASSIGN)));
        TestUtils.assertEqualsStringIgnoringEol(readExample("ASimpleClassWithMoreFormatting_step4"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void addingStatementToAnAddedMethodInASimpleClassWithMoreFormattingFromStep3(){
        considerExample("ASimpleClassWithMoreFormatting_step3");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        setter.getBody().get().getStatements().add(new ExpressionStmt(
                new AssignExpr(
                        new FieldAccessExpr(new ThisExpr(), "aField"),
                        new NameExpr("aField"),
                        AssignExpr.Operator.ASSIGN)));
        assertEquals(readExample("ASimpleClassWithMoreFormatting_step4"), LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void nodeTextForMethod(){
        considerExample("ASimpleClassWithMoreFormatting_step4");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        NodeText nodeText;

        nodeText = getTextForNode(setter);
        int index = 0;
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(Modifier.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(VoidType.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(SimpleName.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.LPAREN));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(Parameter.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.RPAREN));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(BlockStmt.class));
        assertEquals(index, nodeText.getElements().size());

        nodeText = getTextForNode(setter.getBody().get());
        index = 0;
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.LBRACE));
        assertTrue(nodeText.getElements().get(index++).isNewline());
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(ExpressionStmt.class));
        assertTrue(nodeText.getElements().get(index++).isNewline());
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.RBRACE));
        assertEquals(index, nodeText.getElements().size());

        nodeText = getTextForNode(setter.getBody().get().getStatement(0));
        index = 0;
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(AssignExpr.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SEMICOLON));
        assertEquals(index, nodeText.getElements().size());
    }

    [TestMethod]
    void nodeTextForModifiedMethod(){
        considerExample("ASimpleClassWithMoreFormatting_step3");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        setter.getBody().get().getStatements().add(new ExpressionStmt(
                new AssignExpr(
                        new FieldAccessExpr(new ThisExpr(), "aField"),
                        new NameExpr("aField"),
                        AssignExpr.Operator.ASSIGN)));
        NodeText nodeText;

        nodeText = getTextForNode(setter);
        int index = 0;
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(Modifier.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(VoidType.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(SimpleName.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.LPAREN));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(Parameter.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.RPAREN));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(BlockStmt.class));
        assertEquals(index, nodeText.getElements().size());

        nodeText = getTextForNode(setter.getBody().get());
        index = 0;
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.LBRACE));
        assertTrue(nodeText.getElements().get(index++).isNewline());
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(ExpressionStmt.class));
        assertTrue(nodeText.getElements().get(index++).isNewline());
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SPACE));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.RBRACE));
        assertEquals(index, nodeText.getElements().size());

        nodeText = LexicalPreservingPrinter.getOrCreateNodeText(setter.getBody().get().getStatement(0));
        index = 0;
        assertTrue(nodeText.getElements().get(index++).isChildOfClass(AssignExpr.class));
        assertTrue(nodeText.getElements().get(index++).isToken(GeneratedJavaParserConstants.SEMICOLON));
        assertEquals(index, nodeText.getElements().size());
    }

    // See issue #926
    [TestMethod]
    void addASecondStatementToExistingMethod(){
        considerExample("MethodWithOneStatement");

        MethodDeclaration methodDeclaration = cu.getType(0).getMethodsByName("someMethod").get(0);
        methodDeclaration.getBody().get().getStatements().add(new ExpressionStmt(
                new VariableDeclarationExpr(
                        new VariableDeclarator(
                                parseClassOrInterfaceType("String"),
                                "test2",
                                new StringLiteralExpr("")))));
        TestUtils.assertEqualsStringIgnoringEol("public void someMethod() {" + SYSTEM_EOL
                + "        string test = \"\";" + SYSTEM_EOL
                + "        string test2 = \"\";" + SYSTEM_EOL
                // HACK: The right closing brace should not have indentation
                // because the original method did not introduce indentation,
                // however due to necessity this test was left with indentation,
                // _in a later version it should be revised.
                + "    }", LexicalPreservingPrinter.print(methodDeclaration));
    }

    // See issue #866
    [TestMethod]
    void moveOverrideAnnotations() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected @Override void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> member.ifMethodDeclaration(methodDeclaration -> {
                            if (methodDeclaration.getAnnotationByName("Override").isPresent()) {

                                while (methodDeclaration.getAnnotations().isNonEmpty()) {
                                    AnnotationExpr annotationExpr = methodDeclaration.getAnnotations().get(0);
                                    annotationExpr.remove();
                                }

                                methodDeclaration.addMarkerAnnotation("Override");
                            }
                        })));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    // See issue #866
    [TestMethod]
    void moveOrAddOverrideAnnotations() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected @Override void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> {
                            if (member is MethodDeclaration) {
                                MethodDeclaration methodDeclaration = (MethodDeclaration) member;
                                if (methodDeclaration.getAnnotationByName("Override").isPresent()) {

                                    while (methodDeclaration.getAnnotations().isNonEmpty()) {
                                        AnnotationExpr annotationExpr = methodDeclaration.getAnnotations().get(0);
                                        annotationExpr.remove();
                                    }
                                }
                                methodDeclaration.addMarkerAnnotation("Override");
                            }
                        }));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    // See issue #865
    [TestMethod]
    void handleAddingMarkerAnnotation() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> {
                            if (member is MethodDeclaration) {
                                MethodDeclaration methodDeclaration = (MethodDeclaration) member;
                                if (!methodDeclaration.getAnnotationByName("Override").isPresent()) {
                                    methodDeclaration.addMarkerAnnotation("Override");
                                }
                            }
                        }));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    // See issue #865
    [TestMethod]
    void handleOverrideMarkerAnnotation() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> member.ifMethodDeclaration(
                                methodDeclaration -> methodDeclaration.addMarkerAnnotation("Override"))));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    // See issue #865
    [TestMethod]
    void handleOverrideAnnotationAlternative() {
        considerCode("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}");

        cu.getTypes()
                .forEach(type -> type.getMembers()
                        .forEach(member -> member.ifMethodDeclaration(
                                methodDeclaration -> methodDeclaration.addAnnotation("Override"))));
        assertEquals("public class TestPage:Page {" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void test() {}" + SYSTEM_EOL +
                SYSTEM_EOL +
                "   @Override" + SYSTEM_EOL +
                "   protected void initializePage() {}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void invokeModifierVisitor() {
        considerCode("class A {" + SYSTEM_EOL
                + "  Object f() {" + SYSTEM_EOL
                + "    return (Comparator<Map.Entry<K, V>> & Serializable)(c1, c2) -> c1.getKey().compareTo(c2.getKey()); "
                + SYSTEM_EOL
                + "}}");
        cu.accept(new ModifierVisitor<>(), null);
    }

    [TestMethod]
    void handleDeprecatedAnnotationFinalClass() {
        considerCode("public /*final*/class A {}");

        cu.getTypes().forEach(type -> type.addAndGetAnnotation(Deprecated.class));

        assertEquals("//@Deprecated" + SYSTEM_EOL +
                "public /*final*/class A {}", LexicalPreservingPrinter.print(cu));

    }

    [TestMethod]
    void handleDeprecatedAnnotationAbstractClass() {
    	considerCode("public abstract class A {}");

        cu.getTypes().forEach(type -> type.addAndGetAnnotation(Deprecated.class));

        assertEquals("//@Deprecated" + SYSTEM_EOL +
                "public abstract class A {}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void issue1244() {
    	considerCode("public class Foo {" + SYSTEM_EOL + SYSTEM_EOL
                + "// Some comment" + SYSTEM_EOL + SYSTEM_EOL // does work with only one \n
                + "public void writeExternal() {}" + SYSTEM_EOL + "}");

        cu.findAll(ClassOrInterfaceDeclaration.class).forEach(c -> {
            List<MethodDeclaration> methods = c.getMethodsByName("writeExternal");
            for (MethodDeclaration method : methods) {
                c.remove(method);
            }
        });
        assertEqualsStringIgnoringEol("public class Foo {\n" +
                "// Some comment\n\n" +
                "}", LexicalPreservingPrinter.print(cu));
    }

    static class AddFooCallModifierVisitor:ModifierVisitor<Void> {
        //@Override
        public Visitable visit(MethodCallExpr n, Void arg) {
            // Add a call to foo() on every found method call
            return new MethodCallExpr(n, "foo");
        }
    }

    // See issue 1277
    [TestMethod]
    void testInvokeModifierVisitor() {
    	considerCode("class A {" + SYSTEM_EOL +
                "  public string message = \"hello\";" + SYSTEM_EOL +
                "   void bar() {" + SYSTEM_EOL +
                "     System._out.println(\"hello\");" + SYSTEM_EOL +
                "   }" + SYSTEM_EOL +
                "}");

        cu.accept(new AddFooCallModifierVisitor(), null);
    }

    static class CallModifierVisitor:ModifierVisitor<Void> {
        //@Override
        public Visitable visit(MethodCallExpr n, Void arg) {
            // Add a call to foo() on every found method call
            return new MethodCallExpr(n.clone(), "foo");
        }
    }

    [TestMethod]
    void invokeModifierVisitorIssue1297() {
    	considerCode("class A {" + SYSTEM_EOL +
                "   public void bar() {" + SYSTEM_EOL +
                "     System._out.println(\"hello\");" + SYSTEM_EOL +
                "     System._out.println(\"hello\");" + SYSTEM_EOL +
                "     // comment" + SYSTEM_EOL +
                "   }" + SYSTEM_EOL +
                "}");

        cu.accept(new CallModifierVisitor(), null);
    }

    [TestMethod]
    void addedBlockCommentsPrinted() {
    	considerCode("public class Foo { }");

        cu.getClassByName("Foo").get()
                .addMethod("mymethod")
                .setBlockComment("block");
        assertEqualsStringIgnoringEol("public class Foo {" + SYSTEM_EOL +
                "    /*block*/" + SYSTEM_EOL +
                "    void mymethod() {" + SYSTEM_EOL +
                "    }" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void addedLineCommentsPrinted() {
    	considerCode("public class Foo { }");

        cu.getClassByName("Foo").get()
                .addMethod("mymethod")
                .setLineComment("line");
        assertEqualsStringIgnoringEol("public class Foo {" + SYSTEM_EOL +
                "    //line" + SYSTEM_EOL +
                "    void mymethod() {" + SYSTEM_EOL +
                "    }" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void removedLineCommentsPrinted() {
    	considerCode("public class Foo {" + SYSTEM_EOL +
                "//line" + SYSTEM_EOL +
                "void mymethod() {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL +
                "}");
        cu.getAllContainedComments().get(0).remove();

        assertEqualsStringIgnoringEol("public class Foo {" + SYSTEM_EOL +
                "void mymethod() {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    // Checks if comments get removed properly with Unix style line endings
    [TestMethod]
    void removedLineCommentsPrintedUnix() {
    	considerCode("public class Foo {" + "\n" +
                "//line" + "\n" +
                "void mymethod() {" + "\n" +
                "}" + "\n" +
                "}");
        cu.getAllContainedComments().get(0).remove();

        assertEquals("public class Foo {" + "\n" +
                "void mymethod() {" + "\n" +
                "}" + "\n" +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void removedBlockCommentsPrinted() {
    	considerCode("public class Foo {" + SYSTEM_EOL +
                "/*" + SYSTEM_EOL +
                "Block comment coming through" + SYSTEM_EOL +
                "*/" + SYSTEM_EOL +
                "void mymethod() {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL +
                "}");
        cu.getAllContainedComments().get(0).remove();

        assertEqualsStringIgnoringEol("public class Foo {" + SYSTEM_EOL +
                "void mymethod() {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL +
                "}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void testFixIndentOfMovedNode() {
        try {
        	considerExample("FixIndentOfMovedNode");

            cu.getClassByName("ThisIsASampleClass").get()
                    .getMethodsByName("longerMethod")
                    .get(0)
                    .setBlockComment("Lorem ipsum dolor sit amet, consetetur sadipscing elitr.");

            cu.getClassByName("Foo").get()
                    .getFieldByName("myFoo")
                    .get()
                    .setLineComment("sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat");

            string expectedCode = readExample("FixIndentOfMovedNodeExpected");
            assertEquals(expectedCode, LexicalPreservingPrinter.print(cu));
        } catch (IOException ex) {
            fail("Could not read test code", ex);
        }
    }

    [TestMethod]
    void issue1321() {
    	considerCode("class X { X() {} private void testme() {} }");

        ClassOrInterfaceDeclaration type = cu.getClassByName("X").get();
        type.getConstructors().get(0).setBody(new BlockStmt().addStatement("testme();"));

        assertEqualsStringIgnoringEol("class X { X() {\n    testme();\n} private void testme() {} }",
                LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void issue2001() {
    	considerCode("class X {void blubb(){X.p(\"blaubb04\");}}");

        cu.findAll(MethodCallExpr.class).forEach(Node::removeForced);

        assertEqualsStringIgnoringEol("class X {void blubb(){}}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void testIndentOfCodeBlocks(){
        considerExample("IndentOfInsertedCodeBlocks");

        IfStmt ifStmt = new IfStmt();
        ifStmt.setCondition(StaticJavaParser.parseExpression("name.equals(\"foo\")"));
        BlockStmt blockStmt = new BlockStmt();
        blockStmt.addStatement(StaticJavaParser.parseStatement("int i = 0;"));
        blockStmt.addStatement(StaticJavaParser.parseStatement("System._out.println(i);"));
        blockStmt.addStatement(
                new IfStmt().setCondition(StaticJavaParser.parseExpression("i < 0"))
                        .setThenStmt(new BlockStmt().addStatement(StaticJavaParser.parseStatement("i = 0;"))));
        blockStmt.addStatement(StaticJavaParser.parseStatement("new Object(){};"));
        ifStmt.setThenStmt(blockStmt);
        ifStmt.setElseStmt(new BlockStmt());

        cu.findFirst(BlockStmt.class).get().addStatement(ifStmt);
        string expected = considerExample("IndentOfInsertedCodeBlocksExpected");
        TestUtils.assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    void commentAddedAtTopLevel() {
        considerCode("package x;class X{}");

        cu.setComment(new LineComment("Bla"));
        assertEqualsStringIgnoringEol("//Bla\npackage x;class X{}", LexicalPreservingPrinter.print(cu));

        cu.setComment(new LineComment("BlaBla"));
        assertEqualsStringIgnoringEol("//BlaBla\npackage x;class X{}", LexicalPreservingPrinter.print(cu));

        cu.removeComment();
        assertEqualsStringIgnoringEol("package x;class X{}", LexicalPreservingPrinter.print(cu));
    }

    [TestMethod]
    public void testReplaceStringLiteral() {
        considerExpression("\"asd\"");
        /*final*/string expected = "\"REPLACEMENT\"";

        assertTrue(expression.isStringLiteralExpr());
        StringLiteralExpr sle = (StringLiteralExpr) expression;
        sle.setValue("REPLACEMENT");

        /*final*/string actual = LexicalPreservingPrinter.print(expression);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceStringLiteralWithinStatement() {
        considerStatement("string str = \"aaa\";");
        string expected = "string str = \"REPLACEMENT\";";

        statement.findAll(StringLiteralExpr.class).forEach(stringLiteralExpr -> {
            stringLiteralExpr.setValue("REPLACEMENT");
        });

        assertEquals(expected, LexicalPreservingPrinter.print(statement));
        assertEquals(expected, statement.toString());
    }

    [TestMethod]
    public void testReplaceClassName() {
    	considerCode("class A {}");

        assertEquals(1, cu.findAll(ClassOrInterfaceDeclaration.class).size());
        cu.findAll(ClassOrInterfaceDeclaration.class).forEach(coid -> coid.setName("B"));

        /*final*/string expected = "class B {}";

        /*final*/string actual = LexicalPreservingPrinter.print(cu);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceIntLiteral() {
        considerExpression("5");
        /*final*/string expected = "10";

        assertTrue(expression.isIntegerLiteralExpr());
        ((IntegerLiteralExpr) expression).setValue("10");

        /*final*/string actual = LexicalPreservingPrinter.print(expression);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceLongLiteral() {
        considerStatement("long x = 5L;");
        string expected = "long x = 10L;";

        statement.findAll(LongLiteralExpr.class).forEach(longLiteralExpr -> {
            longLiteralExpr.setValue("10L");
        });

        /*final*/string actual = LexicalPreservingPrinter.print(statement);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceBooleanLiteral() {
        considerStatement("bool x = true;");
        string expected = "bool x = false;";

        statement.findAll(BooleanLiteralExpr.class).forEach(booleanLiteralExpr -> {
            booleanLiteralExpr.setValue(false);
        });

        /*final*/string actual = LexicalPreservingPrinter.print(statement);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceDoubleLiteral() {
        considerStatement("double x = 5.0D;");
        string expected = "double x = 10.0D;";

        statement.findAll(DoubleLiteralExpr.class).forEach(doubleLiteralExpr -> {
            doubleLiteralExpr.setValue("10.0D");
        });

        /*final*/string actual = LexicalPreservingPrinter.print(statement);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceCharLiteral() {
        considerStatement("char x = 'a';");
        string expected = "char x = 'b';";

        statement.findAll(CharLiteralExpr.class).forEach(charLiteralExpr -> {
            charLiteralExpr.setValue("b");
        });

        /*final*/string actual = LexicalPreservingPrinter.print(statement);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceCharLiteralUnicode() {
        considerStatement("char x = 'a';");
        string expected = "char x = '\\u0000';";

        statement.findAll(CharLiteralExpr.class).forEach(charLiteralExpr -> {
            charLiteralExpr.setValue("\\u0000");
        });

        /*final*/string actual = LexicalPreservingPrinter.print(statement);
        assertEquals(expected, actual);
    }

    [TestMethod]
    public void testReplaceTextBlockLiteral() {
        /*final*/JavaParser javaParser = new JavaParser(
                new ParserConfiguration()
                        .setLexicalPreservationEnabled(true)
                        .setLanguageLevel(ParserConfiguration.LanguageLevel.JAVA_14)
        );

        string code = "string x = \"\"\"a\"\"\";";
        string expected = "string x = \"\"\"\n" +
                "     REPLACEMENT\n" +
                "     \"\"\";";

        /*final*/Statement b = javaParser.parseStatement(code).getResult().orElseThrow(AssertionError::new);
        b.findAll(TextBlockLiteralExpr.class).forEach(textblockLiteralExpr -> {
            textblockLiteralExpr.setValue("\n     REPLACEMENT\n     ");
        });

        /*final*/string actual = LexicalPreservingPrinter.print(b);
        assertEquals(expected, actual);
    }
    
    [TestMethod]
	void testTextBlockSupport() {
		string code = 
				"string html = \"\"\"\n" +
                "  <html>\n" +
                "    <body>\n" +
                "      <p>Hello, world</p>\n" +
                "    </body>\n" +
                "  </html>\n" +
                "\"\"\";";
		string expected =
				"string html = \"\"\"\r\n"
				+ "  <html>\r\n"
				+ "    <body>\r\n"
				+ "      <p>Hello, world</p>\r\n"
				+ "    </body>\r\n"
				+ "  </html>\r\n"
				+ "\"\"\";";
		/*final*/JavaParser javaParser = new JavaParser(
                new ParserConfiguration()
                        .setLexicalPreservationEnabled(true)
                        .setLanguageLevel(ParserConfiguration.LanguageLevel.JAVA_15)
        );
		Statement stmt = javaParser.parseStatement(code).getResult().orElseThrow(AssertionError::new);
		LexicalPreservingPrinter.setup(stmt);
		assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(stmt));
	}

    [TestMethod]
    void testArrayPreservation_WithSingleLanguageStyle() {

        // Given
        considerCode("class Test {\n" +
                    "  int[] foo;\n" +
                    "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int[] foo;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);
    }

    [TestMethod]
    void testArrayPreservation_WithMultipleLanguageStyle() {

        // Given
        considerCode("class Test {\n" +
                    "  int[][] foo;\n" +
                    "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int[][] foo;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);
    }

    [TestMethod]
    void testArrayPreservation_WithSingleCLanguageStyle() {

        // Given
        considerCode("class Test {\n" +
                    "  int foo[];\n" +
                    "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int foo[];\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);
    }

    /**
     * Given a field that have arrays declared _in C style and
     * When a marker annotation is added to the code
     * Assert that the result matches the expected.
     *
     * Issue: 3419
     */
    [TestMethod]
    void testArrayPreservation_WithMultipleCLanguageStyle() {

        // Given
        considerCode("class Test {\n" +
                     "  int foo[][];\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int foo[][];\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);
    }

    [TestMethod]
    void testArrayPreservation_WithSingleBracketWithoutSpace() {

        // Given
        considerCode("class Test {\n" +
                     "  int[]foo;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int[]foo;\n" +
                                 "}";
        assertTransformedToString(expectedCode, cu);
    }

    [TestMethod]
    void testArrayPreservation_WithMultipleBracketWithoutSpace() {

        // Given
        considerCode("class Test {\n" +
                     "  int[][]foo;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  int[][]foo;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);
    }
    
    [TestMethod]
    void testClassOrInterfacePreservationWithFullyQualifiedName_SingleType() {
        // Given
        considerCode("class Test {\n" +
                     "  java.lang.Object foo;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        // modification of the AST
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  java.lang.Object foo;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    [TestMethod]
    void testClassOrInterfacePreservationWithFullyQualifiedName_ArrayType() {
        // Given
        considerCode("class Test {\n" +
                     "  java.lang.Object[] foo;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        // modification of the AST
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  java.lang.Object[] foo;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    [TestMethod]
    void testClassOrInterfacePreservationWithFullyQualifiedName_MultipleVariablesDeclarationWithSameType() {
        // Given
        considerCode("class Test {\n" +
                     "  java.lang.Object[] foo, bar;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        // modification of the AST
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  java.lang.Object[] foo, bar;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    [TestMethod]
    void testClassOrInterfacePreservationWithFullyQualifiedName_MultipleVariablesDeclarationwithDifferentType() {
        // Given
        considerCode("class Test {\n" +
                     "  java.lang.Object foo[], bar;\n" +
                     "}");

        // When
        FieldDeclaration fooField = cu.findFirst(FieldDeclaration.class).orElseThrow(AssertionError::new);
        // modification of the AST
        fooField.addMarkerAnnotation("Nullable");

        // Assert
        string expectedCode =   "class Test {\n" +
                                "  @Nullable\n" +
                                "  java.lang.Object foo[], bar;\n" +
                                "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    // issue 3588 Modifier is removed when removing an annotation. 
    [TestMethod]
    void testRemovingInlinedAnnotation() {
        // Given
        considerCode("public class Foo{\n"
                + "     protected @Nullable Object bar;\n"
                + "}");

        // When
        FieldDeclaration fd = cu.findFirst(FieldDeclaration.class).get();
        // modification of the AST
        AnnotationExpr ae = fd.getAnnotations().get(0);
        ae.remove();

        // Assert
        string expectedCode =   "public class Foo{\n"
                + "     protected Object bar;\n"
                + "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    // issue 3588 Modifier is removed when removing an annotation. 
    [TestMethod]
    void testRemovingInlinedAnnotation_alternate_case() {
        // Given
        considerCode("public class Foo{\n"
                + "     @Nullable protected Object bar;\n"
                + "}");

        // When
        FieldDeclaration fd = cu.findFirst(FieldDeclaration.class).get();
        // modification of the AST
        AnnotationExpr ae = fd.getAnnotations().get(0);
        ae.remove();

        // Assert
        string expectedCode =   "public class Foo{\n"
                + "     protected Object bar;\n"
                + "}";
        assertTransformedToString(expectedCode, cu);

    }
    
    // issue 3216 LexicalPreservingPrinter add Wrong indentation when removing comments
    [TestMethod]
    void removedIndentationLineCommentsPrinted() {
		considerCode("public class Foo {\n" +
    			"  //line \n" +
    			"  void mymethod() {\n" +
    			"  }\n" +
    			"}");
		string expected =
				"public class Foo {\n" + 
		    	"  void mymethod() {\n" +
		    	"  }\n" +
		    	"}";
    	cu.getAllContainedComments().get(0).remove();
    	assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }
    
    // issue 3216 LexicalPreservingPrinter add Wrong indentation when removing comments
    [TestMethod]
    void removedIndentationBlockCommentsPrinted() {
    	considerCode("public class Foo {\n" +
    			"  /*\n" +
    			"  *Block comment coming through\n" +
    			"  */\n" +
    			"  void mymethod() {\n" +
    			"  }\n" +
    			"}");
    	string expected =
    			"public class Foo {\n" +
    	    	"  void mymethod() {\n" +
    	    	"  }\n" +
    	    	"}";
    	cu.getAllContainedComments().get(0).remove();
    	
    	assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }
    
 // issue 3216 LexicalPreservingPrinter add Wrong indentation when removing comments
    [TestMethod]
    void removedIndentationJavaDocCommentsPrinted() {
        considerCode("public class Foo {\n" +
                "  /**\n" +
                "   *JavaDoc comment coming through\n" +
                "   */\n" +
                "  void mymethod() {\n" +
                "  }\n" +
                "}");
        string expected =
        		"public class Foo {\n" +
                "  void mymethod() {\n" +
                "  }\n" +
                "}";
        cu.getAllContainedComments().get(0).remove();

        assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }
    
    // issue 3800 determine whether active
    [TestMethod]
    void checkLPPIsAvailableOnNode() {
        string code = "class A {void foo(int p1, float p2) { }}";
        CompilationUnit cu = StaticJavaParser.parse(code);
        MethodDeclaration md = cu.findFirst(MethodDeclaration.class).get();
        LexicalPreservingPrinter.setup(md);
        
        assertTrue(LexicalPreservingPrinter.isAvailableOn(md));
        assertFalse(LexicalPreservingPrinter.isAvailableOn(cu));
    }

}
