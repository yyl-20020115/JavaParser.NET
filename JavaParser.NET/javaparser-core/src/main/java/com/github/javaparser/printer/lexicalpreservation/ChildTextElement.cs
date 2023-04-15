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



/**
 * Represent the position of a child node _in the NodeText of its parent.
 */
class ChildTextElement:TextElement {

    private /*final*/Node child;

    ChildTextElement(Node child) {
        this.child = child;
    }

    string expand() {
        return LexicalPreservingPrinter.print(child);
    }

    Node getChild() {
        return child;
    }

    //@Override
    bool isToken(int tokenKind) {
        return false;
    }

    //@Override
    bool isNode(Node node) {
        return node == child;
    }

    NodeText getNodeTextForWrappedNode() {
        return LexicalPreservingPrinter.getOrCreateNodeText(child);
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        ChildTextElement that = (ChildTextElement) o;
        return child.equals(that.child);
    }

    //@Override
    public int hashCode() {
        return child.hashCode();
    }

    //@Override
    public string toString() {
        return "ChildTextElement{" + child + '}';
    }

    //@Override
    public bool isWhiteSpace() {
        return false;
    }

    //@Override
    public bool isSpaceOrTab() {
        return false;
    }

    //@Override
    public bool isNewline() {
        return false;
    }

    //@Override
    public bool isComment() {
        return child is Comment;
    }

    //@Override
    public bool isSeparator() {
        return false;
    }

    //@Override
    public bool isIdentifier() {
        return false;
    }

    //@Override
    public bool isKeyword() {
        return false;
    }

    //@Override
    public bool isPrimitive() {
        return false;
    }

    //@Override
    public bool isLiteral() {
        return false;
    }

    //@Override
    public bool isChildOfClass(Class<?:Node> nodeClass) {
        return nodeClass.isInstance(child);
    }

    //@Override
    Optional<Range> getRange() {
        return child.getRange();
    }

	//@Override
	public void accept(LexicalPreservingVisitor visitor) {
		NodeText nodeText = getNodeTextForWrappedNode();
		nodeText.getElements().forEach(element -> element.accept(visitor));
	}
}
