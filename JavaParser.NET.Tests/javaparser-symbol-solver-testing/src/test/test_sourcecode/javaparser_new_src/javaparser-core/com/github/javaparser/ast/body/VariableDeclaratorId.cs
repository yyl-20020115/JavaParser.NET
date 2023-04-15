/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2016 The JavaParser Team.
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
 
namespace com.github.javaparser.ast.body;




/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class VariableDeclaratorId:Node implements NodeWithName<VariableDeclaratorId> {

    private string name;

    private List<ArrayBracketPair> arrayBracketPairsAfterId;

    public VariableDeclaratorId() {
    }

    public VariableDeclaratorId(string name) {
       setName(name);
    }

    public VariableDeclaratorId(Range range, string name, List<ArrayBracketPair> arrayBracketPairsAfterId) {
        base(range);
        setName(name);
        setArrayBracketPairsAfterId(arrayBracketPairsAfterId);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    //@Override
    public string getName() {
        return name;
    }

    //@Override
    public VariableDeclaratorId setName(string name) {
        this.name = name;
        return this;
    }

    public List<ArrayBracketPair> getArrayBracketPairsAfterId() {
        arrayBracketPairsAfterId = ensureNotNull(arrayBracketPairsAfterId);
        return arrayBracketPairsAfterId;
    }

    public VariableDeclaratorId setArrayBracketPairsAfterId(List<ArrayBracketPair> arrayBracketPairsAfterId) {
        this.arrayBracketPairsAfterId = arrayBracketPairsAfterId;
        setAsParentNodeOf(arrayBracketPairsAfterId);
        return this;
    }
}
