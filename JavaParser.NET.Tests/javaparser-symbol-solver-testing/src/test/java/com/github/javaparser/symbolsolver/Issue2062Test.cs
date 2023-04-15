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




public class Issue2062Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {

        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        string s = "import java.util.Optional;\n" +
                "public class Base{\n" +
                "    class Derived:Base{\n" +
                "    }\n" +
                "    \n" +
                "    public void bar(Optional<Base> o) {\n" +
                "    }\n" +
                "    public void foo() {\n" +
                "        bar(Optional.of(new Derived()));\n" +
                "    }\n" +
                "}";
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<MethodCallExpr> mces = cu.findAll(MethodCallExpr.class);
        assertEquals("bar(Optional.of(new Derived()))", mces.get(0).toString());
        assertEquals("Base.bar(java.util.Optional<Base>)", mces.get(0).resolve().getQualifiedSignature());

    }

}
