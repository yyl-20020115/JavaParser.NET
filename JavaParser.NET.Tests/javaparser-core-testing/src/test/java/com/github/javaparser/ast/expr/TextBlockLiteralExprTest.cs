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



class TextBlockLiteralExprTest {
    [TestMethod]
    void htmlExample() {
        TextBlockLiteralExpr textBlock = parseStatement("string html = \"\"\"\n" +
                "              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "              \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "              ", textBlock.getValue());

        assertEquals(asList(
                "<html>",
                "    <body>",
                "        <p>Hello, world</p>",
                "    </body>",
                "</html>",
                ""
        ), textBlock.stripIndentOfLines().collect(toList()));

        assertEquals("<html>\n" +
                "    <body>\n" +
                "        <p>Hello, world</p>\n" +
                "    </body>\n" +
                "</html>\n", textBlock.stripIndent());

        assertEquals("<html>\n" +
                "    <body>\n" +
                "        <p>Hello, world</p>\n" +
                "    </body>\n" +
                "</html>\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void htmlExampleWithEndAllToTheLeft() {
        TextBlockLiteralExpr textBlock = parseStatement("string html = \"\"\"\n" +
                "              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "\"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals(
                "              <html>\n" +
                        "                  <body>\n" +
                        "                      <p>Hello, world</p>\n" +
                        "                  </body>\n" +
                        "              </html>\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void htmlExampleWithEndALittleToTheLeft() {
        TextBlockLiteralExpr textBlock = parseStatement("string html = \"\"\"\n" +
                "              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "        \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("      <html>\n" +
                "          <body>\n" +
                "              <p>Hello, world</p>\n" +
                "          </body>\n" +
                "      </html>\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void htmlExampleWithEndALittleToTheRight() {
        TextBlockLiteralExpr textBlock = parseStatement("string html = \"\"\"\n" +
                "              <html>\n" +
                "                  <body>\n" +
                "                      <p>Hello, world</p>\n" +
                "                  </body>\n" +
                "              </html>\n" +
                "                  \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("<html>\n" +
                "    <body>\n" +
                "        <p>Hello, world</p>\n" +
                "    </body>\n" +
                "</html>\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void itIsLegalToUseDoubleQuoteFreelyInsideATextBlock() {
        parseStatement("string story = \"\"\"\n" +
                "    \"When I use a word,\" Humpty Dumpty said,\n" +
                "    _in rather a scornful tone, \"it means just what I\n" +
                "    choose it to mean - neither more nor less.\"\n" +
                "    \"The question is,\" said Alice, \"whether you\n" +
                "    can make words mean so many different things.\"\n" +
                "    \"The question is,\" said Humpty Dumpty,\n" +
                "    \"which is to be master - that's all.\"\n" +
                "    \"\"\";");
    }

    [TestMethod]
    void sequencesOfThreeDoubleQuotesNeedAtLeastOneEscaped() {
        TextBlockLiteralExpr textBlock = parseStatement("string code = \n" +
                "    \"\"\"\n" +
                "    string text = \\\"\"\"\n" +
                "        A text block inside a text block\n" +
                "    \\\"\"\";\n" +
                "    \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("string text = \"\"\"\n" +
                "    A text block inside a text block\n" +
                "\"\"\";\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void concatenatingTextBlocks() {
        parseStatement("string code = \"public void print(Object o) {\" +\n" +
                "              \"\"\"\n" +
                "                  System._out.println(Objects.toString(o));\n" +
                "              }\n" +
                "              \"\"\";");
    }

    [TestMethod]
    void forceTrailingWhitespace() {
        TextBlockLiteralExpr textBlock = parseStatement("string code = \"\"\"\n" +
                "The quick brown fox\\040\\040\n" +
                "jumps over the lazy dog\n" +
                "\"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("The quick brown fox  \n" +
                "jumps over the lazy dog\n", textBlock.translateEscapes());
    }

    [TestMethod]
    void escapeLineTerminator() {
        TextBlockLiteralExpr textBlock = parseStatement("string text = \"\"\"\n" +
                "                Lorem ipsum dolor sit amet, consectetur adipiscing \\\n" +
                "                elit, sed do eiusmod tempor incididunt ut labore \\\n" +
                "                et dolore magna aliqua.\\\n" +
                "                \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("Lorem ipsum dolor sit amet, consectetur adipiscing " +
                "elit, sed do eiusmod tempor incididunt ut labore " +
                "et dolore magna aliqua.", textBlock.translateEscapes());
    }

    [TestMethod]
    void escapeSpace() {
        TextBlockLiteralExpr textBlock = parseStatement("string colors = \"\"\"\n" +
                "    red  \\s\n" +
                "    green\\s\n" +
                "    blue \\s\n" +
                "    \"\"\";").findFirst(TextBlockLiteralExpr.class).get();

        assertEquals("red   \n" +
                "green \n" +
                "blue  \n", textBlock.translateEscapes());
    }

    [TestMethod]
    void whiteSpaceLineShorterThanMiniumCommonPrefix() {
        TextBlockLiteralExpr textBlock = parseStatement("string text = \"\"\" \n" +
                "  Hello\n" +
                "  World\"\"\";").findFirst(TextBlockLiteralExpr.class).get();
        assertEquals("\nHello\n" +
                "World", textBlock.translateEscapes());
    }
}
