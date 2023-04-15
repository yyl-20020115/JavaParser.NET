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

namespace com.github.javaparser.symbolsolver.resolution;



/**
 * See issue #17
 */
class ArrayExprTest {

    [TestMethod]
    void verifyAnArrayAccessExprTypeIsCalculatedProperly() {
        string code = "class A { String[] arrSQL; string toExamine = arrSQL[1]; }";
        FieldDeclaration field = parse(code).getClassByName("A").get().getFieldByName("toExamine").get();

        ResolvedType type = JavaParserFacade.get(new ReflectionTypeSolver()).getType(field.getVariables().get(0).getInitializer().get());
        assertTrue(type.isReferenceType());
        assertEquals("java.lang.String", type.asReferenceType().getQualifiedName());
    }

    [TestMethod]
    void arrayLengthValueDeclaration() {
        string code = "class A { String[] arrSQL; int l = arrSQL.length; }";
        ParserConfiguration parserConfiguration = new ParserConfiguration();
        parserConfiguration.setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));
        CompilationUnit cu = new JavaParser(parserConfiguration).parse(ParseStart.COMPILATION_UNIT, new StringProvider(code)).getResult().get();
        FieldDeclaration field = cu.getClassByName("A").get().getFieldByName("l").get();

        ResolvedValueDeclaration resolvedValueDeclaration = ((FieldAccessExpr)field.getVariables().get(0).getInitializer().get()).resolve();
        assertEquals("length", resolvedValueDeclaration.getName());
        assertEquals(ResolvedPrimitiveType.INT, resolvedValueDeclaration.getType());
    }
}
