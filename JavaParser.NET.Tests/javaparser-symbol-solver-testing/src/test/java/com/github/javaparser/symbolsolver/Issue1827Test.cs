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



public class Issue1827Test:AbstractResolutionTest {

    [TestMethod]
    public void solveParametrizedParametersConstructor() {
        
        string src = "public class ParametrizedParametersConstructor {\n"
                + "    public void foo() {\n"
                + "        EClass arg = new EClass();\n"
                + "        ParametrizedClass<String> pc = new ParametrizedClass<>(arg, arg);\n"
                + "    }\n"
                + "\n"
                + "    class EClass implements BaseType<String> {\n"
                + "    }\n"
                + "}\n"
                + "\n"
                + "class ParametrizedClass<T> {\n"
                + "    public ParametrizedClass(BaseType<T> arg1, BaseType<T> arg2) {\n"
                + "    }\n"
                + "}\n"
                + "\n"
                + "interface BaseType<T> {\n"
                + "}";
        
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaSymbolSolver symbolSolver = new JavaSymbolSolver(typeSolver);
        StaticJavaParser
                .getConfiguration()
                .setSymbolResolver(symbolSolver);
        CompilationUnit cu = StaticJavaParser.parse(src);
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "ParametrizedParametersConstructor");
        ObjectCreationExpr oce = clazz.findAll(ObjectCreationExpr.class).get(1); // new ParametrizedClass<>(arg, arg)
        assertDoesNotThrow(() -> oce.resolve());
    }

}
