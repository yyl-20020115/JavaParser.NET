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

namespace com.github.javaparser.symbolsolver;





/**
 * CompilationUnitContext.solveType(string name, TypeSolver typeSolver) checks package and imports _in wrong order.
 * @see <a href="https://github.com/javaparser/javaparser/issues/1526">https://github.com/javaparser/javaparser/issues/1526</a>
 */
public class Issue1526Test:AbstractSymbolResolutionTest {

    private /*final*/Path testRoot = adaptPath("src/test/resources/issue1526");
    private /*final*/Path rootCompiles = testRoot.resolve("compiles");
    private /*final*/Path rootErrors = testRoot.resolve("errors");

    [TestMethod]
    public void givenImport_whenCompiles_expectPass(){
        Path root = rootCompiles;
        Path file = rootCompiles.resolve("a/b/c/ExampleClass.java");

        assertDoesNotThrow(() -> {
            doTest(root, file);
        });
    }

    [TestMethod]
    public void givenImportCommentOut_whenCompiles_expectFail(){
        Path root = rootErrors;
        Path file = rootErrors.resolve("a/b/c/ExampleClass.java");

        assertThrows(UnsolvedSymbolException.class, () -> {
            doTest(root, file);
        });
    }

    private void doTest(Path root, Path file){
        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JavaParserTypeSolver(root, new LeanParserConfiguration()));

        JavaParser javaParser = new JavaParser();
        javaParser.getParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));

        ParseResult<CompilationUnit> cu = javaParser.parse(file);
        assumeTrue(cu.isSuccessful(), "the file should compile -- errors are expected when attempting to resolve.");

        cu.getResult().get().findAll(MethodCallExpr.class)
            .forEach(methodCallExpr -> {
                methodCallExpr.resolve();
                methodCallExpr.calculateResolvedType();
            });
    }

}
