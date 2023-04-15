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

namespace com.github.javaparser.symbolsolver.resolution.javaparser.contexts;



/**
 *
 */
class BlockStmtContextResolutionTest:AbstractResolutionTest {

    @BeforeEach
    void setup() {
    }

    // issue #3526
    [TestMethod]
    void must_be_resolved_from_previous_declaration(){
        string src = "public class Example {\n"
                + "    int a = 3;\n"
                + "    public void bla() {\n"
                + "        a = 7; // 'a' must be resolved as int not String\n"
                + "        string a = \"\";\n"
                + "        a = \"test\";\n"
                + "    }\n"
                + "}";
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(new ReflectionTypeSolver())));
        StaticJavaParser.setConfiguration(configuration);
        CompilationUnit cu = StaticJavaParser.parse(src);
        AssignExpr expr = cu.findFirst(AssignExpr.class).get();
        ResolvedType rt = expr.calculateResolvedType();
        assertEquals("int", rt.describe());
    }
    
    [TestMethod]
    void must_be_resolved_from_previous_declaration_second_declaration_of_the_same_field_name(){
        string src = "public class Example {\n"
                + "    int a = 3;\n"
                + "    public void bla() {\n"
                + "        a = 7; // 'a' must be resolved as int not String\n"
                + "        string a = \"\";\n"
                + "        a = \"test\";\n"
                + "    }\n"
                + "}";
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(new ReflectionTypeSolver())));
        StaticJavaParser.setConfiguration(configuration);
        CompilationUnit cu = StaticJavaParser.parse(src);
        AssignExpr expr = cu.findAll(AssignExpr.class).get(1);
        ResolvedType rt2 = expr.calculateResolvedType();
        assertEquals("java.lang.String", rt2.describe());
    }

}
