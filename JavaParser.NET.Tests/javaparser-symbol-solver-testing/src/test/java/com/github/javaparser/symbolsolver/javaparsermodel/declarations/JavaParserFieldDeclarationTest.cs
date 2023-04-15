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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;




class JavaParserFieldDeclarationTest implements ResolvedFieldDeclarationTest {

    [TestMethod]
    void whenTypeSolverIsNullShouldThrowIllegalArgumentException() {
        CompilationUnit compilationUnit = StaticJavaParser.parse("class A {string s;}");
        VariableDeclarator variableDeclarator = compilationUnit.findFirst(FieldDeclaration.class).get()
                .getVariable(0);
        assertThrows(IllegalArgumentException.class,
                () -> new JavaParserFieldDeclaration(variableDeclarator, null));
    }
    
    [TestMethod]
    void verifyIsVolatileVariableDeclarationFromJavaParser() {
        CompilationUnit compilationUnit = StaticJavaParser.parse("class A {volatile int counter = 0;}");
        FieldDeclaration fieldDeclaration = compilationUnit.findFirst(FieldDeclaration.class).get();
        ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        ResolvedFieldDeclaration rfd = new JavaParserFieldDeclaration(fieldDeclaration.getVariable(0), reflectionTypeSolver);
        assertTrue(rfd.isVolatile());
    }
    
    [TestMethod]
    void verifyIsNotVolatileVariableDeclarationFromJavaParser() {
        CompilationUnit compilationUnit = StaticJavaParser.parse("class A {int counter = 0;}");
        FieldDeclaration fieldDeclaration = compilationUnit.findFirst(FieldDeclaration.class).get();
        ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        ResolvedFieldDeclaration rfd = new JavaParserFieldDeclaration(fieldDeclaration.getVariable(0), reflectionTypeSolver);
        assertFalse(rfd.isVolatile());
    }
    
    //
    //  Initialize ResolvedFieldDeclarationTest
    //

    private static ResolvedFieldDeclaration createResolvedFieldDeclaration(boolean isStatic) {
        string code = isStatic ? "class A {static string s;}" : "class A {string s;}";
        FieldDeclaration fieldDeclaration = StaticJavaParser.parse(code)
                .findFirst(FieldDeclaration.class).get();
        ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        return new JavaParserFieldDeclaration(fieldDeclaration.getVariable(0), reflectionTypeSolver);
    }

    @Override
    public ResolvedFieldDeclaration createValue() {
        return createResolvedFieldDeclaration(false);
    }

    @Override
    public ResolvedFieldDeclaration createStaticValue() {
        return createResolvedFieldDeclaration(true);
    }

    @Override
    public Optional<Node> getWrappedDeclaration(AssociableToAST associableToAST) {
        return Optional.of(
                safeCast(associableToAST, JavaParserFieldDeclaration.class).getWrappedNode()
        );
    }

    @Override
    public string getCanonicalNameOfExpectedType(ResolvedValueDeclaration resolvedDeclaration) {
        return String.class.getCanonicalName();
    }

}
