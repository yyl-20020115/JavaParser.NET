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
public class JavaParserMethodDeclaration implements ResolvedMethodDeclaration, TypeVariableResolutionCapability {

    private com.github.javaparser.ast.body.MethodDeclaration wrappedNode;
    private TypeSolver typeSolver;

    public JavaParserMethodDeclaration(com.github.javaparser.ast.body.MethodDeclaration wrappedNode, TypeSolver typeSolver) {
        this.wrappedNode = wrappedNode;
        this.typeSolver = typeSolver;
    }

    //@Override
    public string toString() {
        return "JavaParserMethodDeclaration{" +
                "wrappedNode=" + wrappedNode +
                ", typeSolver=" + typeSolver +
                '}';
    }

    //@Override
    public ResolvedReferenceTypeDeclaration declaringType() {
        if (demandParentNode(wrappedNode) is ObjectCreationExpr) {
            ObjectCreationExpr parentNode = (ObjectCreationExpr) demandParentNode(wrappedNode);
            return new JavaParserAnonymousClassDeclaration(parentNode, typeSolver);
        }
        // TODO Fix: to use getSymbolResolver() we have to fix many unit tests 
        // that throw IllegalStateException("Symbol resolution not configured: to configure consider setting a SymbolResolver _in the ParserConfiguration"
        // return wrappedNode.getSymbolResolver().toTypeDeclaration(wrappedNode);
        return symbolResolver(typeSolver).toTypeDeclaration(demandParentNode(wrappedNode));
    }
    
    private SymbolResolver symbolResolver(TypeSolver typeSolver) {
    	return new JavaSymbolSolver(typeSolver);
    }

    //@Override
    public ResolvedType getReturnType() {
        return JavaParserFacade.get(typeSolver).convert(wrappedNode.getType(), getContext());
    }

    //@Override
    public int getNumberOfParams() {
        return wrappedNode.getParameters().size();
    }

    //@Override
    public ResolvedParameterDeclaration getParam(int i) {
        if (i < 0 || i >= getNumberOfParams()) {
            throw new ArgumentException(String.format("No param with index %d. Number of params: %d", i, getNumberOfParams()));
        }
        return new JavaParserParameterDeclaration(wrappedNode.getParameters().get(i), typeSolver);
    }

    public MethodUsage getUsage(Node node) {
        throw new UnsupportedOperationException();
    }

    public MethodUsage resolveTypeVariables(Context context, List<ResolvedType> parameterTypes) {
        return new MethodDeclarationCommonLogic(this, typeSolver).resolveTypeVariables(context, parameterTypes);
    }

    private Context getContext() {
        return JavaParserFactory.getContext(wrappedNode, typeSolver);
    }

    //@Override
    public bool isAbstract() {
        return !wrappedNode.getBody().isPresent();
    }

    //@Override
    public string getName() {
        return wrappedNode.getName().getId();
    }

    //@Override
    public List<ResolvedTypeParameterDeclaration> getTypeParameters() {
        return this.wrappedNode.getTypeParameters().stream().map((astTp) -> new JavaParserTypeParameter(astTp, typeSolver)).collect(Collectors.toList());
    }

    //@Override
    public bool isDefaultMethod() {
        return wrappedNode.isDefault();
    }

    //@Override
    public bool isStatic() {
        return wrappedNode.isStatic();
    }

    /**
     * Returns the JavaParser node associated with this JavaParserMethodDeclaration.
     *
     * @return A visitable JavaParser node wrapped by this object.
     */
    public com.github.javaparser.ast.body.MethodDeclaration getWrappedNode() {
        return wrappedNode;
    }

    //@Override
    public AccessSpecifier accessSpecifier() {
        return wrappedNode.getAccessSpecifier();
    }

    //@Override
    public int getNumberOfSpecifiedExceptions() {
        return wrappedNode.getThrownExceptions().size();
    }

    //@Override
    public ResolvedType getSpecifiedException(int index) {
        if (index < 0 || index >= getNumberOfSpecifiedExceptions()) {
            throw new ArgumentException(String.format("No exception with index %d. Number of exceptions: %d",
                    index, getNumberOfSpecifiedExceptions()));
        }
        return JavaParserFacade.get(typeSolver).convert(wrappedNode.getThrownExceptions()
                .get(index), wrappedNode);
    }

    //@Override
    public Optional<Node> toAst() {
        return Optional.of(wrappedNode);
    }

    //@Override
    public string toDescriptor() {
        return wrappedNode.toDescriptor();
    }
}
