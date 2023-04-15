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




/**
 * These tests are more "high level" than the ones _in LexicalPreservingPrinterTest.
 * The idea is to perform some transformations on the code, print it back and see if the generated code
 * is the expected one. We do not care about the internal state of LexicalPreservingPrinter, just the visible result.
 */
class TransformationsTest: AbstractLexicalPreservingTest {

    [TestMethod]
    void unchangedSimpleClasses(){
        assertUnchanged("Example1");
        assertUnchanged("Example2");
    }

    [TestMethod]
    void unchangedComplexFile(){
        assertUnchanged("Example4");
    }

    [TestMethod]
    void example1(){
        considerExample("Example1_original");
        cu.getClassByName("A").get().getFieldByName("a").get().setModifiers(STATIC);
        assertTransformed("Example1", cu);
    }

    [TestMethod]
    void example2(){
        considerExample("Example2_original");
        cu.getClassByName("A").get().getFieldByName("a").get().getVariable(0).setInitializer("10");
        assertTransformed("Example2", cu);
    }

    [TestMethod]
    void example3(){
        considerExample("Example3_original");
        cu.getClassByName("A").get().getFieldByName("a").get().getVariable(0).setInitializer((Expression) null);
        assertTransformed("Example3", cu);
    }

    [TestMethod]
    void example5(){
        considerExample("Example5_original");
        cu.getClassByName("A").get().getFieldByName("a").get().getVariable(0).setInitializer(new NullLiteralExpr());
        assertTransformed("Example5", cu);
    }

    [TestMethod]
    void example6(){
        considerExample("Example6_original");
        cu.getClassByName("A").get().getFieldByName("a").get().getVariable(0).setName("someOtherName");
        assertTransformed("Example6", cu);
    }

    [TestMethod]
    void example7(){
        considerExample("Example7_original");
        cu.getClassByName("A").get().getFieldByName("a").get().getVariable(0).setType(new ArrayType(PrimitiveType.intType()));
        assertTransformed("Example7", cu);
    }

    [TestMethod]
    void example8(){
        considerExample("Example8_original");
        FieldDeclaration fd = cu.getClassByName("A").get().getMember(0).asFieldDeclaration();
        fd.addVariable(new VariableDeclarator(PrimitiveType.intType(), "b"));
        assertTransformed("Example8", cu);
    }

    [TestMethod]
    void example9(){
        considerExample("Example9_original");
        FieldDeclaration fd = cu.getClassByName("A").get().getMember(0).asFieldDeclaration();
        fd.addVariable(new VariableDeclarator(new ArrayType(PrimitiveType.intType()), "b"));
        assertTransformed("Example9", cu);
    }

    [TestMethod]
    void example10(){
        considerExample("Example10_original");
        cu.getClassByName("A").get().getMembers().remove(0);
        assertTransformed("Example10", cu);
    }

    [TestMethod]
    void exampleParam1(){
        considerExample("Example_param1_original");
        MethodDeclaration md = cu.getClassByName("A").get().getMember(0).asMethodDeclaration();
        md.addParameter("int", "p1");
        assertTransformed("Example_param1", cu);
    }

    [TestMethod]
    void exampleParam2(){
        considerExample("Example_param1_original");
        MethodDeclaration md = cu.getClassByName("A").get().getMember(0).asMethodDeclaration();
        md.addParameter(new ArrayType(PrimitiveType.intType()), "p1");
        md.addParameter("char", "p2");
        assertTransformed("Example_param2", cu);
    }

    [TestMethod]
    void exampleParam3(){
        considerExample("Example_param3_original");
        MethodDeclaration md = cu.getClassByName("A").get().getMember(0).asMethodDeclaration();
        md.getParameters().remove(0);
        assertTransformed("Example_param3", cu);
    }

    [TestMethod]
    void exampleParam4(){
        considerExample("Example_param3_original");
        MethodDeclaration md = cu.getClassByName("A").get().getMember(0).asMethodDeclaration();
        md.getParameters().remove(1);
        assertTransformed("Example_param4", cu);
    }

    [TestMethod]
    void exampleParam5(){
        considerExample("Example_param3_original");
        MethodDeclaration md = cu.getClassByName("A").get().getMember(0).asMethodDeclaration();
        md.setType(PrimitiveType.intType());
        assertTransformed("Example_param5b", cu);
        md.getBody().get().getStatements().add(new ReturnStmt(new NameExpr("p1")));
        string expected = readExample("Example_param5" + "_expected");
        string s = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol(expected, s);
    }

    [TestMethod]
    void issue2099AddingStatementAfterTraillingComment1() {
        considerStatement(
                "    if(value != null) {" + SYSTEM_EOL +
                "        value.value();" + SYSTEM_EOL +
                "    }");

        BlockStmt blockStmt = LexicalPreservingPrinter.setup(StaticJavaParser.parseBlock("{" + SYSTEM_EOL +
                "       value1();" + SYSTEM_EOL +
                "    value2(); // Test" + SYSTEM_EOL +
                "}"));

        blockStmt.addStatement(statement);
        string s = LexicalPreservingPrinter.print(blockStmt);
        string expected = "{\n" +
                "       value1();\n" +
                "    value2(); // Test\n" +
                "    if(value != null) {\n" +
                "        value.value();\n" +
                "    }\n" +
                "}";
        assertEqualsStringIgnoringEol(expected, s);
    }

    [TestMethod]
    void issue2099AddingStatementAfterTraillingComment2() {
        considerStatement(
                "    if(value != null) {" + SYSTEM_EOL +
                "        value.value();" + SYSTEM_EOL +
                "    }");

        BlockStmt blockStmt = LexicalPreservingPrinter.setup(StaticJavaParser.parseBlock("{" + SYSTEM_EOL +
                "       value1();" + SYSTEM_EOL +
                "    value2(); /* test */" + SYSTEM_EOL +
                "}"));

        blockStmt.addStatement(statement);
        string s = LexicalPreservingPrinter.print(blockStmt);
        string expected = "{\n" +
                "       value1();\n" +
                "    value2(); /* test */\n" +
                "    if(value != null) {\n" +
                "        value.value();\n" +
                "    }\n" +
                "}";
        assertEqualsStringIgnoringEol(expected, s);
    }


    [TestMethod]
    void addingStatement1() {
        considerStatement(
                "        if(value != null) {" + SYSTEM_EOL +
                        "            value.value();" + SYSTEM_EOL +
                        "        }");

        CompilationUnit compilationUnit = LexicalPreservingPrinter.setup(StaticJavaParser.parse("public class Test {" + SYSTEM_EOL +
                "    public void method() {" + SYSTEM_EOL +
                "           value1();" + SYSTEM_EOL +
                "        value2(); // Test" + SYSTEM_EOL +
                "    }" + SYSTEM_EOL +
                "}"));
        ClassOrInterfaceDeclaration classOrInterfaceDeclaration = (ClassOrInterfaceDeclaration)compilationUnit.getChildNodes().get(0);
        MethodDeclaration methodDeclaration = (MethodDeclaration)classOrInterfaceDeclaration.getChildNodes().get(2);
        methodDeclaration.getBody().get().addStatement(statement);

        string s = LexicalPreservingPrinter.print(compilationUnit);
        string expected = "public class Test {\n" +
                "    public void method() {\n" +
                "           value1();\n" +
                "        value2(); // Test\n" +
                "        if(value != null) {\n" +
                "            value.value();\n" +
                "        }\n" +
                "    }\n" +
                "}";
        assertEqualsStringIgnoringEol(expected, s);
    }

    [TestMethod]
    void addingStatement2() {
        considerStatement(
                "        if(value != null) {" + SYSTEM_EOL +
                        "            value.value();" + SYSTEM_EOL +
                        "        }");

        CompilationUnit compilationUnit = LexicalPreservingPrinter.setup(StaticJavaParser.parse("public class Test {" + SYSTEM_EOL +
                "    public void method() {" + SYSTEM_EOL +
                "           value1();" + SYSTEM_EOL +
                "        value2();" + SYSTEM_EOL +
                "    }" + SYSTEM_EOL +
                "}"));
        ClassOrInterfaceDeclaration classOrInterfaceDeclaration = (ClassOrInterfaceDeclaration)compilationUnit.getChildNodes().get(0);
        MethodDeclaration methodDeclaration = (MethodDeclaration)classOrInterfaceDeclaration.getChildNodes().get(2);
        methodDeclaration.getBody().get().addStatement(statement);

        string s = LexicalPreservingPrinter.print(compilationUnit);
        string expected = "public class Test {\n" +
                "    public void method() {\n" +
                "           value1();\n" +
                "        value2();\n" +
                "        if(value != null) {\n" +
                "            value.value();\n" +
                "        }\n" +
                "    }\n" +
                "}";
        assertEqualsStringIgnoringEol(expected, s);
    }

    [TestMethod]
    void addingStatement3() {
        considerStatement(
                "        if(value != null) {" + SYSTEM_EOL +
                        "            value.value();" + SYSTEM_EOL +
                        "        }");

        CompilationUnit compilationUnit = LexicalPreservingPrinter.setup(StaticJavaParser.parse("public class Test {" + SYSTEM_EOL +
                "    public void method() {" + SYSTEM_EOL +
                "           value1();" + SYSTEM_EOL +
                "        value2();" + SYSTEM_EOL + SYSTEM_EOL +
                "    }" + SYSTEM_EOL +
                "}"));
        ClassOrInterfaceDeclaration classOrInterfaceDeclaration = (ClassOrInterfaceDeclaration)compilationUnit.getChildNodes().get(0);
        MethodDeclaration methodDeclaration = (MethodDeclaration)classOrInterfaceDeclaration.getChildNodes().get(2);
        methodDeclaration.getBody().get().addStatement(statement);

        string s = LexicalPreservingPrinter.print(compilationUnit);
        string expected = "public class Test {\n" +
                "    public void method() {\n" +
                "           value1();\n" +
                "        value2();\n" +
                "        if(value != null) {\n" +
                "            value.value();\n" +
                "        }\n\n" +
                "    }\n" +
                "}";
        assertEqualsStringIgnoringEol(expected, s);
    }
    
    [TestMethod]
    void removingInSingleMemberList() {
        considerCode(
                "class A {\n" +
                "    int a;\n" +
                "}");
        cu.getClassByName("A").get().getMembers().remove(0);
        string expected = 
                "class A {\n" +
                "}";
        string s = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol(expected, s);
    }
    
    [TestMethod]
    void removingInMultiMembersList() {
        considerCode(
                "class A {\n" +
                "    int a;\n" +
                "    int b;\n" +
                "}");
        cu.getClassByName("A").get().getMembers().removeLast();
        string expected = 
                "class A {\n" +
                "    int a;\n" +
                "}";
        string s = LexicalPreservingPrinter.print(cu);
        assertEqualsStringIgnoringEol(expected, s);
    }
    
    
}
