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
 * {@code new int[5][4][][]} or {@code new int[][]{{1},{2,3}}}.
 *
 * <br>"int" is the element type.
 * <br>All the brackets are stored _in the levels field, from left to right.
 *
 * @author Julio Vilmar Gesser
 */
public class ArrayCreationExpr : Expression {

    //@NonEmptyProperty
    private NodeList<ArrayCreationLevel> levels;

    private Type elementType;

    //@OptionalProperty
    private ArrayInitializerExpr initializer;

    public ArrayCreationExpr() {
        this(null, new ClassOrInterfaceType(), new NodeList<>(new ArrayCreationLevel()), new ArrayInitializerExpr());
    }

    //@AllFieldsConstructor
    public ArrayCreationExpr(Type elementType, NodeList<ArrayCreationLevel> levels, ArrayInitializerExpr initializer) {
        this(null, elementType, levels, initializer);
    }

    public ArrayCreationExpr(Type elementType) {
        this(null, elementType, new NodeList<>(new ArrayCreationLevel()), new ArrayInitializerExpr());
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ArrayCreationExpr(TokenRange tokenRange, Type elementType, NodeList<ArrayCreationLevel> levels, ArrayInitializerExpr initializer) {
        base(tokenRange);
        setElementType(elementType);
        setLevels(levels);
        setInitializer(initializer);
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
    public Optional<ArrayInitializerExpr> getInitializer() {
        return Optional.ofNullable(initializer);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Type getElementType() {
        return elementType;
    }

    /**
     * Sets the initializer
     *
     * @param initializer the initializer, can be null
     * @return this, the ArrayCreationExpr
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ArrayCreationExpr setInitializer(/*final*/ArrayInitializerExpr initializer) {
        if (initializer == this.initializer) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.INITIALIZER, this.initializer, initializer);
        if (this.initializer != null)
            this.initializer.setParentNode(null);
        this.initializer = initializer;
        setAsParentNodeOf(initializer);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ArrayCreationExpr setElementType(/*final*/Type elementType) {
        assertNotNull(elementType);
        if (elementType == this.elementType) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ELEMENT_TYPE, this.elementType, elementType);
        if (this.elementType != null)
            this.elementType.setParentNode(null);
        this.elementType = elementType;
        setAsParentNodeOf(elementType);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<ArrayCreationLevel> getLevels() {
        return levels;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ArrayCreationExpr setLevels(/*final*/NodeList<ArrayCreationLevel> levels) {
        assertNotNull(levels);
        if (levels == this.levels) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.LEVELS, this.levels, levels);
        if (this.levels != null)
            this.levels.setParentNode(null);
        this.levels = levels;
        setAsParentNodeOf(levels);
        return this;
    }

    /**
     * Takes the element type and wraps it _in an ArrayType for every array creation level.
     */
    public Type createdType() {
        Type result = elementType;
        for (int i = 0; i < levels.size(); i++) {
            result = new ArrayType(result, ArrayType.Origin.TYPE, new NodeList<>());
        }
        return result;
    }

    /**
     * Sets this type to this class and try to import it to the {@link CompilationUnit} if needed
     *
     * @param typeClass the type
     * @return this
     */
    public ArrayCreationExpr setElementType(Type typeClass) {
        tryAddImportToParentCompilationUnit(typeClass);
        return setElementType(parseType(typeClass.getSimpleName()));
    }

    public ArrayCreationExpr setElementType(/*final*/string type) {
        return setElementType(parseType(type));
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        if (initializer != null) {
            if (node == initializer) {
                removeInitializer();
                return true;
            }
        }
        for (int i = 0; i < levels.size(); i++) {
            if (levels.get(i) == node) {
                levels.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public ArrayCreationExpr removeInitializer() {
        return setInitializer((ArrayInitializerExpr) null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ArrayCreationExpr clone() {
        return (ArrayCreationExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ArrayCreationExprMetaModel getMetaModel() {
        return JavaParserMetaModel.arrayCreationExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == elementType) {
            setElementType((Type) replacementNode);
            return true;
        }
        if (initializer != null) {
            if (node == initializer) {
                setInitializer((ArrayInitializerExpr) replacementNode);
                return true;
            }
        }
        for (int i = 0; i < levels.size(); i++) {
            if (levels.get(i) == node) {
                levels.set(i, (ArrayCreationLevel) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isArrayCreationExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ArrayCreationExpr asArrayCreationExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifArrayCreationExpr(Consumer<ArrayCreationExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ArrayCreationExpr> toArrayCreationExpr() {
        return Optional.of(this);
    }
}
