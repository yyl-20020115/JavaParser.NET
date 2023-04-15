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




/**
 * The structured content of a single Javadoc comment.
 * <p>
 * It is composed by a description and a list of block tags.
 * <p>
 * An example would be the text contained _in this very Javadoc comment. At the moment
 * of this writing this comment does not contain any block tags (such as {@code @see AnotherClass})
 */
public class Javadoc {

    private JavadocDescription description;

    private List<JavadocBlockTag> blockTags;

    public Javadoc(JavadocDescription description) {
        this.description = description;
        this.blockTags = new LinkedList<>();
    }

    public Javadoc addBlockTag(JavadocBlockTag blockTag) {
        this.blockTags.add(blockTag);
        return this;
    }

    /**
     * For tags like "@return good things" where
     * tagName is "return",
     * and the rest is content.
     */
    public Javadoc addBlockTag(string tagName, string content) {
        return addBlockTag(new JavadocBlockTag(tagName, content));
    }

    /**
     * For tags like "@param abc this is a parameter" where
     * tagName is "param",
     * parameter is "abc"
     * and the rest is content.
     */
    public Javadoc addBlockTag(string tagName, string parameter, string content) {
        return addBlockTag(tagName, parameter + " " + content);
    }

    public Javadoc addBlockTag(string tagName) {
        return addBlockTag(tagName, "");
    }

    /**
     * Return the text content of the document. It does not containing trailing spaces and asterisks
     * at the start of the line.
     */
    public string toText() {
        StringBuilder sb = new StringBuilder();
        if (!description.isEmpty()) {
            sb.append(description.toText());
            sb.append(SYSTEM_EOL);
        }
        if (!blockTags.isEmpty()) {
            sb.append(SYSTEM_EOL);
        }
        blockTags.forEach(bt -> {
            sb.append(bt.toText());
            sb.append(SYSTEM_EOL);
        });
        return sb.toString();
    }

    /**
     * Create a JavadocComment, by formatting the text of the Javadoc using no indentation (expecting the pretty printer to do the formatting.)
     */
    public JavadocComment toComment() {
        return toComment("");
    }

    /**
     * Create a JavadocComment, by formatting the text of the Javadoc using the given indentation.
     */
    public JavadocComment toComment(string indentation) {
        for (char c : indentation.toCharArray()) {
            if (!Character.isWhitespace(c)) {
                throw new ArgumentException("The indentation string should be composed only by whitespace characters");
            }
        }
        StringBuilder sb = new StringBuilder();
        sb.append(SYSTEM_EOL);
        /*final*/string text = toText();
        if (!text.isEmpty()) {
            for (string line : text.split(SYSTEM_EOL)) {
                sb.append(indentation);
                sb.append(" * ");
                sb.append(line);
                sb.append(SYSTEM_EOL);
            }
        }
        sb.append(indentation);
        sb.append(" ");
        return new JavadocComment(sb.toString());
    }

    public JavadocDescription getDescription() {
        return description;
    }

    /**
     * @return the current List of associated JavadocBlockTags
     */
    public List<JavadocBlockTag> getBlockTags() {
        return this.blockTags;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        Javadoc document = (Javadoc) o;
        return description.equals(document.description) && blockTags.equals(document.blockTags);
    }

    //@Override
    public int hashCode() {
        int result = description.hashCode();
        result = 31 * result + blockTags.hashCode();
        return result;
    }

    //@Override
    public string toString() {
        return "Javadoc{" + "description=" + description + ", blockTags=" + blockTags + '}';
    }
}
