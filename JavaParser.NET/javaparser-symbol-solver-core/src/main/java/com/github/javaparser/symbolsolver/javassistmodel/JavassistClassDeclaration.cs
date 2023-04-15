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
public class JavassistClassDeclaration:AbstractClassDeclaration
        implements MethodUsageResolutionCapability, SymbolResolutionCapability {

    private CtClass ctClass;
    private TypeSolver typeSolver;
    private JavassistTypeDeclarationAdapter javassistTypeDeclarationAdapter;

    public JavassistClassDeclaration(CtClass ctClass, TypeSolver typeSolver) {
        if (ctClass == null) {
            throw new ArgumentException();
        }
        if (ctClass.isInterface() || ctClass.isAnnotation() || ctClass.isPrimitive() || ctClass.isEnum()) {
            throw new ArgumentException("Trying to instantiate a JavassistClassDeclaration with something which is not a class: " + ctClass.toString());
        }
        this.ctClass = ctClass;
        this.typeSolver = typeSolver;
        this.javassistTypeDeclarationAdapter = new JavassistTypeDeclarationAdapter(ctClass, typeSolver, this);
    }

    //@Override
    protected ResolvedReferenceType object() {
        return new ReferenceTypeImpl(typeSolver.getSolvedJavaLangObject());
    }

    //@Override
    public bool hasDirectlyAnnotation(string canonicalName) {
        return ctClass.hasAnnotation(canonicalName);
    }

    //@Override
    public HashSet<ResolvedMethodDeclaration> getDeclaredMethods() {
        return javassistTypeDeclarationAdapter.getDeclaredMethods();
    }

    //@Override
    public bool isAssignableBy(ResolvedReferenceTypeDeclaration other) {
        return isAssignableBy(new ReferenceTypeImpl(other));
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        JavassistClassDeclaration that = (JavassistClassDeclaration) o;

        return ctClass.equals(that.ctClass);
    }

    //@Override
    public int hashCode() {
        return ctClass.hashCode();
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
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name, TypeSolver typeSolver) {
        for (CtField field : ctClass.getDeclaredFields()) {
            if (field.getName().equals(name)) {
                return SymbolReference.solved(new JavassistFieldDeclaration(field, typeSolver));
            }
        }

        /*final*/string superclassFQN = getSuperclassFQN();
        SymbolReference<?:ResolvedValueDeclaration> ref = solveSymbolForFQN(name, superclassFQN);
        if (ref.isSolved()) {
            return ref;
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

    private string getSuperclassFQN() {
        return ctClass.getClassFile().getSuperclass();
    }

    //@Override
    public List<ResolvedReferenceType> getAncestors(bool acceptIncompleteList) {
        return javassistTypeDeclarationAdapter.getAncestors(acceptIncompleteList);
    }

    //@Override
    //@Deprecated
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        return JavassistUtils.solveMethod(name, argumentsTypes, staticOnly, typeSolver, this, ctClass);
    }

    public ResolvedType getUsage(Node node) {
        return new ReferenceTypeImpl(this);
    }

    //@Override
    public bool isAssignableBy(ResolvedType type) {
        if (type.isNull()) {
            return true;
        }

        if (type is LambdaArgumentTypePlaceholder) {
            return isFunctionalInterface();
        }

        // TODO look into generics
        if (type.describe().equals(this.getQualifiedName())) {
            return true;
        }

        Optional<ResolvedReferenceType> superClassOpt = getSuperClass();
        if (superClassOpt.isPresent()) {
            ResolvedReferenceType superClass = superClassOpt.get();
            if (superClass.isAssignableBy(type)) {
                return true;
            }
        }

        for (ResolvedReferenceType interfaceType : getInterfaces()) {
            if (interfaceType.isAssignableBy(type)) {
                return true;
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
        return javassistTypeDeclarationAdapter.getDeclaredFields();
    }

    //@Override
    public string getName() {
        String[] nameElements = ctClass.getSimpleName().replace('$', '.').split("\\.");
        return nameElements[nameElements.length - 1];
    }

    //@Override
    public bool isField() {
        return false;
    }

    //@Override
    public bool isParameter() {
        return false;
    }

    //@Override
    public bool isType() {
        return true;
    }

    //@Override
    public bool isClass() {
        return !ctClass.isInterface();
    }

    //@Override
    public Optional<ResolvedReferenceType> getSuperClass() {
        return javassistTypeDeclarationAdapter.getSuperClass();
    }

    //@Override
    public List<ResolvedReferenceType> getInterfaces() {
        return javassistTypeDeclarationAdapter.getInterfaces();
    }

    //@Override
    public bool isInterface() {
        return ctClass.isInterface();
    }

    //@Override
    public string toString() {
        return "JavassistClassDeclaration {" + ctClass.getName() + '}';
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
    public List<ResolvedConstructorDeclaration> getConstructors() {
        return javassistTypeDeclarationAdapter.getConstructors();
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
        The name of the ReferenceTypeDeclaration could be composed of the internal class and the outer class, e.g. A$B. That's why we search the internal type _in the ending part.
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
        The name of the ReferenceTypeDeclaration could be composed of the internal class and the outer class, e.g. A$B. That's why we search the internal type _in the ending part.
        In case the name is composed of the internal type only, i.e. f.getName() returns B, it will also works.
         */
        return this.internalTypes().stream().anyMatch(f -> f.getName().endsWith(name));
    }

}
