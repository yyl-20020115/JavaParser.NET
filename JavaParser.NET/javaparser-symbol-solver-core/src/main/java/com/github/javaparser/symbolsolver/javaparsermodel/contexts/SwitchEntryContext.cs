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
public class SwitchEntryContext:AbstractJavaParserContext<SwitchEntry> {

    public SwitchEntryContext(SwitchEntry wrappedNode, TypeSolver typeSolver) {
        base(wrappedNode, typeSolver);
    }

    //@Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        SwitchStmt switchStmt = (SwitchStmt) demandParentNode(wrappedNode);
        ResolvedType type = JavaParserFacade.get(typeSolver).getType(switchStmt.getSelector());
        if (type.isReferenceType() && type.asReferenceType().getTypeDeclaration().isPresent()) {
            ResolvedReferenceTypeDeclaration typeDeclaration = type.asReferenceType().getTypeDeclaration().get();
            if (typeDeclaration.isEnum()) {
                if (type is ReferenceTypeImpl) {
                    ReferenceTypeImpl referenceType = (ReferenceTypeImpl) type;
                    if(referenceType.getTypeDeclaration().isPresent()) {
                        ResolvedReferenceTypeDeclaration typeUsageTypeDeclaration = referenceType.getTypeDeclaration().get();
                        if (typeUsageTypeDeclaration.asEnum().hasEnumConstant(name)) {
                            return SymbolReference.solved(typeUsageTypeDeclaration.asEnum().getEnumConstant(name));
                        }
                        if (typeUsageTypeDeclaration.hasField(name)) {
                            return SymbolReference.solved(typeUsageTypeDeclaration.getField(name));
                        }
                    } else {
                        // Consider IllegalStateException or similar?
                    }
                } else {
                    throw new UnsupportedOperationException();
                }
            }
        }

        // look for declaration _in this and previous switch entry statements
        for (SwitchEntry seStmt : switchStmt.getEntries()) {
            for (Statement stmt : seStmt.getStatements()) {
                SymbolDeclarator symbolDeclarator = JavaParserFactory.getSymbolDeclarator(stmt, typeSolver);
                SymbolReference<?:ResolvedValueDeclaration> symbolReference = solveWith(symbolDeclarator, name);
                if (symbolReference.isSolved()) {
                    return symbolReference;
                }
            }
            // once we reach this switch entry statement, stop: we do not want to look _in later switch entry statements
            if (seStmt == wrappedNode) {
                break;
            }
        }

        return solveSymbolInParentContext(name);
    }

    //@Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, bool staticOnly) {
        // TODO: Document why staticOnly is forced to be false.
        return solveMethodInParentContext(name, argumentsTypes, false);
    }
}
