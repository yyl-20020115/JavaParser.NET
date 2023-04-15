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




public class Issue2259Test:AbstractResolutionTest {

    @BeforeEach
    void setup() {
    }
    
    [TestMethod]
    void test(){
        // Source code
        string src = "public class TestClass2 {\n" + 
                "    public static void foo(Object o) {\n" + 
                "    }\n" + 
                "    public static void main(String[] args) {\n" + 
                "        foo(new Object[5]);\n" + 
                "    }\n" + 
                "}";
        TypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver());

        // Setup symbol solver
        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(typeSolver)).setLanguageLevel(LanguageLevel.JAVA_8);
        // Setup parser
        StaticJavaParser.setConfiguration(configuration);
        CompilationUnit cu = StaticJavaParser.parse(src);
        MethodCallExpr mce = cu.findFirst(MethodCallExpr.class).get();
        assertEquals("foo(new Object[5])",mce.toString());
        assertEquals("TestClass2.foo(java.lang.Object)",mce.resolve().getQualifiedSignature());
        assertEquals("void",mce.calculateResolvedType().describe());

    }

}
