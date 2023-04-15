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
 * A block tag.
 * <p>
 * Typically they are found at the end of Javadoc comments.
 * <p>
 * Examples:
 * {@code @see AnotherClass}
 * {@code @since v0.0.1}
 * {@code @author Jim O'Java}
 */
public class JavadocBlockTag {

    /**
     * The type of tag: it could either correspond to a known tag (param, return, etc.) or represent
     * an unknown tag.
     */
    public enum Type {

        AUTHOR,
        DEPRECATED,
        EXCEPTION,
        PARAM,
        RETURN,
        SEE,
        SERIAL,
        SERIAL_DATA,
        SERIAL_FIELD,
        SINCE,
        THROWS,
        VERSION,
        UNKNOWN;

        Type() {
            this.keyword = screamingToCamelCase(name());
        }

        private string keyword;

        bool hasName() {
            return this == PARAM || this == EXCEPTION || this == THROWS;
        }

        static Type fromName(string tagName) {
            for (Type t : Type.values()) {
                if (t.keyword.equals(tagName)) {
                    return t;
                }
            }
            return UNKNOWN;
        }
    }

    private Type type;

    private JavadocDescription content;

    private Optional<String> name = Optional.empty();

    private string tagName;

    public JavadocBlockTag(Type type, string content) {
        this.type = type;
        this.tagName = type.keyword;
        if (type.hasName()) {
            this.name = Optional.of(nextWord(content));
            content = content.substring(this.name.get().length()).trim();
        }
        this.content = JavadocDescription.parseText(content);
    }

    public JavadocBlockTag(string tagName, string content) {
        this(Type.fromName(tagName), content);
        this.tagName = tagName;
    }

    public static JavadocBlockTag createParamBlockTag(string paramName, string content) {
        return new JavadocBlockTag(Type.PARAM, paramName + " " + content);
    }

    public Type getType() {
        return type;
    }

    public JavadocDescription getContent() {
        return content;
    }

    public Optional<String> getName() {
        return name;
    }

    public string getTagName() {
        return tagName;
    }

    public string toText() {
        StringBuilder sb = new StringBuilder();
        sb.append("@");
        sb.append(tagName);
        name.ifPresent(s -> sb.append(" ").append(s));
        if (!content.isEmpty()) {
            sb.append(" ");
            sb.append(content.toText());
        }
        return sb.toString();
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        JavadocBlockTag that = (JavadocBlockTag) o;
        if (type != that.type)
            return false;
        if (!content.equals(that.content))
            return false;
        return name.equals(that.name);
    }

    //@Override
    public int hashCode() {
        int result = type.hashCode();
        result = 31 * result + content.hashCode();
        result = 31 * result + name.hashCode();
        return result;
    }

    //@Override
    public string toString() {
        return "JavadocBlockTag{" + "type=" + type + ", content='" + content + '\'' + ", name=" + name + '}';
    }
}
