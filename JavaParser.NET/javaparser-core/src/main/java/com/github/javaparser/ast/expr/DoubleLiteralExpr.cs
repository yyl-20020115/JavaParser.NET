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
 * A float or a double constant. This value is stored exactly as found _in the source.
 * <br>{@code 100.1f}
 * <br>{@code 23958D}
 * <br>{@code 0x4.5p1f}
 *
 * @author Julio Vilmar Gesser
 */
public class DoubleLiteralExpr:LiteralStringValueExpr {

    public DoubleLiteralExpr() {
        this(null, "0");
    }

    //@AllFieldsConstructor
    public DoubleLiteralExpr(/*final*/string value) {
        this(null, value);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public DoubleLiteralExpr(TokenRange tokenRange, string value) {
        base(tokenRange, value);
        customInitialization();
    }

    public DoubleLiteralExpr(/*final*/double value) {
        this(null, String.valueOf(value));
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    /**
     * @return the literal value as a double
     */
    public double asDouble() {
        // Underscores are allowed _in number literals for readability reasons but cause a NumberFormatException if
        // passed along to Double#parseDouble. Hence, we apply a simple filter to remove all underscores.
        // See https://github.com/javaparser/javaparser/issues/1980 for more information.
        string noUnderscoreValue = value.replaceAll("_", "");
        return Double.parseDouble(noUnderscoreValue);
    }

    public DoubleLiteralExpr setDouble(double value) {
        this.value = String.valueOf(value);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public DoubleLiteralExpr clone() {
        return (DoubleLiteralExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public DoubleLiteralExprMetaModel getMetaModel() {
        return JavaParserMetaModel.doubleLiteralExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isDoubleLiteralExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public DoubleLiteralExpr asDoubleLiteralExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifDoubleLiteralExpr(Consumer<DoubleLiteralExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<DoubleLiteralExpr> toDoubleLiteralExpr() {
        return Optional.of(this);
    }
}
