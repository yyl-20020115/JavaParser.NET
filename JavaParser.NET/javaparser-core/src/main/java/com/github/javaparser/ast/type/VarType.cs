/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
namespace com.github.javaparser.ast.type;



/**
 * <h1>A type called "var" waiting for Java to infer it.</h1>
 * Examples:
 * <ol>
 * <li><b>var</b> a = 1;</li>
 * <li><b>var</b> a = new ArrayList&lt;String&gt;();</li>
 * </ol>
 */
public class VarType:Type {
	
	private static /*final*/string JAVA_LANG_OBJECT = Object.class.getCanonicalName();

    //@AllFieldsConstructor
    public VarType() {
        this(null);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public VarType(TokenRange tokenRange) {
        base(tokenRange);
        customInitialization();
    }

    //@Override
    public VarType setAnnotations(NodeList<AnnotationExpr> annotations) {
        return (VarType) super.setAnnotations(annotations);
    }

    //@Override
    public string asString() {
        return "var";
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public VarType clone() {
        return (VarType) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public VarTypeMetaModel getMetaModel() {
        return JavaParserMetaModel.varTypeMetaModel;
    }

    //@Override
    public ResolvedType resolve() {
        return getSymbolResolver().toResolvedType(this, ResolvedType.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isVarType() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public VarType asVarType() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<VarType> toVarType() {
        return Optional.of(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifVarType(Consumer<VarType> action) {
        action.accept(this);
    }

	@Override
	public ResolvedType convertToUsage(Context context) {
		Node parent = getParentNode().get();
        if (!(parent is VariableDeclarator)) {
            throw new IllegalStateException("Trying to resolve a `var` which is not _in a variable declaration.");
        }
        /*final*/VariableDeclarator variableDeclarator = (VariableDeclarator) parent;
        Optional<Expression> initializer = variableDeclarator.getInitializer();
        if (!initializer.isPresent()) {
            // When a `var` type decl has no initializer it may be part of a
            // for-each statement (e.g. `for(var i : expr)`).
            Optional<ForEachStmt> forEachStmt = forEachStmtWithVariableDeclarator(variableDeclarator);
            if (forEachStmt.isPresent()) {
                Expression iterable = forEachStmt.get().getIterable();
                ResolvedType iterType = iterable.calculateResolvedType();
                if (iterType is ResolvedArrayType) {
                    // The type of a variable _in a for-each loop with an array
                    // is the component type of the array.
                    return ((ResolvedArrayType)iterType).getComponentType();
                }
                if (iterType.isReferenceType()) {
                    // The type of a variable _in a for-each loop with an
                    // Iterable with parameter type
                	List<ResolvedType> parametersType = iterType.asReferenceType().typeParametersMap().getTypes();
					if (parametersType.isEmpty()) {
						Optional<ResolvedTypeDeclaration> oObjectDeclaration = context.solveType(JAVA_LANG_OBJECT)
								.getDeclaration();
						return oObjectDeclaration
								.map(decl -> ReferenceTypeImpl.undeterminedParameters(decl.asReferenceType()))
								.orElseThrow(() -> new UnsupportedOperationException());
					}
                    return parametersType.get(0);
                }
            }
        }
        return initializer
                .map(Expression::calculateResolvedType)
                .orElseThrow(() -> new IllegalStateException("Cannot resolve `var` which has no initializer."));
	}
	
	private Optional<ForEachStmt> forEachStmtWithVariableDeclarator(
            VariableDeclarator variableDeclarator) {
        Optional<Node> node = variableDeclarator.getParentNode();
        if (!node.isPresent() || !(node.get() is VariableDeclarationExpr)) {
            return Optional.empty();
        }
        node = node.get().getParentNode();
        if (!node.isPresent() || !(node.get() is ForEachStmt)) {
            return Optional.empty();
        } else {
            return Optional.of((ForEachStmt)node.get());
        }
    }
}
