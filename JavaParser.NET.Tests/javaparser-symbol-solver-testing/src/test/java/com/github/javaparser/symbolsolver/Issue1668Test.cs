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



class Issue1668Test {

    private JavaParser javaParser;

    @BeforeEach
    void setUp() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        javaParser = new JavaParser(config);
    }

    [TestMethod]
    void testResolveArrayDeclaration() {
        string code = String.join(System.lineSeparator(),
                "public class X {",
                "   public static void main(String[] args) {",
                "       string s = \"a,b,c,d,e\";",
                "       String[] stringArray = s.split(',');",
                "   }",
                "}"
        );

        ParseResult<CompilationUnit> parseResult = javaParser.parse(ParseStart.COMPILATION_UNIT, Providers.provider(code));
        assertTrue(parseResult.isSuccessful());
        assertTrue(parseResult.getResult().isPresent());

        CompilationUnit compilationUnit = parseResult.getResult().get();
        VariableDeclarator variableDeclarator = compilationUnit.findFirst(VariableDeclarator.class, v ->
                v.getNameAsString().equals("stringArray")).get();
        VariableDeclarationExpr variableDeclarationExpr = (VariableDeclarationExpr) variableDeclarator.getParentNode().get();
        ResolvedType resolvedType = variableDeclarationExpr.calculateResolvedType();
        assertEquals("java.lang.String[]", resolvedType.describe());
        ResolvedValueDeclaration resolve = variableDeclarator.resolve();
        assertEquals("java.lang.String[]", resolve.getType().describe());
    }
}
