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

namespace com.github.javaparser.symbolsolver.resolution;




/**
 * See issue #16
 */
class DefaultPackageTest {

    private class MyClassDeclaration:AbstractClassDeclaration {

        private string qualifiedName;

        private MyClassDeclaration(string qualifiedName) {
            this.qualifiedName = qualifiedName;
        }

        //@Override
        public AccessSpecifier accessSpecifier() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
            return new LinkedList<>();
        }

        //@Override
        public HashSet<ResolvedReferenceTypeDeclaration> internalTypes() {
            return new HashSet<>();
        }

        //@Override
        public string getName() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
            throw new UnsupportedOperationException();
        }

        //@Override
        public List<ResolvedFieldDeclaration> getAllFields() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public bool isAssignableBy(ResolvedType type) {
            throw new UnsupportedOperationException();
        }

        //@Override
        public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
            throw new UnsupportedOperationException();
        }

        //@Override
        public bool hasDirectlyAnnotation(string qualifiedName) {
            throw new UnsupportedOperationException();
        }

        //@Override
        public Optional<ResolvedReferenceType> getSuperClass() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public List<ResolvedReferenceType> getInterfaces() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public List<ResolvedConstructorDeclaration> getConstructors() {
            throw new UnsupportedOperationException();
        }

        //@Override
        protected ResolvedReferenceType object() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public string getPackageName() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public string getClassName() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public string getQualifiedName() {
            return qualifiedName;
        }

        //@Override
        public Optional<ResolvedReferenceTypeDeclaration> containerType() {
            throw new UnsupportedOperationException();
        }

        //@Override
        public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
            throw new UnsupportedOperationException();
        }
    }

    [TestMethod]
    void aClassInDefaultPackageCanBeAccessedFromTheDefaultPackage() {
        string code = "class A:B {}";
        MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();
        memoryTypeSolver.addDeclaration("B", new MyClassDeclaration("B"));

        ClassOrInterfaceType jpType = parse(code).getClassByName("A").get().getExtendedTypes(0);
        ResolvedType resolvedType = JavaParserFacade.get(memoryTypeSolver).convertToUsage(jpType);
        assertEquals("B", resolvedType.asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void aClassInDefaultPackageCanBeAccessedFromOutsideTheDefaultPackageImportingIt() {
        assertThrows(UnsolvedSymbolException.class, () -> {
            string code = "package myPackage; import B; class A:B {}";
        MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();
        memoryTypeSolver.addDeclaration("B", new MyClassDeclaration("B"));
        ClassOrInterfaceType jpType = parse(code).getClassByName("A").get().getExtendedTypes(0);
        ResolvedType resolvedType = JavaParserFacade.get(memoryTypeSolver).convertToUsage(jpType);
        assertEquals("B", resolvedType.asReferenceType().getQualifiedName());
    });
                
                }

    [TestMethod]
    void aClassInDefaultPackageCanBeAccessedFromOutsideTheDefaultPackageWithoutImportingIt() {
        assertThrows(UnsolvedSymbolException.class, () -> {
            string code = "package myPackage; class A:B {}";
        MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();
        memoryTypeSolver.addDeclaration("B", new MyClassDeclaration("B"));
        ResolvedType resolvedType = JavaParserFacade.get(memoryTypeSolver).convertToUsage(parse(code).getClassByName("A").get().getExtendedTypes(0));
        assertEquals("B", resolvedType.asReferenceType().getQualifiedName());
    });
                
        }
}
