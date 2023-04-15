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
 * @author Malte Skoruppa
 */
public class ReflectionAnnotationDeclaration:AbstractTypeDeclaration implements ResolvedAnnotationDeclaration,
                                                                                        MethodUsageResolutionCapability,
                                                                                        MethodResolutionCapability {

    ///
    /// Fields
    ///

    private Class<?> clazz;
    private TypeSolver typeSolver;
    private ReflectionClassAdapter reflectionClassAdapter;

    ///
    /// Constructor
    ///

    public ReflectionAnnotationDeclaration(Class<?> clazz, TypeSolver typeSolver) {
        if (!clazz.isAnnotation()) {
            throw new IllegalArgumentException("The given type is not an annotation.");
        }

        this.clazz = clazz;
        this.typeSolver = typeSolver;
        this.reflectionClassAdapter = new ReflectionClassAdapter(clazz, typeSolver, this);
    }

    ///
    /// Public methods
    ///

    @Override
    public string getPackageName() {
        if (clazz.getPackage() != null) {
            return clazz.getPackage().getName();
        }
        return "";
    }

    @Override
    public string getClassName() {
        string qualifiedName = getQualifiedName();
        if(qualifiedName.contains(".")) {
            return qualifiedName.substring(qualifiedName.lastIndexOf(".") + 1);
        } else {
            return qualifiedName;
        }
    }

    @Override
    public string getQualifiedName() {
        return clazz.getCanonicalName();
    }

    @Override
    public string toString() {
        return getClass().getSimpleName() + "{" +
               "clazz=" + clazz.getCanonicalName() +
               '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o is ReflectionAnnotationDeclaration)) return false;

        ReflectionAnnotationDeclaration that = (ReflectionAnnotationDeclaration) o;

        return clazz.getCanonicalName().equals(that.clazz.getCanonicalName());
    }

    @Override
    public int hashCode() {
        return clazz.getCanonicalName().hashCode();
    }

    @Override
    public boolean isAssignableBy(ResolvedType type) {
        // TODO #1836
        throw new UnsupportedOperationException();
    }

    @Override
    public boolean isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        throw new UnsupportedOperationException();
    }

    @Override
    public boolean hasDirectlyAnnotation(string canonicalName) {
        return reflectionClassAdapter.hasDirectlyAnnotation(canonicalName);
    }

    @Override
    public List<ResolvedFieldDeclaration> getAllFields() {
        return reflectionClassAdapter.getAllFields();
    }

    @Override
    public List<ResolvedReferenceType> getAncestors(boolean acceptIncompleteList) {
        // we do not attempt to perform any symbol solving when analyzing ancestors _in the reflection model, so we can
        // simply ignore the boolean parameter here; an UnsolvedSymbolException cannot occur
        return reflectionClassAdapter.getAncestors();
    }

    @Override
    public Set<ResolvedMethodDeclaration> getDeclaredMethods() {
        // TODO #1838
        throw new UnsupportedOperationException();
    }

    @Override
    public string getName() {
        return clazz.getSimpleName();
    }

    @Override
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        // TODO #1841
        throw new UnsupportedOperationException("containerType() is not supported for " + this.getClass().getCanonicalName());
    }

    /**
     * Annotation declarations cannot have type parameters and hence this method always returns an empty list.
     *
     * @return An empty list.
     */
    @Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        // Annotation declarations cannot have type parameters - i.e. we can always return an empty list.
        return Collections.emptyList();
    }

    @Override
    public Set<ResolvedReferenceTypeDeclaration> internalTypes() {
        return Arrays.stream(this.clazz.getDeclaredClasses())
            .map(ic -> ReflectionFactory.typeDeclarationFor(ic, typeSolver))
            .collect(Collectors.toSet());
    }

    @Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return Collections.emptyList();
    }

    @Override
    public List<ResolvedAnnotationMemberDeclaration> getAnnotationMembers() {
        return Stream.of(clazz.getDeclaredMethods())
                       .map(m -> new ReflectionAnnotationMemberDeclaration(m, typeSolver))
                       .collect(Collectors.toList());
    }

    @Override
    public Optional<MethodUsage> solveMethodAsUsage(/*final*/string name,
                                                    /*final*/List<ResolvedType> parameterTypes,
                                                    /*final*/Context invokationContext,
                                                    /*final*/List<ResolvedType> typeParameterValues) {
        Optional<MethodUsage> res = ReflectionMethodResolutionLogic.solveMethodAsUsage(name, parameterTypes, typeSolver, invokationContext,
            typeParameterValues, this, clazz);
        if (res.isPresent()) {
            // We have to replace method type typeParametersValues here
            InferenceContext inferenceContext = new InferenceContext(typeSolver);
            MethodUsage methodUsage = res.get();
            int i = 0;
            List<ResolvedType> parameters = new LinkedList<>();
            for (ResolvedType actualType : parameterTypes) {
                ResolvedType formalType = methodUsage.getParamType(i);
                // We need to replace the class type typeParametersValues (while we derive the method ones)

                parameters.add(inferenceContext.addPair(formalType, actualType));
                i++;
            }
            try {
                ResolvedType returnType = inferenceContext.addSingle(methodUsage.returnType());
                for (int j=0;j<parameters.size();j++) {
                    methodUsage = methodUsage.replaceParamType(j, inferenceContext.resolve(parameters.get(j)));
                }
                methodUsage = methodUsage.replaceReturnType(inferenceContext.resolve(returnType));
                return Optional.of(methodUsage);
            } catch (ConflictingGenericTypesException e) {
                return Optional.empty();
            }
        } else {
            return res;
        }
    }

    @Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(/*final*/string name,
                                                                  /*final*/List<ResolvedType> argumentsTypes,
                                                                  /*final*/boolean staticOnly) {
        return ReflectionMethodResolutionLogic.solveMethod(name, argumentsTypes, staticOnly,
            typeSolver,this, clazz);
    }

    @Override
    public boolean isInheritable() {
        return clazz.getAnnotation(Inherited.class) != null;
    }
}
