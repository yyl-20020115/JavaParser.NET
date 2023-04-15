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

namespace com.github.javaparser.symbolsolver.resolution.typeinference.bounds;




class SameAsBoundTest {

    private TypeSolver typeSolver = new ReflectionTypeSolver();
    private ResolvedType stringType = new ReferenceTypeImpl(new ReflectionTypeSolver().solveType(String.class.getCanonicalName()));

    [TestMethod]
    void recognizeInstantiation() {
        // { α = string } contains a single bound, instantiating α as String.
        InferenceVariable inferenceVariable = new InferenceVariable("α", null);
        Bound bound1 = new SameAsBound(inferenceVariable, stringType);
        Bound bound2 = new SameAsBound(stringType, inferenceVariable);

        assertEquals(Optional.of(new Instantiation(inferenceVariable, stringType)), bound1.isAnInstantiation());
        assertEquals(Optional.of(new Instantiation(inferenceVariable, stringType)), bound2.isAnInstantiation());
    }

}
