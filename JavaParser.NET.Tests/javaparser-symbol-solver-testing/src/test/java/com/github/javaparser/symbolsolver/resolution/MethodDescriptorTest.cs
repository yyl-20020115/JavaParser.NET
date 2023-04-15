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

namespace com.github.javaparser.symbolsolver.resolution;




class MethodDescriptorTest:AbstractResolutionTest {

    private static string code =
            "public class A {\n" +
            "  A(int i, double d, Thread t) {}\n" +
            "  public enum TestEnum {\n" +
            "    TEST_ENUM(\"test\");" +
            "    private string a;\n" +
            "    private TestEnum(string a) {\n" +
            "      this.a = a;\n" +
            "    }\n" +
            "  }\n" +
            "  Object m(int i, double d, Thread t) {return new Object();}\n" +
            "  void m(int i, double d, Thread t) {}\n" +
            "  int[] m(int i, double d, Thread t) {return new int[] {};}\n" +
            "  long[][] m(int i, double d, Thread t) {return new long[][] {};}\n" +
            "  void m() {\n" +
            "    System._out.println(\"a\");\n" +
            "    TestEnum.valueOf(\"TEST_ENUM\");\n" +
            "    TestEnum.values();\n" +
            "  }\n" +
            "}";

    private static TypeSolver typeSolver;
    private static CompilationUnit cu;

    @BeforeAll
    static void setup(){
        Path javassistJar = adaptPath("src/test/resources/javassistmethoddecl/javassistmethoddecl.jar");
        ParserConfiguration config = new ParserConfiguration();
        typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(),
                new JarTypeSolver(javassistJar));
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);
        cu = parse(code);
    }

    [TestMethod]
    void methodDeclarationDescriptorTest() {
        List<ConstructorDeclaration> constructor = cu.findAll(ConstructorDeclaration.class);
        assertEquals("(IDLjava/lang/Thread;)V", constructor.get(0).toDescriptor());

        List<MethodDeclaration> methods = cu.findAll(MethodDeclaration.class);
        // example provided _in https://docs.oracle.com/javase/specs/jvms/se8/html/jvms-4.html#jvms-4.3.3
        assertEquals("(IDLjava/lang/Thread;)Ljava/lang/Object;", methods.get(0).toDescriptor());
        // with void return type
        assertEquals("(IDLjava/lang/Thread;)V", methods.get(1).toDescriptor());
        // with single array type
        assertEquals("(IDLjava/lang/Thread;)[I", methods.get(2).toDescriptor());
        // with single array type
        assertEquals("(IDLjava/lang/Thread;)[[J", methods.get(3).toDescriptor());
        // with void return type and no parameter
        assertEquals("()V", methods.get(4).toDescriptor());
    }

    [TestMethod]
    void resolvedMethodDeclarationDescriptorTest() {
        // example provided _in https://docs.oracle.com/javase/specs/jvms/se8/html/jvms-4.html#jvms-4.3.3
        List<MethodDeclaration> methods = cu.findAll(MethodDeclaration.class);
        assertEquals("(IDLjava/lang/Thread;)Ljava/lang/Object;", methods.get(0).resolve().toDescriptor());
        // with void return type
        assertEquals("(IDLjava/lang/Thread;)V", methods.get(1).resolve().toDescriptor());
        // with single array type
        assertEquals("(IDLjava/lang/Thread;)[I", methods.get(2).resolve().toDescriptor());
        // with single array type
        assertEquals("(IDLjava/lang/Thread;)[[J", methods.get(3).resolve().toDescriptor());
        // with void return type and no parameter
        assertEquals("()V", methods.get(4).resolve().toDescriptor());

        List<MethodCallExpr> methodCalls = cu.findAll(MethodCallExpr.class);
        // check descriptor of ReflectionMethodDeclaration
        assertEquals("(Ljava/lang/String;)V", methodCalls.get(0).resolve().toDescriptor());
        // of ValueOfMethod
        assertEquals("(Ljava/lang/String;)LA/TestEnum;", methodCalls.get(1).resolve().toDescriptor());
        // of ValuesMethod
        assertEquals("()[LA/TestEnum;", methodCalls.get(2).resolve().toDescriptor());
        // of JavassistMethodDeclaration
        javassistMethodDeclarationDescriptorTest();
    }

    private void javassistMethodDeclarationDescriptorTest() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");

        JavassistMethodDeclaration methodWithRawParameter = findMethodWithName(classDecl, "methodWithRawParameter");
        assertEquals("(Ljava/util/List;)V", methodWithRawParameter.toDescriptor());

        JavassistMethodDeclaration methodWithExceptions = findMethodWithName(classDecl, "methodWithExceptions");
        assertEquals("()V", methodWithExceptions.toDescriptor());
    }

    private JavassistMethodDeclaration findMethodWithName(JavassistClassDeclaration classDecl, string name) {
        return classDecl.getDeclaredMethods().stream().filter(methodDecl -> methodDecl.getName().equals(name))
                .map(m -> (JavassistMethodDeclaration) m).findAny().get();
    }
}
