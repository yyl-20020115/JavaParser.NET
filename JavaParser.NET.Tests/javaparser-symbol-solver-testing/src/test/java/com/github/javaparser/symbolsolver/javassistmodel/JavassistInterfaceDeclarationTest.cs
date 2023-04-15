/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver.javassistmodel;




class JavassistInterfaceDeclarationTest:AbstractSymbolResolutionTest {

    private TypeSolver typeSolver;

    private TypeSolver anotherTypeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-3.0.0-alpha.2.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());

        Path anotherPathToJar = adaptPath("src/test/resources/test-artifact-1.0.0.jar");
        anotherTypeSolver = new CombinedTypeSolver(new JarTypeSolver(anotherPathToJar), new ReflectionTypeSolver());
    }

    ///
    /// Test misc
    ///

    [TestMethod]
    void testIsClass() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(false, nodeWithAnnotations.isClass());
    }

    [TestMethod]
    void testIsInterface() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(true, nodeWithAnnotations.isInterface());
    }

    [TestMethod]
    void testIsEnum() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(false, nodeWithAnnotations.isEnum());
    }

    [TestMethod]
    void testIsTypeVariable() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(false, nodeWithAnnotations.isTypeParameter());
    }

    [TestMethod]
    void testIsType() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(true, nodeWithAnnotations.isType());
    }

    [TestMethod]
    void testAsType() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(nodeWithAnnotations, nodeWithAnnotations.asType());
    }

    [TestMethod]
    void testAsClass() {
        assertThrows(UnsupportedOperationException.class, () -> {
            JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        nodeWithAnnotations.asClass();
    });
}

    [TestMethod]
    void testAsInterface() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals(nodeWithAnnotations, nodeWithAnnotations.asInterface());
    }

    [TestMethod]
    void testAsEnum() {
        assertThrows(UnsupportedOperationException.class, () -> {
            JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        nodeWithAnnotations.asEnum();
    });
}

    [TestMethod]
    void testGetPackageName() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals("com.github.javaparser.ast.nodeTypes", nodeWithAnnotations.getPackageName());
    }

    [TestMethod]
    void testGetClassName() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals("NodeWithAnnotations", nodeWithAnnotations.getClassName());
    }

    [TestMethod]
    void testGetQualifiedName() {
        JavassistInterfaceDeclaration nodeWithAnnotations = (JavassistInterfaceDeclaration) typeSolver.solveType("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations");
        assertEquals("com.github.javaparser.ast.nodeTypes.NodeWithAnnotations", nodeWithAnnotations.getQualifiedName());
    }

    [TestMethod]
    void testHasDirectlyAnnotation(){
        JavassistInterfaceDeclaration compilationUnit = (JavassistInterfaceDeclaration) anotherTypeSolver.solveType("com.github.javaparser.test.TestInterface");
        assertTrue(compilationUnit.hasDirectlyAnnotation("com.github.javaparser.test.TestAnnotation"));
    }

    [TestMethod]
    void testHasAnnotation(){
        JavassistInterfaceDeclaration compilationUnit = (JavassistInterfaceDeclaration) anotherTypeSolver.solveType("com.github.javaparser.test.TestChildInterface");
        assertTrue(compilationUnit.hasAnnotation("com.github.javaparser.test.TestAnnotation"));
    }

    @Nested
    class TestIsAssignableBy {

        private static /*final*/string CLASS_TO_SOLVE = "com.github.javaparser.ast.nodeTypes.NodeWithImplements";

        [TestMethod]
        void whenNullTypeIsProvided() {
            JavassistInterfaceDeclaration nodeWithImplements = (JavassistInterfaceDeclaration) typeSolver.solveType(CLASS_TO_SOLVE);
            assertTrue(nodeWithImplements.isAssignableBy(NullType.INSTANCE));
        }

        [TestMethod]
        void whenLambdaArgumentTypePlaceholderIsProvided() {
            JavassistInterfaceDeclaration nodeWithImplements = (JavassistInterfaceDeclaration) typeSolver.solveType(CLASS_TO_SOLVE);
            assertFalse(nodeWithImplements.isAssignableBy(new LambdaArgumentTypePlaceholder(0)));
        }

        [TestMethod]
        void whenEqualTypeIsProvided() {
            JavassistInterfaceDeclaration nodeWithImplements = (JavassistInterfaceDeclaration) typeSolver.solveType(CLASS_TO_SOLVE);
            assertTrue(nodeWithImplements.isAssignableBy(nodeWithImplements));
        }

        [TestMethod]
        void whenOtherTypeIsProvided() {
            ResolvedReferenceTypeDeclaration consumer = new ReflectionTypeSolver().solveType(Consumer.class.getCanonicalName());
            JavassistInterfaceDeclaration nodeWithImplements = (JavassistInterfaceDeclaration) typeSolver.solveType(CLASS_TO_SOLVE);
            assertFalse(nodeWithImplements.isAssignableBy(consumer));
        }

        [TestMethod]
        void whenSameClassButWithDifferentTypeParametersIsProvided() {
            ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();

            ReferenceTypeImpl javaLangObject = new ReferenceTypeImpl(reflectionTypeSolver.getSolvedJavaLangObject());
            ResolvedWildcard wildCard = ResolvedWildcard.extendsBound(javaLangObject);

            JavassistInterfaceDeclaration nodeWithImplements = (JavassistInterfaceDeclaration) typeSolver.solveType(CLASS_TO_SOLVE);
            ResolvedType typeA = new ReferenceTypeImpl(nodeWithImplements, Collections.singletonList(wildCard));
            ResolvedType typeB = new ReferenceTypeImpl(nodeWithImplements, Collections.singletonList(javaLangObject));

            assertFalse(typeB.isAssignableBy(typeA), "This should not be allowed:" +
                    " NodeWithImplements<Object> node = new NodeWithImplements<?:Object>()");
            assertTrue(typeA.isAssignableBy(typeB), "This should be allowed:" +
                    " NodeWithImplements<?:Object> node = new NodeWithImplements<Object>()");
        }

        [TestMethod]
        void whenInterfaceIsProvided() {
            MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();
            CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver(
                    memoryTypeSolver,
                    new ReflectionTypeSolver()
            );

            ClassPool classPool = new ClassPool();
            CtClass interfaceA = classPool.makeInterface("A");
            CtClass interfaceB = classPool.makeInterface("B", interfaceA);

            JavassistInterfaceDeclaration declarationA = new JavassistInterfaceDeclaration(interfaceA, combinedTypeSolver);
            JavassistInterfaceDeclaration declarationB = new JavassistInterfaceDeclaration(interfaceB, combinedTypeSolver);
            memoryTypeSolver.addDeclaration("A", declarationA);
            memoryTypeSolver.addDeclaration("B", declarationB);

            // Knowing that B:A we expect:
            assertFalse(declarationA.isAssignableBy(declarationB), "This should not be allowed: B variable = new A()");
            assertTrue(declarationB.isAssignableBy(declarationA), "This should be allowed: A variable = new B()");
        }
    }

    ///
    /// Test ancestors
    ///

    [TestMethod]
    void testGetAncestorsWithGenericAncestors() {
        JavassistInterfaceDeclaration compilationUnit = (JavassistInterfaceDeclaration) anotherTypeSolver.solveType("com.github.javaparser.test.GenericChildInterface");
        List<ResolvedReferenceType> ancestors = compilationUnit.getAncestors();
        ancestors.sort(new Comparator<ResolvedReferenceType>() {
            @Override
            public int compare(ResolvedReferenceType o1, ResolvedReferenceType o2) {
                return o1.describe().compareTo(o2.describe());
            }
        });
        assertEquals(2, ancestors.size());
        assertEquals("com.github.javaparser.test.GenericInterface<S>", ancestors.get(0).describe()); // Type should be 'S', from the GenericChildInterface
        assertEquals("java.lang.Object", ancestors.get(1).describe());

        // check the ancestor generic type is mapped to the type of the child
        List<Pair<ResolvedTypeParameterDeclaration, ResolvedType>> typePamatersMap = ancestors.get(0).getTypeParametersMap();
        assertEquals(1, typePamatersMap.size());

        ResolvedTypeParameterDeclaration genericTypeParameterDeclaration = typePamatersMap.get(0).a;
        assertEquals("com.github.javaparser.test.GenericInterface.T", genericTypeParameterDeclaration.getQualifiedName());
        ResolvedType genericResolvedType = typePamatersMap.get(0).b;
        assertEquals("com.github.javaparser.test.GenericChildInterface.S", genericResolvedType.asTypeParameter().getQualifiedName());
    }

}
