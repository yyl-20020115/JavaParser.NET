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

namespace com.github.javaparser.symbolsolver.logic;




/**
 * @author Federico Tomassetti
 */
class InferenceContextTest {

    private TypeSolver typeSolver;
    private ResolvedReferenceType string;
    private ResolvedReferenceType object;
    private ResolvedReferenceType listOfString;
    private ResolvedReferenceType listOfE;
    private ResolvedTypeParameterDeclaration tpE;

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
        string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver));
        object = new ReferenceTypeImpl(new ReflectionClassDeclaration(Object.class, typeSolver));
        listOfString = listOf(string);
        tpE = mock(ResolvedTypeParameterDeclaration.class);
        when(tpE.getName()).thenReturn("T");

        listOfE = listOf(new ResolvedTypeVariable(tpE));
    }

    private ResolvedReferenceType listOf(ResolvedType elementType) {
        return new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(List.class, typeSolver), ImmutableList.of(elementType));
    }

    [TestMethod]
    void noVariablesArePlacedWhenNotNeeded() {
        ResolvedType result = new InferenceContext(typeSolver).addPair(object, string);
        assertEquals(object, result);
    }

    [TestMethod]
    void placingASingleVariableTopLevel() {
        ResolvedType result = new InferenceContext(typeSolver).addPair(new ResolvedTypeVariable(tpE), listOfString);
        assertEquals(new InferenceVariableType(0, typeSolver), result);
    }

    [TestMethod]
    void placingASingleVariableInside() {
        ResolvedType result = new InferenceContext(typeSolver).addPair(listOfE, listOfString);
        assertEquals(listOf(new InferenceVariableType(0, typeSolver)), result);
    }

}
