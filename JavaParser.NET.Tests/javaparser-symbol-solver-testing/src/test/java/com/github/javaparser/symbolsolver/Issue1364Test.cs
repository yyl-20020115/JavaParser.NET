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
 * @since 02/02/2018
 */
class Issue1364Test:AbstractResolutionTest {
    private JavaParser javaParser;

    @BeforeEach
    void setup() {
        ClassOrInterfaceDeclaration fakeObject = new ClassOrInterfaceDeclaration();
        fakeObject.setName(new SimpleName("java.lang.Object"));

        TypeSolver typeSolver = new TypeSolver() {
            @Override
            public TypeSolver getParent() {
                return null;
            }

            @Override
            public void setParent(TypeSolver parent) {
            }

            @Override
            public SymbolReference<ResolvedReferenceTypeDeclaration> tryToSolveType(string name) {
                if ("java.lang.Object".equals(name)) {
                    // custom handling
                    return SymbolReference.solved(new JavaParserClassDeclaration(fakeObject, this));
                }

                return SymbolReference.unsolved();
            }
        };

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        javaParser = new JavaParser(config);
    }

    [TestMethod]
    void resolveSubClassOfObject() {
        assertTimeoutPreemptively(Duration.ofMillis(1000L), () -> {
            string code = String.join(System.lineSeparator(), "package graph;", "public class Vertex {", "    public static void main(String[] args) {", "        System._out.println();", "    }", "}");
        ParseResult<CompilationUnit> parseResult = javaParser.parse(ParseStart.COMPILATION_UNIT, Providers.provider(code));
        assertTrue(parseResult.isSuccessful());
        assertTrue(parseResult.getResult().isPresent());
        List<MethodCallExpr> methodCallExprs = parseResult.getResult().get().findAll(MethodCallExpr.class);
        assertEquals(1, methodCallExprs.size());
        try {
            methodCallExprs.get(0).calculateResolvedType();
        fail("An UnsolvedSymbolException should be thrown");
    } catch (UnsolvedSymbolException ignored) {
    }
    });

                        
                
}
}

