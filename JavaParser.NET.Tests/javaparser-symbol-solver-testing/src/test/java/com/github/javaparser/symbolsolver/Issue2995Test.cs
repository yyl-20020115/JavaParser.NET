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




public class Issue2995Test:AbstractResolutionTest {

    [TestMethod]
    public void test() {
        ParserConfiguration config = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        string str = "public class MyClass {\n" +
                "   class Inner1 {\n" +
                "       class Inner2 {\n" +
                "       }\n" +
                "   }\n" +
                "   {\n" +
                "       new Inner1().new Inner2();\n" +
                "   }\n" +
                "}\n";
        CompilationUnit cu = StaticJavaParser.parse(str);
        List<ObjectCreationExpr> exprs = cu.findAll(ObjectCreationExpr.class);
        assertEquals("new Inner1().new Inner2()", exprs.get(0).toString());
        assertEquals("MyClass.Inner1.Inner2", exprs.get(0).getType().resolve().describe());
        assertEquals("new Inner1()", exprs.get(1).toString());
        assertEquals("MyClass.Inner1", exprs.get(1).getType().resolve().describe());
    }
}
