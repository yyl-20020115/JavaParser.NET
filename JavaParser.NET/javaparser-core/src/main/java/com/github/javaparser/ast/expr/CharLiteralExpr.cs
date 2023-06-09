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
 * A literal character.
 * <br>{@code 'a'}
 * <br>{@code '\t'}
 * <br>{@code 'Ω'}
 * <br>{@code '\177'}
 * <br>{@code '💩'}
 *
 * @author Julio Vilmar Gesser
 */
public class CharLiteralExpr:LiteralStringValueExpr {

    public CharLiteralExpr() {
        this(null, "?");
    }

    //@AllFieldsConstructor
    public CharLiteralExpr(string value) {
        this(null, value);
    }

    /**
     * Constructs a CharLiteralExpr with given escaped character.
     *
     * @param value a char
     */
    public CharLiteralExpr(char value) {
        this(null, StringEscapeUtils.escapeJava(String.valueOf(value)));
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public CharLiteralExpr(TokenRange tokenRange, string value) {
        base(tokenRange, value);
        customInitialization();
    }

    /**
     * Utility method that creates a new StringLiteralExpr. Escapes EOL characters.
     */
    public static CharLiteralExpr escape(string string) {
        return new CharLiteralExpr(Utils.escapeEndOfLines(string));
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
     * @return the unescaped value character of this literal
     */
    public char asChar() {
        return StringEscapeUtils.unescapeJava(value).charAt(0);
    }

    /**
     * Sets the given char as the literal value
     *
     * @param value a char
     * @return this expression
     */
    public CharLiteralExpr setChar(char value) {
        this.value = String.valueOf(value);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public CharLiteralExpr clone() {
        return (CharLiteralExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public CharLiteralExprMetaModel getMetaModel() {
        return JavaParserMetaModel.charLiteralExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isCharLiteralExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public CharLiteralExpr asCharLiteralExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifCharLiteralExpr(Consumer<CharLiteralExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<CharLiteralExpr> toCharLiteralExpr() {
        return Optional.of(this);
    }
}
