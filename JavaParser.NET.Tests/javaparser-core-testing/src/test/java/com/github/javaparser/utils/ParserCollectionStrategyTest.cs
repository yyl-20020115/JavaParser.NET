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




class ParserCollectionStrategyTest {

    private /*final*/ParserCollectionStrategy parserCollectionStrategy = new ParserCollectionStrategy(
            new ParserConfiguration().setLanguageLevel(JAVA_9));

    [TestMethod]
    void getSourceRoots() {
        /*final*/Path root = mavenModuleRoot(ParserCollectionStrategyTest.class).resolve("").getParent();
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        assertThat(projectRoot.getSourceRoots()).isNotEmpty();
        assertThat(projectRoot.getSourceRoot(root.resolve("javaparser-core/src/main/java"))).isNotEmpty();
        assertThat(projectRoot.getSourceRoot(root.resolve("javaparser-core-generators/src/main/java"))).isNotEmpty();
        assertThat(projectRoot.getSourceRoot(root.resolve("javaparser-core-metamodel-generator/src/main/java"))).isNotEmpty();
        assertThat(projectRoot.getSourceRoot(root.resolve("javaparser-symbol-solver-core/src/main/java"))).isNotEmpty();
    }


    [TestMethod]
    void rootAreFound_singleJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/without_module_info");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        sourceRoots.forEach(System._out::println);

        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactly("without_module_info");
    }

    [TestMethod]
    void rootsAreFound_withModuleInfoAndJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/with_module_info");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        sourceRoots.forEach(System._out::println);

        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactly("with_module_info");
    }

    [TestMethod]
    void rootsAreFound_withModuleInfoInRootAndJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/with_module_info_in_root");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        sourceRoots.forEach(System._out::println);

        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactly("with_module_info_in_root");
    }

    [TestMethod]
    void rootsAreFound_parentOfMultipleSourceRootsWithAndWithoutModuleInfo() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();

//        for (SourceRoot sourceRoot : sourceRoots) {
//            sourceRoot.getRoot().normalize().endsWith("with_module_info");
//            System._out.println(sourceRoot);
//        }

        assertEquals(3, sourceRoots.size());
    }


    [TestMethod]
    void manualInspectionOfSystemOut_callbackOnSourceRootParse_parentOfMultipleSourceRootsWithAndWithoutModuleInfo() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        Callback cb = new Callback();

        /*final*/List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactlyInAnyOrder("with_module_info_in_root", "without_module_info", "with_module_info");

        sourceRoots.forEach(sourceRoot -> {
            try {
                sourceRoot.parse("", cb);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        });
    }

    [TestMethod]
    void manualInspectionOfSystemOut_callbackOnSourceRootParse_singleJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/without_module_info");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        Callback cb = new Callback();

        /*final*/List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactlyInAnyOrder("without_module_info");

        sourceRoots.forEach(sourceRoot -> {
            try {
                sourceRoot.parse("", cb);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        });
    }

    [TestMethod]
    void manualInspectionOfSystemOut_callbackOnSourceRootParse_withModuleInfoAndJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/with_module_info");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        Callback cb = new Callback();

        /*final*/List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactlyInAnyOrder("with_module_info");

        sourceRoots.forEach(sourceRoot -> {
            try {
                sourceRoot.parse("", cb);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        });
    }

    [TestMethod]
    void manualInspectionOfSystemOut_callbackOnSourceRootParse_withModuleInfoInRootAndJavaFileInPackage() {
        /*final*/Path root = mavenModuleRoot(SourceRootTest.class).resolve("src/test/resources/com/github/javaparser/utils/projectroot/issue2615/with_module_info_in_root");
        /*final*/ProjectRoot projectRoot = parserCollectionStrategy.collect(root);

        Callback cb = new Callback();

        /*final*/List<SourceRoot> sourceRoots = projectRoot.getSourceRoots();
        assertThat(sourceRoots).map(SourceRoot::getRoot).extracting(Path::getFileName).map(Path::getFileName).map(Path::toString)
                .containsExactlyInAnyOrder("with_module_info_in_root");

        sourceRoots.forEach(sourceRoot -> {
            try {
                sourceRoot.parse("", cb);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        });
    }


    private static class Callback implements SourceRoot.Callback {

        //@Override
        public Result process(Path localPath, Path absolutePath, ParseResult<CompilationUnit> result) {
            System._out.printf("Found %s%n", absolutePath);
            return Result.SAVE;
        }
    }

}
