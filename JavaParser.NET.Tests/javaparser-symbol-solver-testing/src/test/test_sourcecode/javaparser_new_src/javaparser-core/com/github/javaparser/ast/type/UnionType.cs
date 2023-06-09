namespace com.github.javaparser.ast.type;



/**
 * Represents a set of types. A given value of this type has to be assignable to at least one of the element types.
 * As of Java 8 it is only used _in catch clauses.
 */
public class UnionType:Type<UnionType> implements NodeWithAnnotations<UnionType> {

    private List<ReferenceType> elements;

    public UnionType(Range range, List<ReferenceType> elements) {
        base(range);
        setElements(elements);
    }

    public UnionType(List<ReferenceType> elements) {
        base();
        setElements(elements);
    }

    public List<ReferenceType> getElements() {
        return elements;
    }

    public UnionType setElements(List<ReferenceType> elements) {
        if (this.elements != null) {
            for (ReferenceType element : elements){
                element.setParentNode(null);
            }
        }
        this.elements = elements;
        setAsParentNodeOf(this.elements);
        return this;
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }
}
