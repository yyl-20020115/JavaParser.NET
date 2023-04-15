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




class Issue1599Test:AbstractResolutionTest {

    [TestMethod]()
    void test(){
        Path rootSourceDir = adaptPath("src/test/resources/issue1599");

        string src =
                "public class Foo {\n" +
                "  public void m() {\n" + 
                "    A myVar = new A() {\n" + 
                "      public void bar() {}\n" + 
                "      public void bar2() {}\n" + 
                "    };\n" + 
                "    myVar.bar();\n" + 
                "    myVar.bar2();\n" + 
                "  }\n" +
                "}";
        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new JavaParserTypeSolver(rootSourceDir.toFile())));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(src);

        List<MethodCallExpr> mce = cu.findAll(MethodCallExpr.class);

        assertEquals("void", mce.get(0).calculateResolvedType().describe());
        assertThrows(RuntimeException.class, () -> mce.get(1).calculateResolvedType());
    }
}
