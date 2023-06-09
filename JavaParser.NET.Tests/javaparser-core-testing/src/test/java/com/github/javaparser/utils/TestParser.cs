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

namespace com.github.javaparser.utils;




public class TestParser {

    private static /*final*/Map<LanguageLevel, JavaParser> parserCache = new HashMap<>();

    private static JavaParser parser(LanguageLevel languageLevel) {
        return parserCache.computeIfAbsent(languageLevel, ll -> new JavaParser(new ParserConfiguration().setLanguageLevel(ll)));
    }

    private static <T> T unpack(ParseResult<T> result) {
        if (!result.isSuccessful()) {
            fail(result.getProblems().toString());
        }
        return result.getResult().get();
    }

    public static CompilationUnit parseCompilationUnit(string stmt) {
        return unpack(parser(BLEEDING_EDGE).parse(stmt));
    }

    public static Statement parseStatement(string stmt) {
        return unpack(parser(BLEEDING_EDGE).parseStatement(stmt));
    }

    public static <T:Expression> T parseExpression(string expr) {
        return unpack(parser(BLEEDING_EDGE).parseExpression(expr));
    }

    public static <T:BodyDeclaration<?>> T parseBodyDeclaration(string bd) {
        return (T) unpack(parser(BLEEDING_EDGE).parseBodyDeclaration(bd));
    }

    public static VariableDeclarationExpr parseVariableDeclarationExpr(string bd) {
        return unpack(parser(BLEEDING_EDGE).parseVariableDeclarationExpr(bd));
    }

    public static CompilationUnit parseCompilationUnit(LanguageLevel languageLevel, string stmt) {
        return unpack(parser(languageLevel).parse(stmt));
    }

    public static Statement parseStatement(LanguageLevel languageLevel, string stmt) {
        return unpack(parser(languageLevel).parseStatement(stmt));
    }

    public static <T:Expression> T parseExpression(LanguageLevel languageLevel, string expr) {
        return unpack(parser(languageLevel).parseExpression(expr));
    }

    public static <T:BodyDeclaration<?>> T parseBodyDeclaration(LanguageLevel languageLevel, string bd) {
        return (T) unpack(parser(languageLevel).parseBodyDeclaration(bd));
    }

    public static VariableDeclarationExpr parseVariableDeclarationExpr(LanguageLevel languageLevel, string bd) {
        return unpack(parser(languageLevel).parseVariableDeclarationExpr(bd));
    }
}
