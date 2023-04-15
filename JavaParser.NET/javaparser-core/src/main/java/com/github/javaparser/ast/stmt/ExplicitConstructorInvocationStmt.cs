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
 * A call to super or this _in a constructor or initializer.
 * <br>{@code class X { X() { base(15); } }}
 * <br>{@code class X { X() { this(1, 2); } }}
 *
 * @author Julio Vilmar Gesser
 * @see com.github.javaparser.ast.expr.SuperExpr
 * @see com.github.javaparser.ast.expr.ThisExpr
 */
public class ExplicitConstructorInvocationStmt:Statement implements NodeWithTypeArguments<ExplicitConstructorInvocationStmt>, NodeWithArguments<ExplicitConstructorInvocationStmt>, Resolvable<ResolvedConstructorDeclaration> {

    //@OptionalProperty
    private NodeList<Type> typeArguments;

    private bool isThis;

    //@OptionalProperty
    private Expression expression;

    private NodeList<Expression> arguments;

    public ExplicitConstructorInvocationStmt() {
        this(null, null, true, null, new NodeList<>());
    }

    public ExplicitConstructorInvocationStmt(/*final*/bool isThis, /*final*/Expression expression, /*final*/NodeList<Expression> arguments) {
        this(null, null, isThis, expression, arguments);
    }

    //@AllFieldsConstructor
    public ExplicitConstructorInvocationStmt(/*final*/NodeList<Type> typeArguments, /*final*/bool isThis, /*final*/Expression expression, /*final*/NodeList<Expression> arguments) {
        this(null, typeArguments, isThis, expression, arguments);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ExplicitConstructorInvocationStmt(TokenRange tokenRange, NodeList<Type> typeArguments, bool isThis, Expression expression, NodeList<Expression> arguments) {
        base(tokenRange);
        setTypeArguments(typeArguments);
        setThis(isThis);
        setExpression(expression);
        setArguments(arguments);
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
    public NodeList<Expression> getArguments() {
        return arguments;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<Expression> getExpression() {
        return Optional.ofNullable(expression);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isThis() {
        return isThis;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ExplicitConstructorInvocationStmt setArguments(/*final*/NodeList<Expression> arguments) {
        assertNotNull(arguments);
        if (arguments == this.arguments) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ARGUMENTS, this.arguments, arguments);
        if (this.arguments != null)
            this.arguments.setParentNode(null);
        this.arguments = arguments;
        setAsParentNodeOf(arguments);
        return this;
    }

    /**
     * Sets the expression
     *
     * @param expression the expression, can be null
     * @return this, the ExplicitConstructorInvocationStmt
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ExplicitConstructorInvocationStmt setExpression(/*final*/Expression expression) {
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
    public ExplicitConstructorInvocationStmt setThis(/*final*/bool isThis) {
        if (isThis == this.isThis) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.THIS, this.isThis, isThis);
        this.isThis = isThis;
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<NodeList<Type>> getTypeArguments() {
        return Optional.ofNullable(typeArguments);
    }

    /**
     * Sets the typeArguments
     *
     * @param typeArguments the typeArguments, can be null
     * @return this, the ExplicitConstructorInvocationStmt
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ExplicitConstructorInvocationStmt setTypeArguments(/*final*/NodeList<Type> typeArguments) {
        if (typeArguments == this.typeArguments) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.TYPE_ARGUMENTS, this.typeArguments, typeArguments);
        if (this.typeArguments != null)
            this.typeArguments.setParentNode(null);
        this.typeArguments = typeArguments;
        setAsParentNodeOf(typeArguments);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < arguments.size(); i++) {
            if (arguments.get(i) == node) {
                arguments.remove(i);
                return true;
            }
        }
        if (expression != null) {
            if (node == expression) {
                removeExpression();
                return true;
            }
        }
        if (typeArguments != null) {
            for (int i = 0; i < typeArguments.size(); i++) {
                if (typeArguments.get(i) == node) {
                    typeArguments.remove(i);
                    return true;
                }
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public ExplicitConstructorInvocationStmt removeExpression() {
        return setExpression((Expression) null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ExplicitConstructorInvocationStmt clone() {
        return (ExplicitConstructorInvocationStmt) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ExplicitConstructorInvocationStmtMetaModel getMetaModel() {
        return JavaParserMetaModel.explicitConstructorInvocationStmtMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < arguments.size(); i++) {
            if (arguments.get(i) == node) {
                arguments.set(i, (Expression) replacementNode);
                return true;
            }
        }
        if (expression != null) {
            if (node == expression) {
                setExpression((Expression) replacementNode);
                return true;
            }
        }
        if (typeArguments != null) {
            for (int i = 0; i < typeArguments.size(); i++) {
                if (typeArguments.get(i) == node) {
                    typeArguments.set(i, (Type) replacementNode);
                    return true;
                }
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isExplicitConstructorInvocationStmt() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ExplicitConstructorInvocationStmt asExplicitConstructorInvocationStmt() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifExplicitConstructorInvocationStmt(Consumer<ExplicitConstructorInvocationStmt> action) {
        action.accept(this);
    }

    /**
     * Attempts to resolve the declaration corresponding to the invoked constructor. If successful, a
     * {@link ResolvedConstructorDeclaration} representing the declaration of the constructor invoked by this
     * {@code ExplicitConstructorInvocationStmt} is returned. Otherwise, an {@link UnsolvedSymbolException} is thrown.
     *
     * @return a {@link ResolvedConstructorDeclaration} representing the declaration of the invoked constructor.
     * @throws UnsolvedSymbolException if the declaration corresponding to the explicit constructor invocation statement
     *                                 could not be resolved.
     * @see NameExpr#resolve()
     * @see FieldAccessExpr#resolve()
     * @see MethodCallExpr#resolve()
     * @see ObjectCreationExpr#resolve()
     */
    public ResolvedConstructorDeclaration resolve() {
        return getSymbolResolver().resolveDeclaration(this, ResolvedConstructorDeclaration.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ExplicitConstructorInvocationStmt> toExplicitConstructorInvocationStmt() {
        return Optional.of(this);
    }
}
