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

/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except _in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to _in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace com.github.javaparser.symbolsolver.javassistmodel;




public class JavassistTypeParameterDeclarationTest :AbstractResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-3.0.0-alpha.2.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());
    }

    [TestMethod]
    void testGetBounds() {
        JavassistClassDeclaration compilationUnit = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.utils.PositionUtils");
        assertTrue(compilationUnit.isClass());

        for (ResolvedMethodDeclaration method : compilationUnit.getDeclaredMethods()) {
            switch (method.getName()) {
                case "sortByBeginPosition":
                    List<ResolvedTypeParameterDeclaration> typeParams =  method.getTypeParameters();
                    assertEquals(1, typeParams.size());
                    assertEquals("T", typeParams.get(0).getName());

                    List<ResolvedTypeParameterDeclaration.Bound> bounds = typeParams.get(0).getBounds();
                    assertEquals(1, bounds.size());
                    assertTrue(bounds.get(0).isExtends());
                    assertEquals("com.github.javaparser.ast.Node", bounds.get(0).getType().describe());
            }
        }
    }

    [TestMethod]
    void testGetComplexBounds(){
        Path pathToJar = adaptPath("src/test/resources/javassist_generics/generics.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());

        JavassistClassDeclaration compilationUnit = (JavassistClassDeclaration) typeSolver.solveType("javaparser.GenericClass");
        assertTrue(compilationUnit.isClass());

        for (ResolvedMethodDeclaration method : compilationUnit.getDeclaredMethods()) {
            if ("complexGenerics".equals(method.getName())) {
                List<ResolvedTypeParameterDeclaration> typeParams =  method.getTypeParameters();

                assertEquals(1, typeParams.size());
                assertEquals("T", typeParams.get(0).getName());

                List<ResolvedTypeParameterDeclaration.Bound> bounds = typeParams.get(0).getBounds();
                assertEquals(2, bounds.size());
                assertTrue(bounds.get(0).isExtends());
                assertEquals("java.lang.Object", bounds.get(0).getType().describe());
                assertTrue(bounds.get(1).isExtends());
                assertEquals("javaparser.GenericClass.Foo<?:T>", bounds.get(1).getType().describe());
            }
        }
    }

}
