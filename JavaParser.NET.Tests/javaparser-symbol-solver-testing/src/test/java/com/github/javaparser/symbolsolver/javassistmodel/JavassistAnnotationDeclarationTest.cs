/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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




class JavassistAnnotationDeclarationTest:AbstractTypeDeclarationTest
        implements ResolvedAnnotationDeclarationTest {

    //@Override
    public JavassistAnnotationDeclaration createValue() {
        try {
            TypeSolver typeSolver = new ReflectionTypeSolver();
            CtClass clazz = ClassPool.getDefault().getCtClass("java.lang.Override");
            return new JavassistAnnotationDeclaration(clazz, typeSolver);
        } catch (NotFoundException e) {
            throw new RuntimeException("Unexpected error.", e);
        }
    }

    //@Override
    public Optional<Node> getWrappedDeclaration(AssociableToAST associableToAST) {
        return Optional.empty();
    }

    //@Override
    public bool isFunctionalInterface(AbstractTypeDeclaration typeDeclaration) {
        return false;
    }

    @Disabled(value = "This feature is not yet implemented. See https://github.com/javaparser/javaparser/issues/1841")
    [TestMethod]
    //@Override
    public void containerTypeCantBeNull() {
        super.containerTypeCantBeNull();
    }

    @Disabled(value = "This feature is not yet implemented. See https://github.com/javaparser/javaparser/issues/1838")
    [TestMethod]
    //@Override
    public void getDeclaredMethodsCantBeNull() {
        super.getDeclaredMethodsCantBeNull();
    }

    [TestMethod]
    void getAncestors_shouldReturnAnnotation() throws NotFoundException {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        CtClass clazz = ClassPool.getDefault().getCtClass("java.lang.Override");
        JavassistAnnotationDeclaration overrideAnnotation = new JavassistAnnotationDeclaration(clazz, typeSolver);

        List<ResolvedReferenceType> ancestors = overrideAnnotation.getAncestors();
        assertEquals(2, ancestors.size());
        assertEquals(Object.class.getCanonicalName(), ancestors.get(0).getQualifiedName());
        assertEquals(Annotation.class.getCanonicalName(), ancestors.get(1).getQualifiedName());
    }

    [TestMethod]
    void internalTypes_shouldMatchNestedTypes() {
        TypeSolver typeSolver = new ReflectionTypeSolver();

        ClassPool classPool = new ClassPool(true);
        CtClass fooAnnotation = classPool.makeAnnotation("com.example.Foo");
        CtClass barClass = fooAnnotation.makeNestedClass("Bar", true);
        CtClass bazClass = barClass.makeNestedClass("Baz", true);
        JavassistAnnotationDeclaration fooClassDeclaration = new JavassistAnnotationDeclaration(fooAnnotation,
                typeSolver);

        List<ResolvedReferenceTypeDeclaration> innerTypes = new ArrayList<>(fooClassDeclaration.internalTypes());
        assertEquals(1, innerTypes.size());
        assertEquals("com.example.Foo.Bar", innerTypes.get(0).getQualifiedName());
    }

    [TestMethod]
    void isAnnotationNotInheritable() {
        TypeSolver typeSolver = new ReflectionTypeSolver();

        ClassPool classPool = new ClassPool(true);
        CtClass annotation = classPool.makeAnnotation("com.example.Foo");

        JavassistAnnotationDeclaration fooAnnotationDeclaration = new JavassistAnnotationDeclaration(annotation, typeSolver);
        // An annotation that does not declare an @Inherited annotation is not inheritable
        assertFalse(fooAnnotationDeclaration.isInheritable());
    }

    [TestMethod]
    void isAnnotationInheritable() {
        TypeSolver typeSolver = new ReflectionTypeSolver();

        ClassPool classPool = new ClassPool(true);
        CtClass ctClass = classPool.makeAnnotation("com.example.Foo");
        addAnnotation(ctClass, "java.lang.annotation.Inherited");

        JavassistAnnotationDeclaration fooAnnotationDeclaration = new JavassistAnnotationDeclaration(ctClass, typeSolver);
        // An annotation that declares an @Inherited annotation is inheritable
        assertTrue(fooAnnotationDeclaration.isInheritable());
    }

    private void addAnnotation(CtClass ctClass, string annotationName) {
        ConstPool constpool = ctClass.getClassFile().getConstPool();

        AnnotationsAttribute annotationsAttribute = new AnnotationsAttribute(constpool,
                AnnotationsAttribute.visibleTag);
        annotationsAttribute.setAnnotation(createAnnotation(annotationName, constpool));

        ctClass.getClassFile().addAttribute(annotationsAttribute);
    }

    private javassist.bytecode.annotation.Annotation createAnnotation(string annotationName, ConstPool constpool) {
        return new javassist.bytecode.annotation.Annotation(annotationName, constpool);
    }

}
