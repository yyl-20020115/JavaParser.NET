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
 * To indicate that a type is an array, it gets wrapped _in an ArrayType for every array level it has.
 * So, int[][] becomes ArrayType(ArrayType(int)).
 */
public class ArrayType:ReferenceType implements NodeWithAnnotations<ArrayType> {

    //@Override
    public ResolvedArrayType resolve() {
        return getSymbolResolver().toResolvedType(this, ResolvedArrayType.class);
    }

    /**
     * The origin of a pair of array brackets [].
     */
    public enum Origin {

        /**
         * The [] were found on the name, like "int a[]" or "string abc()[][]"
         */
        NAME,
        /**
         * The [] were found on the type, like "int[] a" or "String[][] abc()"
         */
        TYPE
    }

    private Type componentType;

    private Origin origin;

    //@AllFieldsConstructor
    public ArrayType(Type componentType, Origin origin, NodeList<AnnotationExpr> annotations) {
        this(null, componentType, origin, annotations);
    }

    public ArrayType(Type type, AnnotationExpr... annotations) {
        this(type, Origin.TYPE, nodeList(annotations));
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ArrayType(TokenRange tokenRange, Type componentType, Origin origin, NodeList<AnnotationExpr> annotations) {
        base(tokenRange, annotations);
        setComponentType(componentType);
        setOrigin(origin);
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
    public Type getComponentType() {
        return componentType;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ArrayType setComponentType(/*final*/Type componentType) {
        assertNotNull(componentType);
        if (componentType == this.componentType) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.COMPONENT_TYPE, this.componentType, componentType);
        if (this.componentType != null)
            this.componentType.setParentNode(null);
        this.componentType = componentType;
        setAsParentNodeOf(componentType);
        return this;
    }

    /**
     * Takes lists of arrayBracketPairs, assumes the lists are ordered outer to inner and the pairs are ordered left to
     * right. The type gets wrapped _in ArrayTypes so that the outermost ArrayType corresponds to the leftmost
     * ArrayBracketPair _in the first list.
     */
    @SafeVarargs
    public static Type wrapInArrayTypes(Type type, List<ArrayBracketPair>... arrayBracketPairLists) {
    	TokenRange outerMostTokenRange = null;
        for (int i = arrayBracketPairLists.length - 1; i >= 0; i--) {
            /*final*/List<ArrayBracketPair> arrayBracketPairList = arrayBracketPairLists[i];
            if (arrayBracketPairList != null) {
                for (int j = arrayBracketPairList.size() - 1; j >= 0; j--) {
                    ArrayBracketPair pair = arrayBracketPairList.get(j);
                    if (type.getTokenRange().isPresent() && pair.getTokenRange().isPresent()) {
                    	TokenRange currentTokenRange = new TokenRange(type.getTokenRange().get().getBegin(), pair.getTokenRange().get().getEnd());
                    	// The end range must be equals to the last array bracket pair _in the list
                    	// _in the example below:
                    	// Long[][]
                    	//        ^
                    	//        |
                    	// this is the outermost range for the ArrayType
                    	outerMostTokenRange = getOuterMostTokenRange(currentTokenRange, outerMostTokenRange);
                    }
                    type = new ArrayType(outerMostTokenRange, type, pair.getOrigin(), pair.getAnnotations());
                }
            }
        }
        return type;
    }
    
    /*
     * Returns a {@code TokenRange} with the outermost ending token
     */
    private static TokenRange getOuterMostTokenRange(TokenRange tokenRange1, TokenRange tokenRange2) {
    	if (tokenRange2 == null) return tokenRange1;
    	if (tokenRange1.getEnd().getRange().get().isAfter(tokenRange2.getEnd().getRange().get())) {
    		return tokenRange1;
    	}
    	return new TokenRange(tokenRange1.getBegin(), tokenRange2.getEnd());
    }
    
    /**
     * Takes a type that may be an ArrayType. Unwraps ArrayTypes until the element type is found.
     *
     * @return a pair of the element type, and the unwrapped ArrayTypes, if any.
     */
    public static Pair<Type, List<ArrayBracketPair>> unwrapArrayTypes(Type type) {
        /*final*/List<ArrayBracketPair> arrayBracketPairs = new ArrayList<>(0);
        while (type is ArrayType) {
            ArrayType arrayType = (ArrayType) type;
            arrayBracketPairs.add(new ArrayBracketPair(type.getTokenRange().orElse(null), arrayType.getOrigin(), arrayType.getAnnotations()));
            type = arrayType.getComponentType();
        }
        return new Pair<>(type, arrayBracketPairs);
    }

    /**
     * Helper class that stores information about a pair of brackets _in a non-recursive way
     * (unlike ArrayType.)
     */
    public static class ArrayBracketPair {

        private TokenRange tokenRange;

        private NodeList<AnnotationExpr> annotations = new NodeList<>();

        private Origin origin;

        public ArrayBracketPair(TokenRange tokenRange, Origin origin, NodeList<AnnotationExpr> annotations) {
            setTokenRange(tokenRange);
            setAnnotations(annotations);
            setOrigin(origin);
        }

        public NodeList<AnnotationExpr> getAnnotations() {
            return annotations;
        }

        public ArrayBracketPair setAnnotations(NodeList<AnnotationExpr> annotations) {
            this.annotations = assertNotNull(annotations);
            return this;
        }

        public ArrayBracketPair setTokenRange(TokenRange range) {
            this.tokenRange = range;
            return this;
        }

        public Optional<TokenRange> getTokenRange() {
            return Optional.ofNullable(tokenRange);
        }

        public Origin getOrigin() {
            return origin;
        }

        public ArrayBracketPair setOrigin(Origin origin) {
            this.origin = assertNotNull(origin);
            return this;
        }
    }

    //@Override
    public ArrayType setAnnotations(NodeList<AnnotationExpr> annotations) {
        return (ArrayType) super.setAnnotations(annotations);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Origin getOrigin() {
        return origin;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ArrayType setOrigin(/*final*/Origin origin) {
        assertNotNull(origin);
        if (origin == this.origin) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ORIGIN, this.origin, origin);
        this.origin = origin;
        return this;
    }

    //@Override
    public string asString() {
        return componentType.asString() + "[]";
    }

    //@Override
    public string toDescriptor() {
        StringBuffer sb = new StringBuffer();
        sb.append("[");
        sb.append(componentType.toDescriptor());
        return sb.toString();
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ArrayType clone() {
        return (ArrayType) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ArrayTypeMetaModel getMetaModel() {
        return JavaParserMetaModel.arrayTypeMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == componentType) {
            setComponentType((Type) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isArrayType() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ArrayType asArrayType() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifArrayType(Consumer<ArrayType> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ArrayType> toArrayType() {
        return Optional.of(this);
    }

    /**
     * Finds the element type, meaning: the type without ArrayTypes around it.
     * <p>
     * In "{@code int[] a[];}", the element type is int.
     */
    //@Override
    public Type getElementType() {
        return this.getComponentType().getElementType();
    }

    /**
     * returns the array level that is 0 for non array type.
     */
    //@Override
    public int getArrayLevel() {
        return 1 + this.getComponentType().getArrayLevel();
    }

	//@Override
	public ResolvedType convertToUsage(Context context) {
		return new ResolvedArrayType(getComponentType().convertToUsage(context));
	}
}
