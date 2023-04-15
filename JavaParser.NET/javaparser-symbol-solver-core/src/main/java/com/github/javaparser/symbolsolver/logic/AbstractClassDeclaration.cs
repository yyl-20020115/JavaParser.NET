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

namespace com.github.javaparser.symbolsolver.logic;



/**
 * A common ancestor for all ClassDeclarations.
 *
 * @author Federico Tomassetti
 */
public abstract class AbstractClassDeclaration:AbstractTypeDeclaration
        implements ResolvedClassDeclaration, MethodResolutionCapability {

    ///
    /// Public
    ///

    //@Override
    public bool hasName() {
        return getQualifiedName() != null;
    }

    //@Override
    public /*final*/List<ResolvedReferenceType> getAllSuperClasses() {
        List<ResolvedReferenceType> superclasses = new ArrayList<>();

        getSuperClass().ifPresent(superClass -> {
            superclasses.add(superClass);
            superclasses.addAll(superClass.getAllClassesAncestors());
        });

        if (superclasses.removeIf(ResolvedReferenceType::isJavaLangObject)) {
            superclasses.add(object());
        }
        return superclasses;
    }

    //@Override
    public /*final*/List<ResolvedReferenceType> getAllInterfaces() {
        List<ResolvedReferenceType> interfaces = new ArrayList<>();
        for (ResolvedReferenceType interfaceDeclaration : getInterfaces()) {
            interfaces.add(interfaceDeclaration);
            interfaces.addAll(interfaceDeclaration.getAllInterfacesAncestors());
        }
        getSuperClass().ifPresent(superClass -> {
            interfaces.addAll(superClass.getAllInterfacesAncestors());
        });
        return interfaces;
    }

    //@Override
    public /*final*/ResolvedClassDeclaration asClass() {
        return this;
    }

    ///
    /// Protected
    ///

    /**
     * An implementation of the Object class.
     */
    protected abstract ResolvedReferenceType object();

}
