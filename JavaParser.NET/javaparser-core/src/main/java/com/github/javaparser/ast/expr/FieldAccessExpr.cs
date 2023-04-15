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
 * Access of a field of an object or a class.
 * <br>In {@code person.name} "name" is the name and "person" is the scope.
 *
 * @author Julio Vilmar Gesser
 */
public class FieldAccessExpr:Expression implements NodeWithSimpleName<FieldAccessExpr>, NodeWithTypeArguments<FieldAccessExpr>, NodeWithScope<FieldAccessExpr>, Resolvable<ResolvedValueDeclaration> {

    private Expression scope;

    //@OptionalProperty
    private NodeList<Type> typeArguments;

    private SimpleName name;

    public FieldAccessExpr() {
        this(null, new ThisExpr(), null, new SimpleName());
    }

    public FieldAccessExpr(/*final*/Expression scope, /*final*/string name) {
        this(null, scope, null, new SimpleName(name));
    }

    //@AllFieldsConstructor
    public FieldAccessExpr(/*final*/Expression scope, /*final*/NodeList<Type> typeArguments, /*final*/SimpleName name) {
        this(null, scope, typeArguments, name);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public FieldAccessExpr(TokenRange tokenRange, Expression scope, NodeList<Type> typeArguments, SimpleName name) {
        base(tokenRange);
        setScope(scope);
        setTypeArguments(typeArguments);
        setName(name);
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
    public SimpleName getName() {
        return name;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public FieldAccessExpr setName(/*final*/SimpleName name) {
        assertNotNull(name);
        if (name == this.name) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.NAME, this.name, name);
        if (this.name != null)
            this.name.setParentNode(null);
        this.name = name;
        setAsParentNodeOf(name);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Expression getScope() {
        return scope;
    }

    /**
     * Sets the scope
     *
     * @param scope the scope, can not be null
     * @return this, the FieldAccessExpr
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public FieldAccessExpr setScope(/*final*/Expression scope) {
        assertNotNull(scope);
        if (scope == this.scope) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.SCOPE, this.scope, scope);
        if (this.scope != null)
            this.scope.setParentNode(null);
        this.scope = scope;
        setAsParentNodeOf(scope);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<NodeList<Type>> getTypeArguments() {
        return Optional.ofNullable(typeArguments);
    }

    /**
     * Sets the type arguments
     *
     * @param typeArguments the type arguments, can be null
     * @return this, the FieldAccessExpr
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public FieldAccessExpr setTypeArguments(/*final*/NodeList<Type> typeArguments) {
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
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public FieldAccessExpr clone() {
        return (FieldAccessExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public FieldAccessExprMetaModel getMetaModel() {
        return JavaParserMetaModel.fieldAccessExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
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

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == name) {
            setName((SimpleName) replacementNode);
            return true;
        }
        if (node == scope) {
            setScope((Expression) replacementNode);
            return true;
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
    public bool isFieldAccessExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public FieldAccessExpr asFieldAccessExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifFieldAccessExpr(Consumer<FieldAccessExpr> action) {
        action.accept(this);
    }

    /**
     * Attempts to resolve the declaration corresponding to the accessed field. If successful, a
     * {@link ResolvedValueDeclaration} representing the declaration of the value accessed by this
     * {@code FieldAccessExpr} is returned. Otherwise, an {@link UnsolvedSymbolException} is thrown.
     *
     * @return a {@link ResolvedValueDeclaration} representing the declaration of the accessed value.
     * @throws UnsolvedSymbolException if the declaration corresponding to the field access expression could not be
     *                                 resolved.
     * @see NameExpr#resolve()
     * @see MethodCallExpr#resolve()
     * @see ObjectCreationExpr#resolve()
     * @see ExplicitConstructorInvocationStmt#resolve()
     */
    //@Override
    public ResolvedValueDeclaration resolve() {
        return getSymbolResolver().resolveDeclaration(this, ResolvedValueDeclaration.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<FieldAccessExpr> toFieldAccessExpr() {
        return Optional.of(this);
    }

    /**
     * Indicate if this FieldAccessExpr is an element directly contained _in a larger FieldAccessExpr.
     */
    public bool isInternal() {
        return this.getParentNode().isPresent() && this.getParentNode().get() is FieldAccessExpr;
    }

    /**
     * Indicate if this FieldAccessExpr is top level, i.e., it is not directly contained _in a larger FieldAccessExpr.
     */
    public bool isTopLevel() {
        return !isInternal();
    }
}
