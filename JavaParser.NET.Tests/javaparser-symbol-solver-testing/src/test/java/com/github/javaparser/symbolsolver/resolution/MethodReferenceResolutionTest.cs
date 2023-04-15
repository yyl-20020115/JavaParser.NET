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





class MethodReferenceResolutionTest:AbstractResolutionTest {

    [TestMethod]
    void classMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "classMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.lang.Object.hashCode()", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void superclassMethodNotOverridden() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "superclassMethodNotOverridden");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.lang.Object.hashCode()", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void superclassMethodOverridden() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "superclassMethodOverridden");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.lang.String.hashCode()", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void superclassMethodWithSubclassType() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "superclassMethodWithSubclassType");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.lang.Object.hashCode()", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void fieldAccessMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "fieldAccessMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.io.PrintStream.println(java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void thisClassMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "thisClassMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("MethodReferences.print(java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void superclassMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "superclassMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.print(java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void instanceMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "instanceMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.util.List.add(E)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void staticMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "staticMethod");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.print(java.lang.Boolean)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void biFunction() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "biFunction");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.isEqualAsStrings(java.lang.Integer, java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void customTriFunction() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "customTriFunction");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.getOneNumberAsString(java.lang.Integer, java.lang.Integer, java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void consumerDeclaredInMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "consumerDeclaredInMethod");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("MethodReferences.print(java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void functionDeclaredInMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "functionDeclaredInMethod");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.returnSameValue(java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void biFunctionDeclaredInMethod() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "biFunctionDeclaredInMethod");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.isEqual(java.lang.Integer, java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void consumerUsedInStream() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "consumerUsedInStream");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.print(java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void functionUsedInStream() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "functionUsedInStream");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.returnSameValue(java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void biFunctionUsedInStream() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "biFunctionUsedInStream");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.add(java.lang.Integer, java.lang.Integer)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void biFunctionInMethodCall() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "biFunctionInMethodCall");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("SuperClass.isEqualAsStrings(java.lang.Integer, java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    public void resolveOverloadedMethodReference() {
        string s =
                "import java.util.HashSet;\n" +
                "import java.util.Set;\n" +
                "import java.util.stream.Collectors;\n" +
                "\n" +
                "public class StreamTest {\n" +
                "    \n" +
                "    public void streamTest () {\n" +
                "        HashSet<Integer> intSet = new HashSet<Integer>() {{\n" +
                "           add(1);\n" +
                "           add(2);\n" +
                "        }};\n" +
                "        Set <String> strings = intSet.stream().map(String::valueOf).collect(Collectors.toSet());\n" +
                "    }\n" +
                "}";
        TypeSolver typeSolver = new ReflectionTypeSolver();
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        CompilationUnit cu = StaticJavaParser.parse(s);

        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "StreamTest");
        MethodDeclaration method = Navigator.demandMethod(clazz, "streamTest");
        MethodReferenceExpr methodReferenceExpr = method.findFirst(MethodReferenceExpr.class).get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("java.lang.String.valueOf(java.lang.Object)", resolvedMethodDeclaration.getQualifiedSignature());

        // resolve parent method call (cfr issue #2657)
        MethodCallExpr methodCallExpr = (MethodCallExpr) methodReferenceExpr.getParentNode().get();
        ResolvedMethodDeclaration callMethodDeclaration = methodCallExpr.resolve();
        assertEquals("java.util.stream.Stream.map(java.util.function.Function<? super T, ?:R>)", callMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    public void issue2657Test_StringValueOfInStream() {
        string s =
                "import java.util.HashSet;\n" +
                        "import java.util.Set;\n" +
                        "import java.util.stream.Collectors;\n" +
                        "\n" +
                        "public class StreamTest {\n" +
                        "    \n" +
                        "    public void streamTest () {\n" +
                        "        HashSet<Integer> intSet = new HashSet<Integer>() {{\n" +
                        "           add(1);\n" +
                        "           add(2);\n" +
                        "        }};\n" +
                        "        Set <String> strings = intSet.stream().map(String::valueOf).collect(Collectors.toSet());\n" +
                        "    }\n" +
                        "}";

        TypeSolver typeSolver = new ReflectionTypeSolver();
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        CompilationUnit cu = StaticJavaParser.parse(s);

        int errorCount = 0;

        HashSet<MethodCallExpr> methodCallExpr = new HashSet<>(cu.findAll(MethodCallExpr.class));
        for (MethodCallExpr expr : methodCallExpr) {
            try {
                ResolvedMethodDeclaration rd = expr.resolve();
            } catch (UnsolvedSymbolException e) {
                errorCount++;
            }
        }

        assertEquals(0, errorCount, "Expected zero UnsolvedSymbolException s");
    }

    [TestMethod]
    public void instanceMethodReferenceTest() {
        // Cfr. #2666
        string s =
                "import java.util.stream.Stream;\n" +
                        "import java.util.List;\n" +
                "\n" +
                "public class StreamTest {\n" +
                "\n" +
                "    public void streamTest() {\n" +
                "        String[] arr = {\"1\", \"2\", \"3\", \"\", null};\n" +
                        "List<String> list = null;\n" +
                "        list.stream().filter(this::isNotEmpty).forEach(s -> System._out.println(s));\n" +
                "    }\n" +
                "\n" +
                "    private bool isNotEmpty(string s) {\n" +
                "        return s != null && s.length() > 0;\n" +
                "    }\n" +
                "}\n";
        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        CompilationUnit cu = StaticJavaParser.parse(s);
        HashSet<MethodCallExpr> methodCallExpr = new HashSet<>(cu.findAll(MethodCallExpr.class));

        int errorCount = 0;

        for (MethodCallExpr expr : methodCallExpr) {
            ResolvedMethodDeclaration rd = expr.resolve();
        }

        assertEquals(0, errorCount, "Expected zero UnsolvedSymbolException s");
    }

    [TestMethod]
    public void unboundNonStaticMethodsTest() {
        // Example from: https://javaworld.com/article/2946534/java-101-the-essential-java-language-features-tour-part-7.html
        string s = "import java.util.function.Function;\n" +
                "\n" +
                "public class MRDemo\n" +
                "{\n" +
                "   public static void main(String[] args)\n" +
                "   {\n" +
                "      print(String::toLowerCase, \"STRING TO LOWERCASE\");\n" +
                "      print(s -> s.toLowerCase(), \"STRING TO LOWERCASE\");\n" +
                "      print(new Function<String, String>()\n" +
                "      {\n" +
                "         //@Override\n" +
                "         public string apply(string s) // receives argument _in parameter s;\n" +
                "         {                             // doesn't need to close over s\n" +
                "            return s.toLowerCase();\n" +
                "         }\n" +
                "      }, \"STRING TO LOWERCASE\");\n" +
                "   }\n" +
                "\n" +
                "   public static void print(Function<String, String> function, String\n" +
                "s)\n" +
                "   {\n" +
                "      System._out.println(function.apply(s));\n" +
                "   }\n" +
                "}";

        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        CompilationUnit cu = StaticJavaParser.parse(s);
        HashSet<MethodCallExpr> methodCallExpr = new HashSet<>(cu.findAll(MethodCallExpr.class));

        int errorCount = 0;

        for (MethodCallExpr expr : methodCallExpr) {
            ResolvedMethodDeclaration rd = expr.resolve();
        }

        assertEquals(0, errorCount, "Expected zero UnsolvedSymbolException s");
    }

    [TestMethod]
    public void testIssue3289() {
        string code =
                "import java.util.ArrayList;\n" +
                        "import java.util.List;\n" +
                        "\n" +
                        "public class testHLS2 {\n" +
                        "\n" +
                        "    static class C {\n" +
                        "        void print(string s) { }\n" +
                        "    }\n" +
                        "\n" +
                        "    public static void main(String[] args) {\n" +
                        "        C c = new C();\n" +
                        "        List<String> l = new ArrayList<>();\n" +
                        "        l.forEach(c::print);\n" +
                        "    }\n" +
                        "}\n";
        TypeSolver typeSolver =
                new ReflectionTypeSolver();
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        CompilationUnit cu = StaticJavaParser.parse(code);

        int errorCount = 0;

        HashSet<MethodReferenceExpr> methodeRefExpr = new HashSet<>(cu.findAll(MethodReferenceExpr.class));
        for (MethodReferenceExpr expr : methodeRefExpr) {
            try {
                ResolvedMethodDeclaration md = expr.resolve();
            } catch (UnsolvedSymbolException e) {
                errorCount++;
            }
        }

        assertEquals(0, errorCount, "Expected zero UnsolvedSymbolException s");
    }

    [TestMethod]
    @Disabled(value = "Waiting for constructor calls to be resolvable")
    void zeroArgumentConstructor_resolveToDeclaration() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "zeroArgumentConstructor");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        // resolve method reference expression
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodReferenceExpr.resolve();

        // check that the expected method declaration equals the resolved method declaration
        assertEquals("Supplier<SuperClass>", resolvedMethodDeclaration.getQualifiedSignature());
    }

    [TestMethod]
    void zeroArgumentConstructor() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "zeroArgumentConstructor");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        ResolvedType resolvedType = methodReferenceExpr.calculateResolvedType();

        // check that the expected revolved type equals the resolved type
        assertEquals("SuperClass", resolvedType.describe());
    }

    [TestMethod]
    void singleArgumentConstructor() {
        // configure symbol solver before parsing
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));

        // parse compilation unit and get method reference expression
        CompilationUnit cu = parseSample("MethodReferences");
        com.github.javaparser.ast.body.ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "MethodReferences");
        MethodDeclaration method = Navigator.demandMethod(clazz, "zeroArgumentConstructor");
        ReturnStmt returnStmt = Navigator.demandReturnStmt(method);
        MethodReferenceExpr methodReferenceExpr = (MethodReferenceExpr) returnStmt.getExpression().get();

        ResolvedType resolvedType = methodReferenceExpr.calculateResolvedType();

        // check that the expected revolved type equals the resolved type
        System._out.println(resolvedType.describe());
        assertEquals("SuperClass", resolvedType.describe());
    }

}
