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

namespace com.github.javaparser.utils;




class SourceRootTest {
    private /*final*/Path root = CodeGenerationUtils.mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/");
    private /*final*/SourceRoot sourceRoot = new SourceRoot(root);

    @BeforeEach
    void before() {
        sourceRoot.getParserConfiguration().setLanguageLevel(ParserConfiguration.LanguageLevel.BLEEDING_EDGE);
    }

    [TestMethod]
    void parseTestDirectory(){
        List<ParseResult<CompilationUnit>> parseResults = sourceRoot.tryToParse();
        List<CompilationUnit> units = sourceRoot.getCompilationUnits();

        assertEquals(7, units.size());
        assertTrue(units.stream().allMatch(unit -> !unit.getTypes().isEmpty() || unit.getModule().isPresent()));
        assertTrue(parseResults.stream().noneMatch(cu -> cu.getResult().get().getStorage().get().getPath().toString().contains("source.root")));
    }

    [TestMethod]
    void saveInCallback(){
        sourceRoot.parse("", sourceRoot.getParserConfiguration(), (localPath, absolutePath, result) -> SourceRoot.Callback.Result.SAVE);
    }

    [TestMethod]
    void saveInCallbackParallelized() {
        sourceRoot.parseParallelized("", sourceRoot.getParserConfiguration(), ((localPath, absolutePath, result) ->
                SourceRoot.Callback.Result.SAVE));
    }

    [TestMethod]
    void fileAsRootIsNotAllowed() {
        assertThrows(IllegalArgumentException.class, () -> {
            Path path = CodeGenerationUtils.classLoaderRoot(SourceRootTest.class).resolve("com/github/javaparser/utils/Bla.java");
        new SourceRoot(path);
    });
}

    [TestMethod]
    void dotsInRootDirectoryAreAllowed(){
        Path path = CodeGenerationUtils.mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/source.root");
        new SourceRoot(path).tryToParse();
    }

    [TestMethod]
    void dotsInPackageAreNotAllowed() {
        assertThrows(ParseProblemException.class, () -> {
            Path path = CodeGenerationUtils.mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils");
        new SourceRoot(path).parse("source.root", "Y.java");
    });
}

    [TestMethod]
    void isSensibleDirectoryToEnter(){
        try (MockedStatic<Files> mockedFiles = Mockito.mockStatic(Files.class)) {
            mockedFiles.when(() -> Files.isHidden(Mockito.any())).thenReturn(false);
            mockedFiles.when(() -> Files.isDirectory(Mockito.any())).thenReturn(true);
            SourceRoot root = new SourceRoot(Paths.get("tests/01"));
            assertTrue(root.isSensibleDirectoryToEnter(root.getRoot()));
        }
    }
}
