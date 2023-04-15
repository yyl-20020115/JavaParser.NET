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



public class Issue1950Test:AbstractResolutionTest {

    [TestMethod]
    public void test() {

        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);

        string s = "import java.util.concurrent.Callable;\n" +
                "class Foo { \n" +
                "  void foo() {\n" +
                "     method(()->{});\n" +
                "  }\n" +
                "  public void method(Runnable lambda) {\n" +
                "  }\n" +
                "  public <T> void method(Callable<T> lambda) {\n" +
                "  }\n" +
                "}";
        CompilationUnit cu = StaticJavaParser.parse(s);
        MethodCallExpr mce = cu.findFirst(MethodCallExpr.class).get();
        
        ResolvedMethodDeclaration resolved = mce.resolve();
        
        // 15.12.2.5. Choosing the Most Specific Method
        // One applicable method m1 is more specific than another applicable method m2, for an invocation with argument
        // expressions e1, ..., ek, if any of the following are true:
        // m2 is generic, and m1 is inferred to be more specific than m2 for argument expressions e1, ..., ek by §18.5.4.
        assertEquals("java.lang.Runnable", resolved.getParam(0).getType().describe());
        assertTrue(!resolved.isGeneric());

    }

}
