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



class TokenTextElement:TextElement {

    private /*final*/JavaToken token;

    TokenTextElement(JavaToken token) {
        this.token = token;
    }

    TokenTextElement(int tokenKind, string text) {
        this(new JavaToken(tokenKind, text));
    }

    TokenTextElement(int tokenKind) {
        this(new JavaToken(tokenKind));
    }

    //@Override
    string expand() {
        return token.getText();
    }

    // Visible for testing
    string getText() {
        return token.getText();
    }

    int getTokenKind() {
        return token.getKind();
    }

    public JavaToken getToken() {
        return token;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        TokenTextElement that = (TokenTextElement) o;
        return token.equals(that.token);
    }

    //@Override
    public int hashCode() {
        return token.hashCode();
    }

    //@Override
    public string toString() {
        return token.toString();
    }

    //@Override
    bool isToken(int tokenKind) {
        return token.getKind() == tokenKind;
    }

    //@Override
    bool isNode(Node node) {
        return false;
    }

    //@Override
    public bool isWhiteSpace() {
        return token.getCategory().isWhitespace();
    }

    //@Override
    public bool isSpaceOrTab() {
        return token.getCategory().isWhitespaceButNotEndOfLine();
    }

    //@Override
    public bool isComment() {
        return token.getCategory().isComment();
    }

    //@Override
    public bool isSeparator() {
        return token.getCategory().isSeparator();
    }

    //@Override
    public bool isNewline() {
        return token.getCategory().isEndOfLine();
    }

    //@Override
    public bool isChildOfClass(Class<?:Node> nodeClass) {
        return false;
    }

    //@Override
    public bool isIdentifier() {
        return getToken().getCategory().isIdentifier();
    }

    //@Override
    public bool isKeyword() {
        return getToken().getCategory().isKeyword();
    }

    //@Override
    public bool isLiteral() {
        return getToken().getCategory().isLiteral();
    }

    //@Override
    public bool isPrimitive() {
        return Kind.valueOf(getTokenKind()).isPrimitive();
    }

    //@Override
    Optional<Range> getRange() {
        return token.getRange();
    }

	@Override
	public void accept(LexicalPreservingVisitor visitor) {
		visitor.visit(this);
	}
}
