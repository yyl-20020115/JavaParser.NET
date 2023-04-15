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
 * @author Malte Skoruppa
 */
public class JavassistAnnotationDeclaration:AbstractTypeDeclaration implements ResolvedAnnotationDeclaration {

    private CtClass ctClass;
    private TypeSolver typeSolver;
    private JavassistTypeDeclarationAdapter javassistTypeDeclarationAdapter;

    //@Override
    public string toString() {
        return getClass().getSimpleName() + "{" +
                "ctClass=" + ctClass.getName() +
                ", typeSolver=" + typeSolver +
                '}';
    }

    public JavassistAnnotationDeclaration(CtClass ctClass, TypeSolver typeSolver) {
        if (!ctClass.isAnnotation()) {
            throw new ArgumentException("Not an annotation: " + ctClass.getName());
        }
        this.ctClass = ctClass;
        this.typeSolver = typeSolver;
        this.javassistTypeDeclarationAdapter = new JavassistTypeDeclarationAdapter(ctClass, typeSolver, this);
    }

    //@Override
    public string getPackageName() {
        return ctClass.getPackageName();
    }

    //@Override
    public string getClassName() {
        string qualifiedName = getQualifiedName();
        if (qualifiedName.contains(".")) {
            return qualifiedName.substring(qualifiedName.lastIndexOf(".") + 1, qualifiedName.length());
        } else {
            return qualifiedName;
        }
    }

    //@Override
    public string getQualifiedName() {
        return ctClass.getName().replace('$', '.');
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        // TODO #1836
        throw new UnsupportedOperationException();
    }

    //@Override
    public List<ResolvedFieldDeclaration> getAllFields() {
        return javassistTypeDeclarationAdapter.getDeclaredFields();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        throw new UnsupportedOperationException();
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        return javassistTypeDeclarationAdapter.getAncestors(acceptIncompleteList);
    }

    //@Override
    public HashSet<ResolvedReferenceTypeDeclaration> internalTypes() {
        return javassistTypeDeclarationAdapter.internalTypes();
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        // TODO #1838
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool hasDirectlyAnnotation(string canonicalName) {
        return ctClass.hasAnnotation(canonicalName);
    }

    //@Override
    public string getName() {
        return getClassName();
    }

    /**
     * Annotation declarations cannot have type parameters and hence this method always returns an empty list.
     *
     * @return An empty list.
     */
    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        // Annotation declarations cannot have type parameters - i.e. we can always return an empty list.
        return Collections.emptyList();
    }

    //@Override
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        // TODO #1841
        throw new UnsupportedOperationException("containerType() is not supported for " + this.getClass().getCanonicalName());
    }

    //@Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return Collections.emptyList();
    }

    //@Override
    public List<ResolvedAnnotationMemberDeclaration> getAnnotationMembers() {
        return Stream.of(ctClass.getDeclaredMethods())
                .map(m -> new JavassistAnnotationMemberDeclaration(m, typeSolver))
                .collect(Collectors.toList());
    }

    //@Override
    public bool isInheritable() {
        try {
            return ctClass.getAnnotation(Inherited.class) != null;
        } catch (ClassNotFoundException e) {
            return false;
        }
    }
}
