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




public class Issue2289Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {

        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);

        string s = 
                "public class Test \n" + 
                "{\n" + 
                "    public class InnerClass \n" + 
                "    {\n" + 
                "        public InnerClass(int i, int j) {  \n" + 
                "        }\n" + 
                "\n" + 
                "        public InnerClass(int i, int ...j) {  \n" + 
                "        } \n" + 
                "    }\n" + 
                " \n" + 
                "    public Test() { \n" + 
                "        new InnerClass(1,2);\n" + 
                "        new InnerClass(1,2,3);\n" + 
                "    } \n" + 
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<ObjectCreationExpr> exprs = cu.findAll(ObjectCreationExpr .class);
        
        assertEquals("Test.InnerClass.InnerClass(int, int)", exprs.get(0).resolve().getQualifiedSignature());
        assertEquals("Test.InnerClass.InnerClass(int, int...)", exprs.get(1).resolve().getQualifiedSignature());

    }
    
}
