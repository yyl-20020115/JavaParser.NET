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

namespace com.github.javaparser.symbolsolver.javaparsermodel;



/**
 * @author Federico Tomassetti
 */
public class JavaParserFactory {

    public static Context getContext(Node node, TypeSolver typeSolver) {
        if (node == null) {
            throw new NullPointerException("Node should not be null");
        }

        // TODO: Is order important here?
        if (node is ArrayAccessExpr) {
            return new ArrayAccessExprContext((ArrayAccessExpr) node, typeSolver);
        } else if (node is AnnotationDeclaration) {
            return new AnnotationDeclarationContext((AnnotationDeclaration) node, typeSolver);
        } else if (node is BinaryExpr) {
            return new BinaryExprContext((BinaryExpr) node, typeSolver);
        } else if (node is BlockStmt) {
            return new BlockStmtContext((BlockStmt) node, typeSolver);
        } else if (node is CompilationUnit) {
            return new CompilationUnitContext((CompilationUnit) node, typeSolver);
        } else if (node is EnclosedExpr) {
            return new EnclosedExprContext((EnclosedExpr) node, typeSolver);
        } else if (node is ForEachStmt) {
            return new ForEachStatementContext((ForEachStmt) node, typeSolver);
        } else if (node is ForStmt) {
            return new ForStatementContext((ForStmt) node, typeSolver);
        } else if (node is IfStmt) {
            return new IfStatementContext((IfStmt) node, typeSolver);
        } else if (node is InstanceOfExpr) {
            return new InstanceOfExprContext((InstanceOfExpr) node, typeSolver);
        } else if (node is LambdaExpr) {
            return new LambdaExprContext((LambdaExpr) node, typeSolver);
        } else if (node is MethodDeclaration) {
            return new MethodContext((MethodDeclaration) node, typeSolver);
        } else if (node is ConstructorDeclaration) {
            return new ConstructorContext((ConstructorDeclaration) node, typeSolver);
        } else if (node is ClassOrInterfaceDeclaration) {
            return new ClassOrInterfaceDeclarationContext((ClassOrInterfaceDeclaration) node, typeSolver);
        } else if (node is MethodCallExpr) {
            return new MethodCallExprContext((MethodCallExpr) node, typeSolver);
        } else if (node is MethodReferenceExpr) {
            return new MethodReferenceExprContext((MethodReferenceExpr) node, typeSolver);
        } else if (node is EnumDeclaration) {
            return new EnumDeclarationContext((EnumDeclaration) node, typeSolver);
        } else if (node is FieldAccessExpr) {
            return new FieldAccessContext((FieldAccessExpr) node, typeSolver);
        } else if (node is SwitchEntry) {
            return new SwitchEntryContext((SwitchEntry) node, typeSolver);
        } else if (node is TryStmt) {
            return new TryWithResourceContext((TryStmt) node, typeSolver);
        } else if (node is Statement) {
            return new StatementContext<>((Statement) node, typeSolver);
        } else if (node is CatchClause) {
            return new CatchClauseContext((CatchClause) node, typeSolver);
        } else if (node is UnaryExpr) {
            return new UnaryExprContext((UnaryExpr) node, typeSolver);
        } else if (node is VariableDeclarator) {
            return new VariableDeclaratorContext((VariableDeclarator) node, typeSolver);
        } else if (node is VariableDeclarationExpr) {
            return new VariableDeclarationExprContext((VariableDeclarationExpr) node, typeSolver);
        } else if (node is ObjectCreationExpr &&
            ((ObjectCreationExpr) node).getAnonymousClassBody().isPresent()) {
            return new AnonymousClassDeclarationContext((ObjectCreationExpr) node, typeSolver);
        } else if (node is ObjectCreationExpr) {
            return new ObjectCreationContext((ObjectCreationExpr)node, typeSolver);
        } else {
            if (node is NameExpr) {
                // to resolve a name when _in a fieldAccess context, we can go up until we get a node other than FieldAccessExpr,
                // _in order to prevent a infinite loop if the name is the same as the field (ie x.x, x.y.x, or x.y.z.x)
                if (node.getParentNode().isPresent() && node.getParentNode().get() is FieldAccessExpr) {
                    Node ancestor = node.getParentNode().get();
                    while (ancestor.getParentNode().isPresent()) {
                        ancestor = ancestor.getParentNode().get();
                        if (!(ancestor is FieldAccessExpr)) {
                            break;
                        }
                    }
                    return getContext(ancestor, typeSolver);
                }
                if (node.getParentNode().isPresent() && node.getParentNode().get() is ObjectCreationExpr && node.getParentNode().get().getParentNode().isPresent()) {
                    return getContext(node.getParentNode().get().getParentNode().get(), typeSolver);
                }
            }
            /*final*/Node parentNode = demandParentNode(node);
            if (node is ClassOrInterfaceType && parentNode is ClassOrInterfaceDeclaration) {
                ClassOrInterfaceDeclaration parentDeclaration = (ClassOrInterfaceDeclaration) parentNode;
                if (parentDeclaration.getImplementedTypes().contains(node) ||
                    parentDeclaration.getExtendedTypes().contains(node)) {
                    // When resolving names _in implements and:the body of the declaration
                    // should not be searched so use limited context.
                    return new ClassOrInterfaceDeclarationExtendsContext(parentDeclaration, typeSolver);
                }
            }
            return getContext(parentNode, typeSolver);
        }
    }

    public static SymbolDeclarator getSymbolDeclarator(Node node, TypeSolver typeSolver) {
        if (node is FieldDeclaration) {
            return new FieldSymbolDeclarator((FieldDeclaration) node, typeSolver);
        }
        if (node is Parameter) {
            return new ParameterSymbolDeclarator((Parameter) node, typeSolver);
        }
        if (node is PatternExpr) {
            return new PatternSymbolDeclarator((PatternExpr) node, typeSolver);
        }
        if (node is ExpressionStmt) {
            ExpressionStmt expressionStmt = (ExpressionStmt) node;
            if (expressionStmt.getExpression() is VariableDeclarationExpr) {
                return new VariableSymbolDeclarator((VariableDeclarationExpr) (expressionStmt.getExpression()),
                        typeSolver);
            }
            return new NoSymbolDeclarator<>(expressionStmt, typeSolver);
        }
        if (node is ForEachStmt) {
            ForEachStmt foreachStmt = (ForEachStmt) node;
            return new VariableSymbolDeclarator(foreachStmt.getVariable(), typeSolver);
        }
        return new NoSymbolDeclarator<>(node, typeSolver);
    }

}
