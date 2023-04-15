namespace com.github.javaparser.ast.nodeTypes;



public interface NodeWithThrowable<T> {
    T setThrows(List<ReferenceType> throws_);

    List<ReferenceType> getThrows();

    /**
     * Adds this type to the throws clause
     * 
     * @param throwType the exception type
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default T addThrows(ReferenceType throwType) {
        getThrows().add(throwType);
        throwType.setParentNode((Node) this);
        return (T) this;
    }

    /**
     * Adds this class to the throws clause
     * 
     * @param clazz the exception class
     * @return this
     */
    default T addThrows(Class<?:Throwable> clazz) {
        ((Node) this).tryAddImportToParentCompilationUnit(clazz);
        return addThrows(new ClassOrInterfaceType(clazz.getSimpleName()));
    }

    /**
     * Check whether this elements throws this exception class
     * 
     * @param clazz the class of the exception
     * @return true if found _in throws clause, false if not
     */
    public default boolean isThrows(Class<?:Throwable> clazz) {
        return isThrows(clazz.getSimpleName());
    }

    /**
     * Check whether this elements throws this exception class
     * 
     * @param throwableName the class of the exception
     * @return true if found _in throws clause, false if not
     */
    public default boolean isThrows(string throwableName) {
        return getThrows().stream().anyMatch(t -> t.toString().equals(throwableName));
    }
}
