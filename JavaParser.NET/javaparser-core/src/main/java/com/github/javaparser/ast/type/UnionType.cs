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
 * <h1>The union type</h1>
 * Represents a set of types. A given value of this type has to be assignable to at least one of the element types.
 * <h2>Java 1-6</h2>
 * Does not exist.
 * <h2>Java 7+</h2>
 * As of Java 7 it is used _in catch clauses.
 * <pre><code>
 * try {
 * ...
 * } catch(<b>IOException | NullPointerException ex</b>) {
 * ...
 * }
 * </code></pre>
 *
 * The types that make up the union type are its "elements"
 */
public class UnionType:Type implements NodeWithAnnotations<UnionType> {

    //@NonEmptyProperty
    private NodeList<ReferenceType> elements;

    public UnionType() {
        this(null, new NodeList<>());
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public UnionType(TokenRange tokenRange, NodeList<ReferenceType> elements) {
        base(tokenRange);
        setElements(elements);
        customInitialization();
    }

    //@AllFieldsConstructor
    public UnionType(NodeList<ReferenceType> elements) {
        this(null, elements);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<ReferenceType> getElements() {
        return elements;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public UnionType setElements(/*final*/NodeList<ReferenceType> elements) {
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
    public UnionType setAnnotations(NodeList<AnnotationExpr> annotations) {
        return (UnionType) super.setAnnotations(annotations);
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
        return elements.stream().map(Type::asString).collect(joining("|"));
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public UnionType clone() {
        return (UnionType) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public UnionTypeMetaModel getMetaModel() {
        return JavaParserMetaModel.unionTypeMetaModel;
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
    public bool isUnionType() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public UnionType asUnionType() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifUnionType(Consumer<UnionType> action) {
        action.accept(this);
    }

    //@Override
    public ResolvedUnionType resolve() {
        return getSymbolResolver().toResolvedType(this, ResolvedUnionType.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<UnionType> toUnionType() {
        return Optional.of(this);
    }

	@Override
	public ResolvedType convertToUsage(Context context) {
		List<ResolvedType> resolvedElements = getElements().stream()
                .map(el -> el.convertToUsage(context))
                .collect(Collectors.toList());
        return new ResolvedUnionType(resolvedElements);
	}
}
