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

namespace com.github.javaparser.symbolsolver.resolution.typeinference.constraintformulas;



class ConstraintFormulaTest {

    private TypeSolver typeSolver = new ReflectionTypeSolver();
    private ResolvedType stringType = new ReferenceTypeImpl(new ReflectionTypeSolver().solveType(String.class.getCanonicalName()));

    /**
     * From JLS 18.1.2
     *
     * From Collections.singleton("hi"), we have the constraint formula ‹"hi" → α›.
     * Through reduction, this will become the constraint formula: ‹string <: α›.
     */
    [TestMethod]
    void testExpressionCompatibleWithTypeReduce1() {
        ResolvedTypeParameterDeclaration tp = mock(ResolvedTypeParameterDeclaration.class);

        Expression e = new StringLiteralExpr("hi");
        InferenceVariable inferenceVariable = new InferenceVariable("α", tp);

        ExpressionCompatibleWithType formula = new ExpressionCompatibleWithType(typeSolver, e, inferenceVariable);

        ConstraintFormula.ReductionResult res1 = formula.reduce(BoundSet.empty());
        assertEquals(
                ConstraintFormula.ReductionResult.empty().withConstraint(new TypeCompatibleWithType(typeSolver, stringType, inferenceVariable)),
                res1);

        assertEquals(
                ConstraintFormula.ReductionResult.empty().withConstraint(new TypeSubtypeOfType(typeSolver, stringType, inferenceVariable)),
                res1.getConstraint(0).reduce(BoundSet.empty()));
    }

//    /**
//     * From JLS 18.1.2
//     *
//     * From Arrays.asList(1, 2.0), we have the constraint formulas ‹1 → α› and ‹2.0 → α›. Through reduction,
//     * these will become the constraint formulas ‹int → α› and ‹double → α›, and then ‹Integer <: α› and ‹Double <: α›.
//     */
//    [TestMethod]
//    public void testExpressionCompatibleWithTypeReduce2() {
//        throw new UnsupportedOperationException();
//    }
//
//    /**
//     * From JLS 18.1.2
//     *
//     * From the target type of the constructor invocation List<Thread> lt = new ArrayList<>(), we have the constraint
//     * formula ‹ArrayList<α> → List<Thread>›. Through reduction, this will become the constraint formula ‹α <= Thread›,
//     * and then ‹α = Thread›.
//     */
//    [TestMethod]
//    public void testExpressionCompatibleWithTypeReduce3() {
//        throw new UnsupportedOperationException();
//    }
}
