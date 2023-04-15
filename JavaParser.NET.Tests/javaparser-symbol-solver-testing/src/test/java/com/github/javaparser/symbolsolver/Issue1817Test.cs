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





public class Issue1817Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {
        
        Path testResources= adaptPath("src/test/resources/issue1817");
        
        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JavaParserTypeSolver(testResources));

        StaticJavaParser.setConfiguration(new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver)));

        string s = 
                "interface A:X.A {\n" + 
                "    default void foo() {\n" + 
                "        X.A xa = null;\n" + 
                "        xa.bar();\n" + 
                "    }\n" + 
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        
        MethodCallExpr mce = cu.findFirst(MethodCallExpr.class).get();
        
        assertEquals("X.A.bar",mce.resolve().getQualifiedName());
    }

}
