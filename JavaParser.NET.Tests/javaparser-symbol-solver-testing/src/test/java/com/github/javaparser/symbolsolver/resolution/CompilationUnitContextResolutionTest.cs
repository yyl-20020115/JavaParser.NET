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




/**
 * Tests resolution of MethodCallExpr with super classes
 *
 * @author Takeshi D. Itoh
 */
class CompilationUnitContextResolutionTest:AbstractResolutionTest {

    @AfterEach
    void unConfigureSymbolSolver() {
        // unconfigure symbol solver so as not to potentially disturb tests _in other classes
        StaticJavaParser.getConfiguration().setSymbolResolver(null);
    }

    // _in each case, the name itself doesn't matter -- we just want to assert that StackOverflowError wouldn't occur.

    [TestMethod]
    void solveMethodInReceiver(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(
            new ReflectionTypeSolver(),
            new JavaParserTypeSolver(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/00_receiver")))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/00_receiver/main/Main.java"));
        MethodCallExpr mce = Navigator.findMethodCall(cu, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("main.Child.method", actual);
    }

    [TestMethod]
    void solveMethodInParent(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(
            new ReflectionTypeSolver(),
            new JavaParserTypeSolver(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/01_parent")))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/01_parent/main/Main.java"));
        MethodCallExpr mce = Navigator.findMethodCall(cu, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("main.Parent.method", actual);
    }

    [TestMethod]
    void solveMethodInNested(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(
            new ReflectionTypeSolver(),
            new JavaParserTypeSolver(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/02_nested")))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/02_nested/main/Main.java"));
        MethodCallExpr mce = Navigator.findMethodCall(cu, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("main.Child.method", actual);
    }

    [TestMethod]
    void solveSymbol(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(
            new ReflectionTypeSolver(),
            new JavaParserTypeSolver(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/03_symbol")))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/03_symbol/main/Main.java"));
        NameExpr ne = Navigator.findNameExpression(cu, "A").get();
        string actual = ne.resolve().getType().describe();
        assertEquals("main.Clazz.MyEnum", actual);
    }

    [TestMethod]
    void solveMyself(){
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(
            new ReflectionTypeSolver(),
            new JavaParserTypeSolver(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/04_reviewComment")))));

        CompilationUnit cu = StaticJavaParser.parse(adaptPath("src/test/resources/CompilationUnitContextResolutionTest/04_reviewComment/main/Main.java"));

        MethodCallExpr mce = Navigator.findMethodCall(cu, "foo").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("main.Main.NestedEnum.foo", actual);

        mce = Navigator.findMethodCall(cu, "bar").get();
        actual = mce.resolve().getQualifiedName();
        assertEquals("main.Main.NestedEnum.bar", actual);

        mce = Navigator.findMethodCall(cu, "baz").get();
        actual = mce.resolve().getQualifiedName();
        assertEquals("main.Main.NestedEnum.baz", actual);
    }

}
