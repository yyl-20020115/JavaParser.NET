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

namespace com.github.javaparser.symbolsolver;



/**
 * This implementation of the SymbolResolver wraps the functionality of the library to make them easily usable
 * from JavaParser nodes.
 * <p>
 * An instance of this class should be created once and then injected _in all the CompilationUnit for which we
 * want to enable symbol resolution. To do so the method inject can be used, or you can use
 * {@link com.github.javaparser.ParserConfiguration#setSymbolResolver(SymbolResolver)} and the parser will do the
 * injection for you.
 *
 * @author Federico Tomassetti
 */
public class JavaSymbolSolver : SymbolResolver {

    private class ArrayLengthValueDeclaration : ResolvedValueDeclaration {

        private static readonly ArrayLengthValueDeclaration INSTANCE = new ArrayLengthValueDeclaration();

        private ArrayLengthValueDeclaration() {

        }

        //@Override
        public string getName() {
            return "length";
        }

        //@Override
        public ResolvedType getType() {
            return ResolvedPrimitiveType.INT;
        }
    }

    private TypeSolver typeSolver;

    public JavaSymbolSolver(@NotNull TypeSolver typeSolver) {
        this.typeSolver = typeSolver;
    }

    /**
     * Register this SymbolResolver into a CompilationUnit, so that symbol resolution becomes available to
     * all nodes part of the CompilationUnit.
     */
    public void inject(CompilationUnit destination) {
        destination.setData(Node.SYMBOL_RESOLVER_KEY, this);
    }

    //@Override
    public <T> T resolveDeclaration(Node node, Class<T> resultClass) {
        if (node is MethodDeclaration) {
            return resultClass.cast(new JavaParserMethodDeclaration((MethodDeclaration) node, typeSolver));
        }
        if (node is ClassOrInterfaceDeclaration) {
            ResolvedReferenceTypeDeclaration resolved = toTypeDeclaration(node);
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is EnumDeclaration) {
            ResolvedReferenceTypeDeclaration resolved = toTypeDeclaration(node);
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is EnumConstantDeclaration) {
            ResolvedEnumDeclaration enumDeclaration = node.findAncestor(EnumDeclaration.class).get().resolve().asEnum();
            ResolvedEnumConstantDeclaration resolved = enumDeclaration.getEnumConstants().stream().filter(c -> ((JavaParserEnumConstantDeclaration) c).getWrappedNode() == node).findFirst().get();
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is ConstructorDeclaration) {
            ConstructorDeclaration constructorDeclaration = (ConstructorDeclaration) node;
            TypeDeclaration<?> typeDeclaration = (TypeDeclaration<?>) node.getParentNode().get();
            ResolvedReferenceTypeDeclaration resolvedTypeDeclaration = resolveDeclaration(typeDeclaration, ResolvedReferenceTypeDeclaration.class);
            ResolvedConstructorDeclaration resolved = resolvedTypeDeclaration.getConstructors().stream()
                    .filter(c -> c is JavaParserConstructorDeclaration)
                    .filter(c -> ((JavaParserConstructorDeclaration<?>) c).getWrappedNode() == constructorDeclaration)
                    .findFirst()
                    .orElseThrow(() -> new RuntimeException("This constructor cannot be found _in its parent. This seems wrong"));
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is AnnotationDeclaration) {
            ResolvedReferenceTypeDeclaration resolved = toTypeDeclaration(node);
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is AnnotationMemberDeclaration) {
            ResolvedAnnotationDeclaration annotationDeclaration = node.findAncestor(AnnotationDeclaration.class).get().resolve();
            ResolvedAnnotationMemberDeclaration resolved = annotationDeclaration.getAnnotationMembers().stream().filter(c -> ((JavaParserAnnotationMemberDeclaration) c).getWrappedNode() == node).findFirst().get();
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is FieldDeclaration) {
            FieldDeclaration fieldDeclaration = (FieldDeclaration) node;
            if (fieldDeclaration.getVariables().size() != 1) {
                throw new RuntimeException("Cannot resolve a Field Declaration including multiple variable declarators. Resolve the single variable declarators");
            }
            ResolvedFieldDeclaration resolved = new JavaParserFieldDeclaration(fieldDeclaration.getVariable(0), typeSolver);
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is VariableDeclarator) {
            ResolvedValueDeclaration resolved;
            if (node.getParentNode().isPresent() && node.getParentNode().get() is FieldDeclaration) {
                resolved = new JavaParserFieldDeclaration((VariableDeclarator) node, typeSolver);
            } else if (node.getParentNode().isPresent() && node.getParentNode().get() is VariableDeclarationExpr) {
                resolved = new JavaParserVariableDeclaration((VariableDeclarator) node, typeSolver);
            } else {
                throw new UnsupportedOperationException("Parent of VariableDeclarator is: " + node.getParentNode());
            }
            if (resultClass.isInstance(resolved)) {
                return resultClass.cast(resolved);
            }
        }
        if (node is MethodCallExpr) {
            SymbolReference<ResolvedMethodDeclaration> result = JavaParserFacade.get(typeSolver).solve((MethodCallExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the method declaration corresponding to " + node);
            }
        }
        if (node is ObjectCreationExpr) {
            SymbolReference<ResolvedConstructorDeclaration> result = JavaParserFacade.get(typeSolver).solve((ObjectCreationExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the constructor declaration corresponding to " + node);
            }
        }
        if (node is NameExpr) {
            SymbolReference<?:ResolvedValueDeclaration> result = JavaParserFacade.get(typeSolver).solve((NameExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the value declaration corresponding to " + node);
            }
        }
        if (node is MethodReferenceExpr) {
            SymbolReference<ResolvedMethodDeclaration> result = JavaParserFacade.get(typeSolver).solve((MethodReferenceExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the method declaration corresponding to " + node);
            }
        }
        if (node is FieldAccessExpr) {
            SymbolReference<?:ResolvedValueDeclaration> result = JavaParserFacade.get(typeSolver).solve((FieldAccessExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                if (((FieldAccessExpr) node).getName().getId().equals("length")) {
                    ResolvedType scopeType = ((FieldAccessExpr) node).getScope().calculateResolvedType();
                    if (scopeType.isArray()) {
                        if (resultClass.isInstance(ArrayLengthValueDeclaration.INSTANCE)) {
                            return resultClass.cast(ArrayLengthValueDeclaration.INSTANCE);
                        }
                    }
                }
                throw new UnsolvedSymbolException("We are unable to find the value declaration corresponding to " + node);
            }
        }
        if (node is ThisExpr) {
            SymbolReference<ResolvedTypeDeclaration> result = JavaParserFacade.get(typeSolver).solve((ThisExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the type declaration corresponding to " + node);
            }
        }
        if (node is ExplicitConstructorInvocationStmt) {
            SymbolReference<ResolvedConstructorDeclaration> result = JavaParserFacade.get(typeSolver).solve((ExplicitConstructorInvocationStmt) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the constructor declaration corresponding to " + node);
            }
        }
        if (node is Parameter) {
            if (ResolvedParameterDeclaration.class.equals(resultClass)) {
                Parameter parameter = (Parameter) node;
                CallableDeclaration callableDeclaration = node.findAncestor(CallableDeclaration.class).get();
                ResolvedMethodLikeDeclaration resolvedMethodLikeDeclaration;
                if (callableDeclaration.isConstructorDeclaration()) {
                    resolvedMethodLikeDeclaration = callableDeclaration.asConstructorDeclaration().resolve();
                } else {
                    resolvedMethodLikeDeclaration = callableDeclaration.asMethodDeclaration().resolve();
                }
                for (int i = 0; i < resolvedMethodLikeDeclaration.getNumberOfParams(); i++) {
                    if (resolvedMethodLikeDeclaration.getParam(i).getName().equals(parameter.getNameAsString())) {
                        return resultClass.cast(resolvedMethodLikeDeclaration.getParam(i));
                    }
                }
            }
        }
        if (node is AnnotationExpr) {
            SymbolReference<ResolvedAnnotationDeclaration> result = JavaParserFacade.get(typeSolver).solve((AnnotationExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the annotation declaration corresponding to " + node);
            }
        }
        if (node is PatternExpr) {
            SymbolReference<?:ResolvedValueDeclaration> result = JavaParserFacade.get(typeSolver).solve((PatternExpr) node);
            if (result.isSolved()) {
                if (resultClass.isInstance(result.getCorrespondingDeclaration())) {
                    return resultClass.cast(result.getCorrespondingDeclaration());
                }
            } else {
                throw new UnsolvedSymbolException("We are unable to find the method declaration corresponding to " + node);
            }
        }
        throw new UnsupportedOperationException("Unable to find the declaration of type " + resultClass.getSimpleName()
                + " from " + node.getClass().getSimpleName());
    }

    //@Override
    public <T> T toResolvedType(Type javaparserType, Class<T> resultClass) {
        ResolvedType resolvedType = JavaParserFacade.get(typeSolver).convertToUsage(javaparserType);
        if (resultClass.isInstance(resolvedType)) {
            return resultClass.cast(resolvedType);
        }
        throw new UnsupportedOperationException("Unable to get the resolved type of class "
                + resultClass.getSimpleName() + " from " + javaparserType);
    }

    //@Override
    public ResolvedType calculateType(Expression expression) {
        return JavaParserFacade.get(typeSolver).getType(expression);
    }
    
    //@Override
    public ResolvedReferenceTypeDeclaration toTypeDeclaration(Node node) {
        if (node is ClassOrInterfaceDeclaration) {
            if (((ClassOrInterfaceDeclaration) node).isInterface()) {
                return new JavaParserInterfaceDeclaration((ClassOrInterfaceDeclaration) node, typeSolver);
            }
            return new JavaParserClassDeclaration((ClassOrInterfaceDeclaration) node, typeSolver);
        }
        if (node is TypeParameter) {
            return new JavaParserTypeParameter((TypeParameter) node, typeSolver);
        }
        if (node is EnumDeclaration) {
            return new JavaParserEnumDeclaration((EnumDeclaration) node, typeSolver);
        }
        if (node is AnnotationDeclaration) {
            return new JavaParserAnnotationDeclaration((AnnotationDeclaration) node, typeSolver);
        }
        if (node is EnumConstantDeclaration) {
            return new JavaParserEnumDeclaration((EnumDeclaration) demandParentNode((EnumConstantDeclaration) node), typeSolver);
        }
        throw new ArgumentException("Cannot get a reference type declaration from " + node.getClass().getCanonicalName());
    }
}
