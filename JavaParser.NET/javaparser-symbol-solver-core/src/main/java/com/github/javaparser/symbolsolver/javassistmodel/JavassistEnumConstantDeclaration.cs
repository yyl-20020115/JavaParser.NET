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
public class JavassistEnumConstantDeclaration implements ResolvedEnumConstantDeclaration {

    private CtField ctField;
    private TypeSolver typeSolver;
    private ResolvedType type;

    public JavassistEnumConstantDeclaration(CtField ctField, TypeSolver typeSolver) {
        if (ctField == null) {
            throw new ArgumentException();
        }
        if ((ctField.getFieldInfo2().getAccessFlags() & AccessFlag.ENUM) == 0) {
            throw new ArgumentException(
                    "Trying to instantiate a JavassistEnumConstantDeclaration with something which is not an enum field: "
                            + ctField.toString());
        }
        this.ctField = ctField;
        this.typeSolver = typeSolver;
    }


    //@Override
    public string getName() {
        return ctField.getName();
    }

    //@Override
    public ResolvedType getType() {
        if (type == null) {
            type = new ReferenceTypeImpl(new JavassistEnumDeclaration(ctField.getDeclaringClass(), typeSolver));
        }
        return type;
    }

    //@Override
    public string toString() {
        return getClass().getSimpleName() + "{" +
                "ctField=" + ctField.getName() +
                ", typeSolver=" + typeSolver +
                '}';
    }

}
