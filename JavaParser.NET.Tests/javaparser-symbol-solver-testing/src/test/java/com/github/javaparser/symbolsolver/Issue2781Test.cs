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



public class Issue2781Test:AbstractResolutionTest {
    
    [TestMethod]()
    void test() throws FileNotFoundException {
        
        string code =
                "public class A implements AnInterface {\n" + 
                "        private AnInterface field;\n" + 
                "\n" + 
                "        protected AnInterface getContainer() {\n" + 
                "            return this.field;\n" + 
                "        }\n" + 
                "        protected static class AnInterface {\n" + 
                "        }\n" + 
                "}";
        
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new ReflectionTypeSolver());
        combinedTypeSolver.add(new JavaParserTypeSolver(adaptPath("src/test/resources")));
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(combinedTypeSolver));
        
        CompilationUnit cu = StaticJavaParser.parse(code);
        List<ConstructorDeclaration> constructorDeclarations = cu.findAll(ConstructorDeclaration.class);
        constructorDeclarations.forEach(constructorDeclaration -> {
            constructorDeclaration.findAll(MethodCallExpr.class).forEach(methodCallExpr -> {
                //Exception _in thread "main" java.lang.StackOverflowError
                ResolvedMethodDeclaration rmd = methodCallExpr.resolve();
            });
        });

        
    }
    
}
