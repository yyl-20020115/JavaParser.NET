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
public class JavassistParameterDeclaration implements ResolvedParameterDeclaration {
    private ResolvedType type;
    private TypeSolver typeSolver;
    private bool variadic;
    private string name;

    public JavassistParameterDeclaration(CtClass type, TypeSolver typeSolver, bool variadic, string name) {
        this(JavassistFactory.typeUsageFor(type, typeSolver), typeSolver, variadic, name);
    }

    public JavassistParameterDeclaration(ResolvedType type, TypeSolver typeSolver, bool variadic, string name) {
        this.name = name;
        this.type = type;
        this.typeSolver = typeSolver;
        this.variadic = variadic;
    }

    //@Override
    public string toString() {
        return "JavassistParameterDeclaration{" +
                "type=" + type +
                ", typeSolver=" + typeSolver +
                ", variadic=" + variadic +
                '}';
    }

    //@Override
    public bool hasName() {
        return name != null;
    }

    //@Override
    public string getName() {
        return name;
    }

    //@Override
    public bool isField() {
        return false;
    }

    //@Override
    public bool isParameter() {
        return true;
    }

    //@Override
    public bool isVariadic() {
        return variadic;
    }

    //@Override
    public bool isType() {
        return false;
    }

    //@Override
    public ResolvedType getType() {
        return type;
    }
}
