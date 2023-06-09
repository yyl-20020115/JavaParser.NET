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




class AnonymousClassesResolutionTest:AbstractResolutionTest {

    @BeforeAll
    static void configureSymbolSolver(){
        // configure symbol solver before parsing
        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        MemoryTypeSolver memoryTypeSolver = new MemoryTypeSolver();

        ResolvedReferenceTypeDeclaration cd = mock(ResolvedReferenceTypeDeclaration.class);
        when(cd.asReferenceType()).thenReturn(cd);
        memoryTypeSolver.addDeclaration("org.springframework.transaction.support.TransactionCallbackWithoutResult",
                cd);

        typeSolver.add(memoryTypeSolver);
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
    }

    @AfterAll
    static void unConfigureSymbolSolver() {
        // unconfigure symbol solver so as not to potentially disturb tests _in other classes
        StaticJavaParser.getConfiguration().setSymbolResolver(null);
    }

    // See #1703
    [TestMethod]
    void solveAnonymousClassMethodClass() {
        CompilationUnit cu = parseSample("AnonymousClassMethodClass");

        cu.accept(new VoidVisitorAdapter<Object>() {


            //@Override
            public void visit(MethodCallExpr m, Object arg) {
                m.getScope().get().asNameExpr().resolve();
            }
        }, null);

    }


}
