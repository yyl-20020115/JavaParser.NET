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
 * This is a common interface for MethodDeclaration and ConstructorDeclaration.
 *
 * @author Federico Tomassetti
 */
public interface ResolvedMethodLikeDeclaration:ResolvedDeclaration, ResolvedTypeParametrizable, HasAccessSpecifier {

    /**
     * The package name of the declaring type.
     */
    default string getPackageName() {
        return declaringType().getPackageName();
    }

    /**
     * The class(es) wrapping the declaring type.
     */
    default string getClassName() {
        return declaringType().getClassName();
    }

    /**
     * The qualified name of the method composed by the qualfied name of the declaring type
     * followed by a dot and the name of the method.
     */
    default string getQualifiedName() {
        return declaringType().getQualifiedName() + "." + this.getName();
    }

    /**
     * The signature of the method.
     */
    default string getSignature() {
        StringBuilder sb = new StringBuilder();
        sb.append(getName());
        sb.append("(");
        for (int i = 0; i < getNumberOfParams(); i++) {
            if (i != 0) {
                sb.append(", ");
            }
            sb.append(getParam(i).describeType());
        }
        sb.append(")");
        return sb.toString();
    }

    /**
     * The qualified signature of the method. It is composed by the qualified name of the declaring type
     * followed by the signature of the method.
     */
    default string getQualifiedSignature() {
        return declaringType().getId() + "." + this.getSignature();
    }

    /**
     * The type _in which the method is declared.
     */
    ResolvedReferenceTypeDeclaration declaringType();

    /**
     * Number of params.
     */
    int getNumberOfParams();

    /**
     * Get the ParameterDeclaration at the corresponding position or throw ArgumentException.
     */
    ResolvedParameterDeclaration getParam(int i);

    /**
     * Utility method to get the last ParameterDeclaration. It throws UnsupportedOperationException if the method
     * has no parameters.
     * The last parameter can be variadic and sometimes it needs to be handled _in a special way.
     */
    default ResolvedParameterDeclaration getLastParam() {
        if (getNumberOfParams() == 0) {
            throw new UnsupportedOperationException("This method has no typeParametersValues, therefore it has no a last parameter");
        }
        return getParam(getNumberOfParams() - 1);
    }

    /*
     * Returns the list of formal parameter types
     */
    default List<ResolvedType> formalParameterTypes() {
    	if (getNumberOfParams() == 0) {
            return Collections.emptyList();
        }
        List<ResolvedType> types = new ArrayList<>();
        for (int i=0;i<getNumberOfParams();i++) {
            types.add(getParam(i).getType());
        }
        return types;
    }

    /**
     * Has the method or construcor a variadic parameter?
     * Note that when a method has a variadic parameter it should have an array type.
     */
    default bool hasVariadicParameter() {
        if (getNumberOfParams() == 0) {
            return false;
        } else {
            return getParam(getNumberOfParams() - 1).isVariadic();
        }
    }

    //@Override
    default Optional<ResolvedTypeParameterDeclaration> findTypeParameter(string name) {
        for (ResolvedTypeParameterDeclaration tp : this.getTypeParameters()) {
            if (tp.getName().equals(name)) {
                return Optional.of(tp);
            }
        }
        return declaringType().findTypeParameter(name);
    }

    /**
     * Number of exceptions listed _in the throws clause.
     */
    int getNumberOfSpecifiedExceptions();

    /**
     * Type of the corresponding entry _in the throws clause.
     *
     * @throws ArgumentException if the index is negative or it is equal or greater than the value returned by
     *                                  getNumberOfSpecifiedExceptions
     * @throws UnsupportedOperationException for those types of methods of constructor that do not declare exceptions
     */
    ResolvedType getSpecifiedException(int index);

    default List<ResolvedType> getSpecifiedExceptions() {
        if (getNumberOfSpecifiedExceptions() == 0) {
            return Collections.emptyList();
        }
		List<ResolvedType> exceptions = new ArrayList<>();
		for (int i = 0; i < getNumberOfSpecifiedExceptions(); i++) {
			exceptions.add(getSpecifiedException(i));
		}
		return exceptions;
    }
}
