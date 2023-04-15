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



class Issue2360:AbstractSymbolResolutionTest {

    [TestMethod]
    void testUnaryExprResolvedViaUnaryNumericPromotion_char() {
        string source = "public class Test\n" + 
                "{\n" + 
                "   public class InnerClass\n" + 
                "   {\n" + 
                "       public InnerClass(char c) {}\n" + 
                "       public InnerClass(int i) {}\n" + 
                "   }\n" + 
                "    \n" + 
                "   public Test() {\n" + 
                "     new InnerClass(+'.'); \n" + 
                "   }\n" + 
                "}";
        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(source);
        ObjectCreationExpr expr = cu.findFirst(ObjectCreationExpr.class).get();
        ResolvedConstructorDeclaration rcd = expr.resolve();
        assertEquals("InnerClass(int)", rcd.getSignature());
    }
    
    [TestMethod]
    void testUnaryExprResolvedViaUnaryNumericPromotion_byte() {
        string source = "public class Test\n" + 
                "{\n" + 
                "   public class InnerClass\n" + 
                "   {\n" + 
                "       public InnerClass(char c) {}\n" + 
                "       public InnerClass(int i) {}\n" + 
                "   }\n" + 
                "    \n" + 
                "   public Test() {\n" + 
                "     byte b = 0;\n" +
                "     new InnerClass(+b); \n" + 
                "   }\n" + 
                "}";
        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(source);
        ObjectCreationExpr expr = cu.findFirst(ObjectCreationExpr.class).get();
        ResolvedConstructorDeclaration rcd = expr.resolve();
        assertEquals("InnerClass(int)", rcd.getSignature());
    }
    
    [TestMethod]
    void testUnaryExprResolvedViaUnaryNumericPromotion_short() {
        string source = "public class Test\n" + 
                "{\n" + 
                "   public class InnerClass\n" + 
                "   {\n" + 
                "       public InnerClass(char c) {}\n" + 
                "       public InnerClass(int i) {}\n" + 
                "   }\n" + 
                "    \n" + 
                "   public Test() {\n" + 
                "     short b = 0;\n" +
                "     new InnerClass(+b); \n" + 
                "   }\n" + 
                "}";
        
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(source);
        ObjectCreationExpr expr = cu.findFirst(ObjectCreationExpr.class).get();
        ResolvedConstructorDeclaration rcd = expr.resolve();
        assertEquals("InnerClass(int)", rcd.getSignature());
    }
    
}
