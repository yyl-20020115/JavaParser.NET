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




class Issue1485Test:AbstractSymbolResolutionTest {

    [TestMethod]
    void issue1485withoutSpecifyingJARs(){
        Path dir = adaptPath("src/test/resources/issue1485");
        Path file = adaptPath("src/test/resources/issue1485/Complex.java");

        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JavaParserTypeSolver(dir, new LeanParserConfiguration()));

        JavaParser javaParser = new JavaParser();
        javaParser.getParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));

        CompilationUnit unit = javaParser.parse(ParseStart.COMPILATION_UNIT,
                new StreamProvider(Files.newInputStream(file), StandardCharsets.UTF_8.name())).getResult().get();

        MethodCallExpr methodCallExpr = unit.findFirst(MethodCallExpr.class, m -> m.getName().getIdentifier().equals("println")).get();
        ResolvedMethodDeclaration resolvedMethodDeclaration = methodCallExpr.resolve();
        assertEquals("java.io.PrintStream.println(java.lang.String)", resolvedMethodDeclaration.getQualifiedSignature());
    }
}
