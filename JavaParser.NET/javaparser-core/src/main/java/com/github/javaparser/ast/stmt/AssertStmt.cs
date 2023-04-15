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
 * A usage of the keyword "assert"
 * <br>In {@code assert dead : "Wasn't expecting to be dead here";} the check is "dead" and the message is the string.
 * @author Julio Vilmar Gesser
 */
public class AssertStmt:Statement {

    private Expression check;

    //@OptionalProperty
    private Expression message;

    public AssertStmt() {
        this(null, new BooleanLiteralExpr(), null);
    }

    public AssertStmt(/*final*/Expression check) {
        this(null, check, null);
    }

    //@AllFieldsConstructor
    public AssertStmt(/*final*/Expression check, /*final*/Expression message) {
        this(null, check, message);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public AssertStmt(TokenRange tokenRange, Expression check, Expression message) {
        base(tokenRange);
        setCheck(check);
        setMessage(message);
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
    public Expression getCheck() {
        return check;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<Expression> getMessage() {
        return Optional.ofNullable(message);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AssertStmt setCheck(/*final*/Expression check) {
        assertNotNull(check);
        if (check == this.check) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.CHECK, this.check, check);
        if (this.check != null)
            this.check.setParentNode(null);
        this.check = check;
        setAsParentNodeOf(check);
        return this;
    }

    /**
     * Sets the message
     *
     * @param message the message, can be null
     * @return this, the AssertStmt
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AssertStmt setMessage(/*final*/Expression message) {
        if (message == this.message) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.MESSAGE, this.message, message);
        if (this.message != null)
            this.message.setParentNode(null);
        this.message = message;
        setAsParentNodeOf(message);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        if (message != null) {
            if (node == message) {
                removeMessage();
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public AssertStmt removeMessage() {
        return setMessage((Expression) null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public AssertStmt clone() {
        return (AssertStmt) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public AssertStmtMetaModel getMetaModel() {
        return JavaParserMetaModel.assertStmtMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == check) {
            setCheck((Expression) replacementNode);
            return true;
        }
        if (message != null) {
            if (node == message) {
                setMessage((Expression) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isAssertStmt() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public AssertStmt asAssertStmt() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifAssertStmt(Consumer<AssertStmt> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<AssertStmt> toAssertStmt() {
        return Optional.of(this);
    }
}
