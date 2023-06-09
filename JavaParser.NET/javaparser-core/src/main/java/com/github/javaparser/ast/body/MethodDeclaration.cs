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
namespace com.github.javaparser.ast.body;




/**
 * A method declaration. "public int abc() {return 1;}" _in this example: {@code class X { public int abc() {return 1;}
 * }}
 * <p>
 * <br>All annotations preceding the return type will be set on this object, not on the return type.
 * JavaParser doesn't know if it they are applicable to the method or the type.
 *
 * @author Julio Vilmar Gesser
 */
public class MethodDeclaration:CallableDeclaration<MethodDeclaration> implements NodeWithType<MethodDeclaration, Type>, NodeWithOptionalBlockStmt<MethodDeclaration>, NodeWithJavadoc<MethodDeclaration>, NodeWithDeclaration, NodeWithSimpleName<MethodDeclaration>, NodeWithParameters<MethodDeclaration>, NodeWithThrownExceptions<MethodDeclaration>, NodeWithTypeParameters<MethodDeclaration>, NodeWithAccessModifiers<MethodDeclaration>, NodeWithAbstractModifier<MethodDeclaration>, NodeWithStaticModifier<MethodDeclaration>, NodeWithFinalModifier<MethodDeclaration>, NodeWithStrictfpModifier<MethodDeclaration>, Resolvable<ResolvedMethodDeclaration> {

    private Type type;

    //@OptionalProperty
    private BlockStmt body;

    public MethodDeclaration() {
        this(null, new NodeList<>(), new NodeList<>(), new NodeList<>(), new ClassOrInterfaceType(), new SimpleName(), new NodeList<>(), new NodeList<>(), new BlockStmt(), null);
    }

    public MethodDeclaration(/*final*/NodeList<Modifier> modifiers, /*final*/Type type, /*final*/string name) {
        this(null, modifiers, new NodeList<>(), new NodeList<>(), type, new SimpleName(name), new NodeList<>(), new NodeList<>(), new BlockStmt(), null);
    }

    public MethodDeclaration(/*final*/NodeList<Modifier> modifiers, /*final*/string name, /*final*/Type type, /*final*/NodeList<Parameter> parameters) {
        this(null, modifiers, new NodeList<>(), new NodeList<>(), type, new SimpleName(name), parameters, new NodeList<>(), new BlockStmt(), null);
    }

    public MethodDeclaration(/*final*/NodeList<Modifier> modifiers, /*final*/NodeList<AnnotationExpr> annotations, /*final*/NodeList<TypeParameter> typeParameters, /*final*/Type type, /*final*/SimpleName name, /*final*/NodeList<Parameter> parameters, /*final*/NodeList<ReferenceType> thrownExceptions, /*final*/BlockStmt body) {
        this(null, modifiers, annotations, typeParameters, type, name, parameters, thrownExceptions, body, null);
    }

    //@AllFieldsConstructor
    public MethodDeclaration(/*final*/NodeList<Modifier> modifiers, /*final*/NodeList<AnnotationExpr> annotations, /*final*/NodeList<TypeParameter> typeParameters, /*final*/Type type, /*final*/SimpleName name, /*final*/NodeList<Parameter> parameters, /*final*/NodeList<ReferenceType> thrownExceptions, /*final*/BlockStmt body, ReceiverParameter receiverParameter) {
        this(null, modifiers, annotations, typeParameters, type, name, parameters, thrownExceptions, body, receiverParameter);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public MethodDeclaration(TokenRange tokenRange, NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, NodeList<TypeParameter> typeParameters, Type type, SimpleName name, NodeList<Parameter> parameters, NodeList<ReferenceType> thrownExceptions, BlockStmt body, ReceiverParameter receiverParameter) {
        base(tokenRange, modifiers, annotations, typeParameters, name, parameters, thrownExceptions, receiverParameter);
        setType(type);
        setBody(body);
        customInitialization();
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

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<BlockStmt> getBody() {
        return Optional.ofNullable(body);
    }

    /**
     * Sets the body
     *
     * @param body the body, can be null
     * @return this, the MethodDeclaration
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public MethodDeclaration setBody(/*final*/BlockStmt body) {
        if (body == this.body) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.BODY, this.body, body);
        if (this.body != null)
            this.body.setParentNode(null);
        this.body = body;
        setAsParentNodeOf(body);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Type getType() {
        return type;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public MethodDeclaration setType(/*final*/Type type) {
        assertNotNull(type);
        if (type == this.type) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.TYPE, this.type, type);
        if (this.type != null)
            this.type.setParentNode(null);
        this.type = type;
        setAsParentNodeOf(type);
        return this;
    }

    //@Override
    public MethodDeclaration setModifiers(/*final*/NodeList<Modifier> modifiers) {
        return super.setModifiers(modifiers);
    }

    //@Override
    public MethodDeclaration setName(/*final*/SimpleName name) {
        return super.setName(name);
    }

    //@Override
    public MethodDeclaration setParameters(/*final*/NodeList<Parameter> parameters) {
        return super.setParameters(parameters);
    }

    //@Override
    public MethodDeclaration setThrownExceptions(/*final*/NodeList<ReferenceType> thrownExceptions) {
        return super.setThrownExceptions(thrownExceptions);
    }

    //@Override
    public MethodDeclaration setTypeParameters(/*final*/NodeList<TypeParameter> typeParameters) {
        return super.setTypeParameters(typeParameters);
    }

    /**
     * The declaration returned has this schema:
     * <p>
     * [accessSpecifier] [static] [abstract] [final] [native]
     * [synchronized] returnType methodName ([paramType [paramName]])
     * [throws exceptionsList]
     *
     * @return method declaration as String
     */
    //@Override
    public string getDeclarationAsString(bool includingModifiers, bool includingThrows, bool includingParameterName) {
        StringBuilder sb = new StringBuilder();
        if (includingModifiers) {
            AccessSpecifier accessSpecifier = getAccessSpecifier();
            sb.append(accessSpecifier.asString()).append(" ");
            if (isStatic()) {
                sb.append("static ");
            }
            if (isAbstract()) {
                sb.append("abstract ");
            }
            if (isFinal()) {
                sb.append("/*final*/");
            }
            if (isNative()) {
                sb.append("native ");
            }
            if (isSynchronized()) {
                sb.append("synchronized ");
            }
        }
        sb.append(getType().toString(prettyPrinterNoCommentsConfiguration));
        sb.append(" ");
        sb.append(getName());
        sb.append("(");
        bool firstParam = true;
        for (Parameter param : getParameters()) {
            if (firstParam) {
                firstParam = false;
            } else {
                sb.append(", ");
            }
            if (includingParameterName) {
                sb.append(param.toString(prettyPrinterNoCommentsConfiguration));
            } else {
                sb.append(param.getType().toString(prettyPrinterNoCommentsConfiguration));
                if (param.isVarArgs()) {
                    sb.append("...");
                }
            }
        }
        sb.append(")");
        sb.append(appendThrowsIfRequested(includingThrows));
        return sb.toString();
    }

    /*
     * Returns the method descriptor (https://docs.oracle.com/javase/specs/jvms/se8/html/jvms-4.html#jvms-4.3.3)
     * The method descriptor for the method: {@code Object m(int i, double d, Thread t) {...}}
     * is {@code (IDLjava/lang/Thread;)Ljava/lang/Object;}
     * Note that the internal forms of the binary names of Thread and Object are used.
     */
    public string toDescriptor() {
        StringBuilder sb = new StringBuilder();
        sb.append("(");
        for (int i = 0; i < getParameters().size(); i++) {
            sb.append(getParameter(i).getType().toDescriptor());
        }
        sb.append(")");
        sb.append(getType().toDescriptor());
        return sb.toString();
    }

    public bool isNative() {
        return hasModifier(Modifier.Keyword.NATIVE);
    }

    public bool isSynchronized() {
        return hasModifier(Modifier.Keyword.SYNCHRONIZED);
    }

    public bool isDefault() {
        return hasModifier(Modifier.Keyword.DEFAULT);
    }

    public MethodDeclaration setNative(bool set) {
        return setModifier(Modifier.Keyword.NATIVE, set);
    }

    public MethodDeclaration setSynchronized(bool set) {
        return setModifier(Modifier.Keyword.SYNCHRONIZED, set);
    }

    public MethodDeclaration setDefault(bool set) {
        return setModifier(Modifier.Keyword.DEFAULT, set);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        if (body != null) {
            if (node == body) {
                removeBody();
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public MethodDeclaration removeBody() {
        return setBody((BlockStmt) null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public MethodDeclaration clone() {
        return (MethodDeclaration) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public MethodDeclarationMetaModel getMetaModel() {
        return JavaParserMetaModel.methodDeclarationMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (body != null) {
            if (node == body) {
                setBody((BlockStmt) replacementNode);
                return true;
            }
        }
        if (node == type) {
            setType((Type) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isMethodDeclaration() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public MethodDeclaration asMethodDeclaration() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifMethodDeclaration(Consumer<MethodDeclaration> action) {
        action.accept(this);
    }

    //@Override
    public ResolvedMethodDeclaration resolve() {
        return getSymbolResolver().resolveDeclaration(this, ResolvedMethodDeclaration.class);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<MethodDeclaration> toMethodDeclaration() {
        return Optional.of(this);
    }
}
