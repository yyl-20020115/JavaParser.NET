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

namespace com.github.javaparser.ast.validator;



class Java1_0ValidatorTest {
    public static /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_1_0));

    [TestMethod]
    void tryWithoutResources() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("try(X x=new Y()){}"));
        assertProblems(result,
                "(line 1,col 1) Catch with resource is not supported.",
                "(line 1,col 1) Try has no finally and no catch.");
    }

    [TestMethod]
    void classExtendingMoreThanOne() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("class X:Y, Z {}"));
        assertProblems(result, "(line 1,col 20) A class cannot extend more than one other class.");
    }

    [TestMethod]
    void interfaceUsingImplements() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("interface X implements Y {}"));
        assertProblems(result, "(line 1,col 24) An interface cannot implement other interfaces.");
    }

    [TestMethod]
    void interfaceWithInitializer() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("interface X {{}}"));
        assertProblems(result, "(line 1,col 14) An interface cannot have initializers.");
    }

    [TestMethod]
    void defaultInClass() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("class X {default void a(){};}"));
        assertProblems(result, "(line 1,col 10) 'default' is not allowed here.");
    }

    [TestMethod]
    void leftHandAssignmentCannotBeAConditional() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("(1==2)=3"));
        assertProblems(result, "(line 1,col 1) Illegal left hand side of an assignment.");
    }

    [TestMethod]
    void leftHandAssignmentCannotBeEmptyBraces() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("()=3"));
        assertProblems(result,
                "(line 1,col 1) Illegal left hand side of an assignment.",
                "(line 1,col 1) Lambdas are not supported.");
    }

    [TestMethod]
    void leftHandAssignmentCanBeInBraces() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("(i) += (i) += 1"));
        assertNoProblems(result);
    }

    [TestMethod]
    void noInnerClasses() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("class X{class Y{}}"));
        assertProblems(result, "(line 1,col 9) inner classes or interfaces are not supported.");
    }

    [TestMethod]
    void noReflection() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("Abc.class"));
        assertProblems(result, "(line 1,col 1) Reflection is not supported.");
    }

    [TestMethod]
    void noForEach() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("for(X x : xs){}"));
        assertProblems(result, "(line 1,col 1) For-each loops are not supported.");
    }

    [TestMethod]
    void labelBreakAllowed() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("switch(x){case 3: break bla;}"));
        assertNoProblems(result);
    }

    [TestMethod]
    void emptyBreakAllowed() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("switch(x){case 3: break;}"));
        assertNoProblems(result);
    }
}
