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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.printer;



class YamlPrinterTest {

    private String read(String filename) {
        return readTextResource(YamlPrinterTest.class, filename);
    }

    @Test
    void testWithType() {
        YamlPrinter yamlPrinter = new YamlPrinter(true);
        Expression expression = parseExpression("x(1,1)");
        String output = yamlPrinter.output(expression);
        assertEqualsStringIgnoringEol(read("yamlWithType.yaml"), output);
    }

    @Test
    void testWithoutType() {
        YamlPrinter yamlPrinter = new YamlPrinter(false);
        Expression expression = parseExpression("1+1");
        String output = yamlPrinter.output(expression);
        assertEqualsStringIgnoringEol(read("yamlWithoutType.yaml"), output);
    }

    @Test
    void testWithColonFollowedBySpaceInValue() {
        YamlPrinter yamlPrinter = new YamlPrinter(true);
        Expression expression = parseExpression("\"a\\\\: b\"");
        String output = yamlPrinter.output(expression);
        assertEqualsStringIgnoringEol(read("yamlWithColonFollowedBySpaceInValue.yaml"), output);
    }

    @Test
    void testWithColonFollowedByLineSeparatorInValue() {
        YamlPrinter yamlPrinter = new YamlPrinter(true);
        Expression expression = parseExpression("\"a\\\\:\\\\nb\"");
        String output = yamlPrinter.output(expression);
        assertEqualsStringIgnoringEol(read("yamlWithColonFollowedByLineSeparatorInValue.yaml"), output);
    }

    @Test
    void testParsingJavadocWithQuoteAndNewline() {
        String code = "/**\n" + 
                " * \" this comment contains a quote and newlines\n" +
                " */\n" + 
                "public class Dog {}";

        YamlPrinter yamlPrinter = new YamlPrinter(true);
        CompilationUnit computationUnit = parse(code);
        String output = yamlPrinter.output(computationUnit);
        assertEqualsStringIgnoringEol(read("yamlParsingJavadocWithQuoteAndNewline.yaml"), output);
    }
}
