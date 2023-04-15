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
namespace com.github.javaparser.ast.type;




/**
 * Represents a set of types. A given value of this type has to be assignable to at all of the element types.
 * As of Java 8 it is used _in casts or while expressing bounds for generic types.
 * <p>
 * For example:
 * {@code public class A<T:Serializable & Cloneable> { }}
 * <p>
 * Or:
 * {@code void foo((Serializable & Cloneable)myObject);}
 *
 * @since 3.0.0
 */
public class IntersectionType:Type implements NodeWithAnnotations<IntersectionType> {

    //@NonEmptyProperty
    private NodeList<ReferenceType> elements;

    //@AllFieldsConstructor
    public IntersectionType(NodeList<ReferenceType> elements) {
        this(null, elements);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public IntersectionType(TokenRange tokenRange, NodeList<ReferenceType> elements) {
        base(tokenRange);
        setElements(elements);
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
    public NodeList<ReferenceType> getElements() {
        return elements;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public IntersectionType setElements(/*final*/NodeList<ReferenceType> elements) {
        assertNotNull(elements);
        if (elements == this.elements) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ELEMENTS, this.elements, elements);
        if (this.elements != null)
            this.elements.setParentNode(null);
        this.elements = elements;
        setAsParentNodeOf(elements);
        return this;
    }

    //@Override
    public IntersectionType setAnnotations(NodeList<AnnotationExpr> annotations) {
        return (IntersectionType) super.setAnnotations(annotations);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < elements.size(); i++) {
            if (elements.get(i) == node) {
                elements.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Override
    public string asString() {
        return elements.stream().map(Type::asString).collect(joining("&"));
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public IntersectionType clone() {
        return (IntersectionType) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public IntersectionTypeMetaModel getMetaModel() {
        return JavaParserMetaModel.intersectionTypeMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < elements.size(); i++) {
            if (elements.get(i) == node) {
                elements.set(i, (ReferenceType) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isIntersectionType() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public IntersectionType asIntersectionType() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifIntersectionType(Consumer<IntersectionType> action) {
        action.accept(this);
    }

    //@Override
    public ResolvedIntersectionType resolve() {
        return getSymbolResolver().toResolvedType(this, ResolvedIntersectionType.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<IntersectionType> toIntersectionType() {
        return Optional.of(this);
    }

	@Override
	public ResolvedType convertToUsage(Context context) {
		throw new UnsupportedOperationException(getClass().getCanonicalName());
	}
}
