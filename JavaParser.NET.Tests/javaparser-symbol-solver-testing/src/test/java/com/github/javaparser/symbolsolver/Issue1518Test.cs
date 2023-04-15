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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;




class Issue1518Test extends AbstractResolutionTest {

    @Test()
    void test() throws IOException {
        Path rootSourceDir = adaptPath("src/test/resources/issue1518");

        String src =
                "public class App {\n" + 
                "    public static void main(String[] args) {\n" + 
                "        Test1.Test2 test2 = new Test1.Test2();\n" + 
                "        Test1.Test3 test3 = new Test1.Test3();\n" + 
                "    }\n" + 
                "}";
        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new JavaParserTypeSolver(rootSourceDir.toFile())));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(src);

        List<ObjectCreationExpr> oce = cu.findAll(ObjectCreationExpr.class);

        assertEquals("Test1.Test2", oce.get(0).calculateResolvedType().describe());
        assertEquals("Test1.Test3", oce.get(1).calculateResolvedType().describe());
    }
}
