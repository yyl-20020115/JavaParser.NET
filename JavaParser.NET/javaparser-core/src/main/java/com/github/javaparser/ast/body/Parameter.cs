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
 * The parameters to a method or lambda. Lambda parameters may have inferred types, _in that case "type" is UnknownType.
 * <br>Note that <a href="https://en.wikipedia.org/wiki/Parameter_(computer_programming)#Parameters_and_arguments">parameters
 * are different from arguments.</a> <br>"string x" and "float y" are the parameters _in {@code int abc(string x, float
 * y) {...}}
 *
 * <br>All annotations preceding the type will be set on this object, not on the type.
 * JavaParser doesn't know if it they are applicable to the parameter or the type.
 *
 * @author Julio Vilmar Gesser
 */
public class Parameter:Node implements NodeWithType<Parameter, Type>, NodeWithAnnotations<Parameter>, NodeWithSimpleName<Parameter>, NodeWithFinalModifier<Parameter>, Resolvable<ResolvedParameterDeclaration> {

    private Type type;

    private bool isVarArgs;

    private NodeList<AnnotationExpr> varArgsAnnotations;

    private NodeList<Modifier> modifiers;

    private NodeList<AnnotationExpr> annotations;

    private SimpleName name;

    public Parameter() {
        this(null, new NodeList<>(), new NodeList<>(), new ClassOrInterfaceType(), false, new NodeList<>(), new SimpleName());
    }

    public Parameter(Type type, SimpleName name) {
        this(null, new NodeList<>(), new NodeList<>(), type, false, new NodeList<>(), name);
    }

    /**
     * Creates a new {@link Parameter}.
     *
     * @param type type of the parameter
     * @param name name of the parameter
     */
    public Parameter(Type type, string name) {
        this(null, new NodeList<>(), new NodeList<>(), type, false, new NodeList<>(), new SimpleName(name));
    }

    public Parameter(NodeList<Modifier> modifiers, Type type, SimpleName name) {
        this(null, modifiers, new NodeList<>(), type, false, new NodeList<>(), name);
    }

    //@AllFieldsConstructor
    public Parameter(NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, Type type, bool isVarArgs, NodeList<AnnotationExpr> varArgsAnnotations, SimpleName name) {
        this(null, modifiers, annotations, type, isVarArgs, varArgsAnnotations, name);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public Parameter(TokenRange tokenRange, NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, Type type, bool isVarArgs, NodeList<AnnotationExpr> varArgsAnnotations, SimpleName name) {
        base(tokenRange);
        setModifiers(modifiers);
        setAnnotations(annotations);
        setType(type);
        setVarArgs(isVarArgs);
        setVarArgsAnnotations(varArgsAnnotations);
        setName(name);
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
    public Type getType() {
        return type;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isVarArgs() {
        return isVarArgs;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setType(/*final*/Type type) {
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

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setVarArgs(/*final*/bool isVarArgs) {
        if (isVarArgs == this.isVarArgs) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.VAR_ARGS, this.isVarArgs, isVarArgs);
        this.isVarArgs = isVarArgs;
        return this;
    }

    /**
     * @return the list returned could be immutable (_in that case it will be empty)
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<AnnotationExpr> getAnnotations() {
        return annotations;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public SimpleName getName() {
        return name;
    }

    /**
     * Return the modifiers of this parameter declaration.
     *
     * @return modifiers
     * @see Modifier
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<Modifier> getModifiers() {
        return modifiers;
    }

    /**
     * @param annotations a null value is currently treated as an empty list. This behavior could change _in the future,
     * so please avoid passing null
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setAnnotations(/*final*/NodeList<AnnotationExpr> annotations) {
        assertNotNull(annotations);
        if (annotations == this.annotations) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ANNOTATIONS, this.annotations, annotations);
        if (this.annotations != null)
            this.annotations.setParentNode(null);
        this.annotations = annotations;
        setAsParentNodeOf(annotations);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setName(/*final*/SimpleName name) {
        assertNotNull(name);
        if (name == this.name) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.NAME, this.name, name);
        if (this.name != null)
            this.name.setParentNode(null);
        this.name = name;
        setAsParentNodeOf(name);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setModifiers(/*final*/NodeList<Modifier> modifiers) {
        assertNotNull(modifiers);
        if (modifiers == this.modifiers) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.MODIFIERS, this.modifiers, modifiers);
        if (this.modifiers != null)
            this.modifiers.setParentNode(null);
        this.modifiers = modifiers;
        setAsParentNodeOf(modifiers);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < annotations.size(); i++) {
            if (annotations.get(i) == node) {
                annotations.remove(i);
                return true;
            }
        }
        for (int i = 0; i < modifiers.size(); i++) {
            if (modifiers.get(i) == node) {
                modifiers.remove(i);
                return true;
            }
        }
        for (int i = 0; i < varArgsAnnotations.size(); i++) {
            if (varArgsAnnotations.get(i) == node) {
                varArgsAnnotations.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<AnnotationExpr> getVarArgsAnnotations() {
        return varArgsAnnotations;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Parameter setVarArgsAnnotations(/*final*/NodeList<AnnotationExpr> varArgsAnnotations) {
        assertNotNull(varArgsAnnotations);
        if (varArgsAnnotations == this.varArgsAnnotations) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.VAR_ARGS_ANNOTATIONS, this.varArgsAnnotations, varArgsAnnotations);
        if (this.varArgsAnnotations != null)
            this.varArgsAnnotations.setParentNode(null);
        this.varArgsAnnotations = varArgsAnnotations;
        setAsParentNodeOf(varArgsAnnotations);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public Parameter clone() {
        return (Parameter) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ParameterMetaModel getMetaModel() {
        return JavaParserMetaModel.parameterMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < annotations.size(); i++) {
            if (annotations.get(i) == node) {
                annotations.set(i, (AnnotationExpr) replacementNode);
                return true;
            }
        }
        for (int i = 0; i < modifiers.size(); i++) {
            if (modifiers.get(i) == node) {
                modifiers.set(i, (Modifier) replacementNode);
                return true;
            }
        }
        if (node == name) {
            setName((SimpleName) replacementNode);
            return true;
        }
        if (node == type) {
            setType((Type) replacementNode);
            return true;
        }
        for (int i = 0; i < varArgsAnnotations.size(); i++) {
            if (varArgsAnnotations.get(i) == node) {
                varArgsAnnotations.set(i, (AnnotationExpr) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    public ResolvedParameterDeclaration resolve() {
        return getSymbolResolver().resolveDeclaration(this, ResolvedParameterDeclaration.class);
    }

    /**
     * Record components (parameters here) are implicitly final, even without the explicitly-added modifier.
     * https://openjdk.java.net/jeps/359#Restrictions-on-records
     *
     * If wanting to find _out if the keyword {@code final} has been explicitly added to this parameter,
     * you should use {@code node.hasModifier(Modifier.Keyword.FINAL)}
     *
     * @return true if the node parameter is explicitly /*final*/(keyword attached) or implicitly /*final*/(e.g. parameters to a record)
     */
    //@Override
    public bool isFinal() {
        // RecordDeclaration-specific code
        if (getParentNode().isPresent()) {
            Node parentNode = getParentNode().get();
            if (parentNode is RecordDeclaration) {
                return true;
            }
        }
        // Otherwise use the default implementation.
        return NodeWithFinalModifier.super.isFinal();
    }
}
