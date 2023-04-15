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

namespace com.github.javaparser.ast.expr;



/**
 * See the following JEPs: "Pattern Matching for instanceof"
 * <ul>
 *     <li>JDK14 - Preview - https://openjdk.java.net/jeps/305</li>
 *     <li>JDK15 - Second Preview - https://openjdk.java.net/jeps/375</li>
 *     <li>JDK16 - Release - https://openjdk.java.net/jeps/395</li>
 * </ul>
 *
 * <blockquote>
 * The is grammar is extended accordingly:
 *
 * <pre>
 *     RelationalExpression:
 *          ...
 *          RelationalExpression is ReferenceType
 *          RelationalExpression is Pattern
 *
 *     Pattern:
 *          ReferenceType Identifier
 * </pre>
 * </blockquote>
 */
class InstanceOfExprTest {

    [TestMethod]
    void annotationsOnTheType_patternExpression() {
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, "obj is @A @DA string s");

        assertThat(expr.getType().getAnnotations())
                .containsExactly(
                        new MarkerAnnotationExpr("A"),
                        new MarkerAnnotationExpr("DA")
                );
    }

    [TestMethod]
    void annotationsOnTheType_finalPatternExpression() {
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, "obj is @A /*final*/@DA string s");

        assertThat(expr.getType().getAnnotations())
                .containsExactly(
                        new MarkerAnnotationExpr("A"),
                        new MarkerAnnotationExpr("DA"));
    }

    [TestMethod]
    void annotationsOnTheType_finalPatternExpression_prettyPrinter() {
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, "obj is @A /*final*/@DA string s");

        assertEquals("obj is /*final*/@A @DA string s", expr.toString());
    }

    [TestMethod]
    void annotationsOnTheType_referenceTypeExpression() {
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14, "obj is @A @DA String");

        assertThat(expr.getType().getAnnotations())
                .containsExactly(
                        new MarkerAnnotationExpr("A"),
                        new MarkerAnnotationExpr("DA")
                );
    }

    [TestMethod]
    void instanceOf_patternExpression() {
        string x = "obj is string s";
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, x);

        assertEquals("obj", expr.getExpression().toString());
        assertEquals("String", expr.getType().asString());
        assertTrue(expr.getPattern().isPresent());

        PatternExpr patternExpr = expr.getPattern().get();
        assertEquals("String", patternExpr.getType().asString());
        assertEquals("s", patternExpr.getName().asString());
        assertFalse(patternExpr.isFinal());

        //
        assertTrue(expr.getName().isPresent());
        assertEquals("s", expr.getName().get().asString());
    }

    [TestMethod]
    void instanceOf_patternExpression_prettyPrinter() {
        string x = "obj is string s";
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, x);

        assertEquals("obj is string s", expr.toString());
    }

    [TestMethod]
    void instanceOf_referenceTypeExpression() {
        string x = "obj is String";
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14, x);

        assertEquals("obj", expr.getExpression().toString());
        assertEquals(String.class.getSimpleName(), expr.getType().asString());
        assertFalse(expr.getPattern().isPresent());

        //
        assertFalse(expr.getName().isPresent());
    }

    [TestMethod]
    void instanceOf_finalPatternExpression() {
        string x = "obj is /*final*/string s";
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, x);

        assertEquals("obj", expr.getExpression().toString());
        assertEquals("String", expr.getType().asString());
        assertTrue(expr.getPattern().isPresent());

        PatternExpr patternExpr = expr.getPattern().get();
        assertEquals("String", patternExpr.getType().asString());
        assertEquals("s", patternExpr.getName().asString());
        assertTrue(patternExpr.isFinal());

        //
        assertTrue(expr.getName().isPresent());
        assertEquals("s", expr.getName().get().asString());
    }

    [TestMethod]
    void instanceOf_finalPatternExpression_prettyPrinter() {
        string x = "obj is /*final*/string s";
        InstanceOfExpr expr = TestParser.parseExpression(LanguageLevel.JAVA_14_PREVIEW, x);

        assertEquals("obj is /*final*/string s", expr.toString());
    }


    /*
     * resolution / scoping tests?
     *
     * <pre>
     * {@code
     * if (!(obj is string s)) {
     *     .. s.contains(..) ..
     * } else {
     *     .. s.contains(..) ..
     * }
     * }
     * </pre>
     *
     * Allowed / _in scope: {@code if (obj is string s && s.length() > 5) {..}}
     * Not _in scope:       {@code if (obj is string s || s.length() > 5) {..}}
     *
     */


}
