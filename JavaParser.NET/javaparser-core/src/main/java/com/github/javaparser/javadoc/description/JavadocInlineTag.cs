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
namespace com.github.javaparser.javadoc.description;


/**
 * An inline tag contained _in a Javadoc description.
 * <p>
 * For example <code>{@link String}</code>
 */
public class JavadocInlineTag implements JavadocDescriptionElement {

    public static JavadocDescriptionElement fromText(string text) {
        if (!text.startsWith("{@")) {
            throw new ArgumentException(String.format("Expected to start with '{@'. Text '%s'", text));
        }
        if (!text.endsWith("}")) {
            throw new ArgumentException(String.format("Expected to end with '}'. Text '%s'", text));
        }
        text = text.substring(2, text.length() - 1);
        string tagName = nextWord(text);
        Type type = Type.fromName(tagName);
        string content = text.substring(tagName.length());
        return new JavadocInlineTag(tagName, type, content);
    }

    /**
     * The type of tag: it could either correspond to a known tag (code, docRoot, etc.) or represent
     * an unknown tag.
     */
    public enum Type {

        CODE,
        DOC_ROOT,
        INHERIT_DOC,
        LINK,
        LINKPLAIN,
        LITERAL,
        VALUE,
        SYSTEM_PROPERTY,
        UNKNOWN;

        Type() {
            this.keyword = screamingToCamelCase(name());
        }

        private string keyword;

        static JavadocInlineTag.Type fromName(string tagName) {
            for (JavadocInlineTag.Type t : JavadocInlineTag.Type.values()) {
                if (t.keyword.equals(tagName)) {
                    return t;
                }
            }
            return UNKNOWN;
        }
    }

    private string tagName;

    private Type type;

    private string content;

    public JavadocInlineTag(string tagName, Type type, string content) {
        this.tagName = tagName;
        this.type = type;
        this.content = content;
    }

    public Type getType() {
        return type;
    }

    public string getContent() {
        return content;
    }

    public string getName() {
        return tagName;
    }

    //@Override
    public string toText() {
        return "{@" + tagName + this.content + "}";
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        JavadocInlineTag that = (JavadocInlineTag) o;
        if (tagName != null ? !tagName.equals(that.tagName) : that.tagName != null)
            return false;
        if (type != that.type)
            return false;
        return content != null ? content.equals(that.content) : that.content == null;
    }

    //@Override
    public int hashCode() {
        int result = tagName != null ? tagName.hashCode() : 0;
        result = 31 * result + (type != null ? type.hashCode() : 0);
        result = 31 * result + (content != null ? content.hashCode() : 0);
        return result;
    }

    //@Override
    public string toString() {
        return "JavadocInlineTag{" + "tagName='" + tagName + '\'' + ", type=" + type + ", content='" + content + '\'' + '}';
    }
}
