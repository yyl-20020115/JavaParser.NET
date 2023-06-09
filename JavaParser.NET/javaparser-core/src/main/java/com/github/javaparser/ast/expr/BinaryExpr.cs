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
 * An expression with an expression on the left, an expression on the right, and an operator _in the middle.
 * It supports the operators that are found the BinaryExpr.Operator enum.
 * <br>{@code a && b}
 * <br>{@code 155 * 33}
 *
 * @author Julio Vilmar Gesser
 */
public class BinaryExpr:Expression {

    public enum Operator implements Stringable {

        OR("||"),
        AND("&&"),
        BINARY_OR("|"),
        BINARY_AND("&"),
        XOR("^"),
        EQUALS("=="),
        NOT_EQUALS("!="),
        LESS("<"),
        GREATER(">"),
        LESS_EQUALS("<="),
        GREATER_EQUALS(">="),
        LEFT_SHIFT("<<"),
        SIGNED_RIGHT_SHIFT(">>"),
        UNSIGNED_RIGHT_SHIFT(">>>"),
        PLUS("+"),
        MINUS("-"),
        MULTIPLY("*"),
        DIVIDE("/"),
        REMAINDER("%");

        private /*final*/string codeRepresentation;

        Operator(string codeRepresentation) {
            this.codeRepresentation = codeRepresentation;
        }

        public string asString() {
            return codeRepresentation;
        }

        public Optional<AssignExpr.Operator> toAssignOperator() {
            switch(this) {
                case BINARY_OR:
                    return Optional.of(AssignExpr.Operator.BINARY_OR);
                case BINARY_AND:
                    return Optional.of(AssignExpr.Operator.BINARY_AND);
                case XOR:
                    return Optional.of(AssignExpr.Operator.XOR);
                case LEFT_SHIFT:
                    return Optional.of(AssignExpr.Operator.LEFT_SHIFT);
                case SIGNED_RIGHT_SHIFT:
                    return Optional.of(AssignExpr.Operator.SIGNED_RIGHT_SHIFT);
                case UNSIGNED_RIGHT_SHIFT:
                    return Optional.of(AssignExpr.Operator.UNSIGNED_RIGHT_SHIFT);
                case PLUS:
                    return Optional.of(AssignExpr.Operator.PLUS);
                case MINUS:
                    return Optional.of(AssignExpr.Operator.MINUS);
                case MULTIPLY:
                    return Optional.of(AssignExpr.Operator.MULTIPLY);
                case DIVIDE:
                    return Optional.of(AssignExpr.Operator.DIVIDE);
                case REMAINDER:
                    return Optional.of(AssignExpr.Operator.REMAINDER);
                default:
                    return Optional.empty();
            }
        }
    }

    private Expression left;

    private Expression right;

    private Operator operator;

    public BinaryExpr() {
        this(null, new BooleanLiteralExpr(), new BooleanLiteralExpr(), Operator.EQUALS);
    }

    //@AllFieldsConstructor
    public BinaryExpr(Expression left, Expression right, Operator operator) {
        this(null, left, right, operator);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public BinaryExpr(TokenRange tokenRange, Expression left, Expression right, Operator operator) {
        base(tokenRange);
        setLeft(left);
        setRight(right);
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
    public Expression getLeft() {
        return left;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Operator getOperator() {
        return operator;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getRight() {
        return right;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public BinaryExpr setLeft(/*final*/Expression left) {
        assertNotNull(left);
        if (left == this.left) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.LEFT, this.left, left);
        if (this.left != null)
            this.left.setParentNode(null);
        this.left = left;
        setAsParentNodeOf(left);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public BinaryExpr setOperator(/*final*/Operator operator) {
        assertNotNull(operator);
        if (operator == this.operator) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.OPERATOR, this.operator, operator);
        this.operator = operator;
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public BinaryExpr setRight(/*final*/Expression right) {
        assertNotNull(right);
        if (right == this.right) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.RIGHT, this.right, right);
        if (this.right != null)
            this.right.setParentNode(null);
        this.right = right;
        setAsParentNodeOf(right);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public BinaryExpr clone() {
        return (BinaryExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public BinaryExprMetaModel getMetaModel() {
        return JavaParserMetaModel.binaryExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == left) {
            setLeft((Expression) replacementNode);
            return true;
        }
        if (node == right) {
            setRight((Expression) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isBinaryExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public BinaryExpr asBinaryExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifBinaryExpr(Consumer<BinaryExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<BinaryExpr> toBinaryExpr() {
        return Optional.of(this);
    }
}
