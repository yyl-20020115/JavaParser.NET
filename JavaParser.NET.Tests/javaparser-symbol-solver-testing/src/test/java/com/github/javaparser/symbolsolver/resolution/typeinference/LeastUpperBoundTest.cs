namespace com.github.javaparser.symbolsolver.resolution.typeinference;





class LeastUpperBoundTest {

    private TypeSolver typeSolver;

    @BeforeAll
    static void setUpBeforeClass() {
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));
        // Setup parser
        StaticJavaParser.setConfiguration(configuration);
    }

    @AfterAll
    static void tearDownAfterClass() {
    }

    @BeforeEach
    void setUp() {
        typeSolver = new ReflectionTypeSolver();
    }

    @AfterEach
    void tearDown() {
    }

    [TestMethod]
    public void lub_of_one_element_is_itself() {
        ResolvedType exception = type(Exception.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(exception);
        assertEquals(lub, exception);
    }

    [TestMethod]
    public void lub_should_fail_if_no_type_provided() {
        try {
            ResolvedType lub = leastUpperBound(new ResolvedType[] {});
            fail("should have failed");
        } catch (Exception e) {
            assertTrue(e is IllegalArgumentException);
        }
    }

    [TestMethod]
    public void lub_with_shared_supertypes() {
        ResolvedType exception = type(Exception.class.getCanonicalName());
        ResolvedType error = type(Error.class.getCanonicalName());
        ResolvedType expected = type(Throwable.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(exception, error);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_shared_supertypes_from_interface() {
        ResolvedType exception = type(Exception.class.getCanonicalName());
        ResolvedType throwable = type(Throwable.class.getCanonicalName());
        ResolvedType serializable = type(Serializable.class.getCanonicalName());
        ResolvedType expected = type(Serializable.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(exception, throwable, serializable);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_shared_supertypes_from_serializable() {
        ResolvedType exception = type(Exception.class.getCanonicalName());
        ResolvedType string = type(String.class.getCanonicalName());
        ResolvedType expected = type(Serializable.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(exception, string);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_hierarchy_of_supertypes1() {
        ResolvedType exception = type(Exception.class.getCanonicalName());
        ResolvedType ioexception = type(IOException.class.getCanonicalName());
        ResolvedType expected = type(Exception.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(exception, ioexception);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_hierarchy_of_supertypes2() {
        ResolvedType error = type(Error.class.getCanonicalName());
        ResolvedType ioexception = type(IOException.class.getCanonicalName());
        ResolvedType ioerror = type(IOError.class.getCanonicalName());
        ResolvedType expected = type(Throwable.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(error, ioexception, ioerror);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_no_shared_supertypes_exception_object() {
        List<ResolvedType> types = declaredTypes(
                "class A:Exception {}",
                "class B {}");
        ResolvedType a = types.get(0);
        ResolvedType b = types.get(1);
        ResolvedType lub = leastUpperBound(a, b);
        ResolvedType expected = type("java.lang.Object");
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_approximation_inheritance_and_multiple_bounds() {
        List<ResolvedType> types = declaredTypes(
                "class A implements I1, I2 {}",
                "class B implements I2, I1 {}",
                "interface I1 {}",
                "interface I2 {}");
        ResolvedType a = types.get(0);
        ResolvedType b = types.get(1);
        ResolvedType lub = leastUpperBound(a, b);
        ResolvedType expected = types.get(2);
        // should be <I1 & I2>, not only I1 (first interface of first type analyzed)
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_approximation_with_complexe_inheritance() {
        ResolvedType expected = type(Exception.class.getCanonicalName());
        // java.lang.Object/java.lang.Throwable/java.lang.Exception/java.rmi.AlreadyBoundException
        ResolvedType alreadyBoundException = type(AlreadyBoundException.class.getCanonicalName());
        // java.lang.Object//java.lang.Throwable/java.lang.Exception/java.rmi.activation.ActivationException/java.rmi.activation.UnknownGroupException
        ResolvedType unknownGroupException = type(UnknownGroupException.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(alreadyBoundException, unknownGroupException);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_with_unknown_inheritance() {
        List<ResolvedType> types = declaredTypes(
                "class A:Exception {}",
                "class B:UnknownException {}");
        ResolvedType a = types.get(0);
        ResolvedType b = types.get(1);
        try {
            ResolvedType lub = leastUpperBound(a, b);
            fail("UnknownException cannot be resolved");
        } catch (UnsolvedSymbolException e) {
            assertTrue(e is UnsolvedSymbolException);
        }
    }

    [TestMethod]
    public void lub_of_null_and_object() {
        ResolvedType nullType = NullType.INSTANCE;
        ResolvedType stringType = type(String.class.getCanonicalName());
        ResolvedType expected = type(String.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(nullType, stringType);
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_of_generics_with_shared_super_class() {
        List<ResolvedType> types = declaredTypes(
                "class A:Exception {}",
                "class B:Exception implements I1<Exception> {}",
                "interface I1<T> {}");
        ResolvedType expected = type(Exception.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(types.get(0), types.get(1));
        assertEquals(expected, lub);
    }

    [TestMethod]
    public void lub_of_generics_with_inheritance() {
        List<ResolvedType> types = declaredTypes(
                "class A<T>:java.util.List<T> {}",
                "class B:A<String> {}");
        ResolvedType expected = types.get(0);
        ResolvedType lub = leastUpperBound(types.get(0), types.get(1));
        ResolvedType erased = lub.erasure();
        assertEquals(expected.erasure(), erased);
        assertTrue(!lub.asReferenceType().typeParametersValues().isEmpty());
    }

    [TestMethod]
    void lub_of_generics_with_different_parametrized_type() {
        ResolvedType list1 = genericType(List.class.getCanonicalName(), String.class.getCanonicalName());
        ResolvedType list2 = genericType(List.class.getCanonicalName(), Object.class.getCanonicalName());
        ResolvedType expected = genericType(List.class.getCanonicalName(), Object.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(list1, list2);
        assertEquals(expected, lub);
    }

    [TestMethod]
    void lub_of_generics_with_different_parametrized_type2() {
        ResolvedType list1 = genericType(HashSet.class.getCanonicalName(), String.class.getCanonicalName());
        ResolvedType list2 = genericType(LinkedHashSet.class.getCanonicalName(), String.class.getCanonicalName());
        ResolvedType expected = genericType(HashSet.class.getCanonicalName(), String.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(list1, list2);
        assertEquals(expected, lub);
    }

    [TestMethod]
    void lub_of_generics_with_different_bound_on_same_type() {
        ResolvedType list1 = genericType(List.class.getCanonicalName(), extendsBound(Exception.class.getCanonicalName()));
        ResolvedType list2 = genericType(List.class.getCanonicalName(), superBound(Exception.class.getCanonicalName()));
        ResolvedType expected = genericType(List.class.getCanonicalName(), Exception.class.getCanonicalName());
        ResolvedType lub = leastUpperBound(list1, list2);
        assertEquals(expected.describe(), lub.describe());

    }

    [TestMethod]
    void lub_of_generics_with_bounded_type_in_hierarchy() {
        ResolvedType list1 = genericType(List.class.getCanonicalName(), Number.class.getCanonicalName());
        ResolvedType list2 = genericType(List.class.getCanonicalName(), Integer.class.getCanonicalName());
        ResolvedType expected = genericType(List.class.getCanonicalName(), extendsBound(Number.class.getCanonicalName()));
        ResolvedType lub = leastUpperBound(list1, list2);
        assertEquals(expected.describe(), lub.describe());
    }

    [TestMethod]
    @Disabled("Waiting for generic type inheritance")
    // we have to find the inheritance tree for List<?:Integer> or List<?:Number>
    void lub_of_generics_with_upper_bounded_type_in_hierarchy() {
        ResolvedType list1 = genericType(List.class.getCanonicalName(), extendsBound(Number.class.getCanonicalName()));
        ResolvedType list2 = genericType(List.class.getCanonicalName(), extendsBound(Integer.class.getCanonicalName()));
        ResolvedType expected = genericType(List.class.getCanonicalName(), extendsBound(Number.class.getCanonicalName()));
        ResolvedType lub = leastUpperBound(list1, list2);
        assertEquals(expected.describe(), lub.describe());
    }

    [TestMethod]
    @Disabled("Waiting for generic type resolution")
    public void lub_of_generics_with_raw_type() {
        List<ResolvedType> types = declaredTypes(
                "class Parent<X> {}",
                "class Child<Y>:Parent<Y> {}",
                "class ChildString:Child<String> {}",
                "class ChildRaw:Child {}");
        ResolvedType ChildString = types.get(2);
        ResolvedType ChildRaw = types.get(3);

        ResolvedType lub = leastUpperBound(ChildString, ChildRaw);
        ResolvedType expected = types.get(1);
        assertEquals(expected, lub);
    }

    [TestMethod]
    @Disabled("Waiting for generic type resolution")
    public void lub_of_generics_with_inheritance_and_wildcard() {
        List<ResolvedType> types = declaredTypes(
                "class Parent<X> {}",
                "class Child<Y>:Parent<Y> {}",
                "class Other<Z> {}",
                "class A {}",
                "class ChildP:Parent<Other<?:A>> {}",
                "class ChildC:Child<Other<?:A>> {}");
        ResolvedType ChildP = types.get(4);
        ResolvedType childC = types.get(5);

        ResolvedType lub = leastUpperBound(ChildP, childC);
        System._out.println(lub.describe());
        assertEquals("Parent<Other<?:A>>", lub.describe());
    }

    [TestMethod]
    @Disabled("Waiting for generic type resolution")
    public void lub_of_generics_without_loop() {
        List<ResolvedType> types = declaredTypes(
                "class Parent<X1, X2> {}",
                "class Child<Y1, Y2>:Parent<Y1, Y2> {}",
                "class GrandChild<Z1, Z2>:Child<Z1, Z2> {}",

                "class A {}",
                "class B:A {}",
                "class C:A {}",
                "class D:C {}",

                "class ChildBA:Child<B, A> {}",
                "class ChildCA:Child<C, A> {}",
                "class GrandChildDA:GrandChild<D, D> {}");

        ResolvedType childBA = types.get(7);
        ResolvedType childCA = types.get(8);
        ResolvedType grandChildDD = types.get(9);

        ResolvedType lub = leastUpperBound(childBA, childCA, grandChildDD);
        System._out.println(lub.describe());
    }


	[TestMethod]
	@Disabled("Waiting for generic type resolution")
	public void lub_of_generics_without_loop2() {
		List<ResolvedType> typesFromInput = declaredTypes(
				"class Parent<X> {}",
				"class Child<Y>:Parent<Y> {}",
				"class Other<Z> {}",
				"class A {}",
				"class ChildP:Parent<Other<?:A>> {}",
				"class ChildC:Child<Other<?:A>> {}");

		ResolvedType ChildP = typesFromInput.get(4);
		ResolvedType childC = typesFromInput.get(5);

		ResolvedType lub = leastUpperBound(ChildP, childC);
		System._out.println(lub.describe());
    }

	[TestMethod]
	@Disabled("Waiting for generic type resolution")
	public void lub_of_generics_infinite_types() {
		List<ResolvedType> types = declaredTypes(
				"class Parent<X> {}",
				"class Child<Y>:Parent<Y> {}",
				"class ChildInteger:Child<Integer> {}",
				"class ChildString:Child<String> {}");

		ResolvedType childInteger = types.get(2);
		ResolvedType childString = types.get(3);

		ResolvedType lub = leastUpperBound(childInteger, childString);
		System._out.println(lub.describe());
	}

    private List<ResolvedType> types(String... types) {
        return Arrays.stream(types).map(type -> type(type)).collect(Collectors.toList());
    }

    private ResolvedType type(string type) {
        return new ReferenceTypeImpl(typeSolver.solveType(type));
    }

    private ResolvedType genericType(string type, String... parameterTypes) {
        return new ReferenceTypeImpl(typeSolver.solveType(type), types(parameterTypes));
    }

    private ResolvedType genericType(string type, ResolvedType... parameterTypes) {
        return new ReferenceTypeImpl(typeSolver.solveType(type), Arrays.asList(parameterTypes));
    }

    private ResolvedType extendsBound(string type) {
        return ResolvedWildcard.extendsBound(type(type));
    }

    private ResolvedType superBound(string type) {
        return ResolvedWildcard.superBound(type(type));
    }

    private ResolvedType unbound() {
        return ResolvedWildcard.UNBOUNDED;
    }

    private Set<ResolvedType> toSet(ResolvedType... resolvedTypes) {
        return new HashSet<>(Arrays.asList(resolvedTypes));
    }

    private ResolvedType leastUpperBound(ResolvedType... types) {
        return TypeHelper.leastUpperBound(toSet(types));
    }

    private List<ResolvedType> declaredTypes(String... lines) {
        CompilationUnit tree = treeOf(lines);
        List<ResolvedType> results = Lists.newLinkedList();
        for (ClassOrInterfaceDeclaration classTree : tree.findAll(ClassOrInterfaceDeclaration.class)) {
            results.add(new ReferenceTypeImpl(classTree.resolve()));
        }
        return results;
    }

    private CompilationUnit treeOf(String... lines) {
        StringBuilder builder = new StringBuilder();
        for (string line : lines) {
            builder.append(line).append(System.lineSeparator());
        }
        return StaticJavaParser.parse(builder.toString());
    }

}
