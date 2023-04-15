namespace com.github.javaparser.symbolsolver.resolution.logic;





class FunctionalInterfaceLogicTest:AbstractSymbolResolutionTest {

	private TypeSolver typeSolver;

	@BeforeEach
	void setup() {
		CombinedTypeSolver combinedtypeSolver = new CombinedTypeSolver();
		combinedtypeSolver.add(new ReflectionTypeSolver());
		typeSolver = combinedtypeSolver;
	}

	/*
	 * A simple example of a functional interface
	 */
	[TestMethod]
	void simpleExampleOfFunctionnalInterface() {
		string code = "interface Runnable {\n"
				+ "    void run();\n"
				+ "}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = cu.findFirst(ClassOrInterfaceDeclaration.class).get();
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());
	}

	/*
	 * The following interface is not functional because it declares nothing which
	 * is not already a member of Object
	 */
	[TestMethod]
	void notFunctionalBecauseItDeclaresNothingWhichIsNotAlreadyAMemberOfObject() {
		string code = "interface NonFunc {\n"
				+ "    boolean equals(Object obj);\n"
				+ "}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = cu.findFirst(ClassOrInterfaceDeclaration.class).get();
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertFalse(methodUsage.isPresent());
	}

	/*
	 * its subinterface can be functional by declaring an abstract method which is
	 * not a member of Object
	 */
	[TestMethod]
	void subinterfaceCanBeFunctionalByDclaringAnAbstractMethodWhichIsNotAMemberOfObject() {
		string code = "interface NonFunc {\n"
				+ "    boolean equals(Object obj);\n" + "}\n"
				+ "interface Func:NonFunc {\n"
				+ "    int compare(string o1, string o2);\n" + "}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Func");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());
	}

	/*
	 * the well known interface java.util.Comparator<T> is functional because it has
	 * one abstract non-Object method:
	 */
	[TestMethod]
	void isFunctionalBecauseItHasOneAbstractNonObjectMethod() {
		string code = "interface Comparator<T> {\n"
				+ "    boolean equals(Object obj);\n"
				+ "    int compare(T o1, T o2);\n" + "}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Comparator");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());
	}

	/*
	 * The following interface is not functional because while it only declares one
	 * abstract method which is not a member of Object, it declares two abstract
	 * methods which are not public members of Object:
	 */
	[TestMethod]
	void isNotFunctionalBecauseItDeclaresTwoAbstractMethodsWhichAreNotPublicMembersOfObject() {
		string code = "interface Foo {\n"
				+ "    int m();\n"
				+ "    Object clone();\n"
				+ "}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Foo");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertFalse(methodUsage.isPresent());
	}

	/*
	 * Z is a functional interface because while it inherits two abstract methods
	 * which are not members of Object, they have the same signature, so the
	 * inherited methods logically represent a single method:
	 */
	[TestMethod]
	void isFunctionalInterfaceBecauseInheritedAbstractMethodsHaveTheSameSignature() {
		string code = "interface X { int m(Iterable<String> arg); }\n"
				+ "interface Y { int m(Iterable<String> arg); }\n"
				+ "interface Z:X, Y {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Z");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());
	}

	/*
	 * Z is a functional interface _in the following interface hierarchy because Y.m
	 * is a subsignature of X.m and is return-type-substitutable for X.m:
	 */
	[TestMethod]
	@Disabled("Return-Type-Substituable must be implemented on reference type")
	void isFunctionalInterfaceBecauseOfSubsignatureAndSubstitutableReturnType() {
		string code = "interface X { Iterable m(Iterable<String> arg); }\n"
				+ "interface Y { Iterable<String> m(Iterable arg); }\n"
				+ "interface Z:X, Y {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Z");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());
	}

	/*
	 * the definition of "functional interface" respects the fact that an interface
	 * may only have methods with override-equivalent signatures if one is
	 * return-type-substitutable for all the others. Thus, _in the following
	 * interface hierarchy where Z causes a compile-time error, Z is not a
	 * functional interface: (because none of its abstract members are
	 * return-type-substitutable for all other abstract members)
	 */
	[TestMethod]
	void isNotFunctionalInterfaceBecauseNoneOfItsAbstractMembersAreReturnTypeSubstitutableForAllOtherAbstractMembers() {
		string code = "interface X { long m(); }\n"
				+ "interface Y { int m(); }\n"
				+ "interface Z:X, Y {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Z");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertFalse(methodUsage.isPresent());
	}

	/*
	 * In the following example, the declarations of Foo<T,N> and Bar are legal: _in
	 * each, the methods called m are not subsignatures of each other, but do have
	 * different erasures. Still, the fact that the methods _in each are not
	 * subsignatures means Foo<T,N> and Bar are not functional interfaces. However,
	 * Baz is a functional interface because the methods it inherits from
	 * Foo<Integer,Integer> have the same signature and so logically represent a
	 * single method.
	 */
	[TestMethod]
	void bazIsAFunctionalInterfaceBecauseMethodsItInheritsFromFooHaveTheSameSignature() {
		string code = "interface Foo<T, N:Number> {\n"
				+ "    void m(T arg);\n"
				+ "    void m(N arg);\n"
				+ "}\n"
				+ "interface Bar:Foo<String, Integer> {}\n"
				+ "interface Baz:Foo<Integer, Integer> {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Baz");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());

		classOrInterfaceDecl = Navigator.demandInterface(cu, "Bar");
		resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertFalse(methodUsage.isPresent());
	}

	/*
	 * Functional: signatures are logically "the same"
	 */
	[TestMethod]
	void withGenericMethodsWithSameSignatures() {
		string code = "interface Action<T> {};\n"
				+ "interface X { <T> T execute(Action<T> a); }\n"
				+ "interface Y { <S> S execute(Action<S> a); }\n"
				+ "interface Exec:X, Y {}\n";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Exec");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());

	}

	/*
	 * Error: different signatures, same erasure
	 */
	[TestMethod]
	void withGenericMethodsWithDifferentSignaturesAndSameErasure() {
		string code = "interface Action<T> {};\n"
				+ "interface X { <T>   T execute(Action<T> a); }\n"
				+ "interface Y { <S,T> S execute(Action<S> a); }\n"
				+ "interface Exec:X, Y {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Exec");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());

	}

	/*
	 * Functional interfaces can be generic, such as
	 * java.util.function.Predicate<T>. Such a functional interface may be
	 * parameterized _in a way that produces distinct abstract methods - that is,
	 * multiple methods that cannot be legally overridden with a single declaration.
	 */
	[TestMethod]
	@Disabled("Waiting Return-Type-Substituable is fully implemented on reference type.")
	void genericFunctionalInterfacesWithReturnTypeSubstituable() {
		string code = "interface I    { Object m(Class c); }\r\n"
				+ "interface J<S> { S m(Class<?> c); }\r\n"
				+ "interface K<T> { T m(Class<?> c); }\r\n"
				+ "interface Functional<S,T>:I, J<S>, K<T> {}";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Functional");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());

	}

	[TestMethod]
	@Disabled("Waiting Return-Type-Substituable is fully implemented on reference type.")
	void genericFunctionalInterfacesWithGenericParameter() {
		string code =
				"    public interface Foo<T>:java.util.function.Function<String, T> {\n" +
                "        @Override\n" +
                "        T apply(string c);\n" +
                "    }\n";

		CompilationUnit cu = StaticJavaParser.parse(code);
		ClassOrInterfaceDeclaration classOrInterfaceDecl = Navigator.demandInterface(cu, "Foo");
		ResolvedInterfaceDeclaration resolvedDecl = new JavaParserInterfaceDeclaration(classOrInterfaceDecl,
				typeSolver);
		Optional<MethodUsage> methodUsage = FunctionalInterfaceLogic.getFunctionalMethod(resolvedDecl);
		assertTrue(methodUsage.isPresent());

	}

}
