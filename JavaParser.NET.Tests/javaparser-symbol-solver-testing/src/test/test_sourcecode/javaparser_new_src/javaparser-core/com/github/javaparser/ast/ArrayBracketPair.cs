namespace com.github.javaparser.ast;




/**
 * In, for example, <code>int[] a[];</code> there are two ArrayBracketPair objects,
 * one for the [] after int, one for the [] after a.
 */
public class ArrayBracketPair:Node implements NodeWithAnnotations<ArrayBracketPair> {
    private List<AnnotationExpr> annotations;

    public ArrayBracketPair(Range range, List<AnnotationExpr> annotations) {
        base(range);
        setAnnotations(annotations);
    }

    ////@Override
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
    }

    public List<AnnotationExpr> getAnnotations() {
        annotations = ensureNotNull(annotations);
        return annotations;
    }

    public ArrayBracketPair setAnnotations(List<AnnotationExpr> annotations) {
        setAsParentNodeOf(annotations);
        this.annotations = annotations;
        return this;
    }
}
