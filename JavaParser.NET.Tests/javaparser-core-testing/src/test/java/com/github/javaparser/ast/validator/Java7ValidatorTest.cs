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




class Java7ValidatorTest {
    public static /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_7));

    [TestMethod]
    void generics() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("class X<A>{List<String> b = new ArrayList<>();}"));
        assertNoProblems(result);
    }

    [TestMethod]
    void defaultMethodWithoutBody() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("interface X {default void a();}"));
        assertProblems(result, "(line 1,col 14) 'default' is not allowed here.");
    }

    [TestMethod]
    void tryWithoutAnything() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("try{}"));
        assertProblems(result, "(line 1,col 1) Try has no finally, no catch, and no resources.");
    }

    [TestMethod]
    void tryWithResourceVariableDeclaration() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("try(Reader r = new Reader()){}"));
        assertNoProblems(result);
    }

    [TestMethod]
    void tryWithResourceReference() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("try(a.b.c){}"));
        assertProblems(result, "(line 1,col 1) Try with resources only supports variable declarations.");
    }

    [TestMethod]
    void stringsInSwitch() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("switch(x){case \"abc\": ;}"));
        assertNoProblems(result);
    }

    [TestMethod]
    void binaryIntegerLiterals() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("0b01"));
        assertNoProblems(result);
    }

    [TestMethod]
    void underscoresInIntegerLiterals() {
        ParseResult<Expression> result = javaParser.parse(EXPRESSION, provider("1_000_000"));
        assertNoProblems(result);
    }

    [TestMethod]
    void multiCatch() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("try{}catch(Abc|Def e){}"));
        assertNoProblems(result);
    }

    [TestMethod]
    void multiCatchWithoutElements() {
        UnionType unionType = new UnionType();

        List<Problem> problems = new ArrayList<>();
        new Java7Validator().accept(unionType, new ProblemReporter(problems::add));

        assertProblems(problems, "UnionType.elements can not be empty.");
    }

    [TestMethod]
    void multiCatchWithOneElement() {
        UnionType unionType = new UnionType();
        unionType.getElements().add(new ClassOrInterfaceType());

        List<Problem> problems = new ArrayList<>();
        new Java7Validator().accept(unionType, new ProblemReporter(problems::add));

        assertProblems(problems, "Union type (multi catch) must have at least two elements.");
    }

    [TestMethod]
    void noLambdas() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("a(() -> 1);"));
        assertProblems(result, "(line 1,col 3) Lambdas are not supported.");
    }
}
