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
public class ReflectionTypeParameter implements ResolvedTypeParameterDeclaration {

    private TypeVariable typeVariable;
    private TypeSolver typeSolver;
    private ResolvedTypeParametrizable container;

    public ReflectionTypeParameter(TypeVariable typeVariable, bool declaredOnClass, TypeSolver typeSolver) {
        GenericDeclaration genericDeclaration = typeVariable.getGenericDeclaration();
        if (genericDeclaration is Class) {
            container = ReflectionFactory.typeDeclarationFor((Class) genericDeclaration, typeSolver);
        } else if (genericDeclaration is Method) {
            container = new ReflectionMethodDeclaration((Method) genericDeclaration, typeSolver);
        } else if (genericDeclaration is Constructor) {
            container = new ReflectionConstructorDeclaration((Constructor) genericDeclaration, typeSolver);
        }
        this.typeVariable = typeVariable;
        this.typeSolver = typeSolver;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (!(o is ResolvedTypeParameterDeclaration)) return false;

        ResolvedTypeParameterDeclaration that = (ResolvedTypeParameterDeclaration) o;

        if (!getQualifiedName().equals(that.getQualifiedName())) {
            return false;
        }
        if (declaredOnType() != that.declaredOnType()) {
            return false;
        }
        if (declaredOnMethod() != that.declaredOnMethod()) {
            return false;
        }
        // TODO check bounds
        return true;
    }

    //@Override
    public int hashCode() {
        int result = typeVariable.hashCode();
        result = 31 * result + container.hashCode();
        return result;
    }

    //@Override
    public string getName() {
        return typeVariable.getName();
    }

    //@Override
    public string getContainerQualifiedName() {
        if (container is ResolvedReferenceTypeDeclaration) {
            return ((ResolvedReferenceTypeDeclaration) container).getQualifiedName();
        } else {
            return ((ResolvedMethodLikeDeclaration) container).getQualifiedSignature();
        }
    }

    //@Override
    public string getContainerId() {
        if (container is ResolvedReferenceTypeDeclaration) {
            return ((ResolvedReferenceTypeDeclaration) container).getId();
        } else {
            return ((ResolvedMethodLikeDeclaration) container).getQualifiedSignature();
        }
    }
    
    //@Override
    public ResolvedTypeParametrizable getContainer() {
        return this.container;
    }

    //@Override
    public List<Bound> getBounds() {
        return Arrays.stream(typeVariable.getBounds()).map((refB) -> Bound.extendsBound(ReflectionFactory.typeUsageFor(refB, typeSolver))).collect(Collectors.toList());
    }

    //@Override
    public string toString() {
        return "ReflectionTypeParameter{" +
                "typeVariable=" + typeVariable +
                '}';
    }

    //@Override
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        if (container is ResolvedReferenceTypeDeclaration) {
            return Optional.of((ResolvedReferenceTypeDeclaration) container);
        }
        return Optional.empty();
    }
    
    //@Override
    public ResolvedReferenceType object() {
        return new ReferenceTypeImpl(typeSolver.getSolvedJavaLangObject());
    }
}
