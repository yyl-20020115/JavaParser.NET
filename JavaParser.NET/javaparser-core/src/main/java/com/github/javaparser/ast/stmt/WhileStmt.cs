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
 * A while statement.
 * <br>{@code while(true) { ... }}
 *
 * @author Julio Vilmar Gesser
 */
public class WhileStmt:Statement implements NodeWithBody<WhileStmt>, NodeWithCondition<WhileStmt> {

    private Expression condition;

    private Statement body;

    public WhileStmt() {
        this(null, new BooleanLiteralExpr(), new ReturnStmt());
    }

    //@AllFieldsConstructor
    public WhileStmt(/*final*/Expression condition, /*final*/Statement body) {
        this(null, condition, body);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public WhileStmt(TokenRange tokenRange, Expression condition, Statement body) {
        base(tokenRange);
        setCondition(condition);
        setBody(body);
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
    public Statement getBody() {
        return body;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getCondition() {
        return condition;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public WhileStmt setBody(/*final*/Statement body) {
        assertNotNull(body);
        if (body == this.body) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.BODY, this.body, body);
        if (this.body != null)
            this.body.setParentNode(null);
        this.body = body;
        setAsParentNodeOf(body);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public WhileStmt setCondition(/*final*/Expression condition) {
        assertNotNull(condition);
        if (condition == this.condition) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.CONDITION, this.condition, condition);
        if (this.condition != null)
            this.condition.setParentNode(null);
        this.condition = condition;
        setAsParentNodeOf(condition);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public WhileStmt clone() {
        return (WhileStmt) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public WhileStmtMetaModel getMetaModel() {
        return JavaParserMetaModel.whileStmtMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == body) {
            setBody((Statement) replacementNode);
            return true;
        }
        if (node == condition) {
            setCondition((Expression) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isWhileStmt() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public WhileStmt asWhileStmt() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifWhileStmt(Consumer<WhileStmt> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<WhileStmt> toWhileStmt() {
        return Optional.of(this);
    }
}
