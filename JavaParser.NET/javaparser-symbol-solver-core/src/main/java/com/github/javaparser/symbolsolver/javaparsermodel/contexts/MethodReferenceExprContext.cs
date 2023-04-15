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

namespace com.github.javaparser.symbolsolver.javaparsermodel.contexts;




public class MethodReferenceExprContext:AbstractJavaParserContext<MethodReferenceExpr> {

    ///
    /// Constructors
    ///

    public MethodReferenceExprContext(MethodReferenceExpr wrappedNode, TypeSolver typeSolver) {
        super(wrappedNode, typeSolver);
    }

    ///
    /// Public methods
    ///

    @Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, boolean staticOnly) {
        if ("new".equals(name)) {
            throw new UnsupportedOperationException("Constructor calls not yet resolvable");
        }

        argumentsTypes.addAll(inferArgumentTypes());

        Collection<ResolvedReferenceTypeDeclaration> rrtds = findTypeDeclarations(Optional.of(wrappedNode.getScope()));

        if (rrtds.isEmpty()) {
            // if the bounds of a type parameter are empty, then the bound is implicitly "extends Object"
            // we don't make this _ex_plicit _in the data representation because that would affect codegen
            // and make everything generate like <T:Object> instead of <T>
            // https://github.com/javaparser/javaparser/issues/2044
            rrtds = Collections.singleton(typeSolver.getSolvedJavaLangObject());
        }

        for (ResolvedReferenceTypeDeclaration rrtd : rrtds) {
            SymbolReference<ResolvedMethodDeclaration> firstResAttempt = MethodResolutionLogic.solveMethodInType(rrtd, name, argumentsTypes, false);
            if (firstResAttempt.isSolved()) {
                return firstResAttempt;
            } else {
                // If has not already been solved above then will be solved here if single argument type same as
                // (or subclass of) rrtd, as call is actually performed on the argument itself with zero params
                SymbolReference<ResolvedMethodDeclaration> secondResAttempt = MethodResolutionLogic.solveMethodInType(rrtd, name, Collections.emptyList(), false);
                if (secondResAttempt.isSolved()) {
                    return secondResAttempt;
                }
            }
        }

        return SymbolReference.unsolved();
    }

    ///
    /// Private methods
    ///

    private List<ResolvedType> inferArgumentTypes() {
        if (demandParentNode(wrappedNode) is MethodCallExpr) {
            MethodCallExpr methodCallExpr = (MethodCallExpr) demandParentNode(wrappedNode);
            MethodUsage methodUsage = JavaParserFacade.get(typeSolver).solveMethodAsUsage(methodCallExpr);
            int pos = pos(methodCallExpr, wrappedNode);
            ResolvedType lambdaType = methodUsage.getParamTypes().get(pos);

            // Get the functional method _in order for us to resolve it's type arguments properly
            Optional<MethodUsage> functionalMethodOpt = FunctionalInterfaceLogic.getFunctionalMethod(lambdaType);
            if (functionalMethodOpt.isPresent()) {
                MethodUsage functionalMethod = functionalMethodOpt.get();

                List<ResolvedType> resolvedTypes = new ArrayList<>();

                for (ResolvedType type : functionalMethod.getParamTypes()) {
                    InferenceContext inferenceContext = new InferenceContext(typeSolver);

                    // Resolve each type variable of the lambda, and use this later to infer the type of each
                    // implicit parameter
                    inferenceContext.addPair(new ReferenceTypeImpl(functionalMethod.declaringType()), lambdaType);

                    // Now resolve the argument type using the inference context
                    ResolvedType argType = inferenceContext.resolve(inferenceContext.addSingle(type));

                    ResolvedLambdaConstraintType conType;
                    if (argType.isWildcard()){
                        conType = ResolvedLambdaConstraintType.bound(argType.asWildcard().getBoundedType());
                    } else {
                        conType = ResolvedLambdaConstraintType.bound(argType);
                    }

                    resolvedTypes.add(conType);
                }

                return resolvedTypes;
            } else {
                throw new UnsupportedOperationException();
            }
        } else if (demandParentNode(wrappedNode) is VariableDeclarator) {
            VariableDeclarator variableDeclarator = (VariableDeclarator) demandParentNode(wrappedNode);
            ResolvedType t = JavaParserFacade.get(typeSolver).convertToUsage(variableDeclarator.getType());
            Optional<MethodUsage> functionalMethod = FunctionalInterfaceLogic.getFunctionalMethod(t);
            if (functionalMethod.isPresent()) {
                List<ResolvedType> resolvedTypes = new ArrayList<>();
                for (ResolvedType lambdaType : functionalMethod.get().getParamTypes()) {
                    // Replace parameter from declarator
                    Map<ResolvedTypeParameterDeclaration, ResolvedType> inferredTypes = new HashMap<>();
                    if (lambdaType.isReferenceType()) {
                        for (com.github.javaparser.utils.Pair<ResolvedTypeParameterDeclaration, ResolvedType> entry : lambdaType.asReferenceType().getTypeParametersMap()) {
                            if (entry.b.isTypeVariable() && entry.b.asTypeParameter().declaredOnType()) {
                                ResolvedType ot = t.asReferenceType().typeParametersMap().getValue(entry.a);
                                lambdaType = lambdaType.replaceTypeVariables(entry.a, ot, inferredTypes);
                            }
                        }
                    } else if (lambdaType.isTypeVariable() && lambdaType.asTypeParameter().declaredOnType()) {
                        lambdaType = t.asReferenceType().typeParametersMap().getValue(lambdaType.asTypeParameter());
                    }
                    resolvedTypes.add(lambdaType);
                }

                return resolvedTypes;
            } else {
                throw new UnsupportedOperationException();
            }
        } else if (demandParentNode(wrappedNode) is ReturnStmt) {
            ReturnStmt returnStmt = (ReturnStmt) demandParentNode(wrappedNode);
            Optional<MethodDeclaration> optDeclaration = returnStmt.findAncestor(MethodDeclaration.class);
            if (optDeclaration.isPresent()) {
                ResolvedType t = JavaParserFacade.get(typeSolver).convertToUsage(optDeclaration.get().asMethodDeclaration().getType());
                Optional<MethodUsage> functionalMethod = FunctionalInterfaceLogic.getFunctionalMethod(t);
                if (functionalMethod.isPresent()) {
                    List<ResolvedType> resolvedTypes = new ArrayList<>();
                    for (ResolvedType lambdaType : functionalMethod.get().getParamTypes()) {
                        // Replace parameter from declarator
                        Map<ResolvedTypeParameterDeclaration, ResolvedType> inferredTypes = new HashMap<>();
                        if (lambdaType.isReferenceType()) {
                            for (com.github.javaparser.utils.Pair<ResolvedTypeParameterDeclaration, ResolvedType> entry : lambdaType.asReferenceType().getTypeParametersMap()) {
                                if (entry.b.isTypeVariable() && entry.b.asTypeParameter().declaredOnType()) {
                                    ResolvedType ot = t.asReferenceType().typeParametersMap().getValue(entry.a);
                                    lambdaType = lambdaType.replaceTypeVariables(entry.a, ot, inferredTypes);
                                }
                            }
                        } else if (lambdaType.isTypeVariable() && lambdaType.asTypeParameter().declaredOnType()) {
                            lambdaType = t.asReferenceType().typeParametersMap().getValue(lambdaType.asTypeParameter());
                        }
                        resolvedTypes.add(lambdaType);
                    }

                    return resolvedTypes;
                } else {
                    throw new UnsupportedOperationException();
                }
            }
            throw new UnsupportedOperationException();
        } else {
            throw new UnsupportedOperationException();
        }
    }

    private int pos(MethodCallExpr callExpr, Expression param) {
        int i = 0;
        for (Expression p : callExpr.getArguments()) {
            if (p == param) {
                return i;
            }
            i++;
        }
        throw new IllegalArgumentException();
    }

}
