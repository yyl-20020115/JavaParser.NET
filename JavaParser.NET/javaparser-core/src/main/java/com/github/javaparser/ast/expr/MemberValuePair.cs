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
 * A value for a member of an annotation.
 * In {@code @Counters(a=15)} a=15 is a MemberValuePair. Its name is a, and its value is 15.
 *
 * @author Julio Vilmar Gesser
 */
public class MemberValuePair:Node implements NodeWithSimpleName<MemberValuePair> {

    private SimpleName name;

    private Expression value;

    public MemberValuePair() {
        this(null, new SimpleName(), new StringLiteralExpr());
    }

    public MemberValuePair(/*final*/string name, /*final*/Expression value) {
        this(null, new SimpleName(name), value);
    }

    //@AllFieldsConstructor
    public MemberValuePair(/*final*/SimpleName name, /*final*/Expression value) {
        this(null, name, value);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public MemberValuePair(TokenRange tokenRange, SimpleName name, Expression value) {
        super(tokenRange);
        setName(name);
        setValue(value);
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
    public SimpleName getName() {
        return name;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getValue() {
        return value;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public MemberValuePair setName(/*final*/SimpleName name) {
        assertNotNull(name);
        if (name == this.name) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.NAME, this.name, name);
        if (this.name != null)
            this.name.setParentNode(null);
        this.name = name;
        setAsParentNodeOf(name);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public MemberValuePair setValue(/*final*/Expression value) {
        assertNotNull(value);
        if (value == this.value) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.VALUE, this.value, value);
        if (this.value != null)
            this.value.setParentNode(null);
        this.value = value;
        setAsParentNodeOf(value);
        return this;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public MemberValuePair clone() {
        return (MemberValuePair) accept(new CloneVisitor(), null);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public MemberValuePairMetaModel getMetaModel() {
        return JavaParserMetaModel.memberValuePairMetaModel;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public boolean replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == name) {
            setName((SimpleName) replacementNode);
            return true;
        }
        if (node == value) {
            setValue((Expression) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }
}
