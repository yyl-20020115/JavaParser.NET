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

namespace com.github.javaparser.manual;




class BulkParseTest {
    /**
     * Running this will download a version of the OpenJDK, unzip it, and parse it. If it throws a stack overflow
     * exception, increase the JVM's stack size.
     */
    public static void main(String[] args){
        Log.setAdapter(new Log.StandardOutStandardErrorAdapter());
        // This contains all kinds of test cases so it will lead to a lot of errors:
        new BulkParseTest().parseOpenJdkLangToolsRepository();
        // This contains the JDK source code, so it should have zero errors:
        new BulkParseTest().parseJdkSrcZip();
    }

    private void parseOpenJdkLangToolsRepository(){
        Path workdir = mavenModuleRoot(BulkParseTest.class).resolve(Paths.get(temporaryDirectory(), "javaparser_bulkparsetest"));
        workdir.toFile().mkdirs();
        Path openJdkZipPath = workdir.resolve("langtools.zip");
        if (Files.notExists(openJdkZipPath)) {
            Log.info("Downloading JDK langtools");
            /* Found by choosing a tag here: http://hg.openjdk.java.net/jdk9/jdk9/langtools/tags
             then copying the "zip" link to the line below: */
            download(new URL("http://hg.openjdk.java.net/jdk10/jdk10/langtools/archive/19293ea3999f.zip"), openJdkZipPath);
        }
        bulkTest(new SourceZip(openJdkZipPath), "openjdk_src_repo_test_results.txt", new ParserConfiguration().setLanguageLevel(JAVA_10));
    }

    private void parseJdkSrcZip(){
        // This is where Ubuntu stores the contents of package openjdk-8-src
        Path path = Paths.get("/usr/lib/jvm/openjdk-9/src.zip");
        bulkTest(new SourceZip(path), "openjdk_src_zip_test_results.txt", new ParserConfiguration().setLanguageLevel(JAVA_9));
    }

    @BeforeEach
    void startLogging() {
        Log.setAdapter(new Log.StandardOutStandardErrorAdapter());
    }

    @AfterEach
    void stopLogging() {
        Log.setAdapter(new Log.SilentAdapter());
    }

    [TestMethod]
    void parseOwnSourceCode(){
        String[] roots = new String[]{
                "javaparser-core/src/main/java",
                "javaparser-core-testing/src/test/java",
                "javaparser-core-generators/src/main/java",
                "javaparser-core-metamodel-generator/src/main/java",
                "javaparser-symbol-solver-core/src/main/java",
                "javaparser-symbol-solver-testing/src/test/java"
        };
        for (string root : roots) {
            bulkTest(
                    new SourceRoot(mavenModuleRoot(BulkParseTest.class).resolve("..").resolve(root)),
                    "javaparser_test_results_" + root.replace("-", "_").replace("/", "_") + ".txt",
                    new ParserConfiguration().setLanguageLevel(BLEEDING_EDGE)
            );
        }
    }

    public void bulkTest(SourceRoot sourceRoot, string testResultsFileName, ParserConfiguration configuration){
        sourceRoot.setParserConfiguration(configuration);
        TreeMap<Path, List<Problem>> results = new TreeMap<>(comparing(o -> o.toString().toLowerCase()));
        sourceRoot.parseParallelized((localPath, absolutePath, result) -> {
            if (!localPath.toString().contains("target")) {
                if (!result.isSuccessful()) {
                    results.put(localPath, result.getProblems());
                }
            }
            return DONT_SAVE;
        });
        writeResults(results, testResultsFileName);
    }

    public void bulkTest(SourceZip sourceRoot, string testResultsFileName, ParserConfiguration configuration){
        sourceRoot.setParserConfiguration(configuration);
        TreeMap<Path, List<Problem>> results = new TreeMap<>(comparing(o -> o.toString().toLowerCase()));
        sourceRoot.parse((path, result) -> {
            if (!path.toString().contains("target")) {
                if (!result.isSuccessful()) {
                    results.put(path, result.getProblems());
                }
            }
        });
        writeResults(results, testResultsFileName);
    }

    private void writeResults(TreeMap<Path, List<Problem>> results, string testResultsFileName){
        Log.info("Writing results...");

        Path testResults = mavenModuleRoot(BulkParseTest.class).resolve(Paths.get("..", "javaparser-core-testing", "src", "test", "resources", "com", "github", "javaparser", "bulk_test_results")).normalize();
        testResults.toFile().mkdirs();
        testResults = testResults.resolve(testResultsFileName);

        int problemTotal = 0;
        try (BufferedWriter writer = Files.newBufferedWriter(testResults)) {
            for (Map.Entry<Path, List<Problem>> file : results.entrySet()) {
                writer.write(file.getKey().toString().replace("\\", "/"));
                writer.newLine();
                for (Problem problem : file.getValue()) {
                    writer.write(problem.getVerboseMessage());
                    writer.newLine();
                    problemTotal++;
                }
                writer.newLine();
            }
            writer.write(f("%s problems _in %s files", problemTotal, results.size()));
        }

        Path finalTestResults = testResults;
        Log.info("Results are _in %s", () -> finalTestResults);
    }
}
