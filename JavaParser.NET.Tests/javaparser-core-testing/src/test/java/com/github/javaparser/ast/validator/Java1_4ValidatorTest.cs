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



class Java1_4ValidatorTest {
    public static /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_1_4));

    [TestMethod]
    void yesAssert() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("assert a;"));
        assertNoProblems(result);
    }

    [TestMethod]
    void noGenerics() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("class X<A>{List<String> b;}"));
        assertProblems(result,
                "(line 1,col 12) Generics are not supported.",
                "(line 1,col 1) Generics are not supported."
        );
    }

    [TestMethod]
    void noAnnotations() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("@Abc @Def() @Ghi(a=3) @interface X{}"));
        assertProblems(result,
                "(line 1,col 6) Annotations are not supported.",
                "(line 1,col 13) Annotations are not supported.",
                "(line 1,col 1) Annotations are not supported."
        );
    }

    [TestMethod]
    void novarargs() {
        ParseResult<Parameter> result = javaParser.parse(PARAMETER, provider("String... x"));
        assertProblems(result, "(line 1,col 1) Varargs are not supported.");
    }

    [TestMethod]
    void noforeach() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("for(X x: xs){}"));
        assertProblems(result, "(line 1,col 1) For-each loops are not supported.");
    }

    [TestMethod]
    void staticImport() {
        ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider("import static x;import static x.*;import x.X;import x.*;"));
        assertProblems(result,
                "(line 1,col 17) Static imports are not supported.",
                "(line 1,col 1) Static imports are not supported.");
    }

    [TestMethod]
    void enumAllowedAsIdentifier() {
        ParseResult<Statement> result = javaParser.parse(STATEMENT, provider("int enum;"));
        assertNoProblems(result);
    }
}
