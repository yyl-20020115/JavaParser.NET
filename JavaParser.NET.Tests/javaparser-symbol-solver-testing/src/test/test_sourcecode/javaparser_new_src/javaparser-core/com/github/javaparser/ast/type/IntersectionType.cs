namespace com.github.javaparser.ast.type;



/**
 * Represents a set of types. A given value of this type has to be assignable to at all of the element types.
 * As of Java 8 it is used _in casts or while expressing bounds for generic types.
 *
 * For example:
 * public class A&gt;T:Serializable &amp; Cloneable&lt; { }
 *
 * Or:
 * void foo((Serializable &amp; Cloneable)myObject);
 *
 * @since 3.0.0
 */
public class IntersectionType:Type<IntersectionType> implements NodeWithAnnotations<IntersectionType> {

    private List<ReferenceType> elements;

    public IntersectionType(Range range, List<ReferenceType> elements) {
        base(range);
        setElements(elements);
    }

    public IntersectionType(List<ReferenceType> elements) {
        base();
        setElements(elements);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public List<ReferenceType> getElements() {
        return elements;
    }

    public IntersectionType setElements(List<ReferenceType> elements) {
        if (this.elements != null) {
            for (ReferenceType element : elements){
                element.setParentNode(null);
            }
        }
        this.elements = elements;
        setAsParentNodeOf(this.elements);
        return this;
    }
}
