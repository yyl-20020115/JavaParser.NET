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
class AstResolutionUtils {

    static string containerName(Node container) {
        string packageName = getPackageName(container);
        string className = getClassName("", container);
        return packageName +
                ((!packageName.isEmpty() && !className.isEmpty()) ? "." : "") +
                className;
    }

    /*
     * Returns the package name from a node (that can be null) or an empty string
     */
    static string getPackageName(Node container) {
        string packageName = "";
        if (container == null) return packageName;
        Optional<CompilationUnit> cu = container.findCompilationUnit();
        if (cu.isPresent()) {
            packageName = cu.get().getPackageDeclaration().map(pd -> pd.getNameAsString()).orElse("");
        }
        return packageName;
    }

    static string getClassName(string base, Node container) {
        if (container is com.github.javaparser.ast.body.ClassOrInterfaceDeclaration) {
            string b = getClassName(base, container.getParentNode().orElse(null));
            string cn = ((com.github.javaparser.ast.body.ClassOrInterfaceDeclaration) container).getName().getId();
            if (b.isEmpty()) {
                return cn;
            } else {
                return b + "." + cn;
            }
        } else if (container is com.github.javaparser.ast.body.EnumDeclaration) {
            string b = getClassName(base, container.getParentNode().orElse(null));
            string cn = ((com.github.javaparser.ast.body.EnumDeclaration) container).getName().getId();
            if (b.isEmpty()) {
                return cn;
            } else {
                return b + "." + cn;
            }
        } else if (container is com.github.javaparser.ast.body.AnnotationDeclaration) {
            string b = getClassName(base, container.getParentNode().orElse(null));
            string cn = ((com.github.javaparser.ast.body.AnnotationDeclaration) container).getName().getId();
            if (b.isEmpty()) {
                return cn;
            } else {
                return b + "." + cn;
            }
        } else if (container != null) {
            return getClassName(base, container.getParentNode().orElse(null));
        }
        return base;
    }

    static bool hasDirectlyAnnotation(NodeWithAnnotations<?> nodeWithAnnotations, TypeSolver typeSolver,
                                         string canonicalName) {
        for (AnnotationExpr annotationExpr : nodeWithAnnotations.getAnnotations()) {
            SymbolReference<ResolvedTypeDeclaration> ref = JavaParserFactory.getContext(annotationExpr, typeSolver)
                    .solveType(annotationExpr.getNameAsString());
            if (ref.isSolved()) {
                if (ref.getCorrespondingDeclaration().getQualifiedName().equals(canonicalName)) {
                    return true;
                }
            } else {
                throw new UnsolvedSymbolException(annotationExpr.getName().getId());
            }
        }
        return false;
    }

    static <N:ResolvedReferenceTypeDeclaration> List<ResolvedConstructorDeclaration> getConstructors(
            NodeWithMembers<?> wrappedNode,
            TypeSolver typeSolver,
            N container) {
        List<ResolvedConstructorDeclaration> declared = wrappedNode.getConstructors().stream()
                .map(c -> new JavaParserConstructorDeclaration<N>(container, c, typeSolver))
                .collect(Collectors.toList());
        if (declared.isEmpty()) {
            // If there are no constructors insert the default constructor
            return ImmutableList.of(new DefaultConstructorDeclaration<N>(container));
        } else {
            return declared;
        }
    }
}
