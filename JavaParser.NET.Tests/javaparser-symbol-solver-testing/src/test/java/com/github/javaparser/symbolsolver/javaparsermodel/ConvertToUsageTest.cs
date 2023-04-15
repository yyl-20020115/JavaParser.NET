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

namespace com.github.javaparser.symbolsolver.javaparsermodel;




class ConvertToUsageTest:AbstractResolutionTest {

    private /*final*/TypeSolver typeSolver = new ReflectionTypeSolver();

    [TestMethod]
    void testConvertTypeToUsage() {
        CompilationUnit cu = parseSample("LocalTypeDeclarations");
        List<NameExpr> n = cu.findAll(NameExpr.class);

        assertEquals("int", usageDescribe(n, "a"));
        assertEquals("java.lang.Integer", usageDescribe(n, "b"));
        assertEquals("java.lang.Class<java.lang.Integer>", usageDescribe(n, "c"));
        assertEquals("java.lang.Class<? super java.lang.Integer>", usageDescribe(n, "d"));
        assertEquals("java.lang.Class<?:java.lang.Integer>", usageDescribe(n, "e"));
        assertEquals("java.lang.Class<?:java.lang.Class<? super java.lang.Class<?:java.lang.Integer>>>", usageDescribe(n, "f"));
        assertEquals("java.lang.Class<? super java.lang.Class<?:java.lang.Class<? super java.lang.Integer>>>", usageDescribe(n, "g"));
    }

    private string usageDescribe(List<NameExpr> n, string name){
        return n.stream().filter(x -> x.getNameAsString().equals(name))
                .map(JavaParserFacade.get(typeSolver)::getType)
                .map(ResolvedType::describe)
                .findFirst().orElse(null);
    }
}
