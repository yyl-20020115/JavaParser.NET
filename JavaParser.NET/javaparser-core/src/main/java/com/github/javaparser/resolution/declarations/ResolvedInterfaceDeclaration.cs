/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
namespace com.github.javaparser.resolution.declarations;



/**
 * An interface declaration.
 *
 * @author Federico Tomassetti
 */
public interface ResolvedInterfaceDeclaration:ResolvedReferenceTypeDeclaration, ResolvedTypeParametrizable, HasAccessSpecifier {

    @Override
    default boolean isInterface() {
        return true;
    }

    /**
     * Return the list of interfaces extended directly by this one.
     */
    List<ResolvedReferenceType> getInterfacesExtended();

    /**
     * Return the list of interfaces extended directly or indirectly by this one.
     */
    default List<ResolvedReferenceType> getAllInterfacesExtended() {
        List<ResolvedReferenceType> interfaces = new ArrayList<>();
        for (ResolvedReferenceType interfaceDeclaration : getInterfacesExtended()) {
            interfaces.add(interfaceDeclaration);
            interfaces.addAll(interfaceDeclaration.getAllInterfacesAncestors());
        }
        return interfaces;
    }
}
