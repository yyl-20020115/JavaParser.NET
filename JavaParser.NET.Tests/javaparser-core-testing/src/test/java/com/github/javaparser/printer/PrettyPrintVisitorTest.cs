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

namespace com.github.javaparser.printer;




class PrettyPrintVisitorTest:TestParser {

    private Optional<ConfigurationOption> getOption(PrinterConfiguration config, ConfigOption cOption) {
        return config.get(new DefaultConfigurationOption(cOption));
    }

    [TestMethod]
    void getMaximumCommonTypeWithoutAnnotations() {
        VariableDeclarationExpr vde1 = parseVariableDeclarationExpr("int a[], b[]");
        assertEquals("int[]", vde1.getMaximumCommonType().get().toString());

        VariableDeclarationExpr vde2 = parseVariableDeclarationExpr("int[][] a[], b[]");
        assertEquals("int[][][]", vde2.getMaximumCommonType().get().toString());

        VariableDeclarationExpr vde3 = parseVariableDeclarationExpr("int[][] a, b[]");
        assertEquals("int[][]", vde3.getMaximumCommonType().get().toString());
    }

    [TestMethod]
    void getMaximumCommonTypeWithAnnotations() {
        VariableDeclarationExpr vde1 = parseVariableDeclarationExpr("int a @Foo [], b[]");
        assertEquals("int", vde1.getMaximumCommonType().get().toString());

        VariableDeclarationExpr vde2 = parseVariableDeclarationExpr("int[]@Foo [] a[], b[]");
        assertEquals("int[][] @Foo []", vde2.getMaximumCommonType().get().toString());
    }

    private string print(Node node) {
        return new DefaultPrettyPrinter().print(node);
    }

    private string print(Node node, PrinterConfiguration conf) {
        return new DefaultPrettyPrinter(conf).print(node);
    }


    [TestMethod]
    void printSimpleClassExpr() {
        ClassExpr expr = parseExpression("Foo.class");
        assertEquals("Foo.class", print(expr));
    }


    /**
     * Here is a simple test according to R0 (removing spaces)
     */
    [TestMethod]
    void printOperatorsR0(){
        PrinterConfiguration conf1 = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement1 = parseStatement("a = 1 + 1;");
        assertEquals("a=1+1;", print(statement1, conf1));
    }

    /**
     * Here we test different operators according to requirement R1 (handling different operators)
     */
    [TestMethod]
    void printOperatorsR1(){

        Statement statement1 = parseStatement("a = 1 + 1;");
        assertEquals("a = 1 + 1;", print(statement1));

        Statement statement2 = parseStatement("a = 1 - 1;");
        assertEquals("a = 1 - 1;", print(statement2));

        Statement statement3 = parseStatement("a = 1 * 1;");
        assertEquals("a = 1 * 1;", print(statement3));

        Statement statement4 = parseStatement("a = 1 % 1;");
        assertEquals("a = 1 % 1;", print(statement4));

        Statement statement5 = parseStatement("a=1/1;");
        assertEquals("a = 1 / 1;", print(statement5));

        Statement statement6 = parseStatement("if (1 > 2 && 1 < 3 || 1 < 3){}");
        assertEquals("if (1 > 2 && 1 < 3 || 1 < 3) {" + SYSTEM_EOL
                + "}", print(statement6));

    }

    /**
     * Here is a simple test according to R2 (that it should be optional/modifiable)
     */
    [TestMethod]
    void printOperatorsR2(){
        PrinterConfiguration conf1 = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement1 = parseStatement("a = 1 + 1;");
        assertEquals("a=1+1;", print(statement1, conf1));

        PrinterConfiguration conf2 = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement2 = parseStatement("a=1+1;");
        assertEquals("a=1+1;", print(statement2, conf2));

        PrinterConfiguration conf3 = new DefaultPrinterConfiguration().addOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement3 = parseStatement("a = 1 + 1;");
        assertEquals("a = 1 + 1;", print(statement3, conf3));

        PrinterConfiguration conf4 = new DefaultPrinterConfiguration().addOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement4 = parseStatement("a=1+1;");
        assertEquals("a = 1 + 1;", print(statement4, conf4));

    }

    [TestMethod]
    void printOperatorA(){
        PrinterConfiguration conf = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        Statement statement6 = parseStatement("if(1>2&&1<3||1<3){}");
        assertEquals("if (1>2&&1<3||1<3) {" + SYSTEM_EOL
                + "}", print(statement6, conf));
    }

    [TestMethod]
    void printOperator2(){
        Expression expression = parseExpression("1+1");
        PrinterConfiguration spaces = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.SPACE_AROUND_OPERATORS));
        assertEquals("1+1", print(expression, spaces));
    }

    [TestMethod]
    void printArrayClassExpr() {
        ClassExpr expr = parseExpression("Foo[].class");
        assertEquals("Foo[].class", print(expr));
    }

    [TestMethod]
    void printGenericClassExpr() {
        ClassExpr expr = parseExpression("Foo<String>.class");
        assertEquals("Foo<String>.class", print(expr));
    }

    [TestMethod]
    void printSimplestClass() {
        Node node = parse("class A {}");
        assertEquals("class A {" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }

    [TestMethod]
    void printAClassWithField() {
        Node node = parse("class A { int a; }");
        assertEquals("class A {" + SYSTEM_EOL
                + SYSTEM_EOL +
                "    int a;" + SYSTEM_EOL +
                "}" + SYSTEM_EOL, print(node));
    }

    [TestMethod]
    void printAReceiverParameter() {
        Node node = parseBodyDeclaration("int x(@O X A.B.this, int y) { }");
        assertEquals("int x(@O X A.B.this, int y) {" + SYSTEM_EOL + "}", print(node));
    }

    [TestMethod]
    void printLambdaIntersectionTypeAssignment() {
        string code = "class A {" + SYSTEM_EOL +
                "  void f() {" + SYSTEM_EOL +
                "    Runnable r = (Runnable & Serializable) (() -> {});" + SYSTEM_EOL +
                "    r = (Runnable & Serializable)() -> {};" + SYSTEM_EOL +
                "    r = (Runnable & I)() -> {};" + SYSTEM_EOL +
                "  }}";
        CompilationUnit cu = parse(code);
        MethodDeclaration methodDeclaration = (MethodDeclaration) cu.getType(0).getMember(0);

        assertEquals("Runnable r = (Runnable & Serializable) (() -> {" + SYSTEM_EOL + "});", print(methodDeclaration.getBody().get().getStatements().get(0)));
    }

    [TestMethod]
    void printIntersectionType() {
        string code = "(Runnable & Serializable) (() -> {})";
        Expression expression = parseExpression(code);
        Type type = ((CastExpr) expression).getType();

        assertEquals("Runnable & Serializable", print(type));
    }

    [TestMethod]
    void printLambdaIntersectionTypeReturn() {
        string code = "class A {" + SYSTEM_EOL
                + "  Object f() {" + SYSTEM_EOL
                + "    return (Comparator<Map.Entry<K, V>> & Serializable)(c1, c2) -> c1.getKey().compareTo(c2.getKey()); " + SYSTEM_EOL
                + "}}";
        CompilationUnit cu = parse(code);
        MethodDeclaration methodDeclaration = (MethodDeclaration) cu.getType(0).getMember(0);

        assertEquals("return (Comparator<Map.Entry<K, V>> & Serializable) (c1, c2) -> c1.getKey().compareTo(c2.getKey());", print(methodDeclaration.getBody().get().getStatements().get(0)));
    }

    [TestMethod]
    void printClassWithoutJavaDocButWithComment() {
        string code = String.format("/** javadoc */ public class A { %s// stuff%s}", SYSTEM_EOL, SYSTEM_EOL);
        CompilationUnit cu = parse(code);
        PrinterConfiguration ignoreJavaDoc = new DefaultPrinterConfiguration().removeOption(new DefaultConfigurationOption(ConfigOption.PRINT_JAVADOC));
        string content = cu.toString(ignoreJavaDoc);
        assertEquals(String.format("public class A {%s    // stuff%s}%s", SYSTEM_EOL, SYSTEM_EOL, SYSTEM_EOL), content);
    }

    [TestMethod]
    void printImportsDefaultOrder() {
        string code = "import x.y.z;import a.b.c;import static b.c.d;class c {}";
        CompilationUnit cu = parse(code);
        string content = cu.toString();
        assertEqualsStringIgnoringEol("import x.y.z;\n" +
                "import a.b.c;\n" +
                "import static b.c.d;\n" +
                "\n" +
                "class c {\n" +
                "}\n", content);
    }

    [TestMethod]
    void printImportsOrdered() {
        string code = "import x.y.z;import a.b.c;import static b.c.d;class c {}";
        CompilationUnit cu = parse(code);
        PrinterConfiguration orderImports = new DefaultPrinterConfiguration().addOption(new DefaultConfigurationOption(ConfigOption.ORDER_IMPORTS));
        string content = cu.toString(orderImports);
        assertEqualsStringIgnoringEol("import static b.c.d;\n" +
                "import a.b.c;\n" +
                "import x.y.z;\n" +
                "\n" +
                "class c {\n" +
                "}\n", content);
    }

    [TestMethod]
    void multilineJavadocGetsFormatted() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("line1\n   line2 *\n * line3");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     * line1\n" +
                "     *    line2 *\n" +
                "     *  line3\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void emptyJavadocGetsFormatted() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void multilineJavadocWithLotsOfEmptyLinesGetsFormattedNeatly() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("\n\n\n ab\n\n\n cd\n\n\n");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     * ab\n" +
                "     *\n" +
                "     * cd\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void singlelineJavadocGetsFormatted() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("line1");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     * line1\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void javadocAlwaysGetsASpaceBetweenTheAsteriskAndTheRestOfTheLine() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("line1\nline2");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     * line1\n" +
                "     * line2\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void javadocAlwaysGetsAnAdditionalSpaceOrNeverGetsIt() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setJavadocComment("line1\n" +
                "line2\n" +
                "    3");

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    /**\n" +
                "     * line1\n" +
                "     * line2\n" +
                "     *     3\n" +
                "     */\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void singlelineCommentGetsFormatted() {
        CompilationUnit cu = new CompilationUnit();
        cu.addClass("X").addMethod("abc").setComment(new LineComment("   line1  \n "));

        assertEqualsStringIgnoringEol("public class X {\n" +
                "\n" +
                "    //   line1\n" +
                "    void abc() {\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    [TestMethod]
    void blockcommentGetsNoFormatting() {
        CompilationUnit cu = parse("class A {\n" +
                "    public void helloWorld(string greeting, string name) {\n" +
                "        //sdfsdfsdf\n" +
                "            //sdfds\n" +
                "        /*\n" +
                "                            dgfdgfdgfdgfdgfd\n" +
                "         */\n" +
                "    }\n" +
                "}\n");

        assertEqualsStringIgnoringEol("class A {\n" +
                "\n" +
                "    public void helloWorld(string greeting, string name) {\n" +
                "        //sdfsdfsdf\n" +
                "        //sdfds\n" +
                "        /*\n" +
                "                            dgfdgfdgfdgfdgfd\n" +
                "         */\n" +
                "    }\n" +
                "}\n", cu.toString());
    }

    private string expected = "public class SomeClass {\n" +
            "\n" +
            "    /**\n" +
            "     * tester line\n" +
            "     * multi line comment\n" +
            "     *   multi line comment\n" +
            "     * multi line comment\n" +
            "     *    multi line comment\n" +
            "     */\n" +
            "    public void add(int x, int y) {\n" +
            "    }\n" +
            "}\n";

    [TestMethod]
    void javadocIssue1907_allLeadingSpaces() {
        string input_allLeadingSpaces = "public class SomeClass{" +
                "/**\n" +
                " * tester line\n" +
                " * multi line comment\n" +
                " *   multi line comment\n" +
                "   * multi line comment\n" +
                "    multi line comment\n" +
                " */\n" +
                "public void add(int x, int y){}}";

        CompilationUnit cu_allLeadingSpaces = parse(input_allLeadingSpaces);
        assertEqualsStringIgnoringEol(expected, cu_allLeadingSpaces.toString());
    }

    [TestMethod]
    void javadocIssue1907_singleMissingLeadingSpace() {
        string input_singleMissingLeadingSpace = "public class SomeClass{" +
                "/**\n" +
                "* tester line\n" +
                " * multi line comment\n" +
                " *   multi line comment\n" +
                "   * multi line comment\n" +
                "    multi line comment\n" +
                " */\n" +
                "public void add(int x, int y){}}";

        CompilationUnit cu_singleMissingLeadingSpace = parse(input_singleMissingLeadingSpace);
        assertEqualsStringIgnoringEol(expected, cu_singleMissingLeadingSpace.toString());
    }

    [TestMethod]
    void javadocIssue1907_leadingTab() {
        string input_leadingTab = "public class SomeClass{" +
                "/**\n" +
                "\t * tester line\n" +
                " * multi line comment\n" +
                " *   multi line comment\n" +
                "   * multi line comment\n" +
                "    multi line comment\n" +
                " */\n" +
                "public void add(int x, int y){}}";

        CompilationUnit cu_leadingTab = parseCompilationUnit(input_leadingTab);
        assertEqualsStringIgnoringEol(expected, cu_leadingTab.toString());
    }

    [TestMethod]
    void printYield() {
        Statement statement = parseStatement("yield 5*5;");
        assertEqualsStringIgnoringEol("yield 5 * 5;", statement.toString());
    }

    [TestMethod]
    void printTextBlock() {
        CompilationUnit cu = parseCompilationUnit(
                ParserConfiguration.LanguageLevel.JAVA_13_PREVIEW,
                "class X{string html = \"\"\"\n" +
                "              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "              \"\"\";}"
        );

        assertEqualsStringIgnoringEol("string html = \"\"\"\n" +
                "    <html>\n" +
                "        <body>\n" +
                "            <p>Hello, world</p>\n" +
                "        </body>\n" +
                "    </html>\n" +
                "    \"\"\";", cu.getClassByName("X").get().getFieldByName("html").get().toString());
    }

    [TestMethod]
    void printTextBlock2() {
        CompilationUnit cu = parseCompilationUnit(
                ParserConfiguration.LanguageLevel.JAVA_13_PREVIEW,
                "class X{string html = \"\"\"\n" +
                "              <html>\n" +
                "              </html>\"\"\";}"
        );

        assertEqualsStringIgnoringEol("string html = \"\"\"\n" +
                "    <html>\n" +
                "    </html>\"\"\";", cu.getClassByName("X").get().getFieldByName("html").get().toString());
    }


    [TestMethod]
    void innerClassWithConstructorReceiverParameterTest() {
        string innerClassWithConstructorReceiverParam =
                "public class A {\n\n" +
                        "    class InnerA {\n\n" +
                        "        InnerA(A A.this) {\n" +
                        "        }\n" +
                        "    }\n" +
                        "}\n";
        CompilationUnit cu = parseCompilationUnit(innerClassWithConstructorReceiverParam);
        assertEqualsStringIgnoringEol(innerClassWithConstructorReceiverParam, print(cu));
    }
}
