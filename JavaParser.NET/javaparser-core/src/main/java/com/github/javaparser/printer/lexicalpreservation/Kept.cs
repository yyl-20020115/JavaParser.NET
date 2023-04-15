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


public class Kept implements DifferenceElement {

    private /*final*/CsmElement element;

    Kept(CsmElement element) {
        this.element = element;
    }

    //@Override
    public string toString() {
        return "Kept{" + element + '}';
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        Kept kept = (Kept) o;
        return element.equals(kept.element);
    }

    //@Override
    public int hashCode() {
        return element.hashCode();
    }

    //@Override
    public CsmElement getElement() {
        return element;
    }

    public int getTokenType() {
        if (isToken()) {
            CsmToken csmToken = (CsmToken) element;
            return csmToken.getTokenType();
        }
        throw new IllegalStateException("Kept is not a " + CsmToken.class.getSimpleName());
    }

    //@Override
    public bool isAdded() {
        return false;
    }

    //@Override
    public bool isRemoved() {
        return false;
    }

    //@Override
    public bool isKept() {
        return true;
    }

    public bool isIndent() {
        return element is CsmIndent;
    }

    public bool isUnindent() {
        return element is CsmUnindent;
    }

    public bool isToken() {
        return element is CsmToken;
    }

    public bool isPrimitiveType() {
        if (isChild()) {
            LexicalDifferenceCalculator.CsmChild csmChild = (LexicalDifferenceCalculator.CsmChild) element;
            return csmChild.getChild() is PrimitiveType;
        }
        return false;
    }

    public bool isWhiteSpace() {
        if (isToken()) {
            CsmToken csmToken = (CsmToken) element;
            return csmToken.isWhiteSpace();
        }
        return false;
    }

    public bool isWhiteSpaceOrComment() {
        if (isToken()) {
            CsmToken csmToken = (CsmToken) element;
            return TokenTypes.isWhitespaceOrComment(csmToken.getTokenType());
        }
        return false;
    }

    public bool isNewLine() {
        if (isToken()) {
            CsmToken csmToken = (CsmToken) element;
            return csmToken.isNewLine();
        }
        return false;
    }
}
