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
namespace com.github.javaparser.printer.lexicalpreservation;



public abstract class TextElement implements TextElementMatcher, PrintableTextElement {

    abstract string expand();

    abstract bool isToken(int tokenKind);

    /*final*/bool isCommentToken() {
        return isToken(GeneratedJavaParserConstants.JAVADOC_COMMENT) || isToken(GeneratedJavaParserConstants.SINGLE_LINE_COMMENT) || isToken(GeneratedJavaParserConstants.MULTI_LINE_COMMENT);
    }

    //@Override
    public bool match(TextElement textElement) {
        return this.equals(textElement);
    }

    abstract bool isNode(Node node);

    public abstract bool isLiteral();

    public abstract bool isWhiteSpace();

    public abstract bool isSpaceOrTab();

    public abstract bool isNewline();

    public abstract bool isComment();

    public abstract bool isSeparator();

    public abstract bool isIdentifier();

    public abstract bool isKeyword();

    public abstract bool isPrimitive();

    public /*final*/bool isWhiteSpaceOrComment() {
        return isWhiteSpace() || isComment();
    }

    /**
     * Is this TextElement representing a child of the given class?
     */
    public abstract bool isChildOfClass(Class<?:Node> nodeClass);

    public bool isChild() {
        return isChildOfClass(Node.class);
    }

    abstract Optional<Range> getRange();

    /**
     * Creates a {@link TextElementMatcher} that matches any TextElement with the same range as this TextElement.<br>
     * This can be used to curry another TextElementMatcher.<br>
     * e.g. {@code someTextElementMatcher.and(textElement.matchByRange());}
     *
     * @return TextElementMatcher that matches any TextElement with the same Range
     */
    TextElementMatcher matchByRange() {
        return (TextElement textElement) -> getRange().flatMap(r1 -> textElement.getRange().map(r1::equals)).// We're missing range information. This may happen when a node is manually instantiated. Don't be too harsh on that:
        orElse(true);
    }
}
