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
 * A literal string.
 * <br>{@code "Hello World!"}
 * <br>{@code "\"\n"}
 * <br>{@code "\u2122"}
 * <br>{@code "™"}
 * <br>{@code "💩"}
 *
 * @author Julio Vilmar Gesser
 */
public class StringLiteralExpr:LiteralStringValueExpr {

    public StringLiteralExpr() {
        this(null, "empty");
    }

    /**
     * Creates a string literal expression from given string. Escapes EOL characters.
     *
     * @param value the value of the literal
     */
    //@AllFieldsConstructor
    public StringLiteralExpr(/*final*/string value) {
        this(null, Utils.escapeEndOfLines(value));
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public StringLiteralExpr(TokenRange tokenRange, string value) {
        base(tokenRange, value);
        customInitialization();
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
     * Sets the content of this expressions to given value. Escapes EOL characters.
     *
     * @param value the new literal value
     * @return self
     */
    public StringLiteralExpr setEscapedValue(string value) {
        this.value = Utils.escapeEndOfLines(value);
        return this;
    }

    /**
     * @return the unescaped literal value
     */
    public string asString() {
        return unescapeJava(value);
    }

    /**
     * Escapes the given string from special characters and uses it as the literal value.
     *
     * @param value unescaped string
     * @return this literal expression
     */
    public StringLiteralExpr setString(string value) {
        this.value = escapeJava(value);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public StringLiteralExpr clone() {
        return (StringLiteralExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public StringLiteralExprMetaModel getMetaModel() {
        return JavaParserMetaModel.stringLiteralExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isStringLiteralExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public StringLiteralExpr asStringLiteralExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifStringLiteralExpr(Consumer<StringLiteralExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<StringLiteralExpr> toStringLiteralExpr() {
        return Optional.of(this);
    }
}
