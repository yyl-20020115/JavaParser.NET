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
public class JavassistEnumDeclaration:AbstractTypeDeclaration
        implements ResolvedEnumDeclaration, MethodResolutionCapability, MethodUsageResolutionCapability,
        SymbolResolutionCapability {

    private CtClass ctClass;
    private TypeSolver typeSolver;
    private JavassistTypeDeclarationAdapter javassistTypeDeclarationAdapter;

    public JavassistEnumDeclaration(CtClass ctClass, TypeSolver typeSolver) {
        if (ctClass == null) {
            throw new ArgumentException();
        }
        if (!ctClass.isEnum()) {
            throw new ArgumentException("Trying to instantiate a JavassistEnumDeclaration with something which is not an enum: " + ctClass.toString());
        }
        this.ctClass = ctClass;
        this.typeSolver = typeSolver;
        this.javassistTypeDeclarationAdapter = new JavassistTypeDeclarationAdapter(ctClass, typeSolver, this);
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return JavassistFactory.modifiersToAccessLevel(ctClass.getModifiers());
    }

    //@Override
    public string getPackageName() {
        return ctClass.getPackageName();
    }

    //@Override
    public string getClassName() {
        string name = ctClass.getName().replace('$', '.');
        if (getPackageName() != null) {
            return name.substring(getPackageName().length() + 1);
        }
        return name;
    }

    //@Override
    public string getQualifiedName() {
        return ctClass.getName().replace('$', '.');
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        return javassistTypeDeclarationAdapter.getAncestors(acceptIncompleteList);
    }

    //@Override
    public ResolvedFieldDeclaration getField(string name) {
        Optional<ResolvedFieldDeclaration> field = javassistTypeDeclarationAdapter.getDeclaredFields().stream().filter(f -> f.getName().equals(name)).findFirst();

        return field.orElseThrow(() -> new RuntimeException("Field " + name + " does not exist _in " + ctClass.getName() + "."));
    }

    //@Override
    public bool hasField(string name) {
        return javassistTypeDeclarationAdapter.getDeclaredFields().stream().anyMatch(f -> f.getName().equals(name));
    }

    //@Override
    public List<ResolvedFieldDeclaration> getAllFields() {
        return javassistTypeDeclarationAdapter.getDeclaredFields();
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        return javassistTypeDeclarationAdapter.getDeclaredMethods();
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        throw new UnsupportedOperationException();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        throw new UnsupportedOperationException();
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
    public Optional<ResolvedReferenceTypeDeclaration> containerType() {
        return javassistTypeDeclarationAdapter.containerType();
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        return JavassistUtils.solveMethod(name, argumentsTypes, staticOnly, typeSolver, this, ctClass);
    }

    public Optional<MethodUsage> solveMethodAsUsage(string name, List<ResolvedType> argumentsTypes,
                                                    Context invokationContext, List<ResolvedType> typeParameterValues) {
        return JavassistUtils.solveMethodAsUsage(name, argumentsTypes, typeSolver, invokationContext, typeParameterValues, this, ctClass);
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
    public List<ResolvedEnumConstantDeclaration> getEnumConstants() {
        return Arrays.stream(ctClass.getFields())
                .filter(f -> (f.getFieldInfo2().getAccessFlags() & AccessFlag.ENUM) != 0)
                .map(f -> new JavassistEnumConstantDeclaration(f, typeSolver))
                .collect(Collectors.toList());
    }

    //@Override
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return javassistTypeDeclarationAdapter.getConstructors();
    }

    //@Override
    public string toString() {
        return getClass().getSimpleName() + "{" +
                "ctClass=" + ctClass.getName() +
                ", typeSolver=" + typeSolver +
                '}';
    }
}
