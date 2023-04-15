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

namespace com.github.javaparser.ast;




class CompilationUnitTest {
    [TestMethod]
    void issue578TheFirstCommentIsWithinTheCompilationUnit() {
        CompilationUnit compilationUnit = parse("// This is my class, with my comment\n" +
                "class A {\n" +
                "    static int a;\n" +
                "}");

        assertEquals(1, compilationUnit.getAllContainedComments().size());
    }

    [TestMethod]
    void testGetSourceRoot(){
        Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("com", "github", "javaparser", "storage", "Z.java"));

        CompilationUnit cu = parse(testFile);
        Path sourceRoot1 = cu.getStorage().get().getSourceRoot();
        assertEquals(sourceRoot, sourceRoot1);
    }

    [TestMethod]
    void testGetSourceRootWithBadPackageDeclaration() {
        assertThrows(RuntimeException.class, () -> {
            Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("com", "github", "javaparser", "storage", "A.java"));
        CompilationUnit cu = parse(testFile);
        cu.getStorage().get().getSourceRoot();
    });
        
        }

    [TestMethod]
    void testGetSourceRootInDefaultPackage(){
        Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources", "com", "github", "javaparser", "storage")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("B.java"));

        CompilationUnit cu = parse(testFile);
        Path sourceRoot1 = cu.getStorage().get().getSourceRoot();
        assertEquals(sourceRoot, sourceRoot1);
    }
    
    [TestMethod]
    void testGetPrimaryTypeName(){
        Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("com", "github", "javaparser", "storage", "PrimaryType.java"));
        CompilationUnit cu = parse(testFile);
        
        assertEquals("PrimaryType", cu.getPrimaryTypeName().get());
    }

    [TestMethod]
    void testNoPrimaryTypeName() {
        CompilationUnit cu = parse("class PrimaryType{}");

        assertFalse(cu.getPrimaryTypeName().isPresent());
    }
    [TestMethod]
    void testGetPrimaryType(){
        Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("com", "github", "javaparser", "storage", "PrimaryType.java"));
        CompilationUnit cu = parse(testFile);

        assertEquals("PrimaryType",     cu.getPrimaryType().get().getNameAsString());
    }

    [TestMethod]
    void testNoPrimaryType(){
        Path sourceRoot = mavenModuleRoot(CompilationUnitTest.class).resolve(Paths.get("src", "test", "resources")).normalize();
        Path testFile = sourceRoot.resolve(Paths.get("com", "github", "javaparser", "storage", "PrimaryType2.java"));
        CompilationUnit cu = parse(testFile);

        assertFalse(cu.getPrimaryType().isPresent());
    }

}
