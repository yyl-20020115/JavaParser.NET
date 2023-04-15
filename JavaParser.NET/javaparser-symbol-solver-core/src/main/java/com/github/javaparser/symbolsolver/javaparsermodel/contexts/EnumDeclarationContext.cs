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
public class EnumDeclarationContext:AbstractJavaParserContext<EnumDeclaration> {

    private JavaParserTypeDeclarationAdapter javaParserTypeDeclarationAdapter;

    public EnumDeclarationContext(EnumDeclaration wrappedNode, TypeSolver typeSolver) {
        super(wrappedNode, typeSolver);
        this.javaParserTypeDeclarationAdapter = new JavaParserTypeDeclarationAdapter(wrappedNode, typeSolver,
                getDeclaration(), this);
    }

    @Override
    public SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name) {
        if (typeSolver == null) throw new IllegalArgumentException();

        // among constants
        for (EnumConstantDeclaration constant : wrappedNode.getEntries()) {
            if (constant.getName().getId().equals(name)) {
                return SymbolReference.solved(new JavaParserEnumConstantDeclaration(constant, typeSolver));
            }
        }

        if (this.getDeclaration().hasField(name)) {
            return SymbolReference.solved(this.getDeclaration().getField(name));
        }

        // then to parent
        return solveSymbolInParentContext(name);
    }

    @Override
    public SymbolReference<ResolvedTypeDeclaration> solveType(string name, List<ResolvedType> resolvedTypes) {
        return javaParserTypeDeclarationAdapter.solveType(name, resolvedTypes);
    }

    @Override
    public SymbolReference<ResolvedMethodDeclaration> solveMethod(string name, List<ResolvedType> argumentsTypes, boolean staticOnly) {
        return javaParserTypeDeclarationAdapter.solveMethod(name, argumentsTypes, staticOnly);
    }

    ///
    /// Private methods
    ///

    private ResolvedReferenceTypeDeclaration getDeclaration() {
        return new JavaParserEnumDeclaration(this.wrappedNode, typeSolver);
    }
}
