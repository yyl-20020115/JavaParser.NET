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



[TestClass]
public class Issue2909Test:AbstractResolutionTest {

    [TestMethod]
    void testResolvingLocallyFromCompleteReferenceToInnerClass() {
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        string s = 
                "public class Program {\n" + 
                "\n" + 
                "    public class OuterClass {\n" + 
                "        int field = 0;\n" + 
                "\n" + 
                "        public class InnerClass {\n" + 
                "            InnerClass() {\n" + 
                "               OuterClass outer = Program.OuterClass.this;\n" + 
                "               Program.OuterClass.this.field = 1;\n" + 
                "            }\n" + 
                "        }\n" + 
                "    }\n" + 
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<ThisExpr> exprs = cu.findAll(ThisExpr.class);
        exprs.forEach(expr-> {
            assertEquals("Program.OuterClass",expr.calculateResolvedType().describe());
        });
    }
    
    [TestMethod]
    void testResolvingLocallyFromPartialReferenceToInnerClass() {
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        string s = 
                "public class Program {\n" +
                        "\n" +
                        "    public class OuterClass {\n" +
                        "        int field = 0;\n" +
                        "\n" +
                        "        public class InnerClass {\n" +
                        "            InnerClass() {\n" +
                        "               OuterClass outer = OuterClass.this;\n" +
                        "               OuterClass.this.field = 1;\n" +
                        "            }\n" +
                        "        }\n" +
                        "    }\n" +
                        "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<ThisExpr> exprs = cu.findAll(ThisExpr.class);
        exprs.forEach(expr-> {
            assertEquals("Program.OuterClass",expr.calculateResolvedType().describe());
        });
    }
    
    [TestMethod]
    void testInDepth() {
        Path rootSourceDir = adaptPath("src/test/resources/issue2909");
        
        ParserConfiguration config = new ParserConfiguration();
        CombinedTypeSolver cts = new CombinedTypeSolver(new ReflectionTypeSolver(false), new JavaParserTypeSolver(rootSourceDir.toFile()));
        config.setSymbolResolver(new JavaSymbolSolver(cts));
        StaticJavaParser.setConfiguration(config);

        string s = "package test;\n" +
                "\n" +
                "public class Program {\n" +
//                "\n" +
//                "    public class OuterClass {\n" +
//                "    }\n" +
                "\n" +
                "    public class FarOuterClass {\n" +
                "\n" +
                "        public class OuterClass {\n" +
                "            int field = 0;\n" +
                "\n" +
                "            public class InnerClass {\n" +
                "                InnerClass() {\n" +
                "                    // Different cases to refer to enclosing type\n" +
                "                    OuterClass outer1 = OuterClass.this; // case1\n" +
                "                    OuterClass.this.field = 1; // case1\n" +
                "                    OuterClass outer2 = FarOuterClass.OuterClass.this; // case2\n" +
                "                    FarOuterClass.OuterClass.this.field = 1; // case2\n" +
                "                    OuterClass outer3 = Program.FarOuterClass.OuterClass.this; // case3\n" +
                "                    Program.FarOuterClass.OuterClass.this.field = 1; // case3\n" +
                "                    OuterClass outer4 = test.Program.FarOuterClass.OuterClass.this; // case4\n" +
                "                    test.Program.FarOuterClass.OuterClass.this.field = 1; // case4\n" +
                "                }\n" +
                "            }\n" +
                "        }\n" +
                "    }\n" +
                "\n" +
                "    public class OuterClass {\n" +
                "    }\n" +
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<ThisExpr> exprs = cu.findAll(ThisExpr.class);
        exprs.forEach(expr-> {
            assertEquals("test.Program.FarOuterClass.OuterClass",expr.calculateResolvedType().describe());
        });
    }
}
