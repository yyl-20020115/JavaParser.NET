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




public class Issue1479Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test(){
        
        CombinedTypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(), new JavaParserTypeSolver(adaptPath("src/test/resources/issue1479")));
        JavaSymbolSolver symbolSolver = new JavaSymbolSolver(typeSolver);
        StaticJavaParser.getConfiguration().setSymbolResolver(symbolSolver);
        
        string src = 
                "public class Foo {\n" +
                "  public void m() {\n" +
                "    doSomething(B.AFIELD);\n" +
                "  }\n" +
                "  public void doSomething(string a) {\n" +
                "  }\n" +
                "}\n";

        CompilationUnit cu = StaticJavaParser.parse(src);
        FieldAccessExpr fae = cu.findFirst(FieldAccessExpr.class).get();
        assertTrue(fae.calculateResolvedType().describe().equals("java.lang.String"));
        ResolvedFieldDeclaration value = fae.resolve().asField();
        assertTrue(value.getName().equals("AFIELD"));
        Optional<FieldDeclaration> fd = value.toAst(FieldDeclaration.class);
        assertEquals("a", fd.get().getVariable(0).getInitializer().get().asStringLiteralExpr().getValue());
    }
    
}
