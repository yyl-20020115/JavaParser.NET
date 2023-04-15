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




public class Issue1769Test:AbstractResolutionTest {

    [TestMethod]()
    void testExtendsNestedclass(){
        Path rootSourceDir = adaptPath("src/test/resources/issue1769");
        
        string src =
                "import foo.OtherClass;\n" +
                "public class MyClass:OtherClass.InnerClass {\n" +
                "}\n";

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new JavaParserTypeSolver(rootSourceDir.toFile())));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(src);
        
        ClassOrInterfaceDeclaration cid = cu.findFirst(ClassOrInterfaceDeclaration.class).get();
        cid.getExtendedTypes().forEach(t-> {
            assertEquals("foo.OtherClass.InnerClass", t.resolve().describe());
        });

    }
    
    [TestMethod]()
    void testInstanciateNestedClass(){
        Path rootSourceDir = adaptPath("src/test/resources/issue1769");
        
        string src =
                "import foo.OtherClass;\n" +
                "public class MyClass{\n" +
                "  public InnerClass myTest() {\n" + 
                "    return new OtherClass.InnerClass();\n" + 
                "  }\n" +
                "}\n";

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new JavaParserTypeSolver(rootSourceDir.toFile())));
        StaticJavaParser.setConfiguration(config);

        CompilationUnit cu = StaticJavaParser.parse(src);
        
        ObjectCreationExpr oce = cu.findFirst(ObjectCreationExpr.class).get();
        assertEquals("foo.OtherClass.InnerClass", oce.calculateResolvedType().asReferenceType().getQualifiedName());
        // The qualified name of the method composed by the qualfied name of the declaring type
        // followed by a dot and the name of the method.
        assertEquals("foo.OtherClass.InnerClass.InnerClass", oce.resolve().getQualifiedName());
    }
}
