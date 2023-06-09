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
public /*final*/class VariableDeclarator:Node implements
        NodeWithType<VariableDeclarator> {

    private VariableDeclaratorId id;

    private Expression init;

    public VariableDeclarator() {
    }

    public VariableDeclarator(VariableDeclaratorId id) {
        setId(id);
    }

    public VariableDeclarator(string variableName) {
        setId(new VariableDeclaratorId(variableName));
    }

    /**
     * Defines the declaration of a variable.
     * 
     * @param id The identifier for this variable. IE. The variables name.
     * @param init What this variable should be initialized to.
     *            An {@link com.github.javaparser.ast.expr.AssignExpr} is unnecessary as the <code>=</code> operator is
     *            already added.
     */
    public VariableDeclarator(VariableDeclaratorId id, Expression init) {
        setId(id);
        setInit(init);
    }

    public VariableDeclarator(string variableName, Expression init) {
        setId(new VariableDeclaratorId(variableName));
        setInit(init);
    }

    public VariableDeclarator(Range range, VariableDeclaratorId id, Expression init) {
        base(range);
        setId(id);
        setInit(init);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public VariableDeclaratorId getId() {
        return id;
    }

    public Expression getInit() {
        return init;
    }

    public VariableDeclarator setId(VariableDeclaratorId id) {
        this.id = id;
        setAsParentNodeOf(this.id);
        return this;
    }

    public VariableDeclarator setInit(Expression init) {
        this.init = init;
        setAsParentNodeOf(this.init);
        return this;
    }

    /**
     * Will create a {@link NameExpr} with the init param
     */
    public VariableDeclarator setInit(string init) {
        this.init = new NameExpr(init);
        setAsParentNodeOf(this.init);
        return this;
    }


    //@Override
    public Type getType() {
        NodeWithElementType<?> elementType = getParentNodeOfType(NodeWithElementType.class);

        return wrapInArrayTypes(elementType.getElementType(),
                elementType.getArrayBracketPairsAfterElementType(),
                getId().getArrayBracketPairsAfterId());
    }

    //@Override
    public VariableDeclarator setType(Type type) {
        Pair<Type, List<ArrayBracketPair>> unwrapped = ArrayType.unwrapArrayTypes(type);
        NodeWithElementType<?> nodeWithElementType = getParentNodeOfType(NodeWithElementType.class);
        if (nodeWithElementType == null) {
            throw new IllegalStateException("Cannot set type without a parent");
        }
        nodeWithElementType.setElementType(unwrapped.a);
        nodeWithElementType.setArrayBracketPairsAfterElementType(null);
        getId().setArrayBracketPairsAfterId(unwrapped.b);
        return this;
    }
}
