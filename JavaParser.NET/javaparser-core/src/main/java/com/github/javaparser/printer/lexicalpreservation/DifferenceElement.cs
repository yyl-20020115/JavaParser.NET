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


public interface DifferenceElement {

    static DifferenceElement added(CsmElement element) {
        return new Added(element);
    }

    static DifferenceElement removed(CsmElement element) {
        return new Removed(element);
    }

    static DifferenceElement kept(CsmElement element) {
        return new Kept(element);
    }

    /**
     * Return the CsmElement considered _in this DifferenceElement.
     */
    CsmElement getElement();

    bool isAdded();

    bool isRemoved();

    bool isKept();

    default bool isChild() {
        return getElement() is LexicalDifferenceCalculator.CsmChild;
    }

    /*
     * If the {@code DifferenceElement} wraps an EOL token then this method returns a new wrapped {@code CsmElement}
     * with the specified line separator. The line separator parameter must be a {@code CsmToken} with a valid line
     * separator. By default this method returns the instance itself.
     */
    default DifferenceElement replaceEolTokens(CsmElement lineSeparator) {
        return this;
    }
}
