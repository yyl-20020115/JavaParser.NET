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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;



public class Issue2740Test extends AbstractResolutionTest {

    @Test()
    void test() {
        String code =
                "import java.util.function.Consumer;\n" + 
                "import java.util.ArrayList;\n" + 
                "\n" + 
                "public class A {\n" + 
                "    \n" + 
                "    void m() {\n" + 
                "        new Consumer<String>() {\n" + 
                "            private ArrayList<Integer> t = new ArrayList<>();\n" + 
                "            @Override\n" + 
                "            public void accept(String s) {\n" + 
                "                t.add(s);\n" + 
                "            }\n" + 
                "            \n" + 
                "        };" + 
                "    }\n" + 
                "\n" + 
                "}";

        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(code);
        List<MethodCallExpr> methodCallExpr = cu.findAll(MethodCallExpr.class);
        for (MethodCallExpr expr : methodCallExpr) {
            ResolvedMethodDeclaration rd = expr.resolve();
        }
    }
    
}
