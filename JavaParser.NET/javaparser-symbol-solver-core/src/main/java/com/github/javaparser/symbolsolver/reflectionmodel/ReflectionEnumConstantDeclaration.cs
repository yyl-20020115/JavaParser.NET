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



public class ReflectionEnumConstantDeclaration implements ResolvedEnumConstantDeclaration {

    private Field enumConstant;
    private TypeSolver typeSolver;

    public ReflectionEnumConstantDeclaration(Field enumConstant, TypeSolver typeSolver) {
        if (!enumConstant.isEnumConstant()) {
            throw new ArgumentException("The given field does not represent an enum constant");
        }
        this.enumConstant = enumConstant;
        this.typeSolver = typeSolver;
    }

    //@Override
    public string getName() {
        return enumConstant.getName();
    }

    //@Override
    public ResolvedType getType() {
        Type enumClass = enumConstant.getDeclaringClass();
        ResolvedReferenceTypeDeclaration typeDeclaration = new ReflectionEnumDeclaration(enumClass, typeSolver);
        return new ReferenceTypeImpl(typeDeclaration);
    }
}
