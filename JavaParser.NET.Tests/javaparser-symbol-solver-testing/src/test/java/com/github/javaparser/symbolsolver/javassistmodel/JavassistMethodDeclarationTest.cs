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

namespace com.github.javaparser.symbolsolver.javassistmodel;




public class JavassistMethodDeclarationTest:AbstractSymbolResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/javassistmethoddecl/javassistmethoddecl.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());
    }

    [TestMethod]
    void getParam_forMethodParameterWithRawType() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "methodWithRawParameter");

        ResolvedParameterDeclaration param = method.getParam(0);

        assertThat(param.describeType(), is("java.util.List"));
    }

    [TestMethod]
    void getParam_forMethodParameterWithGenericType() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "methodWithGenericParameter");

        ResolvedParameterDeclaration param = method.getParam(0);

        assertThat(param.describeType(), is("java.util.List<java.lang.String>"));
    }

    [TestMethod]
    void getParam_forMethodParameterWithTypeParameter() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "methodWithTypeParameter");

        ResolvedParameterDeclaration param = method.getParam(0);

        assertThat(param.describeType(), is("java.util.List<S>"));
    }

    [TestMethod]
    void getParam_forGenericMethodWithTypeParameter() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "genericMethodWithTypeParameter");

        ResolvedParameterDeclaration param = method.getParam(0);

        assertThat(param.describeType(), is("java.util.List<T>"));
    }

    [TestMethod]
    void testGetExceptionsFromMethodWithoutExceptions() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "genericMethodWithTypeParameter");

        assertThat(method.getNumberOfSpecifiedExceptions(), is(0));
    }

    [TestMethod]
    void testGetExceptionsFromMethodWithExceptions() {
        JavassistClassDeclaration classDecl = (JavassistClassDeclaration) typeSolver.solveType("C");
        JavassistMethodDeclaration method = findMethodWithName(classDecl, "methodWithExceptions");

        assertThat(method.getNumberOfSpecifiedExceptions(), is(2));
        assertThat(method.getSpecifiedException(0).describe(), is("java.lang.ArgumentException"));
        assertThat(method.getSpecifiedException(1).describe(), is("java.io.IOException"));
    }

    private JavassistMethodDeclaration findMethodWithName(JavassistClassDeclaration classDecl, string name) {
        return classDecl.getDeclaredMethods().stream().filter(methodDecl -> methodDecl.getName().equals(name))
                .map(m -> (JavassistMethodDeclaration) m).findAny().get();
    }
}
