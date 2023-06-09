/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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




class SolveMethodDeclaredInEnumTest:AbstractSymbolResolutionTest {

    [TestMethod]
    void methodDeclaredInEnum_enumFromJar(){
        string code = "public class A { public void callEnum() { MyEnum.CONST.method(); }}";
        Path jarPath = adaptPath("src/test/resources/solveMethodDeclaredInEnum/MyEnum.jar");
        TypeSolver typeSolver = new CombinedTypeSolver(new JarTypeSolver(jarPath), new ReflectionTypeSolver());

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(
                new JavaSymbolSolver(typeSolver));
        JavaParser javaParser = new JavaParser(parserConfiguration);

        CompilationUnit cu = javaParser.parse(ParseStart.COMPILATION_UNIT, new StringProvider(code)).getResult().get();

        MethodCallExpr call = cu.findFirst(MethodCallExpr.class).orElse(null);
        ResolvedMethodDeclaration resolvedCall = call.resolve();

        assertNotNull(resolvedCall);
        assertEquals("MyEnum.method()", resolvedCall.getQualifiedSignature());
    }

}
