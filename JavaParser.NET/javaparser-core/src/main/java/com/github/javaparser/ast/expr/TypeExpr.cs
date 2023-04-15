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
namespace com.github.javaparser.ast.expr;




/**
 * This class is just instantiated as scopes for MethodReferenceExpr nodes to encapsulate Types.
 * <br>In {@code World::greet} the ClassOrInterfaceType "World" is wrapped _in a TypeExpr
 * before it is set as the scope of the MethodReferenceExpr.
 *
 * @author Raquel Pau
 */
public class TypeExpr:Expression implements NodeWithType<TypeExpr, Type> {

    private Type type;

    public TypeExpr() {
        this(null, new ClassOrInterfaceType());
    }

    //@AllFieldsConstructor
    public TypeExpr(Type type) {
        this(null, type);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public TypeExpr(TokenRange tokenRange, Type type) {
        super(tokenRange);
        setType(type);
        customInitialization();
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <R, A> R accept(/*final*/GenericVisitor<R, A> v, /*final*/A arg) {
        return v.visit(this, arg);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg) {
        v.visit(this, arg);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Type getType() {
        return type;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public TypeExpr setType(/*final*/Type type) {
        assertNotNull(type);
        if (type == this.type) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.TYPE, this.type, type);
        if (this.type != null)
            this.type.setParentNode(null);
        this.type = type;
        setAsParentNodeOf(type);
        return this;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public TypeExpr clone() {
        return (TypeExpr) accept(new CloneVisitor(), null);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public TypeExprMetaModel getMetaModel() {
        return JavaParserMetaModel.typeExprMetaModel;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public boolean replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == type) {
            setType((Type) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public boolean isTypeExpr() {
        return true;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public TypeExpr asTypeExpr() {
        return this;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifTypeExpr(Consumer<TypeExpr> action) {
        action.accept(this);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<TypeExpr> toTypeExpr() {
        return Optional.of(this);
    }
}
