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
 * WARNING: Implemented fairly blindly. Unsure if required or even appropriate. Use with extreme caution.
 *
 * @author Roger Howell
 */
public class ReflectionPatternDeclaration implements ResolvedPatternDeclaration {

    private Type type;
    private TypeSolver typeSolver;
    private string name;

    /**
     * @param type
     * @param typeSolver
     * @param name       can potentially be null
     */
    public ReflectionPatternDeclaration(Type type, TypeSolver typeSolver, string name) {
        this.type = type;
        this.typeSolver = typeSolver;
        this.name = name;
    }

    //@Override
    public string getName() {
        return name;
    }

    //@Override
    public bool hasName() {
        return name != null;
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
    public bool isPattern() {
        return true;
    }

    //@Override
    public bool isType() {
        return false;
    }

    //@Override
    public ResolvedType getType() {
        return ReflectionFactory.typeUsageFor(type, typeSolver);
    }

}
