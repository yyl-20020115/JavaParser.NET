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

namespace com.github.javaparser.symbolsolver.javassistmodel;



/**
 * @author Federico Tomassetti
 */
public class JavassistMethodDeclaration implements ResolvedMethodDeclaration, TypeVariableResolutionCapability {
    private CtMethod ctMethod;
    private TypeSolver typeSolver;
    private /*final*/JavassistMethodLikeDeclarationAdapter methodLikeAdaper;

    public JavassistMethodDeclaration(CtMethod ctMethod, TypeSolver typeSolver) {
        this.ctMethod = ctMethod;
        this.typeSolver = typeSolver;
        this.methodLikeAdaper = new JavassistMethodLikeDeclarationAdapter(ctMethod, typeSolver, this);
    }

    //@Override
    public bool isDefaultMethod() {
        return ctMethod.getDeclaringClass().isInterface() && !isAbstract();
    }

    //@Override
    public bool isStatic() {
        return Modifier.isStatic(ctMethod.getModifiers());
    }

    //@Override
    public string toString() {
        return "JavassistMethodDeclaration{" +
                "ctMethod=" + ctMethod +
                '}';
    }

    //@Override
    public string getName() {
        return ctMethod.getName();
    }

    //@Override
    public bool isField() {
        return false;
    }

    //@Override
    public bool isParameter() {
        return false;
    }

    //@Override
    public bool isType() {
        return false;
    }

    //@Override
    public ResolvedReferenceTypeDeclaration declaringType() {
        return JavassistFactory.toTypeDeclaration(ctMethod.getDeclaringClass(), typeSolver);
    }

    //@Override
    public ResolvedType getReturnType() {
        return methodLikeAdaper.getReturnType();
    }

    //@Override
    public int getNumberOfParams() {
        return methodLikeAdaper.getNumberOfParams();
    }

    //@Override
    public ResolvedParameterDeclaration getParam(int i) {
        return methodLikeAdaper.getParam(i);
    }

    public MethodUsage getUsage(Node node) {
        throw new UnsupportedOperationException();
    }

    //@Override
    public MethodUsage resolveTypeVariables(Context context, List<ResolvedType> parameterTypes) {
        return new MethodDeclarationCommonLogic(this, typeSolver).resolveTypeVariables(context, parameterTypes);
    }

    //@Override
    public bool isAbstract() {
        return Modifier.isAbstract(ctMethod.getModifiers());
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        return methodLikeAdaper.getTypeParameters();
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return JavassistFactory.modifiersToAccessLevel(ctMethod.getModifiers());
    }

    //@Override
    public int getNumberOfSpecifiedExceptions() {
        return methodLikeAdaper.getNumberOfSpecifiedExceptions();
    }

    //@Override
    public ResolvedType getSpecifiedException(int index) {
        return methodLikeAdaper.getSpecifiedException(index);
    }

    //@Override
    public string toDescriptor() {
        return ctMethod.getMethodInfo().getDescriptor();
    }
}
