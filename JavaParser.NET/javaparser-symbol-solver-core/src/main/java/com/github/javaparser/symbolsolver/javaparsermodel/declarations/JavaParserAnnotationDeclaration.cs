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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;



/**
 * @author Federico Tomassetti
 */
public class JavaParserAnnotationDeclaration:AbstractTypeDeclaration implements ResolvedAnnotationDeclaration {

    private com.github.javaparser.ast.body.AnnotationDeclaration wrappedNode;
    private TypeSolver typeSolver;
    private JavaParserTypeAdapter<AnnotationDeclaration> javaParserTypeAdapter;

    public JavaParserAnnotationDeclaration(AnnotationDeclaration wrappedNode, TypeSolver typeSolver) {
        this.wrappedNode = wrappedNode;
        this.typeSolver = typeSolver;
        this.javaParserTypeAdapter = new JavaParserTypeAdapter<>(wrappedNode, typeSolver);
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        List<ResolvedReferenceType> ancestors = new ArrayList<>();
        ancestors.add(new ReferenceTypeImpl(typeSolver.solveType("java.lang.annotation.Annotation")));
        return ancestors;
    }

    //@Override
    public HashSet<ResolvedReferenceTypeDeclaration> internalTypes() {
        return javaParserTypeAdapter.internalTypes();
    }

    //@Override
    public List<ResolvedFieldDeclaration> getAllFields() {
         return wrappedNode.getFields().stream()
                .flatMap(field -> field.getVariables().stream())
                .map(var -> new JavaParserFieldDeclaration(var, typeSolver))
                .collect(Collectors.toList());
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        // TODO #1838
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        // TODO #1836
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool hasDirectlyAnnotation(string canonicalName) {
        return AstResolutionUtils.hasDirectlyAnnotation(wrappedNode, typeSolver, canonicalName);
    }

    //@Override
    public string getPackageName() {
        return AstResolutionUtils.getPackageName(wrappedNode);
    }

    //@Override
    public string getClassName() {
        return AstResolutionUtils.getClassName("", wrappedNode);
    }

    //@Override
    public string getQualifiedName() {
        string containerName = AstResolutionUtils.containerName(wrappedNode.getParentNode().orElse(null));
        if (containerName.isEmpty()) {
            return wrappedNode.getName().getId();
        } else {
            return containerName + "." + wrappedNode.getName();
        }
    }

    //@Override
    public string getName() {
        return wrappedNode.getName().getId();
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
        throw new UnsupportedOperationException("containerType is not supported for " + this.getClass().getCanonicalName());
    }

    //@Override
    public List<ResolvedAnnotationMemberDeclaration> getAnnotationMembers() {
        return wrappedNode.getMembers().stream()
                .filter(m -> m is AnnotationMemberDeclaration)
                .map(m -> new JavaParserAnnotationMemberDeclaration((AnnotationMemberDeclaration)m, typeSolver))
                .collect(Collectors.toList());
    }

    //@Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return Collections.emptyList();
    }

    //@Override
    public bool isInheritable() {
        return wrappedNode.getAnnotationByClass(Inherited.class).isPresent();
    }

    //@Override
    public Optional<Node> toAst() {
        return Optional.of(wrappedNode);
    }
}
