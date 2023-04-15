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

namespace com.github.javaparser.symbolsolver.resolution.typesolvers;





class JarTypeSolverTest:AbstractTypeSolverTest<JarTypeSolver> {

    private static /*final*/Supplier<JarTypeSolver> JAR_TYPE_PROVIDER = () -> {
        try {
            Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
            return new JarTypeSolver(pathToJar);
        } catch (IOException e) {
            throw new RuntimeException("Unable to create the JarTypeSolver.", e);
        }
    };

    public JarTypeSolverTest() {
        base(JAR_TYPE_PROVIDER);
    }

    [TestMethod]
    void initial(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
        JarTypeSolver jarTypeSolver = new JarTypeSolver(pathToJar);
        assertEquals(true, jarTypeSolver.tryToSolveType("com.github.javaparser.SourcesHelper").isSolved());
        assertEquals(true, jarTypeSolver.tryToSolveType("com.github.javaparser.Token").isSolved());
        assertEquals(true, jarTypeSolver.tryToSolveType("com.github.javaparser.ASTParser.JJCalls").isSolved());
        assertEquals(false, jarTypeSolver.tryToSolveType("com.github.javaparser.ASTParser.Foo").isSolved());
        assertEquals(false, jarTypeSolver.tryToSolveType("com.github.javaparser.Foo").isSolved());
        assertEquals(false, jarTypeSolver.tryToSolveType("Foo").isSolved());
    }

    [TestMethod]
    void dependenciesBetweenJarsNotTriggeringReferences(){
        Path pathToJar1 = adaptPath("src/test/resources/jar1.jar");
        JarTypeSolver jarTypeSolver1 = new JarTypeSolver(pathToJar1);
        assertEquals(true, jarTypeSolver1.tryToSolveType("foo.bar.A").isSolved());

        Path pathToJar2 = adaptPath("src/test/resources/jar2.jar");
        JarTypeSolver jarTypeSolver2 = new JarTypeSolver(pathToJar2);
        assertEquals(true, jarTypeSolver2.tryToSolveType("foo.zum.B").isSolved());
    }

    [TestMethod]
    void dependenciesBetweenJarsTriggeringReferencesThatCannotBeResolved(){
        assertThrows(UnsolvedSymbolException.class, () -> {
                Path pathToJar2 = adaptPath("src/test/resources/jar2.jar");
            JarTypeSolver jarTypeSolver2 = new JarTypeSolver(pathToJar2);
            ResolvedReferenceTypeDeclaration b = jarTypeSolver2.tryToSolveType("foo.zum.B").getCorrespondingDeclaration();
            b.getAncestors();
        });
    }

    [TestMethod]
    void dependenciesBetweenJarsTriggeringReferencesThatCanBeResolved(){
        Path pathToJar1 = adaptPath("src/test/resources/jar1.jar");
        JarTypeSolver jarTypeSolver1 = new JarTypeSolver(pathToJar1);

        Path pathToJar2 = adaptPath("src/test/resources/jar2.jar");
        JarTypeSolver jarTypeSolver2 = new JarTypeSolver(pathToJar2);

        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver(jarTypeSolver1, jarTypeSolver2);

        ResolvedReferenceTypeDeclaration b = combinedTypeSolver.tryToSolveType("foo.zum.B").getCorrespondingDeclaration();
        List<ResolvedReferenceType> ancestors = b.getAncestors();
        assertEquals(1, ancestors.size());
    }

    /**
     * The {@link JarTypeSolver} should not solve the JRE types. If we want to solve the JRE types
     * we should combine it with a {@link ReflectionTypeSolver}.
     *
     * @throws IOException If an I/O exception occur.
     */
    [TestMethod]
    void whenJarTypeSolverShouldNotSolveJREType(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
        JarTypeSolver typeSolver = new JarTypeSolver(pathToJar);
        assertFalse(typeSolver.tryToSolveType("java.lang.Object").isSolved());
    }

    [TestMethod]
    void solveTypeShouldReturnTheCorrespondingDeclarationWhenAvailable(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
        JarTypeSolver typeSolver = new JarTypeSolver(pathToJar);
        ResolvedReferenceTypeDeclaration nodeType = typeSolver.solveType("com.github.javaparser.ast.Node");
        assertEquals("com.github.javaparser.ast.Node", nodeType.getQualifiedName());
    }

    [TestMethod]
    void solveTypeShouldThrowUnsolvedSymbolWhenNotAvailable(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
        JarTypeSolver typeSolver = new JarTypeSolver(pathToJar);
        assertThrows(UnsolvedSymbolException.class, () -> typeSolver.solveType("java.lang.Object"));
    }

    [TestMethod]
    void createTypeSolverFromInputStream(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-2.1.0.jar");
        try (FileInputStream fileInputStream = new FileInputStream(pathToJar.toFile())) {
            JarTypeSolver typeSolver = new JarTypeSolver(fileInputStream);
            assertTrue(typeSolver.tryToSolveType("com.github.javaparser.ast.Node").isSolved());
        }
    }

    [TestMethod]
    void whenTheJarIsNotFoundShouldThrowAFileNotFoundException(@TempDir Path tempDirectory) {
        Path pathToJar = tempDirectory.resolve("a_non_existing_file.jar");
        assertThrows(FileNotFoundException.class, () -> new JarTypeSolver(pathToJar));
    }

    [TestMethod]
    void theJarTypeShouldCacheTheListOfKnownTypes(){
        string typeA = "foo.bar.A";
        string typeB = "foo.zum.B";

        Path pathToJar1 = adaptPath("src/test/resources/jar1.jar");
        JarTypeSolver jarTypeSolver1 = new JarTypeSolver(pathToJar1);
        assertEquals(Sets.newHashSet(typeA), jarTypeSolver1.getKnownClasses());
        assertTrue(jarTypeSolver1.tryToSolveType(typeA).isSolved());
        assertFalse(jarTypeSolver1.tryToSolveType(typeB).isSolved());

        Path pathToJar2 = adaptPath("src/test/resources/jar2.jar");
        JarTypeSolver jarTypeSolver2 = new JarTypeSolver(pathToJar2);
        assertEquals(Sets.newHashSet(typeB), jarTypeSolver2.getKnownClasses());
        assertTrue(jarTypeSolver2.tryToSolveType(typeB).isSolved());
        assertFalse(jarTypeSolver2.tryToSolveType(typeA).isSolved());
    }

}
