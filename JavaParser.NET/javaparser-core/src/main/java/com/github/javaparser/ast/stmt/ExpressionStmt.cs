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
 * Used to wrap an expression so that it can take the place of a statement.
 *
 * @author Julio Vilmar Gesser
 */
public class ExpressionStmt:Statement implements NodeWithExpression<ExpressionStmt> {

    private Expression expression;

    public ExpressionStmt() {
        this(null, new BooleanLiteralExpr());
    }

    //@AllFieldsConstructor
    public ExpressionStmt(/*final*/Expression expression) {
        this(null, expression);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ExpressionStmt(TokenRange tokenRange, Expression expression) {
        super(tokenRange);
        setExpression(expression);
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
    public Expression getExpression() {
        return expression;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ExpressionStmt setExpression(/*final*/Expression expression) {
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

    @Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ExpressionStmt clone() {
        return (ExpressionStmt) accept(new CloneVisitor(), null);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ExpressionStmtMetaModel getMetaModel() {
        return JavaParserMetaModel.expressionStmtMetaModel;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public boolean replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == expression) {
            setExpression((Expression) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public boolean isExpressionStmt() {
        return true;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ExpressionStmt asExpressionStmt() {
        return this;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifExpressionStmt(Consumer<ExpressionStmt> action) {
        action.accept(this);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ExpressionStmt> toExpressionStmt() {
        return Optional.of(this);
    }
}
