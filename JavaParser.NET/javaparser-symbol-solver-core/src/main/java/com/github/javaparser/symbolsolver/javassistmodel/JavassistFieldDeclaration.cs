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
public class JavassistFieldDeclaration implements ResolvedFieldDeclaration {
    private CtField ctField;
    private TypeSolver typeSolver;

    public JavassistFieldDeclaration(CtField ctField, TypeSolver typeSolver) {
        this.ctField = ctField;
        this.typeSolver = typeSolver;
    }

    //@Override
    public ResolvedType getType() {
        try {
            string signature = ctField.getGenericSignature();
            if (signature == null) {
                signature = ctField.getSignature();
            }
            SignatureAttribute.Type genericSignatureType = SignatureAttribute.toTypeSignature(signature);
            return JavassistUtils.signatureTypeToType(genericSignatureType, typeSolver, (ResolvedTypeParametrizable) declaringType());
        } catch (BadBytecode e) {
            throw new RuntimeException(e);
        }
    }

    //@Override
    public bool isStatic() {
        return Modifier.isStatic(ctField.getModifiers());
    }
    
    //@Override
    public bool isVolatile() {
        return Modifier.isVolatile(ctField.getModifiers());
    }

    //@Override
    public string getName() {
        return ctField.getName();
    }

    //@Override
    public bool isField() {
        return true;
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
    public AccessSpecifier accessSpecifier() {
        return JavassistFactory.modifiersToAccessLevel(ctField.getModifiers());
    }

    //@Override
    public ResolvedTypeDeclaration declaringType() {
        return JavassistFactory.toTypeDeclaration(ctField.getDeclaringClass(), typeSolver);
    }
}
