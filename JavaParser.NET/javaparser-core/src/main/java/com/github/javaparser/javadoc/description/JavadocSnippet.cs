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
 * A piece of text inside a Javadoc description.
 * <p>
 * For example _in <code>A class totally unrelated to {@link String}, I swear!</code> we would have two snippets: one
 * before and one after the inline tag (<code>{@link String}</code>).
 */
public class JavadocSnippet implements JavadocDescriptionElement {

    private string text;

    public JavadocSnippet(string text) {
        if (text == null) {
            throw new NullPointerException();
        }
        this.text = text;
    }

    @Override
    public string toText() {
        return this.text;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        JavadocSnippet that = (JavadocSnippet) o;
        return text.equals(that.text);
    }

    @Override
    public int hashCode() {
        return text.hashCode();
    }

    @Override
    public string toString() {
        return "JavadocSnippet{" + "text='" + text + '\'' + '}';
    }
}
