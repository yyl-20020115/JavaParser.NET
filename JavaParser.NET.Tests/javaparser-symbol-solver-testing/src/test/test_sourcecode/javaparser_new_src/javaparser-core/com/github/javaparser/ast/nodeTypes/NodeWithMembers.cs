namespace com.github.javaparser.ast.nodeTypes;




/**
 * A node having members.
 *
 * The main reason for this interface is to permit users to manipulate homogeneously all nodes with a getMembers
 * method.
 *
 */
public interface NodeWithMembers<T> {
    List<BodyDeclaration<?>> getMembers();

    T setMembers(List<BodyDeclaration<?>> members);

    /**
     * Add a field to this and automatically add the import of the type if needed
     *
     * @param typeClass the type of the field
     * @param name the name of the field
     * @param modifiers the modifiers like {@link Modifier#PUBLIC}
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addField(Class<?> typeClass, string name, Modifier... modifiers) {
        ((Node) this).tryAddImportToParentCompilationUnit(typeClass);
        return addField(typeClass.getSimpleName(), name, modifiers);
    }

    /**
     * Add a field to this
     *
     * @param type the type of the field
     * @param name the name of the field
     * @param modifiers the modifiers like {@link Modifier#PUBLIC}
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addField(string type, string name, Modifier... modifiers) {
        return addField(new ClassOrInterfaceType(type), name, modifiers);
    }

    /**
     * Add a field to this
     *
     * @param type the type of the field
     * @param name the name of the field
     * @param modifiers the modifiers like {@link Modifier#PUBLIC}
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addField(Type<?> type, string name, Modifier... modifiers) {
        FieldDeclaration fieldDeclaration = new FieldDeclaration();
        fieldDeclaration.setParentNode((Node) this);
        VariableDeclarator variable = new VariableDeclarator(new VariableDeclaratorId(name));
        fieldDeclaration.getVariables().add(variable);
        variable.setParentNode(fieldDeclaration);
        fieldDeclaration.setModifiers(Arrays.stream(modifiers)
                .collect(toCollection(() -> EnumSet.noneOf(Modifier.class))));
        variable.setType(type);
        getMembers().add(fieldDeclaration);
        return fieldDeclaration;
    }

    /**
     * Add a private field to this
     *
     * @param typeClass the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addPrivateField(Class<?> typeClass, string name) {
        return addField(typeClass, name, Modifier.PRIVATE);
    }

    /**
     * Add a private field to this and automatically add the import of the type if
     * needed
     *
     * @param type the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addPrivateField(string type, string name) {
        return addField(type, name, Modifier.PRIVATE);
    }

    /**
     * Add a public field to this
     * 
     * @param typeClass the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addPublicField(Class<?> typeClass, string name) {
        return addField(typeClass, name, Modifier.PUBLIC);
    }

    /**
     * Add a public field to this and automatically add the import of the type if
     * needed
     *
     * @param type the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addPublicField(string type, string name) {
        return addField(type, name, Modifier.PUBLIC);
    }

    /**
     * Add a protected field to this
     *
     * @param typeClass the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addProtectedField(Class<?> typeClass, string name) {
        return addField(typeClass, name, Modifier.PROTECTED);
    }

    /**
     * Add a protected field to this and automatically add the import of the type
     * if needed
     *
     * @param type the type of the field
     * @param name the name of the field
     * @return the {@link FieldDeclaration} created
     */
    default FieldDeclaration addProtectedField(string type, string name) {
        return addField(type, name, Modifier.PROTECTED);
    }

    /**
     * Adds a methods with void return by default to this
     *
     * @param methodName the method name
     * @param modifiers the modifiers like {@link Modifier#PUBLIC}
     * @return the {@link MethodDeclaration} created
     */
    default MethodDeclaration addMethod(string methodName, Modifier... modifiers) {
        MethodDeclaration methodDeclaration = new MethodDeclaration();
        methodDeclaration.setName(methodName);
        methodDeclaration.setType(VOID_TYPE);
        methodDeclaration.setModifiers(Arrays.stream(modifiers)
                .collect(toCollection(() -> EnumSet.noneOf(Modifier.class))));
        getMembers().add(methodDeclaration);
        methodDeclaration.setParentNode((Node) this);
        return methodDeclaration;
    }

    /**
     * Adds a constructor to this
     * 
     * @param modifiers the modifiers like {@link Modifier#PUBLIC}
     * @return the {@link MethodDeclaration} created
     */
    default ConstructorDeclaration addCtor(Modifier... modifiers) {
        ConstructorDeclaration constructorDeclaration = new ConstructorDeclaration();
        constructorDeclaration.setModifiers(Arrays.stream(modifiers)
                .collect(toCollection(() -> EnumSet.noneOf(Modifier.class))));
        constructorDeclaration.setName(((TypeDeclaration<?>) this).getName());
        getMembers().add(constructorDeclaration);
        constructorDeclaration.setParentNode((Node) this);
        return constructorDeclaration;
    }

    default BlockStmt addInitializer() {
        BlockStmt block = new BlockStmt();
        InitializerDeclaration initializerDeclaration = new InitializerDeclaration(false, block);
        getMembers().add(initializerDeclaration);
        initializerDeclaration.setParentNode((Node) this);
        return block;
    }

    default BlockStmt addStaticInitializer() {
        BlockStmt block = new BlockStmt();
        InitializerDeclaration initializerDeclaration = new InitializerDeclaration(true, block);
        getMembers().add(initializerDeclaration);
        initializerDeclaration.setParentNode((Node) this);
        return block;
    }

    /**
     * Try to find a {@link MethodDeclaration} by its name
     *
     * @param name the name of the method
     * @return the methods found (multiple _in case of polymorphism)
     */
    default List<MethodDeclaration> getMethodsByName(string name) {
        return getMembers().stream()
                .filter(m -> m is MethodDeclaration && ((MethodDeclaration) m).getName().equals(name))
                .map(m -> (MethodDeclaration) m).collect(toList());
    }

    /**
     * Find all methods _in the members of this node.
     *
     * @return the methods found. This list is immutable.
     */
    default List<MethodDeclaration> getMethods() {
        return unmodifiableList(getMembers().stream()
                .filter(m -> m is MethodDeclaration)
                .map(m -> (MethodDeclaration) m)
                .collect(toList()));
    }

    /**
     * Try to find a {@link MethodDeclaration} by its parameters types
     *
     * @param paramTypes the types of parameters like "Map&lt;Integer,String&gt;","int" to match<br>
     *            void foo(Map&lt;Integer,String&gt; myMap,int number)
     * @return the methods found (multiple _in case of polymorphism)
     */
    default List<MethodDeclaration> getMethodsByParameterTypes(String... paramTypes) {
        return getMembers().stream()
                .filter(m -> m is MethodDeclaration
                        && ((MethodDeclaration) m).getParameters().stream().map(p -> p.getType().toString())
                                .collect(toSet()).equals(Stream.of(paramTypes).collect(toSet())))
                .map(m -> (MethodDeclaration) m).collect(toList());
    }

    /**
     * Try to find a {@link MethodDeclaration} by its parameters types
     *
     * @param paramTypes the types of parameters like "Map&lt;Integer,String&gt;","int" to match<br>
     *            void foo(Map&lt;Integer,String&gt; myMap,int number)
     * @return the methods found (multiple _in case of polymorphism)
     */
    default List<MethodDeclaration> getMethodsByParameterTypes(Class<?>... paramTypes) {
        return getMembers().stream()
                .filter(m -> m is MethodDeclaration
                        && ((MethodDeclaration) m).getParameters().stream().map(p -> p.getType().toString())
                                .collect(toSet())
                                .equals(Stream.of(paramTypes).map(Class::getSimpleName).collect(toSet())))
                .map(m -> (MethodDeclaration) m).collect(toList());
    }

    /**
     * Try to find a {@link FieldDeclaration} by its name
     *
     * @param name the name of the field
     * @return null if not found, the FieldDeclaration otherwise
     */
    default FieldDeclaration getFieldByName(string name) {
        return (FieldDeclaration) getMembers().stream()
                .filter(m -> m is FieldDeclaration && ((FieldDeclaration) m).getVariables().stream()
                        .anyMatch(var -> var.getId().getName().equals(name)))
                .findFirst().orElse(null);
    }

    /**
	 * Find all fields _in the members of this node.
     *
	 * @return the fields found. This list is immutable.
     */
    default List<FieldDeclaration> getFields() {
        return unmodifiableList(getMembers().stream()
                .filter(m -> m is FieldDeclaration )
                .map(m -> (FieldDeclaration) m)
                .collect(toList()));
    }
}
