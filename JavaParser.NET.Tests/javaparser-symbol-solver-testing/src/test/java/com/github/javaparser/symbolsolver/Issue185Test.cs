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





class Issue185Test:AbstractResolutionTest {

    [TestMethod]
    void testIssue(){
        Path src = adaptPath("src/test/resources/recursion-issue");
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new JavaParserTypeSolver(src, new LeanParserConfiguration()));
        combinedTypeSolver.add(new ReflectionTypeSolver());
        CompilationUnit agendaCu = parse(adaptPath("src/test/resources/recursion-issue/Usage.java"));
        MethodCallExpr foo = Navigator.findMethodCall(agendaCu, "foo").get();
        assertNotNull(foo);
        JavaParserFacade.get(combinedTypeSolver).getType(foo);
    }

}
