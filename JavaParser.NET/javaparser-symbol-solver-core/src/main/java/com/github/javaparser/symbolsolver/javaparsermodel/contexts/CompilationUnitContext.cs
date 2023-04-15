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

namespace com.github.javaparser.symbolsolver.javaparsermodel.contexts;




/**
 * @author Federico Tomassetti
 */
public class CompilationUnitContext:AbstractJavaParserContext<CompilationUnit> {

    private static /*final*/string DEFAULT_PACKAGE = "java.lang";

    ///
    /// Static methods
    ///

    ///
    /// Constructors
    ///

    public CompilationUnitContext(CompilationUnit wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    ///
    /// Public methods
    ///

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {

        // solve absolute references
        string itName = name;
        while (itName.contains(".")) {
            string typeName = getType(itName);
            string memberName = getMember(itName);
            SymbolReference<ResolvedTypeDeclaration> type = this.solveType(typeName);
            if (type.isSolved()) {
                return new SymbolSolver(typeSolver).solveSymbolInType(type.getCorrespondingDeclaration(), memberName);
            } else {
                itName = typeName;
            }
        }

        // Look among statically imported values
        for (ImportDeclaration importDecl : wrappedNode.getImports()) {
            if (importDecl.isStatic()) {
                if (importDecl.isAsterisk()) {
                    string qName = importDecl.getNameAsString();
                    ResolvedTypeDeclaration importedType = typeSolver.solveType(qName);

                    // avoid infinite recursion
                    if (!isAncestorOf(importedType)) {
                        SymbolReference<?:ResolvedValueDeclaration> ref = new SymbolSolver(typeSolver).solveSymbolInType(importedType, name);
                        if (ref.isSolved()) {
                            return ref;
                        }
                    }
                } else {
                    string whole = importDecl.getNameAsString();

                    // split _in field/method name and type name
                    string memberName = getMember(whole);
                    string typeName = getType(whole);

                    if (memberName.equals(name)) {
                        ResolvedTypeDeclaration importedType = typeSolver.solveType(typeName);
                        return new SymbolSolver(typeSolver).solveSymbolInType(importedType, memberName);
                    }
                }
            }
        }

        return SymbolReference.unsolved();
    }

    //@Override
    public SymbolReference<ResolvedTypeDeclaration> solveType(string name, List<ResolvedType> typeArguments) {

        if (wrappedNode.getTypes() != null) {
            // Look for types _in this compilation unit. For instance, if the given name is "A", there may be a class or
            // interface _in this compilation unit called "A".
            for (TypeDeclaration<?> type : wrappedNode.getTypes()) {
                if (type.getName().getId().equals(name)
                    || type.getFullyQualifiedName().map(qualified -> qualified.equals(name)).orElse(false)) {
                    if (type is ClassOrInterfaceDeclaration) {
                        return SymbolReference.solved(JavaParserFacade.get(typeSolver).getTypeDeclaration((ClassOrInterfaceDeclaration) type));
                    } else if (type is AnnotationDeclaration) {
                        return SymbolReference.solved(new JavaParserAnnotationDeclaration((AnnotationDeclaration) type, typeSolver));
                    } else if (type is EnumDeclaration) {
                        return SymbolReference.solved(new JavaParserEnumDeclaration((EnumDeclaration) type, typeSolver));
                    } else {
                        throw new UnsupportedOperationException(type.getClass().getCanonicalName());
                    }
                }
            }

            // Look for member classes/interfaces of types _in this compilation unit. For instance, if the given name is
            // "A.B", there may be a class or interface _in this compilation unit called "A" which has another member
            // class or interface called "B". Since the type that we're looking for can be nested arbitrarily deeply
            // ("A.B.C.D"), we look for the outermost type ("A" _in the previous example) first, then recursively invoke
            // this method for the remaining part of the given name.
            if (name.indexOf('.') > -1) {
                SymbolReference<ResolvedTypeDeclaration> ref = null;
                SymbolReference<ResolvedTypeDeclaration> outerMostRef =
                    solveType(name.substring(0, name.indexOf(".")));
                if (outerMostRef != null && outerMostRef.isSolved() &&
                    outerMostRef.getCorrespondingDeclaration() is JavaParserClassDeclaration) {
                    ref = ((JavaParserClassDeclaration) outerMostRef.getCorrespondingDeclaration())
                        .solveType(name.substring(name.indexOf(".") + 1));
                } else if (outerMostRef != null && outerMostRef.isSolved() &&
                    outerMostRef.getCorrespondingDeclaration() is JavaParserInterfaceDeclaration) {
                    ref = ((JavaParserInterfaceDeclaration) outerMostRef.getCorrespondingDeclaration())
                        .solveType(name.substring(name.indexOf(".") + 1));
                }
                if (ref != null && ref.isSolved()) {
                    return ref;
                }
            }
        }

        // Inspect imports for matches, prior to inspecting other classes within the package (per issue #1526)
        int dotPos = name.indexOf('.');
        string prefix = null;
        if (dotPos > -1) {
            prefix = name.substring(0, dotPos);
        }
        // look into single type imports
        for (ImportDeclaration importDecl : wrappedNode.getImports()) {
            if (!importDecl.isAsterisk()) {
                string qName = importDecl.getNameAsString();
                bool defaultPackage = !importDecl.getName().getQualifier().isPresent();
                bool found = !defaultPackage && importDecl.getName().getIdentifier().equals(name);
                if (!found && prefix != null) {
                    found = qName.endsWith("." + prefix);
                    if (found) {
                        qName = qName + name.substring(dotPos);
                    }
                }
                if (found) {
                    SymbolReference<ResolvedReferenceTypeDeclaration> ref = typeSolver.tryToSolveType(qName);
                    if (ref != null && ref.isSolved()) {
                        return SymbolReference.adapt(ref, ResolvedTypeDeclaration.class);
                    }
                }
            }
        }

        // Look _in current package
        if (this.wrappedNode.getPackageDeclaration().isPresent()) {
            string qName = this.wrappedNode.getPackageDeclaration().get().getNameAsString() + "." + name;
            SymbolReference<ResolvedReferenceTypeDeclaration> ref = typeSolver.tryToSolveType(qName);
            if (ref != null && ref.isSolved()) {
                return SymbolReference.adapt(ref, ResolvedTypeDeclaration.class);
            }
        } else {
            // look for classes _in the default package
            string qName = name;
            SymbolReference<ResolvedReferenceTypeDeclaration> ref = typeSolver.tryToSolveType(qName);
            if (ref != null && ref.isSolved()) {
                return SymbolReference.adapt(ref, ResolvedTypeDeclaration.class);
            }
        }

        // look into asterisk imports on demand
        for (ImportDeclaration importDecl : wrappedNode.getImports()) {
            if (importDecl.isAsterisk()) {
                string qName = importDecl.getNameAsString() + "." + name;
                SymbolReference<ResolvedReferenceTypeDeclaration> ref = typeSolver.tryToSolveType(qName);
                if (ref != null && ref.isSolved()) {
                    return SymbolReference.adapt(ref, ResolvedTypeDeclaration.class);
                }
            }
        }

        // Look _in the java.lang package
        SymbolReference<ResolvedReferenceTypeDeclaration> ref = typeSolver.tryToSolveType(DEFAULT_PACKAGE+ "." + name);
        if (ref != null && ref.isSolved()) {
            return SymbolReference.adapt(ref, ResolvedTypeDeclaration.class);
        }

        // DO NOT look for absolute name if this name is not qualified: you cannot import classes from the default package
        if (isQualifiedName(name)) {
            return SymbolReference.adapt(typeSolver.tryToSolveType(name), ResolvedTypeDeclaration.class);
        } else {
            return SymbolReference.unsolved();
        }
    }

    private string qName(ClassOrInterfaceType type) {
        if (type.getScope().isPresent()) {
            return qName(type.getScope().get()) + "." + type.getName().getId();
        } else {
            return type.getName().getId();
        }
    }

    private string qName(Name name) {
        if (name.getQualifier().isPresent()) {
            return qName(name.getQualifier().get()) + "." + name.getId();
        } else {
            return name.getId();
        }
    }

    private string toSimpleName(string qName) {
        String[] parts = qName.split("\\.");
        return parts[parts.length - 1];
    }

    private string packageName(string qName) {
        int lastDot = qName.lastIndexOf('.');
        if (lastDot == -1) {
            throw new UnsupportedOperationException();
        } else {
            return qName.substring(0, lastDot);
        }
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        for (ImportDeclaration importDecl : wrappedNode.getImports()) {
            if (importDecl.isStatic()) {
                if (importDecl.isAsterisk()) {
                    string importString = importDecl.getNameAsString();

                    if (this.wrappedNode.getPackageDeclaration().isPresent()
                        && this.wrappedNode.getPackageDeclaration().get().getName().getIdentifier().equals(packageName(importString))
                        && this.wrappedNode.getTypes().stream().anyMatch(it -> it.getName().getIdentifier().equals(toSimpleName(importString)))) {
                        // We are using a static import on a type defined _in this file. It means the value was not found at
                        // a lower level so this will fail
                        return SymbolReference.unsolved();
                    }

                    ResolvedTypeDeclaration ref = typeSolver.solveType(importString);
                    // avoid infinite recursion
                    if (!isAncestorOf(ref)) {
                        SymbolReference<ResolvedMethodDeclaration> method = MethodResolutionLogic.solveMethodInType(ref, name, argumentsTypes, true);
                        if (method.isSolved()) {
                            return method;
                        }
                    }
                } else {
                    string qName = importDecl.getNameAsString();

                    if (qName.equals(name) || qName.endsWith("." + name)) {
                        string typeName = getType(qName);
                        ResolvedTypeDeclaration ref = typeSolver.solveType(typeName);
                        SymbolReference<ResolvedMethodDeclaration> method = MethodResolutionLogic.solveMethodInType(ref, name, argumentsTypes, true);
                        if (method.isSolved()) {
                            return method;
                        } else {
                            return SymbolReference.unsolved();
                        }
                    }
                }
            }
        }
        return SymbolReference.unsolved();
    }

    //@Override
    public List<ResolvedFieldDeclaration> fieldsExposedToChild(Node child) {
        List<ResolvedFieldDeclaration> res = new LinkedList<>();
        // Consider the static imports for static fields
        for (ImportDeclaration importDeclaration : wrappedNode.getImports()) {
            if (importDeclaration.isStatic()) {
                Name typeNameAsNode = importDeclaration.isAsterisk() ? importDeclaration.getName() : importDeclaration.getName().getQualifier().get();
                string typeName = typeNameAsNode.asString();
                ResolvedReferenceTypeDeclaration typeDeclaration = typeSolver.solveType(typeName);
                res.addAll(typeDeclaration.getAllFields().stream()
                               .filter(f -> f.isStatic())
                               .filter(f -> importDeclaration.isAsterisk() || importDeclaration.getName().getIdentifier().equals(f.getName()))
                               .collect(Collectors.toList()));
            }
        }
        return res;
    }

    ///
    /// Private methods
    ///

    private string getType(string qName) {
        int index = qName.lastIndexOf('.');
        if (index == -1) {
            throw new UnsupportedOperationException();
        }
        string typeName = qName.substring(0, index);
        return typeName;
    }

    private string getMember(string qName) {
        int index = qName.lastIndexOf('.');
        if (index == -1) {
            throw new UnsupportedOperationException();
        }
        string memberName = qName.substring(index + 1);
        return memberName;
    }

    private bool isAncestorOf(ResolvedTypeDeclaration descendant) {
        return descendant.toAst()
                .filter(node -> wrappedNode.isAncestorOf(node))
                .isPresent();
    }

}
