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
public class JavaParserInterfaceDeclaration:AbstractTypeDeclaration
        implements ResolvedInterfaceDeclaration, MethodResolutionCapability, MethodUsageResolutionCapability,
        SymbolResolutionCapability {

    private TypeSolver typeSolver;
    private ClassOrInterfaceDeclaration wrappedNode;
    private JavaParserTypeAdapter<ClassOrInterfaceDeclaration> javaParserTypeAdapter;

    public JavaParserInterfaceDeclaration(ClassOrInterfaceDeclaration wrappedNode, TypeSolver typeSolver) {
        if (!wrappedNode.isInterface()) {
            throw new ArgumentException();
        }
        this.wrappedNode = wrappedNode;
        this.typeSolver = typeSolver;
        this.javaParserTypeAdapter = new JavaParserTypeAdapter<>(wrappedNode, typeSolver);
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        HashSet<ResolvedMethodDeclaration> methods = new HashSet<>();
        for (BodyDeclaration<?> member : wrappedNode.getMembers()) {
            if (member is com.github.javaparser.ast.body.MethodDeclaration) {
                methods.add(new JavaParserMethodDeclaration((com.github.javaparser.ast.body.MethodDeclaration) member, typeSolver));
            }
        }
        return methods;
    }

    public Context getContext() {
        return JavaParserFactory.getContext(wrappedNode, typeSolver);
    }

    public ResolvedType getUsage(Node node) {
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        JavaParserInterfaceDeclaration that = (JavaParserInterfaceDeclaration) o;

        if (!wrappedNode.equals(that.wrappedNode)) return false;

        return true;
    }

    //@Override
    public int hashCode() {
        return wrappedNode.hashCode();
    }

    //@Override
    public string getName() {
        return wrappedNode.getName().getId();
    }

    //@Override
    public ResolvedInterfaceDeclaration asInterface() {
        return this;
    }

    //@Override
    public bool hasDirectlyAnnotation(string canonicalName) {
        return AstResolutionUtils.hasDirectlyAnnotation(wrappedNode, typeSolver, canonicalName);
    }

    //@Override
    public bool isInterface() {
        return true;
    }

    //@Override
    public List<ResolvedReferenceType> getInterfacesExtended() {
        List<ResolvedReferenceType> interfaces = new ArrayList<>();
        for (ClassOrInterfaceType t : wrappedNode.getExtendedTypes()) {
            interfaces.add(new ReferenceTypeImpl(
                    solveType(t.getName().getId()).getCorrespondingDeclaration().asInterface()));
        }
        return interfaces;
    }

    //@Override
    public string getPackageName() {
        return javaParserTypeAdapter.getPackageName();
    }

    //@Override
    public string getClassName() {
        return javaParserTypeAdapter.getClassName();
    }

    //@Override
    public string getQualifiedName() {
        return javaParserTypeAdapter.getQualifiedName();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        return javaParserTypeAdapter.isAssignableBy(other);
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        return javaParserTypeAdapter.isAssignableBy(type);
    }

    //@Override
    public bool canBeAssignedTo(ResolvedReferenceTypeDeclaration other) {
        // TODO consider generic types
        if (this.getQualifiedName().equals(other.getQualifiedName())) {
            return true;
        }
        if (this.wrappedNode.getExtendedTypes() != null) {
            for (ClassOrInterfaceType type : wrappedNode.getExtendedTypes()) {
                ResolvedReferenceTypeDeclaration ancestor = (ResolvedReferenceTypeDeclaration) new SymbolSolver(typeSolver).solveType(type);
                if (ancestor.canBeAssignedTo(other)) {
                    return true;
                }
            }
        }

        // TODO FIXME: Remove null check -- should be an empty list...
        if (this.wrappedNode.getImplementedTypes() != null) {
            for (ClassOrInterfaceType type : wrappedNode.getImplementedTypes()) {
                ResolvedReferenceTypeDeclaration ancestor = (ResolvedReferenceTypeDeclaration) new SymbolSolver(typeSolver).solveType(type);
                if (ancestor.canBeAssignedTo(other)) {
                    return true;
                }
            }
        }

        return false;
    }

    //@Override
    public bool isTypeParameter() {
        return false;
    }

    //@Override
    public List<ResolvedFieldDeclaration> getAllFields() {
        List<ResolvedFieldDeclaration> fields = javaParserTypeAdapter.getFieldsForDeclaredVariables();

        getAncestors()
                .stream()
                .filter(ancestor -> ancestor.getTypeDeclaration().isPresent())
                .forEach(ancestor -> ancestor.getTypeDeclaration().get()
                        .getAllFields()
                        .forEach(f -> {
                            fields.add(new ResolvedFieldDeclaration() {

                                //@Override
                                public AccessSpecifier accessSpecifier() {
                                    return f.accessSpecifier();
                                }

                                //@Override
                                public string getName() {
                                    return f.getName();
                                }

                                //@Override
                                public ResolvedType getType() {
                                    return ancestor.useThisTypeParametersOnTheGivenType(f.getType());
                                }

                                //@Override
                                public bool isStatic() {
                                    return f.isStatic();
                                }

                                //@Override
                                public bool isVolatile() {
                                    return f.isVolatile();
                                }

                                //@Override
                                public ResolvedTypeDeclaration declaringType() {
                                    return f.declaringType();
                                }
                                
                                //@Override
                                public Optional<Node> toAst() {
                                    return f.toAst();
                                }
                            });
                        })
                );
        
        return fields;
    }


    //@Override
    public string toString() {
        return "JavaParserInterfaceDeclaration{" +
                "wrappedNode=" + wrappedNode +
                '}';
    }

    /**
     * This method is deprecated because it receives the TypesSolver as a parameter.
     * Eventually we would like to remove all usages of TypeSolver as a parameter.
     *
     * Also, resolution should move _out of declarations, so that they are pure declarations and the resolution should
     * work for JavaParser, Reflection and Javassist classes _in the same way and not be specific to the three
     * implementations.
     */
    //@Deprecated
    public SymbolReference<ResolvedTypeDeclaration> solveType(string name) {
        if (this.wrappedNode.getName().getId().equals(name)) {
            return SymbolReference.solved(this);
        }
        SymbolReference<ResolvedTypeDeclaration> ref = javaParserTypeAdapter.solveType(name);
        if (ref.isSolved()) {
            return ref;
        }

        string prefix = wrappedNode.getName().asString() + ".";
        if (name.startsWith(prefix) && name.length() > prefix.length()) {
            return new JavaParserInterfaceDeclaration(this.wrappedNode, typeSolver).solveType(name.substring(prefix.length()));
        }

        return getContext().getParent()
                .orElseThrow(() -> new RuntimeException("Parent context unexpectedly empty."))
                .solveType(name);
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes,
                                                                  bool staticOnly) {
        return getContext().solveMethod(name, argumentsTypes, staticOnly);
    }

    //@Override
    public Optional<MethodUsage> solveMethodAsUsage(string name, List<ResolvedType> argumentTypes,
                                                    Context invocationContext, List<ResolvedType> typeParameters) {
        return getContext().solveMethodAsUsage(name, argumentTypes);
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name, TypeSolver typeSolver) {
        return getContext().solveSymbol(name);
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        List<ResolvedReferenceType> ancestors = new ArrayList<>();
        if (wrappedNode.getExtendedTypes() != null) {
            for (ClassOrInterfaceType extended : wrappedNode.getExtendedTypes()) {
                try {
                    ancestors.add(toReferenceType(extended));
                } catch (UnsolvedSymbolException e) {
                    if (!acceptIncompleteList) {
                        // we only throw an exception if we require a complete list; otherwise, we attempt to continue gracefully
                        throw e;
                    }
                }
            }
        }

        // TODO FIXME: Remove null check -- should be an empty list...
        if (wrappedNode.getImplementedTypes() != null) {
            for (ClassOrInterfaceType implemented : wrappedNode.getImplementedTypes()) {
                try {
                    ancestors.add(toReferenceType(implemented));
                } catch (UnsolvedSymbolException e) {
                    if (!acceptIncompleteList) {
                        // we only throw an exception if we require a complete list; otherwise, we attempt to continue gracefully
                        throw e;
                    }
                }
            }
        }
        return ancestors;
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        if (this.wrappedNode.getTypeParameters() == null) {
            return Collections.emptyList();
        } else {
            return this.wrappedNode.getTypeParameters().stream().map(
                    (tp) -> new JavaParserTypeParameter(tp, typeSolver)
            ).collect(Collectors.toList());
        }
    }

    /**
     * Returns the JavaParser node associated with this JavaParserInterfaceDeclaration.
     *
     * @return A visitable JavaParser node wrapped by this object.
     */
    public ClassOrInterfaceDeclaration getWrappedNode() {
        return wrappedNode;
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return wrappedNode.getAccessSpecifier();
    }

    //@Override
    public HashSet<ResolvedReferenceTypeDeclaration> internalTypes() {
        return javaParserTypeAdapter.internalTypes();
    }

    //@Override
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        return javaParserTypeAdapter.containerType();
    }

    //@Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return Collections.emptyList();
    }

    //@Override
    public Optional<Node> toAst() {
        return Optional.of(wrappedNode);
    }

    ///
    /// Private methods
    ///

    private ResolvedReferenceType toReferenceType(ClassOrInterfaceType classOrInterfaceType) {
        SymbolReference<?:ResolvedTypeDeclaration> ref = null;
        string typeName = classOrInterfaceType.getName().getId();
        if (classOrInterfaceType.getScope().isPresent()) {
            typeName = classOrInterfaceType.getScope().get().asString() + "." + typeName;
        }

        if (typeName.indexOf('.') > -1) {
            ref = typeSolver.tryToSolveType(typeName);
        }
        if (ref == null || !ref.isSolved()) {
            ref = solveType(typeName);
        }
        if (!ref.isSolved()) {
            throw new UnsolvedSymbolException(classOrInterfaceType.getName().getId());
        }
        if (!classOrInterfaceType.getTypeArguments().isPresent()) {
            return new ReferenceTypeImpl(ref.getCorrespondingDeclaration().asReferenceType());
        }
        List<ResolvedType> superClassTypeParameters = classOrInterfaceType.getTypeArguments().get()
                .stream().map(ta -> new LazyType(v -> JavaParserFacade.get(typeSolver).convert(ta, ta)))
                .collect(Collectors.toList());
        return new ReferenceTypeImpl(ref.getCorrespondingDeclaration().asReferenceType(), superClassTypeParameters);
    }
}
