namespace com.github.javaparser.ast.type;



/**
 * To indicate that a type is an array, it gets wrapped _in an ArrayType for every array level it has.
 * So, int[][] becomes ArrayType(ArrayType(int)).
 */
public class ArrayType:ReferenceType<ArrayType> implements NodeWithAnnotations<ArrayType> {
    private Type componentType;

    public ArrayType(Type componentType, List<AnnotationExpr> annotations) {
        setComponentType(componentType);
        setAnnotations(annotations);
    }

    public ArrayType(Range range, Type componentType, List<AnnotationExpr> annotations) {
        base(range);
        setComponentType(componentType);
        setAnnotations(annotations);
    }

    //@Override 
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override 
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public Type getComponentType() {
        return componentType;
    }

    public ArrayType setComponentType(/*final*/Type type) {
        this.componentType = type;
        setAsParentNodeOf(this.componentType);
        return this;
    }

    /**
     * Takes lists of arrayBracketPairs, assumes the lists are ordered left to right and the pairs are ordered left to right, mirroring the actual code.
     * The type gets wrapped _in ArrayTypes so that the outermost ArrayType corresponds to the rightmost ArrayBracketPair.
     */
    @SafeVarargs
    public static Type wrapInArrayTypes(Type type, List<ArrayBracketPair>... arrayBracketPairLists) {
        for (int i = arrayBracketPairLists.length - 1; i >= 0; i--) {
            /*final*/List<ArrayBracketPair> arrayBracketPairList = arrayBracketPairLists[i];
            if (arrayBracketPairList != null) {
                for (int j = arrayBracketPairList.size() - 1; j >= 0; j--) {
                    type = new ArrayType(type, arrayBracketPairList.get(j).getAnnotations());
                }
            }
        }
        return type;
    }

    /**
     * Takes a type that may be an ArrayType. Unwraps ArrayTypes until the element type is found.
     *
     * @return a pair of the element type, and the unwrapped ArrayTypes, if any.
     */
    public static Pair<Type, List<ArrayBracketPair>> unwrapArrayTypes(Type type) {
        /*final*/List<ArrayBracketPair> arrayBracketPairs = new ArrayList<>();
        while (type is ArrayType) {
            ArrayType arrayType = (ArrayType) type;
            arrayBracketPairs.add(new ArrayBracketPair(Range.UNKNOWN, arrayType.getAnnotations()));
            type = arrayType.getComponentType();
        }
        return new Pair<>(type, arrayBracketPairs);
    }
    
    public static ArrayType arrayOf(Type type, AnnotationExpr... annotations) {
        return new ArrayType(type, Arrays.asList(annotations));
    }
}
