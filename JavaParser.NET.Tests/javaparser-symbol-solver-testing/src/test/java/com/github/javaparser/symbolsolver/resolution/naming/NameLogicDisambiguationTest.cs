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

namespace com.github.javaparser.symbolsolver.resolution.naming;




class NameLogicDisambiguationTest:AbstractNameLogicTest {

    private void assertNameInCodeIsDisambiguited(string code, string name,
                                                 NameCategory syntacticClassification,
                                                 NameCategory nameCategory,
                                                 ParseStart parseStart, TypeSolver typeSolver) {
        Node nameNode = getNameInCodeTollerant(code, name, parseStart, typeSolver);
        assertEquals(syntacticClassification, NameLogic.syntacticClassificationAccordingToContext(nameNode));
        assertEquals(nameCategory, NameLogic.classifyReference(nameNode, typeSolver));
    }

    [TestMethod]
    void ambiguousNameToLocalVar() {
        assertNameInCodeIsDisambiguited("class A { void foo() {\n" +
                "SomeClass a; a.aField;" + "\n" +
                "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToLocalVarInAnnidatedBlocks() {
        assertNameInCodeIsDisambiguited("class A { void foo() {{\n" +
                        "SomeClass a; {{a.aField;}}}" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToLocalVarFromOldFor() {
        assertNameInCodeIsDisambiguited("class A { void foo() {\n" +
                        "for (SomeClass a=null;true;){ a.aField; }" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToLocalVarFromNewFor() {
        assertNameInCodeIsDisambiguited("class A { void foo() {\n" +
                        "for (SomeClass a : null){ a.aField; }" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToLocalVarFromTryWithResource() {
        assertNameInCodeIsDisambiguited("class A { void foo() {\n" +
                        "try (SomeClass a = null){ a.aField; }" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToMethodParameter() {
        assertNameInCodeIsDisambiguited("class A { void foo(SomeClass a) {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToCatchParameter() {
        assertNameInCodeIsDisambiguited("class A { void foo() {\n" +
                        "try { } catch (SomeClass a) { a.aField; }" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new ReflectionTypeSolver());
    }

    [TestMethod]
    void ambiguousNameToInstanceFieldDeclared() {
        assertNameInCodeIsDisambiguited("class A { SomeClass a; void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    [TestMethod]
    void ambiguousNameToStaticFieldDeclared() {
        assertNameInCodeIsDisambiguited("class A { static SomeClass a; void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    [TestMethod]
    void ambiguousNameToInstanceFieldInherited() {
        assertNameInCodeIsDisambiguited("class A { SomeClass a; } class B:A { void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    [TestMethod]
    void ambiguousNameToStaticFieldInherited() {
        assertNameInCodeIsDisambiguited("class A { static SomeClass a; } class B:A {  void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    // Otherwise, if a field of that name is declared _in the compilation unit (§7.3) containing the Identifier by a
    // single-static-import declaration (§7.5.3), or by a static-import-on-demand declaration (§7.5.4) then the
    // AmbiguousName is reclassified as an ExpressionName.

    [TestMethod]
    void ambiguousNameToSingleStaticImportDeclaration() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        ResolvedFieldDeclaration mockedFieldA = mock(ResolvedFieldDeclaration.class);
        when(mockedFieldA.isStatic()).thenReturn(true);
        when(mockedFieldA.getName()).thenReturn("a");
        when(mockedC.getAllFields()).thenReturn(Arrays.asList(mockedFieldA));
        typeSolver.addDeclaration("foo.bar.C", mockedC);

        assertNameInCodeIsDisambiguited("import static foo.bar.C.a; class B {  void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameToStaticImportOnDemandDeclaration() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        ResolvedFieldDeclaration mockedFieldA = mock(ResolvedFieldDeclaration.class);
        when(mockedFieldA.isStatic()).thenReturn(true);
        when(mockedFieldA.getName()).thenReturn("a");
        when(mockedC.getAllFields()).thenReturn(Arrays.asList(mockedFieldA));
        typeSolver.addDeclaration("foo.bar.C", mockedC);

        assertNameInCodeIsDisambiguited("import static foo.bar.C.*; class B {  void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameDefaultToPackageName() {
        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.aField;" + "\n" +
                        "} }", "a", NameCategory.AMBIGUOUS_NAME, NameCategory.PACKAGE_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    // If the AmbiguousName is a qualified name, consisting of a name, a ".", and an Identifier, then the name to the
    // left of the "." is first reclassified, for it is itself an AmbiguousName. There is then a choice:
    //
    // If the name to the left of the "." is reclassified as a PackageName, then:
    //
    // If the Identifier is a valid TypeIdentifier, and there is a package whose name is the name to the left of the
    // ".", and that package contains a declaration of a type whose name is the same as the Identifier, then this
    // AmbiguousName is reclassified as a TypeName.
    //
    // Otherwise, this AmbiguousName is reclassified as a PackageName. A later step determines whether or not a package
    // of that name actually exists.

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsPackageNameLeadingToType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d;" + "\n" +
                        "} }", "a.b.C", NameCategory.AMBIGUOUS_NAME, NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsPackageNameNotLeadingToType() {
        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d;" + "\n" +
                        "} }", "a.b.C", NameCategory.AMBIGUOUS_NAME, NameCategory.PACKAGE_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver()));
    }

    // If the AmbiguousName is a qualified name, consisting of a name, a ".", and an Identifier, then the name to the
    // left of the "." is first reclassified, for it is itself an AmbiguousName. There is then a choice:
    //
    // If the name to the left of the "." is reclassified as a PackageName, then:
    //
    // If the Identifier is a valid TypeIdentifier, and there is a package whose name is the name to the left of the
    // ".", and that package contains a declaration of a type whose name is the same as the Identifier, then this
    // AmbiguousName is reclassified as a TypeName.
    //
    // Otherwise, this AmbiguousName is reclassified as a PackageName. A later step determines whether or not a package
    // of that name actually exists.

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsTypeNameLeadingToField() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        ResolvedFieldDeclaration mockedFieldC = mock(ResolvedFieldDeclaration.class);
        when(mockedC.asReferenceType()).thenReturn(mockedC);
        when(mockedC.getAllMethods()).thenReturn(Collections.emptySet());
        when(mockedFieldC.isStatic()).thenReturn(true);
        when(mockedFieldC.getName()).thenReturn("d");
        when(mockedC.getAllFields()).thenReturn(Arrays.asList(mockedFieldC));
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d.e;" + "\n" +
                        "} }", "a.b.C.d", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsTypeNameLeadingToMethod() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        MethodUsage mockedMethodD = mock(MethodUsage.class);
        when(mockedC.asReferenceType()).thenReturn(mockedC);
        when(mockedMethodD.getName()).thenReturn("d");
        when(mockedC.getAllFields()).thenReturn(Collections.emptyList());
        when(mockedC.getAllMethods()).thenReturn(new HashSet<>(Arrays.asList(mockedMethodD)));
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d.e;" + "\n" +
                        "} }", "a.b.C.d", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsTypeNameLeadingToInternalType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();

        ResolvedReferenceTypeDeclaration mockedD = mock(ResolvedReferenceTypeDeclaration.class);

        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        when(mockedC.asReferenceType()).thenReturn(mockedC);
        when(mockedC.getInternalType("d")).thenReturn(mockedD);
        when(mockedC.hasInternalType("d")).thenReturn(true);
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d.e;" + "\n" +
                        "} }", "a.b.C.d", NameCategory.AMBIGUOUS_NAME, NameCategory.TYPE_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsTypeNameLeadingToCompilationError() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        when(mockedC.asReferenceType()).thenReturn(mockedC);
        when(mockedC.getAllFields()).thenReturn(Arrays.asList());
        when(mockedC.getAllMethods()).thenReturn(Collections.emptySet());
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d.e;" + "\n" +
                        "} }", "a.b.C.d", NameCategory.AMBIGUOUS_NAME, NameCategory.COMPILATION_ERROR, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    [TestMethod]
    void ambiguousNameInQualifiedNameRequalifiedAsExpressionName() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedC = mock(ResolvedReferenceTypeDeclaration.class);
        MethodUsage mockedMethodD = mock(MethodUsage.class);
        when(mockedC.asReferenceType()).thenReturn(mockedC);
        when(mockedMethodD.getName()).thenReturn("d");
        when(mockedC.getAllFields()).thenReturn(Collections.emptyList());
        when(mockedC.getAllMethods()).thenReturn(new HashSet<>(Arrays.asList(mockedMethodD)));
        typeSolver.addDeclaration("a.b.C", mockedC);

        assertNameInCodeIsDisambiguited("class B {  void foo() {\n" +
                        "a.b.C.d.e.f;" + "\n" +
                        "} }", "a.b.C.d.e", NameCategory.AMBIGUOUS_NAME, NameCategory.EXPRESSION_NAME, ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    // 6.5.4.1. Simple PackageOrTypeNames
    //
    // If the PackageOrTypeName, Q, is a valid TypeIdentifier and occurs _in the scope of a type named Q, then the
    // PackageOrTypeName is reclassified as a TypeName.

    [TestMethod]
    void packageOrTypeNameSimpleNameMatchingType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedMyQualified = mock(ResolvedReferenceTypeDeclaration.class);
        when(mockedMyQualified.asReferenceType()).thenReturn(mockedMyQualified);
        typeSolver.addDeclaration("foo.myQualified", mockedMyQualified);

        assertNameInCodeIsDisambiguited("package foo; class Bar {  Bar() { new myQualified.path.to.TypeName(); } }",
                "myQualified", NameCategory.PACKAGE_OR_TYPE_NAME, NameCategory.TYPE_NAME,
                ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    // Otherwise, the PackageOrTypeName is reclassified as a PackageName. The meaning of the PackageOrTypeName is
    // the meaning of the reclassified name.

    [TestMethod]
    void packageOrTypeNameSimpleNameNotMatchingType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();

        assertNameInCodeIsDisambiguited("package foo; class Bar {  Bar() { new myQualified.path.to.TypeName(); } }",
                "myQualified", NameCategory.PACKAGE_OR_TYPE_NAME, NameCategory.PACKAGE_NAME,
                ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    // 6.5.4.2. Qualified PackageOrTypeNames
    //
    // Given a qualified PackageOrTypeName of the form Q.Id, if Id is a valid TypeIdentifier and the type or package
    // denoted by Q has a member type named Id, then the qualified PackageOrTypeName name is reclassified as a
    // TypeName.

    [TestMethod]
    void packageOrTypeNameQualifiedNameMatchingType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();
        ResolvedReferenceTypeDeclaration mockedMyQualified = mock(ResolvedReferenceTypeDeclaration.class);
        when(mockedMyQualified.asReferenceType()).thenReturn(mockedMyQualified);
        typeSolver.addDeclaration("myQualified.path", mockedMyQualified);

        assertNameInCodeIsDisambiguited("package foo; class Bar {  Bar() { new myQualified.path.to.TypeName(); } }",
                "myQualified.path", NameCategory.PACKAGE_OR_TYPE_NAME, NameCategory.TYPE_NAME,
                ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }

    // Otherwise, it is reclassified as a PackageName. The meaning of the qualified PackageOrTypeName is the meaning
    // of the reclassified name.

    [TestMethod]
    void packageOrTypeNameQualifiedNameNotMatchingType() {
        MemoryTypeSolver typeSolver = new MemoryTypeSolver();

        assertNameInCodeIsDisambiguited("package foo; class Bar {  Bar() { new myQualified.path.to.TypeName(); } }",
                "myQualified.path", NameCategory.PACKAGE_OR_TYPE_NAME, NameCategory.PACKAGE_NAME,
                ParseStart.COMPILATION_UNIT,
                new CombinedTypeSolver(new ReflectionTypeSolver(), typeSolver));
    }
}
