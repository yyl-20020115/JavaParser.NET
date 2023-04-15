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




public class Issue3173Test:AbstractResolutionTest {

    [TestMethod]
    public void test() {
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false)));
        StaticJavaParser.setConfiguration(config);

        string s =
                "public class Program {\n" +
                        "\n" +
                        "    public @interface AnnotationClass {\n" +
                        "    }\n" +
                        "}";

        CompilationUnit cu = StaticJavaParser.parse(s);
        List<AnnotationDeclaration> annDecls = cu.findAll(AnnotationDeclaration.class);
        annDecls.forEach(annDecl -> {
            assertTrue(annDecl.resolve().isAnnotation());
            assertEquals("AnnotationClass", annDecl.resolve().asAnnotation().getName());
        });
    }
}
