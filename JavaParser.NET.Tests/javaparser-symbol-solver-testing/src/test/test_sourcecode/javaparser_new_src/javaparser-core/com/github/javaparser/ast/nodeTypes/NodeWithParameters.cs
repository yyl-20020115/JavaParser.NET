namespace com.github.javaparser.ast.nodeTypes;



public interface NodeWithParameters<T> {
    List<Parameter> getParameters();

    T setParameters(List<Parameter> parameters);

    default T addParameter(Type type, string name) {
        return addParameter(new Parameter(type, new VariableDeclaratorId(name)));
    }

    default T addParameter(Type paramClass, string name) {
        ((Node) this).tryAddImportToParentCompilationUnit(paramClass);
        return addParameter(new ClassOrInterfaceType(paramClass.getSimpleName()), name);
    }

    /**
     * Remember to import the class _in the compilation unit yourself
     * 
     * @param className the name of the class, ex : org.test.Foo or Foo if you added manually the import
     * @param name the name of the parameter
     */
    default T addParameter(string className, string name) {
        return addParameter(new ClassOrInterfaceType(className), name);
    }

    //@SuppressWarnings("unchecked")
    default T addParameter(Parameter parameter) {
        getParameters().add(parameter);
        parameter.setParentNode((Node) this);
        return (T) this;
    }

    default Parameter addAndGetParameter(Type type, string name) {
        return addAndGetParameter(new Parameter(type, new VariableDeclaratorId(name)));
    }

    default Parameter addAndGetParameter(Type paramClass, string name) {
        ((Node) this).tryAddImportToParentCompilationUnit(paramClass);
        return addAndGetParameter(new ClassOrInterfaceType(paramClass.getSimpleName()), name);
    }

    /**
     * Remember to import the class _in the compilation unit yourself
     * 
     * @param className the name of the class, ex : org.test.Foo or Foo if you added manually the import
     * @param name the name of the parameter
     * @return the {@link Parameter} created
     */
    default Parameter addAndGetParameter(string className, string name) {
        return addAndGetParameter(new ClassOrInterfaceType(className), name);
    }

    default Parameter addAndGetParameter(Parameter parameter) {
        getParameters().add(parameter);
        parameter.setParentNode((Node) this);
        return parameter;
    }

    /**
     * Try to find a {@link Parameter} by its name
     * 
     * @param name the name of the param
     * @return null if not found, the param found otherwise
     */
    default Parameter getParamByName(string name) {
        return getParameters().stream()
                .filter(p -> p.getName().equals(name)).findFirst().orElse(null);
    }

    /**
     * Try to find a {@link Parameter} by its type
     * 
     * @param type the type of the param
     * @return null if not found, the param found otherwise
     */
    default Parameter getParamByType(string type) {
        return getParameters().stream()
                .filter(p -> p.getType().toString().equals(type)).findFirst().orElse(null);
    }

    /**
     * Try to find a {@link Parameter} by its type
     * 
     * @param type the type of the param <b>take care about generics, it wont work</b>
     * @return null if not found, the param found otherwise
     */
    default Parameter getParamByType(Type type) {
        return getParameters().stream()
                .filter(p -> p.getType().toString().equals(type.getSimpleName())).findFirst().orElse(null);
    }
}
