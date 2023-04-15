/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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



public class Issue2764Test {
    private JavaParser javaParser;

    @BeforeEach
    void setUp() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        javaParser = new JavaParser(config);
    }
    
    [TestMethod]
    void resolveUnaryExpr() {
        string code = 
                "class A {" +
                "  void a() {" +
                "    int e;" + 
                "    for(e++;;){}" +
                "  }" +
                "}";
      
        ParseResult<CompilationUnit> parseResult = javaParser.parse(ParseStart.COMPILATION_UNIT, Providers.provider(code));
        assertTrue(parseResult.isSuccessful());
        assertTrue(parseResult.getResult().isPresent());

        CompilationUnit cu = parseResult.getResult().get();
        NameExpr name = (NameExpr) cu.findFirst(UnaryExpr.class).get().getExpression();
        ResolvedValueDeclaration resolve = name.resolve();

        assertTrue("int".contentEquals(resolve.getType().describe()));
    }
}
