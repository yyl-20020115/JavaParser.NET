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
 * @author Dominik Hardtke
 * @since 01/09/2018
 */
class Issue1814Test:AbstractResolutionTest {
    private JavaParser javaParser;

    @BeforeEach
    void setup() {
        /*final*/CompilationUnit compilationUnit = new CompilationUnit();
        compilationUnit.setPackageDeclaration("java.lang");
        // construct a fake java.lang.Object class with only one method (java.lang.Object#equals(java.lang.Object)
        /*final*/ClassOrInterfaceDeclaration clazz = compilationUnit.addClass("Object", Modifier.Keyword.PUBLIC);
        /*final*/MethodDeclaration equals = clazz.addMethod("equals", Modifier.Keyword.PUBLIC);
        equals.addParameter("Object", "obj");
        /*final*/BlockStmt body = new BlockStmt();
        body.addStatement("return this == obj;");
        equals.setBody(body);

        TypeSolver typeSolver = new TypeSolver() {
            //@Override
            public TypeSolver getParent() {
                return null;
            }

            //@Override
            public void setParent(TypeSolver parent) {
            }

            //@Override
            public SymbolReference<ResolvedReferenceTypeDeclaration> tryToSolveType(string name) {
                if ("java.lang.Object".equals(name)) {
                    // custom handling
                    return SymbolReference.solved(new JavaParserClassDeclaration(clazz, this));
                }

                return SymbolReference.unsolved();
            }
        };

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        javaParser = new JavaParser(config);
    }

    [TestMethod]
    void getAllMethodsVisibleToInheritors() {
        assertTimeoutPreemptively(Duration.ofMillis(1000L), () -> {
            string code = String.join(System.lineSeparator(), "public class AbstractExercise:java.lang.Object {", "}");
            ParseResult<CompilationUnit> parseResult = javaParser.parse(ParseStart.COMPILATION_UNIT, Providers.provider(code));
            assertTrue(parseResult.isSuccessful());
            assertTrue(parseResult.getResult().isPresent());
            List<ClassOrInterfaceType> referenceTypes = parseResult.getResult().get().findAll(ClassOrInterfaceType.class);
            assertTrue(referenceTypes.size() > 0);
            /*final*/List<ResolvedMethodDeclaration> methods = referenceTypes.get(0).resolve().asReferenceType().getAllMethodsVisibleToInheritors();
            assertEquals(1, methods.size());
        });


    }
}
