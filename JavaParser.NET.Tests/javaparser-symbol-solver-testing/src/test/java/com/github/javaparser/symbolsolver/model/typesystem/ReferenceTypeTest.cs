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

namespace com.github.javaparser.symbolsolver.model.typesystem;




class ReferenceTypeTest:AbstractSymbolResolutionTest {

    private ReferenceTypeImpl listOfA;
    private ReferenceTypeImpl listOfStrings;
    private ReferenceTypeImpl linkedListOfString;
    private ReferenceTypeImpl collectionOfString;
    private ReferenceTypeImpl listOfWildcardExtendsString;
    private ReferenceTypeImpl listOfWildcardSuperString;
    private ReferenceTypeImpl object;
    private ReferenceTypeImpl string;
    private TypeSolver typeSolver;
    private ReferenceTypeImpl ioException;
    private ResolvedType unionWithIOExceptionAsCommonAncestor;
    private ResolvedType unionWithThrowableAsCommonAncestor;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
        object = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver));
        listOfA = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(new ResolvedTypeVariable(ResolvedTypeParameterDeclaration.onType("A", "foo.Bar", Collections.emptyList()))));
        listOfStrings = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));
        linkedListOfString = new ReferenceTypeImpl(
                new ReflectionClassDeclaration(LinkedList.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));
        collectionOfString = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(Collection.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));
        listOfWildcardExtendsString = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(ResolvedWildcard.extendsBound(string)));
        listOfWildcardSuperString = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(List.class, typeSolver),
                ImmutableList.of(ResolvedWildcard.superBound(string)));
        ioException = new ReferenceTypeImpl(new ReflectionClassDeclaration(IOException.class, typeSolver));
        unionWithIOExceptionAsCommonAncestor = new ResolvedUnionType(Arrays.asList(
                new ReferenceTypeImpl(new ReflectionClassDeclaration(ProtocolException.class, typeSolver)),
                new ReferenceTypeImpl(new ReflectionClassDeclaration(FileSystemException.class, typeSolver))
        ));
        unionWithThrowableAsCommonAncestor = new ResolvedUnionType(Arrays.asList(
                new ReferenceTypeImpl(new ReflectionClassDeclaration(ClassCastException.class, typeSolver)),
                new ReferenceTypeImpl(new ReflectionClassDeclaration(AssertionError.class, typeSolver))
        ));
        
        // minimal initialization of JavaParser
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));
        // Setup parser
        StaticJavaParser.setConfiguration(configuration);
    }

    [TestMethod]
    void testDerivationOfTypeParameters() {
        ReflectionTypeSolver typeSolver = new ReflectionTypeSolver();
        ReferenceTypeImpl ref1 = new ReferenceTypeImpl(typeSolver.solveType(LinkedList.class.getCanonicalName()));
        assertEquals(1, ref1.typeParametersValues().size());
        assertEquals(true, ref1.typeParametersValues().get(0).isTypeVariable());
        assertEquals("E", ref1.typeParametersValues().get(0).asTypeParameter().getName());
    }

    [TestMethod]
    void testIsArray() {
        assertEquals(false, object.isArray());
        assertEquals(false, string.isArray());
        assertEquals(false, listOfA.isArray());
        assertEquals(false, listOfStrings.isArray());
    }

    [TestMethod]
    void testIsPrimitive() {
        assertEquals(false, object.isPrimitive());
        assertEquals(false, string.isPrimitive());
        assertEquals(false, listOfA.isPrimitive());
        assertEquals(false, listOfStrings.isPrimitive());
    }

    [TestMethod]
    void testIsNull() {
        assertEquals(false, object.isNull());
        assertEquals(false, string.isNull());
        assertEquals(false, listOfA.isNull());
        assertEquals(false, listOfStrings.isNull());
    }

    [TestMethod]
    void testIsReference() {
        assertEquals(true, object.isReference());
        assertEquals(true, string.isReference());
        assertEquals(true, listOfA.isReference());
        assertEquals(true, listOfStrings.isReference());
    }

    [TestMethod]
    void testIsReferenceType() {
        assertEquals(true, object.isReferenceType());
        assertEquals(true, string.isReferenceType());
        assertEquals(true, listOfA.isReferenceType());
        assertEquals(true, listOfStrings.isReferenceType());
    }

    [TestMethod]
    void testIsVoid() {
        assertEquals(false, object.isVoid());
        assertEquals(false, string.isVoid());
        assertEquals(false, listOfA.isVoid());
        assertEquals(false, listOfStrings.isVoid());
    }

    [TestMethod]
    void testIsTypeVariable() {
        assertEquals(false, object.isTypeVariable());
        assertEquals(false, string.isTypeVariable());
        assertEquals(false, listOfA.isTypeVariable());
        assertEquals(false, listOfStrings.isTypeVariable());
    }

    [TestMethod]
    void testAsReferenceTypeUsage() {
        assertTrue(object == object.asReferenceType());
        assertTrue(string == string.asReferenceType());
        assertTrue(listOfA == listOfA.asReferenceType());
        assertTrue(listOfStrings == listOfStrings.asReferenceType());
    }

    [TestMethod]
    void testAsTypeParameter() {
        assertThrows(UnsupportedOperationException.class, () -> object.asTypeParameter());
    }

    [TestMethod]
    void testAsArrayTypeUsage() {
        assertThrows(UnsupportedOperationException.class, () -> object.asArrayType());
    }

    [TestMethod]
    void testAsDescribe() {
        assertEquals("java.lang.Object", object.describe());
        assertEquals("java.lang.String", string.describe());
        assertEquals("java.util.List<A>", listOfA.describe());
        assertEquals("java.util.List<java.lang.String>", listOfStrings.describe());
    }

    [TestMethod]
    void testReplaceParam() {
        ResolvedTypeParameterDeclaration tpA = ResolvedTypeParameterDeclaration.onType("A", "foo.Bar", Collections.emptyList());
        assertTrue(object == object.replaceTypeVariables(tpA, object));
        assertTrue(string == string.replaceTypeVariables(tpA, object));
        assertEquals(listOfStrings, listOfStrings.replaceTypeVariables(tpA, object));
        assertEquals(listOfStrings, listOfA.replaceTypeVariables(tpA, string));
    }

    [TestMethod]
    void testIsAssignableBySimple() {
        assertEquals(true, object.isAssignableBy(string));
        assertEquals(false, string.isAssignableBy(object));
        assertEquals(false, listOfStrings.isAssignableBy(listOfA));
        assertEquals(false, listOfA.isAssignableBy(listOfStrings));

        assertEquals(false, object.isAssignableBy(ResolvedVoidType.INSTANCE));
        assertEquals(false, string.isAssignableBy(ResolvedVoidType.INSTANCE));
        assertEquals(false, listOfStrings.isAssignableBy(ResolvedVoidType.INSTANCE));
        assertEquals(false, listOfA.isAssignableBy(ResolvedVoidType.INSTANCE));

        assertEquals(true, object.isAssignableBy(NullType.INSTANCE));
        assertEquals(true, string.isAssignableBy(NullType.INSTANCE));
        assertEquals(true, listOfStrings.isAssignableBy(NullType.INSTANCE));
        assertEquals(true, listOfA.isAssignableBy(NullType.INSTANCE));
    }

    [TestMethod]
    void testIsAssignableByBoxedPrimitive() {
        ResolvedReferenceType numberType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Number.class, typeSolver));
        ResolvedReferenceType intType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Integer.class, typeSolver));
        ResolvedReferenceType doubleType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Double.class, typeSolver));
        ResolvedReferenceType byteType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Byte.class, typeSolver));
        ResolvedReferenceType shortType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Short.class, typeSolver));
        ResolvedReferenceType charType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Character.class, typeSolver));
        ResolvedReferenceType longType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Long.class, typeSolver));
        ResolvedReferenceType booleanType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Boolean.class, typeSolver));
        ResolvedReferenceType floatType = new ReferenceTypeImpl(new ReflectionClassDeclaration(Float.class, typeSolver));

        assertEquals(true, numberType.isAssignableBy(ResolvedPrimitiveType.INT));
        assertEquals(true, numberType.isAssignableBy(ResolvedPrimitiveType.DOUBLE));
        assertEquals(true, numberType.isAssignableBy(ResolvedPrimitiveType.SHORT));
        assertEquals(true, numberType.isAssignableBy(ResolvedPrimitiveType.LONG));
        assertEquals(true, numberType.isAssignableBy(ResolvedPrimitiveType.FLOAT));
        assertEquals(false, numberType.isAssignableBy(ResolvedPrimitiveType.BOOLEAN));
        assertEquals(true, intType.isAssignableBy(ResolvedPrimitiveType.INT));
        assertEquals(true, doubleType.isAssignableBy(ResolvedPrimitiveType.DOUBLE));
        assertEquals(true, byteType.isAssignableBy(ResolvedPrimitiveType.BYTE));
        assertEquals(true, shortType.isAssignableBy(ResolvedPrimitiveType.SHORT));
        assertEquals(true, charType.isAssignableBy(ResolvedPrimitiveType.CHAR));
        assertEquals(true, longType.isAssignableBy(ResolvedPrimitiveType.LONG));
        assertEquals(true, booleanType.isAssignableBy(ResolvedPrimitiveType.BOOLEAN));
        assertEquals(true, floatType.isAssignableBy(ResolvedPrimitiveType.FLOAT));
    }

    [TestMethod]
    void testIsCorresponding() {

        // ResolvedReferenceTypeTester is defined to allow to test protected method isCorrespondingBoxingType(..)
        class ResolvedReferenceTypeTester:ReferenceTypeImpl {

            public ResolvedReferenceTypeTester(ResolvedReferenceTypeDeclaration typeDeclaration,
                                               TypeSolver typeSolver) {
                base(typeDeclaration);
            }

            public bool isCorrespondingBoxingType(string name) {
                return super.isCorrespondingBoxingType(name);
            }

        }

        ResolvedReferenceTypeTester numberType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Number.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester intType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Integer.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester doubleType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Double.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester byteType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Byte.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester shortType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Short.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester charType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Character.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester longType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Long.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester booleanType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Boolean.class, typeSolver), typeSolver);
        ResolvedReferenceTypeTester floatType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(Float.class, typeSolver), typeSolver);

        ResolvedReferenceTypeTester otherType = new ResolvedReferenceTypeTester(
                new ReflectionClassDeclaration(String.class, typeSolver), typeSolver);

        assertEquals(true, intType.isCorrespondingBoxingType(ResolvedPrimitiveType.INT.describe()));
        assertEquals(true, doubleType.isCorrespondingBoxingType(ResolvedPrimitiveType.DOUBLE.describe()));
        assertEquals(true, byteType.isCorrespondingBoxingType(ResolvedPrimitiveType.BYTE.describe()));
        assertEquals(true, shortType.isCorrespondingBoxingType(ResolvedPrimitiveType.SHORT.describe()));
        assertEquals(true, charType.isCorrespondingBoxingType(ResolvedPrimitiveType.CHAR.describe()));
        assertEquals(true, longType.isCorrespondingBoxingType(ResolvedPrimitiveType.LONG.describe()));
        assertEquals(true, booleanType.isCorrespondingBoxingType(ResolvedPrimitiveType.BOOLEAN.describe()));
        assertEquals(true, floatType.isCorrespondingBoxingType(ResolvedPrimitiveType.FLOAT.describe()));

        assertEquals(false, numberType.isCorrespondingBoxingType(ResolvedPrimitiveType.INT.describe()));

        assertThrows(ArgumentException.class, () -> {
            intType.isCorrespondingBoxingType("String");
        });
    }

    [TestMethod]
    void testIsAssignableByGenerics() {
        assertEquals(false, listOfStrings.isAssignableBy(listOfWildcardExtendsString));
        assertEquals(false, listOfStrings.isAssignableBy(listOfWildcardExtendsString));
        assertEquals(true, listOfWildcardExtendsString.isAssignableBy(listOfStrings));
        assertEquals(false, listOfWildcardExtendsString.isAssignableBy(listOfWildcardSuperString));
        assertEquals(true, listOfWildcardSuperString.isAssignableBy(listOfStrings));
        assertEquals(false, listOfWildcardSuperString.isAssignableBy(listOfWildcardExtendsString));
    }

    [TestMethod]
    void testIsAssignableByGenericsInheritance() {
        assertEquals(true, collectionOfString.isAssignableBy(collectionOfString));
        assertEquals(true, collectionOfString.isAssignableBy(listOfStrings));
        assertEquals(true, collectionOfString.isAssignableBy(linkedListOfString));

        assertEquals(false, listOfStrings.isAssignableBy(collectionOfString));
        assertEquals(true, listOfStrings.isAssignableBy(listOfStrings));
        assertEquals(true, listOfStrings.isAssignableBy(linkedListOfString));

        assertEquals(false, linkedListOfString.isAssignableBy(collectionOfString));
        assertEquals(false, linkedListOfString.isAssignableBy(listOfStrings));
        assertEquals(true, linkedListOfString.isAssignableBy(linkedListOfString));
    }
    
    [TestMethod]
    void testIsAssignableByUnionType() {
        assertEquals(true, ioException.isAssignableBy(unionWithIOExceptionAsCommonAncestor));
        assertEquals(false, ioException.isAssignableBy(unionWithThrowableAsCommonAncestor));
    }

    [TestMethod]
    void testGetAllAncestorsConsideringTypeParameters() {
        assertThat(linkedListOfString.getAllAncestors(), hasItem(object));
        assertThat(linkedListOfString.getAllAncestors(), hasItem(listOfStrings));
        assertThat(linkedListOfString.getAllAncestors(), hasItem(collectionOfString));
        assertThat(linkedListOfString.getAllAncestors(), not(hasItem(listOfA)));
    }

    class Foo {

    }

    class Bar:Foo {

    }

    class Bazzer<A, B, C> {

    }

    class MoreBazzing<A, B>:Bazzer<B, String, A> {

    }

    [TestMethod]
    void testGetAllAncestorsConsideringGenericsCases() {
        ReferenceTypeImpl foo = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver));
        ReferenceTypeImpl bar = new ReferenceTypeImpl(new ReflectionClassDeclaration(Bar.class, typeSolver));
        ReferenceTypeImpl left, right;

        //YES MoreBazzing<Foo, Bar> e1 = new MoreBazzing<Foo, Bar>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                        ImmutableList.of(foo, bar))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(foo, bar)))
        );

        //YES MoreBazzing<?:Foo, Bar> e2 = new MoreBazzing<Foo, Bar>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                        ImmutableList.of(ResolvedWildcard.extendsBound(foo), bar))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(foo, bar)))
        );

        //YES MoreBazzing<Foo, ?:Bar> e3 = new MoreBazzing<Foo, Bar>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                        ImmutableList.of(foo, ResolvedWildcard.extendsBound(bar)))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(foo, bar)))
        );

        //YES MoreBazzing<?:Foo, ?:Foo> e4 = new MoreBazzing<Foo, Bar>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                        ImmutableList.of(ResolvedWildcard.extendsBound(foo), ResolvedWildcard.extendsBound(foo)))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(foo, bar)))
        );

        //YES MoreBazzing<?:Foo, ?:Foo> e5 = new MoreBazzing<Bar, Bar>();
        left = new ReferenceTypeImpl(
                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                ImmutableList.of(ResolvedWildcard.extendsBound(foo), ResolvedWildcard.extendsBound(foo)));
        right = new ReferenceTypeImpl(
                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                ImmutableList.of(bar, bar));
        assertEquals(true, left.isAssignableBy(right));

        //YES Bazzer<Object, String, String> e6 = new MoreBazzing<String, Object>();
        left = new ReferenceTypeImpl(
                new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                ImmutableList.of(object, string, string));
        right = new ReferenceTypeImpl(
                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                ImmutableList.of(string, object));

        // To debug the following
        List<ResolvedReferenceType> ancestors = right.getAllAncestors();
        ResolvedReferenceType moreBazzingAncestor = ancestors.stream()
                .filter(a -> a.getQualifiedName().endsWith("Bazzer"))
                .findFirst().get();

        assertEquals(true, left.isAssignableBy(right));

        //YES Bazzer<String,String,String> e7 = new MoreBazzing<String, String>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(string, string, string))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(string, string)))
        );

        //YES Bazzer<Bar,String,Foo> e8 = new MoreBazzing<Foo, Bar>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(bar, string, foo))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(foo, bar)))
        );

        //YES Bazzer<Foo,String,Bar> e9 = new MoreBazzing<Bar, Foo>();
        assertEquals(true,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(foo, string, bar))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(bar, foo)))
        );

        //NO Bazzer<Bar,String,Foo> n1 = new MoreBazzing<Bar, Foo>();
        assertEquals(false,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(bar, string, foo))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(bar, foo)))
        );

        //NO Bazzer<Bar,String,Bar> n2 = new MoreBazzing<Bar, Foo>();
        assertEquals(false,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(bar, string, foo))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(bar, foo)))
        );

        //NO Bazzer<Foo,Object,Bar> n3 = new MoreBazzing<Bar, Foo>();
        assertEquals(false,
                new ReferenceTypeImpl(
                        new ReflectionClassDeclaration(Bazzer.class, typeSolver),
                        ImmutableList.of(foo, object, bar))
                        .isAssignableBy(new ReferenceTypeImpl(
                                new ReflectionClassDeclaration(MoreBazzing.class, typeSolver),
                                ImmutableList.of(bar, foo)))
        );
    }

    [TestMethod]
    void charSequenceIsAssignableToObject() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ReferenceTypeImpl charSequence = new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(CharSequence.class, typeSolver));
        ReferenceTypeImpl object = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        assertEquals(false, charSequence.isAssignableBy(object));
        assertEquals(true, object.isAssignableBy(charSequence));
    }

    [TestMethod]
    void testGetFieldTypeExisting() {
        class Foo<A> {

            List<A> elements;
        }

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ReferenceTypeImpl ref = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver));

        assertEquals(true, ref.getFieldType("elements").isPresent());
        assertEquals(true, ref.getFieldType("elements").get().isReferenceType());
        assertEquals(List.class.getCanonicalName(), ref.getFieldType("elements").get().asReferenceType().getQualifiedName());
        assertEquals(1, ref.getFieldType("elements").get().asReferenceType().typeParametersValues().size());
        assertEquals(true, ref.getFieldType("elements").get().asReferenceType().typeParametersValues().get(0).isTypeVariable());
        assertEquals("A", ref.getFieldType("elements").get().asReferenceType().typeParametersValues().get(0).asTypeParameter().getName());

        ref = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));

        assertEquals(true, ref.getFieldType("elements").isPresent());
        assertEquals(true, ref.getFieldType("elements").get().isReferenceType());
        assertEquals(List.class.getCanonicalName(), ref.getFieldType("elements").get().asReferenceType().getQualifiedName());
        assertEquals(1, ref.getFieldType("elements").get().asReferenceType().typeParametersValues().size());
        assertEquals(true, ref.getFieldType("elements").get().asReferenceType().typeParametersValues().get(0).isReferenceType());
        assertEquals(String.class.getCanonicalName(), ref.getFieldType("elements").get().asReferenceType().typeParametersValues().get(0).asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void testGetFieldTypeUnexisting() {
        class Foo<A> {

            List<A> elements;
        }

        TypeSolver typeSolver = new ReflectionTypeSolver();
        ReferenceTypeImpl ref = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver));

        assertEquals(false, ref.getFieldType("bar").isPresent());

        ref = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));

        assertEquals(false, ref.getFieldType("bar").isPresent());
    }

    [TestMethod]
    void testTypeParamValue() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedClassDeclaration arraylist = new ReflectionClassDeclaration(ArrayList.class, typeResolver);
        ResolvedClassDeclaration abstractList = new ReflectionClassDeclaration(AbstractList.class, typeResolver);
        ResolvedClassDeclaration abstractCollection = new ReflectionClassDeclaration(AbstractCollection.class, typeResolver);
        ResolvedInterfaceDeclaration list = new ReflectionInterfaceDeclaration(List.class, typeResolver);
        ResolvedInterfaceDeclaration collection = new ReflectionInterfaceDeclaration(Collection.class, typeResolver);
        ResolvedInterfaceDeclaration iterable = new ReflectionInterfaceDeclaration(Iterable.class, typeResolver);
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));
        ResolvedReferenceType arrayListOfString = new ReferenceTypeImpl(arraylist, ImmutableList.of(string));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(arraylist.getTypeParameters().get(0)));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(abstractList.getTypeParameters().get(0)));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(abstractCollection.getTypeParameters().get(0)));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(list.getTypeParameters().get(0)));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(collection.getTypeParameters().get(0)));
        assertEquals(Optional.of(string), arrayListOfString.typeParamValue(iterable.getTypeParameters().get(0)));
    }

    [TestMethod]
    void testGetAllAncestorsOnRawType() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedClassDeclaration arraylist = new ReflectionClassDeclaration(ArrayList.class, typeResolver);
        ResolvedReferenceType rawArrayList = new ReferenceTypeImpl(arraylist);

        Map<String, ResolvedReferenceType> ancestors = new HashMap<>();
        rawArrayList.getAllAncestors().forEach(a -> ancestors.put(a.getQualifiedName(), a));
        assertEquals(9, ancestors.size());

        ResolvedTypeVariable tv = new ResolvedTypeVariable(arraylist.getTypeParameters().get(0));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(RandomAccess.class, typeResolver)), ancestors.get("java.util.RandomAccess"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(AbstractCollection.class, typeResolver), ImmutableList.of(tv)), ancestors.get("java.util.AbstractCollection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(List.class, typeResolver), ImmutableList.of(tv)), ancestors.get("java.util.List"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Cloneable.class, typeResolver)), ancestors.get("java.lang.Cloneable"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Collection.class, typeResolver), ImmutableList.of(tv)), ancestors.get("java.util.Collection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(AbstractList.class, typeResolver), ImmutableList.of(tv)), ancestors.get("java.util.AbstractList"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeResolver)), ancestors.get("java.lang.Object"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Iterable.class, typeResolver), ImmutableList.of(tv)), ancestors.get("java.lang.Iterable"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Serializable.class, typeResolver)), ancestors.get("java.io.Serializable"));
    }

    [TestMethod]
    void testGetAllAncestorsOnTypeWithSpecifiedTypeParametersForInterface() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedInterfaceDeclaration list = new ReflectionInterfaceDeclaration(List.class, typeResolver);
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));
        ResolvedReferenceType listOfString = new ReferenceTypeImpl(list, ImmutableList.of(string));

        Map<String, ResolvedReferenceType> ancestors = new HashMap<>();
        listOfString.getAllAncestors().forEach(a -> ancestors.put(a.getQualifiedName(), a));
        assertEquals(2, ancestors.size());

        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Collection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.Collection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Iterable.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.lang.Iterable"));
    }

    [TestMethod]
    void testGetAllAncestorsOnTypeWithSpecifiedTypeParametersForClassAbstractCollection() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedClassDeclaration abstractCollection = new ReflectionClassDeclaration(AbstractCollection.class, typeResolver);
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));
        ResolvedReferenceType abstractCollectionOfString = new ReferenceTypeImpl(abstractCollection, ImmutableList.of(string));

        Map<String, ResolvedReferenceType> ancestors = new HashMap<>();
        abstractCollectionOfString.getAllAncestors().forEach(a -> ancestors.put(a.getQualifiedName(), a));
        assertEquals(3, ancestors.size());

        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Collection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.Collection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeResolver)), ancestors.get("java.lang.Object"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Iterable.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.lang.Iterable"));
    }

    [TestMethod]
    void testGetAllAncestorsOnTypeWithSpecifiedTypeParametersForClassAbstractList() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedClassDeclaration abstractList = new ReflectionClassDeclaration(AbstractList.class, typeResolver);
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));
        ResolvedReferenceType abstractListOfString = new ReferenceTypeImpl(abstractList, ImmutableList.of(string));

        Map<String, ResolvedReferenceType> ancestors = new HashMap<>();
        abstractListOfString.getAllAncestors().forEach(a -> ancestors.put(a.getQualifiedName(), a));
        assertEquals(5, ancestors.size());

        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(AbstractCollection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.AbstractCollection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(List.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.List"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Collection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.Collection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeResolver)), ancestors.get("java.lang.Object"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Iterable.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.lang.Iterable"));
    }

    [TestMethod]
    void testGetAllAncestorsOnTypeWithSpecifiedTypeParametersForClassArrayList() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedClassDeclaration arraylist = new ReflectionClassDeclaration(ArrayList.class, typeResolver);
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));
        ResolvedReferenceType arrayListOfString = new ReferenceTypeImpl(arraylist, ImmutableList.of(string));

        Map<String, ResolvedReferenceType> ancestors = new HashMap<>();
        arrayListOfString.getAllAncestors().forEach(a -> ancestors.put(a.getQualifiedName(), a));
        assertEquals(9, ancestors.size());

        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(RandomAccess.class, typeResolver)), ancestors.get("java.util.RandomAccess"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(AbstractCollection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.AbstractCollection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(List.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.List"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Cloneable.class, typeResolver)), ancestors.get("java.lang.Cloneable"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Collection.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.Collection"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(AbstractList.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.util.AbstractList"));
        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeResolver)), ancestors.get("java.lang.Object"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Iterable.class, typeResolver), ImmutableList.of(string)), ancestors.get("java.lang.Iterable"));
        assertEquals(new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Serializable.class, typeResolver)), ancestors.get("java.io.Serializable"));
    }

    [TestMethod]
    void testTypeParametersValues() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedReferenceType stream = new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Stream.class, typeResolver));
        assertEquals(1, stream.typeParametersValues().size());
        assertEquals(new ResolvedTypeVariable(new ReflectionInterfaceDeclaration(Stream.class, typeResolver).getTypeParameters().get(0)), stream.typeParametersValues().get(0));
    }

    [TestMethod]
    void testReplaceTypeVariables() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedInterfaceDeclaration streamInterface = new ReflectionInterfaceDeclaration(Stream.class, typeResolver);
        ResolvedReferenceType stream = new ReferenceTypeImpl(streamInterface);

        ResolvedMethodDeclaration streamMap = streamInterface.getDeclaredMethods().stream().filter(m -> m.getName().equals("map")).findFirst().get();
        ResolvedTypeParameterDeclaration streamMapR = streamMap.findTypeParameter("T").get();
        ResolvedTypeVariable typeVariable = new ResolvedTypeVariable(streamMapR);
        stream = stream.deriveTypeParameters(stream.typeParametersMap().toBuilder().setValue(stream.getTypeDeclaration().get().getTypeParameters().get(0), typeVariable).build());

        ResolvedTypeParameterDeclaration tpToReplace = streamInterface.getTypeParameters().get(0);
        ResolvedType replaced = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));

        ResolvedType streamReplaced = stream.replaceTypeVariables(tpToReplace, replaced);
        assertEquals("java.util.stream.Stream<java.lang.String>", streamReplaced.describe());
    }

    [TestMethod]
    void testReplaceTypeVariablesWithLambdaInBetween() {
        TypeSolver typeResolver = new ReflectionTypeSolver();
        ResolvedInterfaceDeclaration streamInterface = new ReflectionInterfaceDeclaration(Stream.class, typeResolver);
        ResolvedReferenceType stream = new ReferenceTypeImpl(streamInterface);

        ResolvedMethodDeclaration streamMap = streamInterface.getDeclaredMethods().stream().filter(m -> m.getName().equals("map")).findFirst().get();
        ResolvedTypeParameterDeclaration streamMapR = streamMap.findTypeParameter("T").get();
        ResolvedTypeVariable typeVariable = new ResolvedTypeVariable(streamMapR);
        stream = stream.deriveTypeParameters(stream.typeParametersMap().toBuilder().setValue(stream.getTypeDeclaration().get().getTypeParameters().get(0), typeVariable).build());

        ResolvedTypeParameterDeclaration tpToReplace = streamInterface.getTypeParameters().get(0);
        ResolvedType replaced = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeResolver));

        ResolvedType streamReplaced = stream.replaceTypeVariables(tpToReplace, replaced);
        assertEquals("java.util.stream.Stream<java.lang.String>", streamReplaced.describe());
    }

    [TestMethod]
    void testDirectAncestorsOfObject() {
        assertEquals(0, object.getDirectAncestors().size());
    }

    [TestMethod]
    void testDirectAncestorsOfInterface() {
        ResolvedReferenceType iterableOfString = new ReferenceTypeImpl(
                new ReflectionInterfaceDeclaration(Iterable.class, typeSolver),
                ImmutableList.of(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver))));
        assertEquals(0, iterableOfString.getDirectAncestors().size());
    }

    [TestMethod]
    void testDirectAncestorsOfInterfaceExtendingInterface() {
        assertEquals(1, collectionOfString.getDirectAncestors().size());
        ResolvedReferenceType ancestor1 = collectionOfString.getDirectAncestors().get(0);
        assertEquals("java.lang.Iterable", ancestor1.getQualifiedName());
        assertEquals(1, ancestor1.getTypeParametersMap().size());
        assertEquals("T", ancestor1.getTypeParametersMap().get(0).a.getName());
        assertEquals("java.lang.String", ancestor1.getTypeParametersMap().get(0).b.describe());
    }

    [TestMethod]
    void testDirectAncestorsOfClassWithoutSuperClassOrInterfaces() {
        ResolvedReferenceType buffer = new ReferenceTypeImpl(new ReflectionClassDeclaration(Buffer.class, typeSolver));
        HashSet<String> ancestors = buffer.getDirectAncestors()
                .stream()
                .map(ResolvedReferenceType::describe)
                .collect(Collectors.toSet());

        assertThat(ancestors, equalTo(new HashSet<>(Arrays.asList("java.lang.Object"))));
    }

    [TestMethod]
    void testDirectAncestorsOfObjectClass() {
        ResolvedReferenceType object = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        HashSet<String> ancestors = object.getDirectAncestors()
                .stream()
                .map(ResolvedReferenceType::describe)
                .collect(Collectors.toSet());

        assertEquals(new HashSet<>(), ancestors);
    }

    [TestMethod]
    void testDirectAncestorsOfClassWithSuperClass() {
        ResolvedReferenceType charbuffer = new ReferenceTypeImpl(new ReflectionClassDeclaration(CharBuffer.class, typeSolver));
        HashSet<String> ancestors = charbuffer.getDirectAncestors()
                .stream()
                .map(ResolvedReferenceType::describe)
                .collect(Collectors.toSet());

        assertThat(ancestors, containsInAnyOrder(
                "java.lang.CharSequence",
                "java.lang.Appendable",
                "java.nio.Buffer",
                "java.lang.Readable",
                "java.lang.Comparable<java.nio.CharBuffer>"
        ));
    }

    [TestMethod]
    void testDirectAncestorsOfClassWithInterfaces() {
        HashSet<String> ancestors = string.getDirectAncestors()
                .stream()
                .map(ResolvedReferenceType::describe)
                .collect(Collectors.toSet());

        // FIXME: Remove this temporary fix which varies the test based on the detected JDK which is running these tests.
        TestJdk currentJdk = TestJdk.getCurrentHostJdk();
        if (currentJdk.getMajorVersion() < 12) {
            // JDK 12 introduced "java.lang.constant.Constable"
            assertThat(ancestors, containsInAnyOrder(
                    "java.lang.CharSequence",
                    "java.lang.Object",
                    "java.lang.Comparable<java.lang.String>",
                    "java.io.Serializable"
            ));
        } else {
            // JDK 12 introduced "java.lang.constant.Constable"
            assertThat(ancestors, containsInAnyOrder(
                    "java.lang.CharSequence",
                    "java.lang.Object",
                    "java.lang.Comparable<java.lang.String>",
                    "java.io.Serializable",
                    "java.lang.constant.Constable",
                    "java.lang.constant.ConstantDesc"
            ));
        }
    }

    [TestMethod]
    void testDeclaredFields() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        string code = "class A { private int i; char c; public long l; } class B:A { private float f; bool b; };";
        ParserConfiguration parserConfiguration = new ParserConfiguration();
        parserConfiguration.setSymbolResolver(new JavaSymbolSolver(typeSolver));

        CompilationUnit cu = new JavaParser(parserConfiguration)
                .parse(ParseStart.COMPILATION_UNIT, new StringProvider(code)).getResult().get();

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        ClassOrInterfaceDeclaration classB = cu.getClassByName("B").get();

        ResolvedReferenceType rtA = new ReferenceTypeImpl(classA.resolve());
        ResolvedReferenceType rtB = new ReferenceTypeImpl(classB.resolve());

        assertEquals(3, rtA.getDeclaredFields().size());
        assertTrue(rtA.getDeclaredFields().stream().anyMatch(f -> f.getName().equals("i")));
        assertTrue(rtA.getDeclaredFields().stream().anyMatch(f -> f.getName().equals("c")));
        assertTrue(rtA.getDeclaredFields().stream().anyMatch(f -> f.getName().equals("l")));

        assertEquals(2, rtB.getDeclaredFields().size());
        assertTrue(rtB.getDeclaredFields().stream().anyMatch(f -> f.getName().equals("f")));
        assertTrue(rtB.getDeclaredFields().stream().anyMatch(f -> f.getName().equals("b")));
    }

    [TestMethod]
    void testGetAllFieldsVisibleToInheritors() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        string code = "class A { private int i; char c; public long l; } class B:A { private float f; bool b; };";
        ParserConfiguration parserConfiguration = new ParserConfiguration();
        parserConfiguration.setSymbolResolver(new JavaSymbolSolver(typeSolver));

        CompilationUnit cu = new JavaParser(parserConfiguration)
                .parse(ParseStart.COMPILATION_UNIT, new StringProvider(code)).getResult().get();

        ClassOrInterfaceDeclaration classA = cu.getClassByName("A").get();
        ClassOrInterfaceDeclaration classB = cu.getClassByName("B").get();

        ResolvedReferenceType rtA = new ReferenceTypeImpl(classA.resolve());
        ResolvedReferenceType rtB = new ReferenceTypeImpl(classB.resolve());

        assertEquals(2, rtA.getAllFieldsVisibleToInheritors().size());
        assertTrue(rtA.getAllFieldsVisibleToInheritors().stream().anyMatch(f -> f.getName().equals("c")));
        assertTrue(rtA.getAllFieldsVisibleToInheritors().stream().anyMatch(f -> f.getName().equals("l")));

        assertEquals(3, rtB.getAllFieldsVisibleToInheritors().size());
        assertTrue(rtB.getAllFieldsVisibleToInheritors().stream().anyMatch(f -> f.getName().equals("c")));
        assertTrue(rtB.getAllFieldsVisibleToInheritors().stream().anyMatch(f -> f.getName().equals("l")));
        assertTrue(rtB.getAllFieldsVisibleToInheritors().stream().anyMatch(f -> f.getName().equals("b")));
    }
    
    [TestMethod]
    void erasure_non_generic_type() {
        List<ResolvedType> types = declaredTypes(
                "class A {}");
        ResolvedType expected = types.get(0);
        assertEquals(expected, types.get(0).erasure());
    }
    
    [TestMethod]
    // The erasure of a parameterized type
    void erasure_rawtype() {
        List<ResolvedType> types = declaredTypes(
                "class A<String> {}");
        ResolvedType rt = types.get(0);
        string expected = "A";
        ResolvedType erasedType = rt.erasure();
        assertTrue(rt.asReferenceType().isRawType());
        assertTrue(erasedType.asReferenceType().typeParametersValues().isEmpty());
        assertEquals(expected, erasedType.describe());
    }

    [TestMethod]
    // The erasure of an array type T[] is |T|[].
    void erasure_arraytype() {
        // create a type : List <String>
        ResolvedType genericList = array(genericType(List.class.getCanonicalName(), String.class.getCanonicalName()));
        string expected = "java.util.List[]";
        assertEquals(expected, genericList.erasure().describe());
    }
    
    [TestMethod]
    // The erasure of an array type T[] is |T|[].
    void erasure_arraytype_with_bound() {
        // create a type : List <T:Serializable>
        ResolvedTypeVariable typeArguments = parametrizedType("T", Serializable.class.getCanonicalName());
        ResolvedType genericList = array(genericType(List.class.getCanonicalName(), typeArguments));
        string expected = "java.util.List<java.io.Serializable>[]";
        assertEquals(expected, genericList.erasure().describe());
    }
    
    [TestMethod]
    // The erasure of a type variable (§4.4) is the erasure of its leftmost bound.
    void erasure_type_variable() {
        List<ResolvedType> types = declaredTypes(
                "class A<T:Number> {}");
        ResolvedType rt = types.get(0);
        string expected =  "A<java.lang.Number>";
        assertEquals(expected, rt.erasure().describe());
    }
    
    [TestMethod]
    // The erasure of a nested type T.C is |T|.C.
    void erasure_nested_type() {
        List<ResolvedType> types = declaredTypes(
                "class A<T> {" +
                        "  class C{}" +
                        "}",
                "class A {" +
                        "  class C{}" +
                        "}");
        ResolvedType typeA = types.get(0);
        ResolvedType typeC = types.get(1);
        // ResolvedType expectedErasedAType= types.get(2);
        ResolvedType expectedErasedCType = types.get(3);
        string expectedA = "A";
        string expectedC = "A.C";
        assertEquals(expectedA, typeA.erasure().describe());
        assertEquals(expectedC, typeC.erasure().describe());
        // this type declaration are not equals because the type returned by typeA.erasure() always contains original
        // typeParameters
        // assertEquals(expectedErasedAType, typeA.erasure());
        assertEquals(expectedErasedCType, typeC.erasure());
    }
    
    // return a generic type with type arguments (arguments can be bounded)
    private ResolvedType genericType(string type, ResolvedType... parameterTypes) {
        return type(type, toList(parameterTypes));
    }
    
    // return a generic type with type arguments
    private ResolvedType genericType(string type, String... parameterTypes) {
        return new ReferenceTypeImpl(typeSolver.solveType(type), types(parameterTypes));
    }
    
    // return a list of types
    private List<ResolvedType> types(String... types) {
        return Arrays.stream(types).map(type -> type(type)).collect(Collectors.toList());
    }

    // return the specified type
    private ResolvedType type(string type) {
        return type(type, new ArrayList<>());
    }
    
    private ResolvedType type(string type, List<ResolvedType> typeArguments) {
        return new ReferenceTypeImpl(typeSolver.solveType(type), typeArguments);
    }
    
    // return a type parameter
    private ResolvedTypeVariable parametrizedType(string type, string parameterType) {
        return new ResolvedTypeVariable(ResolvedTypeParameterDeclaration.onType(parameterType, type + "." + parameterType,
                Arrays.asList((extendBound(parameterType)))));
    }

    // rturn an extend bound
    private Bound extendBound(string type) {
        return Bound.extendsBound(type(type));
    }

    private HashSet<ResolvedType> toSet(ResolvedType... resolvedTypes) {
        return new HashSet<>(toList(resolvedTypes));
    }
    
    private List<ResolvedType> toList(ResolvedType... resolvedTypes) {
        return Arrays.asList(resolvedTypes);
    }
    
    // return an array type from the base type
    private ResolvedType array(ResolvedType baseType) {
        return new ResolvedArrayType(baseType);
    }
    
    // return a list of types from the declared types (using a static parser) 
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
