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

namespace com.github.javaparser.symbolsolver.resolution.javaparser.contexts;




/**
 * @author Federico Tomassetti
a */
class CompilationUnitContextResolutionTest:AbstractResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
    }

    [TestMethod]
    void getParent() {
        CompilationUnit cu = parseSample("ClassWithTypeVariables");
        Context context = new CompilationUnitContext(cu, typeSolver);

        assertFalse(context.getParent().isPresent());
    }

    [TestMethod]
    void solveExistingGenericType() {
        CompilationUnit cu = parseSample("ClassWithTypeVariables");
        Context context = new CompilationUnitContext(cu, typeSolver);

        Optional<ResolvedType> a = context.solveGenericType("A");
        Optional<ResolvedType> b = context.solveGenericType("B");
        Optional<ResolvedType> c = context.solveGenericType("C");

        assertEquals(false, a.isPresent());
        assertEquals(false, b.isPresent());
        assertEquals(false, c.isPresent());
    }

    [TestMethod]
    void solveUnexistingGenericType() {
        CompilationUnit cu = parseSample("ClassWithTypeVariables");
        Context context = new CompilationUnitContext(cu, typeSolver);

        Optional<ResolvedType> d = context.solveGenericType("D");

        assertEquals(false, d.isPresent());
    }

    [TestMethod]
    void solveSymbolReferringToStaticallyImportedValue() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));

        Context context = new CompilationUnitContext(cu, typeSolver);
        SymbolReference<?:ResolvedValueDeclaration> ref = context.solveSymbol("_out");

        assertEquals(true, ref.isSolved());
        assertEquals("java.io.PrintStream", ref.getCorrespondingDeclaration().getType().asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void solveSymbolReferringToStaticallyImportedUsingAsteriskValue() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));

        Context context = new CompilationUnitContext(cu, typeSolver);
        SymbolReference<?:ResolvedValueDeclaration> ref = context.solveSymbol("err");
        assertEquals(true, ref.isSolved());
        assertEquals("java.io.PrintStream", ref.getCorrespondingDeclaration().getType().asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void solveSymbolReferringToStaticField() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");
        Context context = new CompilationUnitContext(cu, new ReflectionTypeSolver());

        SymbolReference<?:ResolvedValueDeclaration> ref = context.solveSymbol("java.lang.System._out");
        assertEquals(true, ref.isSolved());
        assertEquals("java.io.PrintStream", ref.getCorrespondingDeclaration().getType().asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void solveSymbolAsValueReferringToStaticallyImportedValue() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");
        Context context = new CompilationUnitContext(cu, typeSolver);

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));
        Optional<Value> ref = context.solveSymbolAsValue("_out");
        assertEquals(true, ref.isPresent());
        assertEquals("java.io.PrintStream", ref.get().getType().describe());
    }

    [TestMethod]
    void solveSymbolAsValueReferringToStaticallyImportedUsingAsteriskValue() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");
        Context context = new CompilationUnitContext(cu, typeSolver);

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));
        Optional<Value> ref = context.solveSymbolAsValue("err");
        assertEquals(true, ref.isPresent());
        assertEquals("java.io.PrintStream", ref.get().getType().describe());
    }

    [TestMethod]
    void solveSymbolAsValueReferringToStaticField() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");
        Context context = new CompilationUnitContext(cu, new ReflectionTypeSolver());

        Optional<Value> ref = context.solveSymbolAsValue("java.lang.System._out");
        assertEquals(true, ref.isPresent());
        assertEquals("java.io.PrintStream", ref.get().getType().describe());
    }

    [TestMethod]
    void solveTypeInSamePackage() {
        CompilationUnit cu = parseSample("CompilationUnitWithImports");

        ResolvedReferenceTypeDeclaration otherClass = mock(ResolvedReferenceTypeDeclaration.class);
        when(otherClass.getQualifiedName()).thenReturn("com.foo.OtherClassInSamePackage");
        MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();
        memoryTypeSolver.addDeclaration("com.foo.OtherClassInSamePackage", otherClass);

        Context context = new CompilationUnitContext(cu, memoryTypeSolver);
        SymbolReference<ResolvedTypeDeclaration> ref = context.solveType("OtherClassInSamePackage");

        assertEquals(true, ref.isSolved());
        assertEquals("com.foo.OtherClassInSamePackage", ref.getCorrespondingDeclaration().getQualifiedName());
    }

    [TestMethod]
    void solveTypeImported() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitWithImports");
        Context context = new CompilationUnitContext(cu, new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));

        SymbolReference<ResolvedTypeDeclaration> ref = context.solveType("Assert");
        assertEquals(true, ref.isSolved());
        assertEquals("org.junit.Assert", ref.getCorrespondingDeclaration().getQualifiedName());
    }

    [TestMethod]
    void solveTypeNotImported() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitWithImports");
        Context context = new CompilationUnitContext(cu, new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));

        SymbolReference<ResolvedTypeDeclaration> ref = context.solveType("org.junit.Assume");
        assertEquals(true, ref.isSolved());
        assertEquals("org.junit.Assume", ref.getCorrespondingDeclaration().getQualifiedName());
    }

    [TestMethod]
    void solveMethodStaticallyImportedWithAsterisk() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitWithImports");

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));
        typeSolver.add(new ReflectionTypeSolver());

        Context context = new CompilationUnitContext(cu, typeSolver);

        SymbolReference<ResolvedMethodDeclaration> ref = context.solveMethod("assertFalse", ImmutableList.of(ResolvedPrimitiveType.BOOLEAN), false);
        assertEquals(true, ref.isSolved());
        assertEquals("assertFalse", ref.getCorrespondingDeclaration().getName());
        assertEquals(1, ref.getCorrespondingDeclaration().getNumberOfParams());
        assertEquals("boolean", ref.getCorrespondingDeclaration().getParam(0).getType().describe());
        assertEquals(true, ref.getCorrespondingDeclaration().getParam(0).getType().isPrimitive());
    }

    [TestMethod]
    void solveMethodStaticallyImportedWithoutAsterisk() throws ParseException, IOException {
        CompilationUnit cu = parseSample("CompilationUnitSymbols");

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new JarTypeSolver(adaptPath("src/test/resources/junit-4.8.1.jar")));
        typeSolver.add(new ReflectionTypeSolver());

        Context context = new CompilationUnitContext(cu, typeSolver);

        SymbolReference<ResolvedMethodDeclaration> ref = context.solveMethod("assertEquals", ImmutableList.of(NullType.INSTANCE, NullType.INSTANCE), false);
        assertEquals(true, ref.isSolved());
        assertEquals("assertEquals", ref.getCorrespondingDeclaration().getName());
        assertEquals(2, ref.getCorrespondingDeclaration().getNumberOfParams());
        assertEquals("java.lang.Object", ref.getCorrespondingDeclaration().getParam(0).getType().asReferenceType().getQualifiedName());
        assertEquals("java.lang.Object", ref.getCorrespondingDeclaration().getParam(1).getType().asReferenceType().getQualifiedName());

    }

}
