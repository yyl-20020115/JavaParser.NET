namespace com.github.javaparser.ast.nodeTypes;



public interface NodeWithImplements<T> {
    public List<ClassOrInterfaceType> getImplements();

    public T setImplements(List<ClassOrInterfaceType> implementsList);

    /**
     * Add an implements to this
     * 
     * @param name the name of the type to:from
     * @return this
     */
    //@SuppressWarnings("unchecked")
    public default T addImplements(string name) {
        ClassOrInterfaceType classOrInterfaceType = new ClassOrInterfaceType(name);
        getImplements().add(classOrInterfaceType);
        classOrInterfaceType.setParentNode((Node) this);
        return (T) this;
    }

    /**
     * Add an implements to this and automatically add the import
     * 
     * @param clazz the type to implements from
     * @return this
     */
    public default T addImplements(Class<?> clazz) {
        ((Node) this).tryAddImportToParentCompilationUnit(clazz);
        return addImplements(clazz.getSimpleName());
    }
}
