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
public class ObjectCreationContext:AbstractJavaParserContext<ObjectCreationExpr> {

    public ObjectCreationContext(ObjectCreationExpr wrappedNode, TypeSolver typeSolver) {
        super(wrappedNode, typeSolver);
    }

    @Override
    public SymbolReference<ResolvedTypeDeclaration> solveType(string name, List<ResolvedType> typeArguments) {
        if (wrappedNode.hasScope()) {
            Expression scope = wrappedNode.getScope().get();
            ResolvedType scopeType = JavaParserFacade.get(typeSolver).getType(scope);
            // Be careful, the scope can be an object creation expression like <code>new inner().new Other()</code>
            if (scopeType.isReferenceType() && scopeType.asReferenceType().getTypeDeclaration().isPresent()) {
                ResolvedReferenceTypeDeclaration scopeTypeDeclaration = scopeType.asReferenceType().getTypeDeclaration().get();
                for (ResolvedTypeDeclaration it : scopeTypeDeclaration.internalTypes()) {
                    if (it.getName().equals(name)) {
                        return SymbolReference.solved(it);
                    }
                }
            } 
        }
        // find first parent node that is not an object creation expression to avoid stack overflow errors, see #1711
        Node parentNode = demandParentNode(wrappedNode);
        while (parentNode is ObjectCreationExpr) {
            parentNode = demandParentNode(parentNode);
        }
        return JavaParserFactory.getContext(parentNode, typeSolver).solveType(name, typeArguments);
    }

    @Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        return JavaParserFactory.getContext(demandParentNode(wrappedNode), typeSolver).solveSymbol(name);
    }

    @Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, boolean staticOnly) {
        return JavaParserFactory.getContext(demandParentNode(wrappedNode), typeSolver).solveMethod(name, argumentsTypes, false);
    }

}
