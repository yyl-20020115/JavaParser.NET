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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */
namespace com.github.javaparser.ast.expr;



/**
 * The boolean literals.
 * <br>{@code true}
 * <br>{@code false}
 *
 * @author Julio Vilmar Gesser
 */
public class BooleanLiteralExpr extends LiteralExpr {

    private bool value;

    public BooleanLiteralExpr() {
        this(null, false);
    }

    @AllFieldsConstructor
    public BooleanLiteralExpr(bool value) {
        this(null, value);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    @Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public BooleanLiteralExpr(TokenRange tokenRange, bool value) {
        super(tokenRange);
        setValue(value);
        customInitialization();
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <R, A> R accept(final GenericVisitor<R, A> v, final A arg) {
        return v.visit(this, arg);
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <A> void accept(final VoidVisitor<A> v, final A arg) {
        v.visit(this, arg);
    }

    /**
     * The code generator likes to generate an "is" getter for boolean, so this here is the generated version,
     * but "getValue" does the same and makes more sense.
     */
    @Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isValue() {
        return value;
    }

    public bool getValue() {
        return isValue();
    }

    @Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public BooleanLiteralExpr setValue(final bool value) {
        if (value == this.value) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.VALUE, this.value, value);
        this.value = value;
        return this;
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public BooleanLiteralExpr clone() {
        return (BooleanLiteralExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public BooleanLiteralExprMetaModel getMetaModel() {
        return JavaParserMetaModel.booleanLiteralExprMetaModel;
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isBooleanLiteralExpr() {
        return true;
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public BooleanLiteralExpr asBooleanLiteralExpr() {
        return this;
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifBooleanLiteralExpr(Consumer<BooleanLiteralExpr> action) {
        action.accept(this);
    }

    //@Override
    @Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<BooleanLiteralExpr> toBooleanLiteralExpr() {
        return Optional.of(this);
    }
}
