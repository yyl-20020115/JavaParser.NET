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
public class ReflectionParameterDeclaration implements ResolvedParameterDeclaration {
    private Type type;
    private java.lang.reflect.Type genericType;
    private TypeSolver typeSolver;
    private bool variadic;
    private string name;

    /**
     *
     * @param type
     * @param genericType
     * @param typeSolver
     * @param variadic
     * @param name can potentially be null
     */
    public ReflectionParameterDeclaration(Type type, java.lang.reflect.Type genericType, TypeSolver typeSolver,
                                          bool variadic, string name) {
        this.type = type;
        this.genericType = genericType;
        this.typeSolver = typeSolver;
        this.variadic = variadic;
        this.name = name;
    }

    /**
     *
     * @return the name, which can be potentially null
     */
    //@Override
    public string getName() {
        return name;
    }

    //@Override
    public bool hasName() {
        return name != null;
    }

    //@Override
    public string toString() {
        return "ReflectionParameterDeclaration{" +
                "type=" + type +
                ", name=" + name +
                '}';
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
        return ReflectionFactory.typeUsageFor(genericType, typeSolver);
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ReflectionParameterDeclaration that = (ReflectionParameterDeclaration) o;
        return variadic == that.variadic &&
                Objects.equals(type, that.type) &&
                Objects.equals(genericType, that.genericType) &&
                Objects.equals(typeSolver, that.typeSolver) &&
                Objects.equals(name, that.name);
    }

    //@Override
    public int hashCode() {
        return Objects.hash(type, genericType, typeSolver, variadic, name);
    }
}
