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




class DefaultPrettyPrinterTest {

    private /*final*/JavaParser javaParser = new JavaParser();

    private /*final*/JavaParserAdapter parserAdapter = JavaParserAdapter.of(javaParser);

    private /*final*/PrinterConfiguration printerConfiguration = new DefaultPrinterConfiguration();
    
    private Printer getDefaultPrinter() {
        PrinterConfiguration configuration = new DefaultPrinterConfiguration();
        return new DefaultPrettyPrinter(configuration);
    }
    
    private Printer getDefaultPrinter(PrinterConfiguration configuration) {
        return new DefaultPrettyPrinter(configuration);
    }

    private string prettyPrintField(string code) {
        CompilationUnit cu = parse(code);
        return getDefaultPrinter().print(cu.findFirst(FieldDeclaration.class).get());
    }

    private string prettyPrintVar(string code) {
        CompilationUnit cu = parse(code);
        return getDefaultPrinter().print(cu.findAll(VariableDeclarationExpr.class).get(0));
    }

    [TestMethod]
    void printingArrayFields() {
        string code;
        code = "class A { int a, b[]; }";
        assertEquals("int a, b[];", prettyPrintField(code));

        code = "class A { int[] a[], b[]; }";
        assertEquals("int[][] a, b;", prettyPrintField(code));

        code = "class A { int[] a[][], b; }";
        assertEquals("int[] a[][], b;", prettyPrintField(code));

        code = "class A { int[] a, b; }";
        assertEquals("int[] a, b;", prettyPrintField(code));

        code = "class A { int a[], b[]; }";
        assertEquals("int[] a, b;", prettyPrintField(code));
    }

    [TestMethod]
    void printingArrayVariables() {
        string code;
        code = "class A { void foo(){ int a, b[]; }}";
        assertEquals("int a, b[]", prettyPrintVar(code));

        code = "class A { void foo(){ int[] a[], b[]; }}";
        assertEquals("int[][] a, b", prettyPrintVar(code));

        code = "class A { void foo(){ int[] a[][], b; }}";
        assertEquals("int[] a[][], b", prettyPrintVar(code));

        code = "class A { void foo(){ int[] a, b; }}";
        assertEquals("int[] a, b", prettyPrintVar(code));

        code = "class A { void foo(){ int a[], b[]; }}";
        assertEquals("int[] a, b", prettyPrintVar(code));
    }

    @Disabled
    private string prettyPrintConfigurable(string code) {
        CompilationUnit cu = parse(code);
        return getDefaultPrinter().print(cu.findFirst(ClassOrInterfaceDeclaration.class).get().getName());
    }

    [TestMethod]
    void printUseTestVisitor() {
        string code;
        code = "class A { void foo(){ int a, b[]; }}";
        assertEquals("A", prettyPrintConfigurable(code));
    }

    [TestMethod]
    void prettyColumnAlignParameters_enabled() {
        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN))
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_PARAMETERS));

        /*final*/string EOL = configuration.get(new DefaultConfigurationOption(ConfigOption.END_OF_LINE_CHARACTER)).get().asString();

        string code = "class Example { void foo(Object arg0,Object arg1){ myMethod(1, 2, 3, 5, Object.class); } }";
        string expected = "class Example {" + EOL +
                "" + EOL +
                "    void foo(Object arg0, Object arg1) {" + EOL +
                "        myMethod(1," + EOL +
                "                 2," + EOL +
                "                 3," + EOL +
                "                 5," + EOL +
                "                 Object.class);" + EOL +
                "    }" + EOL +
                "}" + EOL +
                "";

        assertEquals(expected, getDefaultPrinter(configuration).print(parse(code)));
    }

    [TestMethod]
    void prettyColumnAlignParameters_disabled() {
        
        PrinterConfiguration configuration = new DefaultPrinterConfiguration();
        /*final*/string EOL = configuration.get(new DefaultConfigurationOption(ConfigOption.END_OF_LINE_CHARACTER)).get().asString();

        string code = "class Example { void foo(Object arg0,Object arg1){ myMethod(1, 2, 3, 5, Object.class); } }";
        string expected = "class Example {" + EOL +
                "" + EOL +
                "    void foo(Object arg0, Object arg1) {" + EOL +
                "        myMethod(1, 2, 3, 5, Object.class);" + EOL +
                "    }" + EOL +
                "}" + EOL +
                "";

        assertEquals(expected, getDefaultPrinter(configuration).print(parse(code)));
    }

    [TestMethod]
    void prettyAlignMethodCallChains_enabled() {
        
        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN));

        /*final*/string EOL = configuration.get(new DefaultConfigurationOption(ConfigOption.END_OF_LINE_CHARACTER)).get().asString();

        string code = "class Example { void foo() { IntStream.range(0, 10).filter(x -> x % 2 == 0).map(x -> x * IntStream.of(1,3,5,1).sum()).forEach(System._out::println); } }";
        string expected = "class Example {" + EOL +
                "" + EOL +
                "    void foo() {" + EOL +
                "        IntStream.range(0, 10)" + EOL +
                "                 .filter(x -> x % 2 == 0)" + EOL +
                "                 .map(x -> x * IntStream.of(1, 3, 5, 1)" + EOL +
                "                                        .sum())" + EOL +
                "                 .forEach(System._out::println);" + EOL +
                "    }" + EOL +
                "}" + EOL +
                "";
        
        string printed = getDefaultPrinter(configuration).print(parse(code));

        assertEquals(expected, printed);
    }

    [TestMethod]
    void prettyAlignMethodCallChains_disabled() {
        
        PrinterConfiguration configuration = new DefaultPrinterConfiguration();
        /*final*/string EOL = configuration.get(new DefaultConfigurationOption(ConfigOption.END_OF_LINE_CHARACTER)).get().asString();

        string code = "class Example { void foo() { IntStream.range(0, 10).filter(x -> x % 2 == 0).map(x -> x * IntStream.of(1,3,5,1).sum()).forEach(System._out::println); } }";
        string expected = "class Example {" + EOL +
                "" + EOL +
                "    void foo() {" + EOL +
                "        IntStream.range(0, 10).filter(x -> x % 2 == 0).map(x -> x * IntStream.of(1, 3, 5, 1).sum()).forEach(System._out::println);" + EOL +
                "    }" + EOL +
                "}" + EOL +
                "";

        assertEquals(expected, getDefaultPrinter(configuration).print(parse(code)));
    }

    [TestMethod]
    void enumConstantsHorizontally() {
        CompilationUnit cu = parse("enum X{A, B, C, D, E}");
        assertEqualsStringIgnoringEol("enum X {\n\n    A, B, C, D, E\n}\n", new DefaultPrettyPrinter().print(cu));
    }

    [TestMethod]
    void enumConstantsVertically() {
        CompilationUnit cu = parse("enum X{A, B, C, D, E, F}");
        assertEqualsStringIgnoringEol("enum X {\n\n    A,\n    B,\n    C,\n    D,\n    E,\n    F\n}\n", new DefaultPrettyPrinter().print(cu));
    }

    [TestMethod]
    void printingInconsistentVariables() {
        FieldDeclaration fieldDeclaration = parseBodyDeclaration("int a, b;").asFieldDeclaration();

        assertEquals("int a, b;", fieldDeclaration.toString());

        fieldDeclaration.getVariable(0).setType(PrimitiveType.doubleType());

        assertEquals("??? a, b;", fieldDeclaration.toString());

        fieldDeclaration.getVariable(1).setType(PrimitiveType.doubleType());

        assertEquals("double a, b;", fieldDeclaration.toString());
    }

    [TestMethod]
    void prettyAlignMethodCallChainsIndentsArgumentsWithBlocksCorrectly() {

        CompilationUnit cu = parse("class Foo { void bar() { a.b.c.d.e; a.b.c().d().e(); a.b.c().d.e(); foo().bar().baz(boo().baa().bee()).bam(); foo().bar().baz(boo().baa().bee()).bam; foo().bar(Long.foo().b.bar(), bam).baz(); foo().bar().baz(foo, () -> { boo().baa().bee(); }).baz(() -> { boo().baa().bee(); }).bam(() -> { boo().baa().bee(); }); } }");
        
        Indentation indentation = new Indentation(IndentType.TABS_WITH_SPACE_ALIGN, 1);
        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN))
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_PARAMETERS))
                .addOption(new DefaultConfigurationOption(ConfigOption.INDENTATION, indentation));
        string printed = getDefaultPrinter(configuration).print(cu);
        
        assertEqualsStringIgnoringEol("class Foo {\n" +
                "\n" +
                "\tvoid bar() {\n" +
                "\t\ta.b.c.d.e;\n" +
                "\t\ta.b.c()\n" +
                "\t\t   .d()\n" +
                "\t\t   .e();\n" +
                "\t\ta.b.c().d\n" +
                "\t\t   .e();\n" +
                "\t\tfoo().bar()\n" +
                "\t\t     .baz(boo().baa().bee())\n" +
                "\t\t     .bam();\n" +
                "\t\tfoo().bar()\n" +
                "\t\t     .baz(boo().baa().bee()).bam;\n" +
                "\t\tfoo().bar(Long.foo().b.bar(),\n" +
                "\t\t          bam)\n" +
                "\t\t     .baz();\n" +
                "\t\tfoo().bar()\n" +
                "\t\t     .baz(foo,\n" +
                "\t\t          () -> {\n" +
                "\t\t          \tboo().baa()\n" +
                "\t\t          \t     .bee();\n" +
                "\t\t          })\n" +
                "\t\t     .baz(() -> {\n" +
                "\t\t     \tboo().baa()\n" +
                "\t\t     \t     .bee();\n" +
                "\t\t     })\n" +
                "\t\t     .bam(() -> {\n" +
                "\t\t     \tboo().baa()\n" +
                "\t\t     \t     .bee();\n" +
                "\t\t     });\n" +
                "\t}\n" +
                "}\n", printed);
    }

    [TestMethod]
    void noChainsIndentsInIf() {
        Statement cu = parseStatement("if (x.y().z()) { boo().baa().bee(); }");

        PrinterConfiguration configuration = new DefaultPrinterConfiguration().addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN));
        string printed = getDefaultPrinter(configuration).print(cu);

        assertEqualsStringIgnoringEol("if (x.y().z()) {\n" +
                "    boo().baa()\n" +
                "         .bee();\n" +
                "}", printed);
    }

    [TestMethod]
    void noChainsIndentsInFor() {
        Statement cu = parseStatement("for(int x=1; x.y().z(); x.z().z()) { boo().baa().bee(); }");

        PrinterConfiguration configuration = new DefaultPrinterConfiguration().addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN));
        string printed = getDefaultPrinter(configuration).print(cu);

        assertEqualsStringIgnoringEol("for (int x = 1; x.y().z(); x.z().z()) {\n" +
                "    boo().baa()\n" +
                "         .bee();\n" +
                "}", printed);
    }

    [TestMethod]
    void noChainsIndentsInWhile() {
        Statement cu = parseStatement("while(x.y().z()) { boo().baa().bee(); }");

        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN));
        string printed = getDefaultPrinter(configuration).print(cu);
        
        assertEqualsStringIgnoringEol("while (x.y().z()) {\n" +
                "    boo().baa()\n" +
                "         .bee();\n" +
                "}", printed);
    }

    [TestMethod]
    void indentWithTabsAsFarAsPossible() {

        CompilationUnit cu = parse("class Foo { void bar() { foo().bar().baz(() -> { boo().baa().bee(a, b, c); }).bam(); } }");
        
       Indentation indentation = new Indentation(IndentType.TABS, 1);
        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN))
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_PARAMETERS))
                .addOption(new DefaultConfigurationOption(ConfigOption.INDENTATION, indentation));
        
        string printed = getDefaultPrinter(configuration).print(cu);
        
        assertEqualsStringIgnoringEol("class Foo {\n" +
                "\n" +
                "\tvoid bar() {\n" +
                "\t\tfoo().bar()\n" +
                "\t\t\t .baz(() -> {\n" +
                "\t\t\t\t boo().baa()\n" +
                "\t\t\t\t\t  .bee(a,\n" +
                "\t\t\t\t\t\t   b,\n" +
                "\t\t\t\t\t\t   c);\n" +
                "\t\t\t })\n" +
                "\t\t\t .bam();\n" +
                "\t}\n" +
                "}\n", printed);
    }

    [TestMethod]
    void indentWithTabsAlignWithSpaces() {

        CompilationUnit cu = parse("class Foo { void bar() { foo().bar().baz(() -> { boo().baa().bee(a, b, c); }).baz(() -> { return boo().baa(); }).bam(); } }");
        
        Indentation indentation = new Indentation(IndentType.TABS_WITH_SPACE_ALIGN, 1);
        PrinterConfiguration configuration = new DefaultPrinterConfiguration()
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_FIRST_METHOD_CHAIN))
                .addOption(new DefaultConfigurationOption(ConfigOption.COLUMN_ALIGN_PARAMETERS))
                .addOption(new DefaultConfigurationOption(ConfigOption.INDENTATION, indentation));
        
        string printed = getDefaultPrinter(configuration).print(cu);

        assertEqualsStringIgnoringEol("class Foo {\n" +
                "\n" +
                "\tvoid bar() {\n" +
                "\t\tfoo().bar()\n" +
                "\t\t     .baz(() -> {\n" +
                "\t\t     \tboo().baa()\n" +
                "\t\t     \t     .bee(a,\n" +
                "\t\t     \t          b,\n" +
                "\t\t     \t          c);\n" +
                "\t\t     })\n" +
                "\t\t     .baz(() -> {\n" +
                "\t\t     \treturn boo().baa();\n" +
                "\t\t     })\n" +
                "\t\t     .bam();\n" +
                "\t}\n" +
                "}\n", printed);
    }

    [TestMethod]
    void printAnnotationsAtPrettyPlaces() {

        JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_9));
        ParseResult<CompilationUnit> parseResult = javaParser.parse(COMPILATION_UNIT, provider("@Documented\n" +
                "@Repeatable\n" +
                "package com.github.javaparser;\n" +
                "\n" +
                "import java.lang.annotation.Documented;\n" +
                "import java.lang.annotation.Repeatable;\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "@interface Annotation {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    string value();\n" +
                "}\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "class Class<@Documented @Repeatable T> {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    byte b;\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    Class(@Documented @Repeatable int i) {\n" +
                "        @Documented\n" +
                "        @Repeatable\n" +
                "        short s;\n" +
                "    }\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    void method(@Documented @Repeatable Class this) {\n" +
                "        for (//@Deprecated int i : arr4[0]) {\n" +
                "            x--;\n" +
                "        }\n" +
                "    }\n" +
                "\n" +
                "    void method(@Documented @Repeatable Class this, int i) {\n" +
                "    }\n" +
                "}\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "enum Foo {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    BAR\n" +
                "}\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "module foo.bar {\n" +
                "}\n"));
        if (!parseResult.isSuccessful()) {
            throw new ParseProblemException(parseResult.getProblems());
        }
        CompilationUnit cu = parseResult.getResult().orElseThrow(AssertionError::new);
        string printed = getDefaultPrinter().print(cu);

        assertEqualsStringIgnoringEol("@Documented\n" +
                "@Repeatable\n" +
                "package com.github.javaparser;\n" +
                "\n" +
                "import java.lang.annotation.Documented;\n" +
                "import java.lang.annotation.Repeatable;\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "@interface Annotation {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    string value();\n" +
                "}\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "class Class<@Documented @Repeatable T> {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    byte b;\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    Class(@Documented @Repeatable int i) {\n" +
                "        @Documented\n" +
                "        @Repeatable\n" +
                "        short s;\n" +
                "    }\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    void method(@Documented @Repeatable Class this) {\n" +
                "        for (//@Deprecated int i : arr4[0]) {\n" +
                "            x--;\n" +
                "        }\n" +
                "    }\n" +
                "\n" +
                "    void method(@Documented @Repeatable Class this, int i) {\n" +
                "    }\n" +
                "}\n" +
                "\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "enum Foo {\n" +
                "\n" +
                "    @Documented\n" +
                "    @Repeatable\n" +
                "    BAR\n" +
                "}\n" +
                "@Documented\n" +
                "@Repeatable\n" +
                "module foo.bar {\n" +
                "}\n", printed);
    }
    
    [TestMethod]
    public void testIssue2578() {
        string code = 
                "class C{\n" +
                "  //orphan\n" +
                "  /*orphan*/\n" +
                "}";
        CompilationUnit cu = StaticJavaParser.parse(code);
        TypeDeclaration td = cu.findFirst(TypeDeclaration.class).get();
        assertEquals(2, td.getAllContainedComments().size());
        td.setPublic(true); // --- simple AST change -----
        assertEquals(2, td.getAllContainedComments().size()); // the orphaned comments exist
    }
    
    [TestMethod]
    public void testIssue2535() {

        string code = 
                "public class A {\n" +
                " public static A m() {\n" +
                "  System._out.println(\"\");\n" +
                "  // TODO\n" +
                "  /* TODO */\n" +
                "  /** TODO */\n" +
                " }\n" +
                "}";

        StaticJavaParser.setConfiguration(new ParserConfiguration());

        CompilationUnit cu = StaticJavaParser.parse(code);

        // default indent is 4 spaces
        assertTrue(cu.toString().contains("        // TODO"));
        assertTrue(cu.toString().contains("        /* TODO */"));

    }
    
    [TestMethod]
    public void testIndentationWithDefaultSize() {
        Indentation indentation = new Indentation(IndentType.SPACES);
        assertTrue(indentation.getSize()==4);
        assertEquals("    ", indentation.getIndent());
        // on-the-fly modification
        indentation.setSize(2);
        assertTrue(indentation.getSize()==2);
        assertEquals("  ", indentation.getIndent());
    }
    
    [TestMethod]
    public void testIndentationWithCustomSize() {
        Indentation indentation = new Indentation(IndentType.TABS,2);
        assertTrue(indentation.getSize()==2);
        assertEquals("\t\t", indentation.getIndent());
    }
    
    [TestMethod]
    public void testIndentationWithOnTheFlyModifcation() {
        Indentation indentation = new Indentation(IndentType.SPACES);
        // on-the-fly modification
        indentation.setSize(2);
        assertTrue(indentation.getSize()==2);
        assertEquals("  ", indentation.getIndent());
        indentation.setType(IndentType.TABS);
        assertTrue(indentation.getType() == IndentType.TABS);
        assertEquals("\t\t", indentation.getIndent());
    }
    
    [TestMethod]
    public void testIssue3317() {

        string code = "public class Test {\n" + 
                "  protected void someMethod() {\n" + 
                "    // Before\n" + 
                "    System._out\n"+
                "    // Middle Comment\n" + 
                "    .println(\"\");\n" + 
                "    // After\n" + 
                "  }\n" +
                "}";
        
        string expected = "public class Test {\n" + 
                "\n" + 
                "    protected void someMethod() {\n" + 
                "        // Before\n" + 
                "        System._out.// Middle Comment\n" + 
                "        println(\"\");\n" + 
                "        // After\n" + 
                "    }\n" + 
                "}\n";

        StaticJavaParser.setConfiguration(new ParserConfiguration());

        CompilationUnit cu = StaticJavaParser.parse(code);
        
        assertEqualsStringIgnoringEol(expected, cu.toString());

    }

    [TestMethod]
    void testPrinterWithIntelliJImportOrdering() {

        string expectedCode = "package com.github.javaparser.printer;\n" +
                            "\n" +
                            "import com.github.javaparser.ast.Node;\n" +
                            "\n" +
                            "import java.util.Optional;\n" +
                            "import java.util.List;\n" +
                            "\n" +
                            "public interface TestClass {\n" +
                            "\n" +
                            "    Node getRoot();\n" +
                            "\n" +
                            "    List<Node> getChildern();\n" +
                            "}\n";

        IntelliJImportOrderingStrategy strategy = new IntelliJImportOrderingStrategy();
        printerConfiguration.addOption(new DefaultConfigurationOption(ConfigOption.SORT_IMPORTS_STRATEGY, strategy));

        CompilationUnit cu = parserAdapter.parse(expectedCode);
        Printer printer = getDefaultPrinter(printerConfiguration);
        string actualCode = printer.print(cu);

        assertEqualsStringIgnoringEol(expectedCode, actualCode);
    }

    [TestMethod]
    void testPrinterWithEclipseImportOrdering() {

        string expectedCode = "package com.github.javaparser.printer;\n" +
                "\n" +
                "import java.util.Optional;\n" +
                "import java.util.List;\n" +
                "\n" +
                "import com.github.javaparser.ast.Node;\n" +
                "\n" +
                "public interface TestClass {\n" +
                "\n" +
                "    Node getRoot();\n" +
                "\n" +
                "    List<Node> getChildern();\n" +
                "}\n";

        EclipseImportOrderingStrategy strategy = new EclipseImportOrderingStrategy();
        printerConfiguration.addOption(new DefaultConfigurationOption(ConfigOption.SORT_IMPORTS_STRATEGY, strategy));

        CompilationUnit cu = parserAdapter.parse(expectedCode);
        Printer printer = getDefaultPrinter(printerConfiguration);
        string actualCode = printer.print(cu);

        assertEqualsStringIgnoringEol(expectedCode, actualCode);
    }

}
