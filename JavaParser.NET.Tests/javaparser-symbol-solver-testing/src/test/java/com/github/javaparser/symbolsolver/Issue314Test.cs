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



class Issue314Test:AbstractResolutionTest{

    private TypeSolver typeResolver;
    private JavaParserFacade javaParserFacade;

    private ResolvedType getExpressionType(TypeSolver typeSolver, Expression expression) {
        return JavaParserFacade.get(typeSolver).getType(expression);
    }

    @BeforeEach
    void setup() {
        typeResolver = new ReflectionTypeSolver();
        javaParserFacade = JavaParserFacade.get(typeResolver);
    }

    [TestMethod]
    void resolveReferenceToFieldInheritedByInterface() {
        string code = "package foo.bar;\n"+
                "interface  A {\n" +
                "        int a = 0;\n" +
                "    }\n" +
                "    \n" +
                "    class B implements A {\n" +
                "        int getA() {\n" +
                "            return a;\n" +
                "        }\n" +
                "    }";
        CompilationUnit cu = parse(code);
        NameExpr refToA = Navigator.findNameExpression(Navigator.demandClass(cu, "B"), "a").get();
        SymbolReference<?:ResolvedValueDeclaration> symbolReference = javaParserFacade.solve(refToA);
        assertEquals(true, symbolReference.isSolved());
        assertEquals(true, symbolReference.getCorrespondingDeclaration().isField());
        assertEquals("a", symbolReference.getCorrespondingDeclaration().getName());
    }



}
