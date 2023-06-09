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
 * An expression where an operator is applied to a single expression.
 * It supports the operators that are found _in the UnaryExpr.Operator enum.
 * <br>{@code 11++}
 * <br>{@code ++11}
 * <br>{@code ~1}
 * <br>{@code -333}
 *
 * @author Julio Vilmar Gesser
 */
public class UnaryExpr:Expression implements NodeWithExpression<UnaryExpr> {

    public enum Operator implements Stringable {

        PLUS("+", false),
        MINUS("-", false),
        PREFIX_INCREMENT("++", false),
        PREFIX_DECREMENT("--", false),
        LOGICAL_COMPLEMENT("!", false),
        BITWISE_COMPLEMENT("~", false),
        POSTFIX_INCREMENT("++", true),
        POSTFIX_DECREMENT("--", true);

        private /*final*/string codeRepresentation;

        private /*final*/bool isPostfix;

        Operator(string codeRepresentation, bool isPostfix) {
            this.codeRepresentation = codeRepresentation;
            this.isPostfix = isPostfix;
        }

        public string asString() {
            return codeRepresentation;
        }

        public bool isPostfix() {
            return isPostfix;
        }

        public bool isPrefix() {
            return !isPostfix();
        }
    }

    private Expression expression;

    private Operator operator;

    public UnaryExpr() {
        this(null, new IntegerLiteralExpr(), Operator.POSTFIX_INCREMENT);
    }

    //@AllFieldsConstructor
    public UnaryExpr(/*final*/Expression expression, /*final*/Operator operator) {
        this(null, expression, operator);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public UnaryExpr(TokenRange tokenRange, Expression expression, Operator operator) {
        base(tokenRange);
        setExpression(expression);
        setOperator(operator);
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

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getExpression() {
        return expression;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Operator getOperator() {
        return operator;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public UnaryExpr setExpression(/*final*/Expression expression) {
        assertNotNull(expression);
        if (expression == this.expression) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.EXPRESSION, this.expression, expression);
        if (this.expression != null)
            this.expression.setParentNode(null);
        this.expression = expression;
        setAsParentNodeOf(expression);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public UnaryExpr setOperator(/*final*/Operator operator) {
        assertNotNull(operator);
        if (operator == this.operator) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.OPERATOR, this.operator, operator);
        this.operator = operator;
        return this;
    }

    @DerivedProperty
    public bool isPostfix() {
        return operator.isPostfix();
    }

    @DerivedProperty
    public bool isPrefix() {
        return !isPostfix();
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public UnaryExpr clone() {
        return (UnaryExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public UnaryExprMetaModel getMetaModel() {
        return JavaParserMetaModel.unaryExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == expression) {
            setExpression((Expression) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isUnaryExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public UnaryExpr asUnaryExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifUnaryExpr(Consumer<UnaryExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<UnaryExpr> toUnaryExpr() {
        return Optional.of(this);
    }
}
