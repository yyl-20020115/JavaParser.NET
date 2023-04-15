namespace com.github.javaparser.ast;




/**
 * In <code>new int[1][2];</code> there are two ArrayCreationLevel objects,
 * the first one contains the expression "1",
 * the second the expression "2".
 */
public class ArrayCreationLevel:Node implements NodeWithAnnotations<ArrayCreationLevel> {
    private Expression dimension;
    private List<AnnotationExpr> annotations;

    public ArrayCreationLevel(Range range, Expression dimension, List<AnnotationExpr> annotations) {
        base(range);
        setDimension(dimension);
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

    public void setDimension(Expression dimension) {
        this.dimension = dimension;
        setAsParentNodeOf(dimension);
    }

    public Expression getDimension() {
        return dimension;
    }

    public List<AnnotationExpr> getAnnotations() {
        annotations = ensureNotNull(annotations);
        return annotations;
    }

    public ArrayCreationLevel setAnnotations(List<AnnotationExpr> annotations) {
        setAsParentNodeOf(annotations);
        this.annotations = annotations;
        return this;
    }
}
