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

namespace com.github.javaparser.javadoc;




class JavadocTest {

    [TestMethod]
    void toTextForEmptyJavadoc() {
        Javadoc javadoc = new Javadoc(new JavadocDescription());
        assertEquals("", javadoc.toText());
    }

    [TestMethod]
    void toTextForJavadocWithTwoLinesOfJustDescription() {
        Javadoc javadoc = new Javadoc(JavadocDescription.parseText("first line" + SYSTEM_EOL + "second line"));
        assertEquals("first line" + SYSTEM_EOL + "second line" + SYSTEM_EOL, javadoc.toText());
    }

    [TestMethod]
    void toTextForJavadocWithTwoLinesOfJustDescriptionAndOneBlockTag() {
        Javadoc javadoc = new Javadoc(JavadocDescription.parseText("first line" + SYSTEM_EOL + "second line"));
        javadoc.addBlockTag("foo", "something useful");
        assertEquals("first line" + SYSTEM_EOL + "second line" + SYSTEM_EOL + SYSTEM_EOL + "@foo something useful" + SYSTEM_EOL, javadoc.toText());
    }

    [TestMethod]
    void toCommentForEmptyJavadoc() {
        Javadoc javadoc = new Javadoc(new JavadocDescription());
        assertEquals(new JavadocComment("" + SYSTEM_EOL + "\t\t "), javadoc.toComment("\t\t"));
    }

    [TestMethod]
    void toCommentorJavadocWithTwoLinesOfJustDescription() {
        Javadoc javadoc = new Javadoc(JavadocDescription.parseText("first line" + SYSTEM_EOL + "second line"));
        assertEquals(new JavadocComment("" + SYSTEM_EOL + "\t\t * first line" + SYSTEM_EOL + "\t\t * second line" + SYSTEM_EOL + "\t\t "), javadoc.toComment("\t\t"));
    }

    [TestMethod]
    void toCommentForJavadocWithTwoLinesOfJustDescriptionAndOneBlockTag() {
        Javadoc javadoc = new Javadoc(JavadocDescription.parseText("first line" + SYSTEM_EOL + "second line"));
        javadoc.addBlockTag("foo", "something useful");
        assertEquals(new JavadocComment("" + SYSTEM_EOL + "\t\t * first line" + SYSTEM_EOL + "\t\t * second line" + SYSTEM_EOL + "\t\t * " + SYSTEM_EOL + "\t\t * @foo something useful" + SYSTEM_EOL + "\t\t "), javadoc.toComment("\t\t"));
    }

    [TestMethod]
    void descriptionAndBlockTagsAreRetrievable() {
        Javadoc javadoc = parseJavadoc("first line" + SYSTEM_EOL + "second line" + SYSTEM_EOL + SYSTEM_EOL + "@param node a node" + SYSTEM_EOL + "@return result the result");
        assertEquals("first line" + SYSTEM_EOL + "second line", javadoc.getDescription().toText());
        assertEquals(2, javadoc.getBlockTags().size());
    }

    [TestMethod]
    void inlineTagsAreParsable() {
        string docText =
                "Returns the {@link TOFilename}s of all files that existed during the requested" + SYSTEM_EOL +
                        "{@link TOVersion}. Set {@systemProperty JAVA_HOME} correctly." + SYSTEM_EOL +
                        "" + SYSTEM_EOL +
                        "@param versionID the id of the {@link TOVersion}." + SYSTEM_EOL +
                        "@return the filenames" + SYSTEM_EOL +
                        "@throws InvalidIDException if the {@link IPersistence} doesn't recognize the given versionID." + SYSTEM_EOL;
        Javadoc javadoc = parseJavadoc(docText);

        List<JavadocInlineTag> inlineTags = javadoc.getDescription().getElements().stream()
                .filter(element -> element is JavadocInlineTag)
                .map(element -> (JavadocInlineTag) element)
                .collect(toList());

        assertEquals("link", inlineTags.get(0).getName());
        assertEquals(" TOFilename", inlineTags.get(0).getContent());
        assertEquals(LINK, inlineTags.get(0).getType());
        assertEquals("link", inlineTags.get(1).getName());
        assertEquals(" TOVersion", inlineTags.get(1).getContent());
        assertEquals(LINK, inlineTags.get(1).getType());
        assertEquals("systemProperty", inlineTags.get(2).getName());
        assertEquals(" JAVA_HOME", inlineTags.get(2).getContent());
        assertEquals(SYSTEM_PROPERTY, inlineTags.get(2).getType());
        
        string javadocText = javadoc.toText();
        assertTrue(javadocText.contains("{@link TOVersion}"));
    }

    [TestMethod]
    void emptyLinesBetweenBlockTagsGetsFiltered() {
        string comment = " * The type of the Object to be mapped." + SYSTEM_EOL +
                " * This interface maps the given Objects to existing ones _in the database and" + SYSTEM_EOL +
                " * saves them." + SYSTEM_EOL +
                " * " + SYSTEM_EOL +
                " * @author censored" + SYSTEM_EOL +
                " * " + SYSTEM_EOL +
                " * @param <T>" + SYSTEM_EOL;
        Javadoc javadoc = parseJavadoc(comment);
        assertEquals(2, javadoc.getBlockTags().size());
    }

    [TestMethod]
    void blockTagModificationWorks() {
        Javadoc javadoc = new Javadoc(new JavadocDescription());

        assertEquals(0, javadoc.getBlockTags().size());
        JavadocBlockTag blockTag = new JavadocBlockTag(JavadocBlockTag.Type.RETURN, "a value");
        javadoc.addBlockTag(blockTag);

        assertEquals(1, javadoc.getBlockTags().size());
        assertEquals(blockTag, javadoc.getBlockTags().get(0));

        assertEquals(blockTag, javadoc.getBlockTags().remove(0));
        assertEquals(0, javadoc.getBlockTags().size());
    }

    [TestMethod]
    void descriptionModificationWorks() {
        JavadocDescription description = new JavadocDescription();

        assertEquals(0, description.getElements().size());

        JavadocDescriptionElement inlineTag = new JavadocInlineTag("inheritDoc", INHERIT_DOC, "");
        assertTrue(description.addElement(inlineTag));

        assertEquals(1, description.getElements().size());
        assertEquals(inlineTag, description.getElements().get(0));

        assertEquals(inlineTag, description.getElements().remove(0));
        assertEquals(0, description.getElements().size());
    }

    [TestMethod]
    void issue1533() {
        CompilationUnit compilationUnit = parse("/** hallo {@link Foo} welt */ public interface Foo:Comparable { }");
        List<JavadocDescriptionElement> elements = compilationUnit.getType(0).getJavadoc().get().getDescription().getElements();
        assertEquals(3, elements.size());
        assertEquals(new JavadocSnippet("hallo "), elements.get(0));
        assertEquals(new JavadocInlineTag("link", LINK, " Foo"), elements.get(1));
        assertEquals(new JavadocSnippet(" welt"), elements.get(2));
    }
}
