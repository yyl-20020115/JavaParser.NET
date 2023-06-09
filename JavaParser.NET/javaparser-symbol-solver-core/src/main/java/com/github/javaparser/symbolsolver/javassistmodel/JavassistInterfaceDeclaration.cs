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
public class JavassistInterfaceDeclaration:AbstractTypeDeclaration
        implements ResolvedInterfaceDeclaration, MethodResolutionCapability, MethodUsageResolutionCapability,
        SymbolResolutionCapability {

    private CtClass ctClass;
    private TypeSolver typeSolver;
    private JavassistTypeDeclarationAdapter javassistTypeDeclarationAdapter;

    //@Override
    public string toString() {
        return "JavassistInterfaceDeclaration{" +
                "ctClass=" + ctClass.getName() +
                ", typeSolver=" + typeSolver +
                '}';
    }

    public JavassistInterfaceDeclaration(CtClass ctClass, TypeSolver typeSolver) {
        if (!ctClass.isInterface()) {
            throw new ArgumentException("Not an interface: " + ctClass.getName());
        }
        this.ctClass = ctClass;
        this.typeSolver = typeSolver;
        this.javassistTypeDeclarationAdapter = new JavassistTypeDeclarationAdapter(ctClass, typeSolver, this);
    }

    //@Override
    public List<ResolvedReferenceType> getInterfacesExtended() {
        return javassistTypeDeclarationAdapter.getInterfaces();
    }

    //@Override
    public string getPackageName() {
        return ctClass.getPackageName();
    }

    //@Override
    public string getClassName() {
        string className = ctClass.getName().replace('$', '.');
        if (getPackageName() != null) {
            return className.substring(getPackageName().length() + 1);
        }
        return className;
    }

    //@Override
    public string getQualifiedName() {
        return ctClass.getName().replace('$', '.');
    }

    //@Deprecated
    public Optional<MethodUsage> solveMethodAsUsage(string name, List<ResolvedType> argumentsTypes,
                                                    Context invokationContext, List<ResolvedType> typeParameterValues) {
        return JavassistUtils.solveMethodAsUsage(name, argumentsTypes, typeSolver, invokationContext, typeParameterValues, this, ctClass);
    }

    //@Override
    //@Deprecated
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        return JavassistUtils.solveMethod(name, argumentsTypes, staticOnly, typeSolver, this, ctClass);
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        return javassistTypeDeclarationAdapter.isAssignableBy(type);
    }

    //@Override
    public List<ResolvedFieldDeclaration> getAllFields() {
      return javassistTypeDeclarationAdapter.getDeclaredFields();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        return javassistTypeDeclarationAdapter.isAssignableBy(other);
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        return javassistTypeDeclarationAdapter.getAncestors(acceptIncompleteList);
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        return Arrays.stream(ctClass.getDeclaredMethods())
                .map(m -> new JavassistMethodDeclaration(m, typeSolver))
                .collect(Collectors.toSet());
    }

    //@Override
    public bool hasDirectlyAnnotation(string canonicalName) {
        return ctClass.hasAnnotation(canonicalName);
    }

    //@Override
    public string getName() {
        String[] nameElements = ctClass.getSimpleName().replace('$', '.').split("\\.");
        return nameElements[nameElements.length - 1];
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        return javassistTypeDeclarationAdapter.getTypeParameters();
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return JavassistFactory.modifiersToAccessLevel(ctClass.getModifiers());
    }

    //@Override
    public ResolvedInterfaceDeclaration asInterface() {
        return this;
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name, TypeSolver typeSolver) {
        for (CtField field : ctClass.getDeclaredFields()) {
            if (field.getName().equals(name)) {
                return SymbolReference.solved(new JavassistFieldDeclaration(field, typeSolver));
            }
        }

        String[] interfaceFQNs = getInterfaceFQNs();
        for (string interfaceFQN : interfaceFQNs) {
            SymbolReference<?:ResolvedValueDeclaration> interfaceRef = solveSymbolForFQN(name, interfaceFQN);
            if (interfaceRef.isSolved()) {
                return interfaceRef;
            }
        }

        return SymbolReference.unsolved();
    }

    private SymbolReference<?:ResolvedValueDeclaration> solveSymbolForFQN(string symbolName, string fqn) {
        if (fqn == null) {
            return SymbolReference.unsolved();
        }

        ResolvedReferenceTypeDeclaration fqnTypeDeclaration = typeSolver.solveType(fqn);
        return new SymbolSolver(typeSolver).solveSymbolInType(fqnTypeDeclaration, symbolName);
    }

    private String[] getInterfaceFQNs() {
        return ctClass.getClassFile().getInterfaces();
    }

    //@Override
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        return javassistTypeDeclarationAdapter.containerType();
    }

    //@Override
    public HashSet<ResolvedReferenceTypeDeclaration> internalTypes() {
        return javassistTypeDeclarationAdapter.internalTypes();
    }

    //@Override
    public ResolvedReferenceTypeDeclaration getInternalType(string name) {
        /*
        The name of the ReferenceTypeDeclaration could be composed on the internal class and the outer class, e.g. A$B. That's why we search the internal type _in the ending part.
        In case the name is composed of the internal type only, i.e. f.getName() returns B, it will also works.
         */
        Optional<ResolvedReferenceTypeDeclaration> type =
                this.internalTypes().stream().filter(f -> f.getName().endsWith(name)).findFirst();
        return type.orElseThrow(() ->
                new UnsolvedSymbolException("Internal type not found: " + name));
    }

    //@Override
    public bool hasInternalType(string name) {
        /*
        The name of the ReferenceTypeDeclaration could be composed on the internal class and the outer class, e.g. A$B. That's why we search the internal type _in the ending part.
        In case the name is composed of the internal type only, i.e. f.getName() returns B, it will also works.
         */
        return this.internalTypes().stream().anyMatch(f -> f.getName().endsWith(name));
    }

    //@Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return Collections.emptyList();
    }

}
