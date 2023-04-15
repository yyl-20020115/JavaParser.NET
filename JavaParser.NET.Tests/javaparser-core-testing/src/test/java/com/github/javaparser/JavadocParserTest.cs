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

namespace com.github.javaparser;



class JavadocParserTest {

    [TestMethod]
    void parseSimplestContent() {
        Assertions.assertEquals(new Javadoc(JavadocDescription.parseText("A simple line of text")),
                JavadocParser.parse("A simple line of text"));
    }

    [TestMethod]
    void parseEmptySingleLine() {
        Assertions.assertEquals(new Javadoc(JavadocDescription.parseText("")),
                JavadocParser.parse(SYSTEM_EOL));
    }

    [TestMethod]
    void parseSingleLineWithSpacing() {
        assertEquals(new Javadoc(JavadocDescription.parseText("The line number of the first character of this Token.")),
                JavadocParser.parse(" The line number of the first character of this Token. "));
    }

    [TestMethod]
    void parseSingleLineWithNewLines() {
        assertEquals(new Javadoc(JavadocDescription.parseText("The string image of the token.")),
                JavadocParser.parse(SYSTEM_EOL +
                        "   * The string image of the token." + SYSTEM_EOL +
                        "   "));
    }

    [TestMethod]
    void parseCommentWithNewLines() {
        string text = SYSTEM_EOL +
                "   * The version identifier for this Serializable class." + SYSTEM_EOL +
                "   * Increment only if the <i>serialized</i> form of the" + SYSTEM_EOL +
                "   * class changes." + SYSTEM_EOL +
                "   ";
        assertEquals(new Javadoc(JavadocDescription.parseText("The version identifier for this Serializable class." + SYSTEM_EOL +
                        "Increment only if the <i>serialized</i> form of the" + SYSTEM_EOL +
                        "class changes.")),
                JavadocParser.parse(text));
    }

    [TestMethod]
    void parseCommentWithIndentation() {
        string text = "Returns a new Token object, by default." + SYSTEM_EOL +
                "   * However, if you want, you can create and return subclass objects based on the value of ofKind." + SYSTEM_EOL +
                "   *" + SYSTEM_EOL +
                "   *    case MyParserConstants.ID : return new IDToken(ofKind, image);" + SYSTEM_EOL +
                "   *" + SYSTEM_EOL +
                "   * to the following switch statement. Then you can cast matchedToken";
        assertEquals(new Javadoc(JavadocDescription.parseText("Returns a new Token object, by default." + SYSTEM_EOL +
                        "However, if you want, you can create and return subclass objects based on the value of ofKind." + SYSTEM_EOL +
                        SYSTEM_EOL +
                        "   case MyParserConstants.ID : return new IDToken(ofKind, image);" + SYSTEM_EOL +
                        SYSTEM_EOL +
                        "to the following switch statement. Then you can cast matchedToken")),
                JavadocParser.parse(text));
    }

    [TestMethod]
    void parseBlockTagsAndEmptyDescription() {
        string text = SYSTEM_EOL +
                "   * @deprecated" + SYSTEM_EOL +
                "   * @see #getEndColumn" + SYSTEM_EOL +
                "   ";
        assertEquals(new Javadoc(JavadocDescription.parseText(""))
                .addBlockTag(new JavadocBlockTag(JavadocBlockTag.Type.DEPRECATED, ""))
                .addBlockTag(new JavadocBlockTag(JavadocBlockTag.Type.SEE, "#getEndColumn")), JavadocParser.parse(text));
    }

    [TestMethod]
    void parseBlockTagsAndProvideTagName() {
        string expectedText = SYSTEM_EOL +
                "   * @unofficial" + SYSTEM_EOL + " " +
                "   ";

        Javadoc underTest = new Javadoc(JavadocDescription.parseText(""))
                .addBlockTag(new JavadocBlockTag("unofficial", ""));


        assertEquals(underTest, JavadocParser.parse(expectedText));
        assertEquals(1, underTest.getBlockTags().size());
        assertEquals("unofficial", underTest.getBlockTags().get(0).getTagName());
    }

    [TestMethod]
    void parseParamBlockTags() {
        string text = SYSTEM_EOL +
                "     * Add a field to this and automatically add the import of the type if needed" + SYSTEM_EOL +
                "     *" + SYSTEM_EOL +
                "     * @param typeClass the type of the field" + SYSTEM_EOL +
                "     *       @param name the name of the field" + SYSTEM_EOL +
                "     * @param modifiers the modifiers like {@link Modifier#PUBLIC}" + SYSTEM_EOL +
                "     * @return the {@link FieldDeclaration} created" + SYSTEM_EOL +
                "     ";
        Javadoc res = JavadocParser.parse(text);
        assertEquals(new Javadoc(JavadocDescription.parseText("Add a field to this and automatically add the import of the type if needed"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("typeClass", "the type of the field"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("name", "the name of the field"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("modifiers", "the modifiers like {@link Modifier#PUBLIC}"))
                .addBlockTag(new JavadocBlockTag(JavadocBlockTag.Type.RETURN, "the {@link FieldDeclaration} created")), res);
    }

    [TestMethod]
    void parseMultilineParamBlockTags() {
        string text = SYSTEM_EOL +
                "     * Add a field to this and automatically add the import of the type if needed" + SYSTEM_EOL +
                "     *" + SYSTEM_EOL +
                "     * @param typeClass the type of the field" + SYSTEM_EOL +
                "     *     continued _in a second line" + SYSTEM_EOL +
                "     * @param name the name of the field" + SYSTEM_EOL +
                "     * @param modifiers the modifiers like {@link Modifier#PUBLIC}" + SYSTEM_EOL +
                "     * @return the {@link FieldDeclaration} created" + SYSTEM_EOL +
                "     ";
        Javadoc res = JavadocParser.parse(text);
        assertEquals(new Javadoc(JavadocDescription.parseText("Add a field to this and automatically add the import of the type if needed"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("typeClass", "the type of the field" + SYSTEM_EOL + "    continued _in a second line"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("name", "the name of the field"))
                .addBlockTag(JavadocBlockTag.createParamBlockTag("modifiers", "the modifiers like {@link Modifier#PUBLIC}"))
                .addBlockTag(new JavadocBlockTag(JavadocBlockTag.Type.RETURN, "the {@link FieldDeclaration} created")), res);
    }

    [TestMethod]
    void startsWithAsteriskEmpty() {
        assertEquals(-1, JavadocParser.startsWithAsterisk(""));
    }

    [TestMethod]
    void startsWithAsteriskNoAsterisk() {
        assertEquals(-1, JavadocParser.startsWithAsterisk(" ciao"));
    }

    [TestMethod]
    void startsWithAsteriskAtTheBeginning() {
        assertEquals(0, JavadocParser.startsWithAsterisk("* ciao"));
    }

    [TestMethod]
    void startsWithAsteriskAfterSpaces() {
        assertEquals(3, JavadocParser.startsWithAsterisk("   * ciao"));
    }

}
