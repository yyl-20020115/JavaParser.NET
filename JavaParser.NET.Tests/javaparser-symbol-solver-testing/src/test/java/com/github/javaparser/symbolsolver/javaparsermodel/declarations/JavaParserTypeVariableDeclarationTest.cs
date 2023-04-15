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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;




class JavaParserTypeVariableDeclarationTest:AbstractTypeDeclarationTest {

    @Override
    public JavaParserTypeVariableDeclaration createValue() {
        CompilationUnit cu = StaticJavaParser.parse("class A<T>{}");
        TypeParameter typeParameter = cu.findFirst(TypeParameter.class).get();
        ReflectionTypeSolver typeSolver = new ReflectionTypeSolver();
        return new JavaParserTypeVariableDeclaration(typeParameter, typeSolver);
    }

    @Override
    public Optional<Node> getWrappedDeclaration(AssociableToAST associableToAST) {
        return Optional.of(
                safeCast(associableToAST, JavaParserTypeVariableDeclaration.class).getWrappedNode()
        );
    }

    @Override
    public boolean isFunctionalInterface(AbstractTypeDeclaration typeDeclaration) {
        return false;
    }

    [TestMethod]
    void getWrappedNodeShouldNotBeNull() {
        assertNotNull(createValue().getWrappedNode());
    }

    @Nested
    class TestGetAncestorAncestorsMethod {

        private /*final*/JavaParser parser = new JavaParser();
        private /*final*/ReflectionTypeSolver typeSolver = new ReflectionTypeSolver();

        private void testGetAncestorWith(Iterable<String> expectedTypes, string sourceCode) {
            CompilationUnit cu = parser.parse(sourceCode).getResult().orElseThrow(AssertionError::new);
            TypeParameter typeParameter = Navigator.demandNodeOfGivenClass(cu, TypeParameter.class);
            JavaParserTypeVariableDeclaration parserTypeParameter = new JavaParserTypeVariableDeclaration(typeParameter, typeSolver);
            assertEquals(expectedTypes, parserTypeParameter.getAncestors().stream()
                    .map(ResolvedReferenceType::getQualifiedName)
                    .sorted()
                    .collect(Collectors.toList()));
        }

        [TestMethod]
        void withoutBound() {
            string sourceCode = "class A<T> {}";
            testGetAncestorWith(Collections.singletonList("java.lang.Object"), sourceCode);
        }

        [TestMethod]
        void withObjectBound() {
            string sourceCode = "class A<T:Object> {}";
            testGetAncestorWith(Collections.singletonList("java.lang.Object"), sourceCode);
        }

        [TestMethod]
        void withMultipleBounds() {
            string sourceCode = "class A {} interface B {} class C<T:A & B> {}";
            testGetAncestorWith(Arrays.asList("A", "B"), sourceCode);
        }

    }

}
