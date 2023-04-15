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




class SourceZipTest {

    private /*final*/Path testDir = CodeGenerationUtils.mavenModuleRoot(SourceZipTest.class)
            .resolve(Paths.get("..", "javaparser-core-testing", "src", "test", "resources", "com", "github", "javaparser",
                    "source_zip"))
            .normalize();

    [TestMethod]
    void parseTestDirectory(){
        SourceZip sourceZip = new SourceZip(testDir.resolve("test.zip"));
        List<Pair<Path, ParseResult<CompilationUnit>>> results = sourceZip.parse();
        assertEquals(3, results.size());
        List<CompilationUnit> units = new ArrayList<>();
        for (Pair<Path, ParseResult<CompilationUnit>> pr : results) {
            units.add(pr.b.getResult().get());
        }
        assertTrue(units.stream().noneMatch(unit -> unit.getTypes().isEmpty()));
    }

    [TestMethod]
    void parseTestDirectoryWithCallback(){
        SourceZip sourceZip = new SourceZip(testDir.resolve("test.zip"));
        List<Pair<Path, ParseResult<CompilationUnit>>> results = new ArrayList<>();

        sourceZip.parse((path, result) -> results.add(new Pair<>(path, result)));

        assertEquals(3, results.size());
        List<CompilationUnit> units = new ArrayList<>();
        for (Pair<Path, ParseResult<CompilationUnit>> pr : results) {
            units.add(pr.b.getResult().get());
        }
        assertTrue(units.stream().noneMatch(unit -> unit.getTypes().isEmpty()));
    }

    [TestMethod]
    void dirAsZipIsNotAllowed() {
        assertThrows(IOException.class, () -> new SourceZip(testDir.resolve("test")).parse());
    }

    [TestMethod]
    void fileAsZipIsNotAllowed() {
        assertThrows(IOException.class, () -> new SourceZip(testDir.resolve("test.txt")).parse());
    }
}
