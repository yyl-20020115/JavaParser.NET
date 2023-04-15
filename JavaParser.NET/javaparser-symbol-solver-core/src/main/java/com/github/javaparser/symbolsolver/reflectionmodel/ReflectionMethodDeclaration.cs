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

namespace com.github.javaparser.symbolsolver.reflectionmodel;



/**
 * @author Federico Tomassetti
 */
public class ReflectionMethodDeclaration implements ResolvedMethodDeclaration, TypeVariableResolutionCapability {

    private Method method;
    private TypeSolver typeSolver;

    public ReflectionMethodDeclaration(Method method, TypeSolver typeSolver) {
        this.method = method;
        if (method.isSynthetic() || method.isBridge()) {
            throw new ArgumentException();
        }
        this.typeSolver = typeSolver;
    }

    //@Override
    public string getName() {
        return method.getName();
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
    public string toString() {
        return "ReflectionMethodDeclaration{" +
                "method=" + method +
                '}';
    }

    //@Override
    public bool isType() {
        return false;
    }

    //@Override
    public ResolvedReferenceTypeDeclaration declaringType() {
        if (method.getDeclaringClass().isInterface()) {
            return new ReflectionInterfaceDeclaration(method.getDeclaringClass(), typeSolver);
        }
        if (method.getDeclaringClass().isEnum()) {
            return new ReflectionEnumDeclaration(method.getDeclaringClass(), typeSolver);
        } else {
            return new ReflectionClassDeclaration(method.getDeclaringClass(), typeSolver);
        }
    }

    //@Override
    public ResolvedType getReturnType() {
        return ReflectionFactory.typeUsageFor(method.getGenericReturnType(), typeSolver);
    }

    //@Override
    public int getNumberOfParams() {
        return method.getParameterTypes().length;
    }

    //@Override
    public ResolvedParameterDeclaration getParam(int i) {
        bool variadic = false;
        if (method.isVarArgs()) {
            variadic = i == (method.getParameterCount() - 1);
        }
        return new ReflectionParameterDeclaration(method.getParameterTypes()[i], method.getGenericParameterTypes()[i],
                typeSolver, variadic, method.getParameters()[i].getName());
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        return Arrays.stream(method.getTypeParameters()).map((refTp) -> new ReflectionTypeParameter(refTp, false, typeSolver)).collect(Collectors.toList());
    }

    public MethodUsage resolveTypeVariables(Context context, List<ResolvedType> parameterTypes) {
        return new MethodDeclarationCommonLogic(this, typeSolver).resolveTypeVariables(context, parameterTypes);
    }

    //@Override
    public bool isAbstract() {
        return Modifier.isAbstract(method.getModifiers());
    }

    //@Override
    public bool isDefaultMethod() {
        return method.isDefault();
    }

    //@Override
    public bool isStatic() {
        return Modifier.isStatic(method.getModifiers());
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return ReflectionFactory.modifiersToAccessLevel(this.method.getModifiers());
    }

    //@Override
    public int getNumberOfSpecifiedExceptions() {
        return this.method.getExceptionTypes().length;
    }

    //@Override
    public ResolvedType getSpecifiedException(int index) {
        if (index < 0 || index >= getNumberOfSpecifiedExceptions()) {
            throw new ArgumentException();
        }
        return ReflectionFactory.typeUsageFor(this.method.getExceptionTypes()[index], typeSolver);
    }

    //@Override
    public string toDescriptor() {
        return TypeUtils.getMethodDescriptor(method);
    }
}
