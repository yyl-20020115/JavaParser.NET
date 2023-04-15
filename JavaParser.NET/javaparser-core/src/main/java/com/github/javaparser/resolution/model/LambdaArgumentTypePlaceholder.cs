/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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

namespace com.github.javaparser.resolution.model;


/**
 * Placeholder used to represent a lambda argument type while it is being
 * calculated.
 *
 * @author Federico Tomassetti
 */
public class LambdaArgumentTypePlaceholder implements ResolvedType {

    private int pos;
    private SymbolReference<?:ResolvedMethodLikeDeclaration> method;

    public LambdaArgumentTypePlaceholder(int pos) {
        this.pos = pos;
    }

    //@Override
    public bool isArray() {
        return false;
    }

    //@Override
    public bool isReferenceType() {
        return false;
    }

    //@Override
    public string describe() {
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool isTypeVariable() {
        return false;
    }

    public void setMethod(SymbolReference<?:ResolvedMethodLikeDeclaration> method) {
        this.method = method;
    }

    //@Override
    public bool isAssignableBy(ResolvedType other) {
        throw new UnsupportedOperationException();
    }

}
