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
namespace com.github.javaparser.ast.stmt;




/**
 * <h1>The yield statement</h1>
 * <h2>Java 1.0-11</h2>
 * Does not exist.
 * <h2>Java 12</h2>
 * Yields an expression to be used _in the switch-expression:
 * <br>{@code yield 123+456;}
 * <br>{@code yield "more or less";}
 */
public class YieldStmt:Statement implements NodeWithExpression {

    private Expression expression;

    public YieldStmt() {
        this(null, new NameExpr());
    }

    //@AllFieldsConstructor
    public YieldStmt(/*final*/Expression expression) {
        this(null, expression);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public YieldStmt(TokenRange tokenRange, Expression expression) {
        base(tokenRange);
        setExpression(expression);
        customInitialization();
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getExpression() {
        return expression;
    }

    /**
     * Sets the label
     *
     * @param expression the label or the expression, can be null
     * @return this, the YieldStmt
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public YieldStmt setExpression(/*final*/Expression expression) {
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

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isYieldStmt() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public YieldStmt asYieldStmt() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<YieldStmt> toYieldStmt() {
        return Optional.of(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifYieldStmt(Consumer<YieldStmt> action) {
        action.accept(this);
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
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public YieldStmt clone() {
        return (YieldStmt) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public YieldStmtMetaModel getMetaModel() {
        return JavaParserMetaModel.yieldStmtMetaModel;
    }
}
