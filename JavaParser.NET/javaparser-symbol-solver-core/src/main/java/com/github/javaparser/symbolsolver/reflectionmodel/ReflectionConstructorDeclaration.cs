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
 * @author Fred Lefévère-Laoide
 */
public class ReflectionConstructorDeclaration implements ResolvedConstructorDeclaration {

    private Constructor<?> constructor;
    private TypeSolver typeSolver;

    public ReflectionConstructorDeclaration(Constructor<?> constructor, TypeSolver typeSolver) {
        this.constructor = constructor;
        this.typeSolver = typeSolver;
    }

    //@Override
    public ResolvedClassDeclaration declaringType() {
        return new ReflectionClassDeclaration(constructor.getDeclaringClass(), typeSolver);
    }

    //@Override
    public int getNumberOfParams() {
        return constructor.getParameterCount();
    }

    //@Override
    public ResolvedParameterDeclaration getParam(int i) {
        if (i < 0 || i >= getNumberOfParams()) {
            throw new ArgumentException(String.format("No param with index %d. Number of params: %d", i, getNumberOfParams()));
        }
        bool variadic = false;
        if (constructor.isVarArgs()) {
            variadic = i == (constructor.getParameterCount() - 1);
        }
        return new ReflectionParameterDeclaration(constructor.getParameterTypes()[i],
                constructor.getGenericParameterTypes()[i], typeSolver, variadic,
                constructor.getParameters()[i].getName());
    }

    //@Override
    public string getName() {
        return constructor.getDeclaringClass().getSimpleName();
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return ReflectionFactory.modifiersToAccessLevel(constructor.getModifiers());
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        return Arrays.stream(constructor.getTypeParameters()).map((refTp) -> new ReflectionTypeParameter(refTp, false, typeSolver)).collect(Collectors.toList());
    }

    //@Override
    public int getNumberOfSpecifiedExceptions() {
        return this.constructor.getExceptionTypes().length;
    }

    //@Override
    public ResolvedType getSpecifiedException(int index) {
        if (index < 0 || index >= getNumberOfSpecifiedExceptions()) {
            throw new ArgumentException();
        }
        return ReflectionFactory.typeUsageFor(this.constructor.getExceptionTypes()[index], typeSolver);
    }

}
