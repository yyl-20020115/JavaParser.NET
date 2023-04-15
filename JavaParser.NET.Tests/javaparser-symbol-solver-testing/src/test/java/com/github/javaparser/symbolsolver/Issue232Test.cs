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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;


class Issue232Test extends AbstractResolutionTest {
    @Test
    void issue232() {
        CompilationUnit cu = parseSample("Issue232");
        ClassOrInterfaceDeclaration cls = Navigator.demandClassOrInterface(cu, "OfDouble");
        TypeSolver typeSolver = new ReflectionTypeSolver();
        JavaParserFacade javaParserFacade = JavaParserFacade.get(typeSolver);
        Context context = JavaParserFactory.getContext(cls, typeSolver);
        SymbolReference<ResolvedTypeDeclaration> reference = context.solveType("OfPrimitive<Double, DoubleConsumer, OfDouble>");
    }
}
