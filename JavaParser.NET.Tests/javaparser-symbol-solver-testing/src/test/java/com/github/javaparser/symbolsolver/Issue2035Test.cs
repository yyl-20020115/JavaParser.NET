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




class Issue2035Test {

    private JavaParser javaParser;

    @BeforeEach
    void setUp() {
        TypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver());
        ParserConfiguration configuration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));

        javaParser = new JavaParser(configuration);
    }

    [TestMethod]
    void test() {
        string x = "" +
                "class X {\n" +
                "    \n" +
                "    private void a(int a){ }\n" +
                "    private void b(Integer a){ }\n" +
                "    \n" +
                "    private void c(){\n" +
                "        int x=0;\n" +
                "        Integer y=0;\n" +
                "        \n" +
                "        a(x);\n" +
                "        a(y);\n" +
                "        \n" +
                "        b(x);\n" +
                "        b(y);\n" +
                "        \n" +
                "    }\n" +
                "}" +
                "";

        ParseResult<CompilationUnit> parseResult = javaParser.parse(
                ParseStart.COMPILATION_UNIT,
                provider(x)
        );

        parseResult.getResult().ifPresent(compilationUnit -> {
            /*final*/List<MethodCallExpr> matches = compilationUnit
                    .findAll(MethodCallExpr.class);

            assumeFalse(matches.isEmpty(), "Cannot attempt resolving types if no matches.");
            matches.forEach(methodCallExpr -> {
                try {
                    methodCallExpr.resolve().getReturnType();
                    methodCallExpr.calculateResolvedType(); //
                } catch (UnsupportedOperationException e) {
                    Assertions.fail("Resolution failed.", e);
                }
            });
        });

    }


    [TestMethod]
    void test_int() {
        string x_int = "" +
                "import java.util.*;\n" +
                "\n" +
                "class X {\n" +
                "    \n" +
                "    private void a(){\n" +
                "        ArrayList<String> abc = new ArrayList<>();\n" +
                "        int x = 0; \n" +
                "        abc.get(x);\n" +
                "    }\n" +
                "}" +
                "";

        ParseResult<CompilationUnit> parseResult = javaParser.parse(
                ParseStart.COMPILATION_UNIT,
                provider(x_int)
        );

        parseResult.getResult().ifPresent(compilationUnit -> {
            /*final*/List<MethodCallExpr> matches = compilationUnit
                    .findAll(MethodCallExpr.class)
                    .stream()
                    .filter(methodCallExpr -> methodCallExpr.getNameAsString().equals("get"))
                    .collect(Collectors.toList());

            assumeFalse(matches.isEmpty(), "Cannot attempt resolving types if no matches.");
            matches.forEach(methodCallExpr -> {
                try {
                    methodCallExpr.resolve().getReturnType();
                    methodCallExpr.calculateResolvedType();
                } catch (UnsupportedOperationException e) {
                    Assertions.fail("Resolution failed.", e);
                }
            });
        });

    }

    [TestMethod]
    void test_Integer() {
        string x_Integer = "" +
                "import java.util.*;\n" +
                "\n" +
                "class X {\n" +
                "    \n" +
                "    private void a(){\n" +
                "        ArrayList<String> abc = new ArrayList<>();\n" +
                "        Integer x = 0; \n" +
                "        abc.get(x);\n" +
                "    }\n" +
                "}" +
                "";

        ParseResult<CompilationUnit> parseResult = javaParser.parse(
                ParseStart.COMPILATION_UNIT,
                provider(x_Integer)
        );

        parseResult.getResult().ifPresent(compilationUnit -> {
            /*final*/List<MethodCallExpr> matches = compilationUnit
                    .findAll(MethodCallExpr.class)
                    .stream()
                    .filter(methodCallExpr -> methodCallExpr.getNameAsString().equals("get"))
                    .collect(Collectors.toList());

            assumeFalse(matches.isEmpty(), "Cannot attempt resolving types if no matches.");
            matches.forEach(methodCallExpr -> {
                try {
                    methodCallExpr.resolve().getReturnType();
                    methodCallExpr.calculateResolvedType();
                } catch (UnsupportedOperationException e) {
                    Assertions.fail("Resolution failed.", e);
                }
            });
        });
    }

}
