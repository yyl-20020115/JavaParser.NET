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
 * The "int id();" _in {@code @interface X { int id(); }}
 * <p>
 * <br>All annotations preceding the type will be set on this object, not on the type. JavaParser doesn't know if it
 * they are applicable to the method or the type.
 *
 * @author Julio Vilmar Gesser
 */
public class AnnotationMemberDeclaration : BodyDeclaration<AnnotationMemberDeclaration>, NodeWithJavadoc<AnnotationMemberDeclaration>, NodeWithSimpleName<AnnotationMemberDeclaration>, NodeWithType<AnnotationMemberDeclaration, Type>, NodeWithPublicModifier<AnnotationMemberDeclaration>, NodeWithAbstractModifier<AnnotationMemberDeclaration>, Resolvable<ResolvedAnnotationMemberDeclaration>
{

    private NodeList<Modifier> modifiers;

    private Type type;

    private SimpleName name;

    ////@OptionalProperty
    private Expression defaultValue;

    public AnnotationMemberDeclaration()
    {
        this(null, new NodeList<>(), new NodeList<>(), new ClassOrInterfaceType(), new SimpleName(), null);
    }

    public AnnotationMemberDeclaration(NodeList<Modifier> modifiers, Type type, string name, Expression defaultValue)
    {
        this(null, modifiers, new NodeList<>(), type, new SimpleName(name), defaultValue);
    }

    ////@AllFieldsConstructor
    public AnnotationMemberDeclaration(NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, Type type, SimpleName name, Expression defaultValue)
    {
        this(null, modifiers, annotations, type, name, defaultValue);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    ////@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public AnnotationMemberDeclaration(TokenRange tokenRange, NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, Type type, SimpleName name, Expression defaultValue)
    {
        base(tokenRange, annotations);
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
        customInitialization();
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public R accept<R, A>(/*final*/GenericVisitor<R, A> v, /*final*/A arg)
    {
        return v.visit(this, arg);
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg)
    {
        v.visit(this, arg);
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<Expression> getDefaultValue()
    {
        return Optional.ofNullable(defaultValue);
    }

    /**
     * Return the modifiers of this member declaration.
     *
     * @return modifiers
     * @see Modifier
     */
    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<Modifier> getModifiers()
    {
        return modifiers;
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public SimpleName getName()
    {
        return name;
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Type getType()
    {
        return type;
    }

    ////@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public AnnotationMemberDeclaration removeDefaultValue()
    {
        return setDefaultValue((Expression)null);
    }

    /**
     * Sets the default value
     *
     * @param defaultValue the default value, can be null
     * @return this, the AnnotationMemberDeclaration
     */
    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AnnotationMemberDeclaration setDefaultValue(/*final*/Expression defaultValue)
    {
        if (defaultValue == this.defaultValue)
        {
            return this;
        }
        notifyPropertyChange(ObservableProperty.DEFAULT_VALUE, this.defaultValue, defaultValue);
        if (this.defaultValue != null)
            this.defaultValue.setParentNode(null);
        this.defaultValue = defaultValue;
        setAsParentNodeOf(defaultValue);
        return this;
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AnnotationMemberDeclaration setModifiers(/*final*/NodeList<Modifier> modifiers)
    {
        assertNotNull(modifiers);
        if (modifiers == this.modifiers)
        {
            return this;
        }
        notifyPropertyChange(ObservableProperty.MODIFIERS, this.modifiers, modifiers);
        if (this.modifiers != null)
            this.modifiers.setParentNode(null);
        this.modifiers = modifiers;
        setAsParentNodeOf(modifiers);
        return this;
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AnnotationMemberDeclaration setName(/*final*/SimpleName name)
    {
        assertNotNull(name);
        if (name == this.name)
        {
            return this;
        }
        notifyPropertyChange(ObservableProperty.NAME, this.name, name);
        if (this.name != null)
            this.name.setParentNode(null);
        this.name = name;
        setAsParentNodeOf(name);
        return this;
    }

    ////@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public AnnotationMemberDeclaration setType(/*final*/Type type)
    {
        assertNotNull(type);
        if (type == this.type)
        {
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
    ////@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node)
    {
        if (node == null)
        {
            return false;
        }
        if (defaultValue != null)
        {
            if (node == defaultValue)
            {
                removeDefaultValue();
                return true;
            }
        }
        for (int i = 0; i < modifiers.size(); i++)
        {
            if (modifiers.get(i) == node)
            {
                modifiers.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public AnnotationMemberDeclaration clone()
    {
        return (AnnotationMemberDeclaration)accept(new CloneVisitor(), null);
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public AnnotationMemberDeclarationMetaModel getMetaModel()
    {
        return JavaParserMetaModel.annotationMemberDeclarationMetaModel;
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode)
    {
        if (node == null)
        {
            return false;
        }
        if (defaultValue != null)
        {
            if (node == defaultValue)
            {
                setDefaultValue((Expression)replacementNode);
                return true;
            }
        }
        for (int i = 0; i < modifiers.size(); i++)
        {
            if (modifiers.get(i) == node)
            {
                modifiers.set(i, (Modifier)replacementNode);
                return true;
            }
        }
        if (node == name)
        {
            setName((SimpleName)replacementNode);
            return true;
        }
        if (node == type)
        {
            setType((Type)replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isAnnotationMemberDeclaration()
    {
        return true;
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public AnnotationMemberDeclaration asAnnotationMemberDeclaration()
    {
        return this;
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifAnnotationMemberDeclaration(Consumer<AnnotationMemberDeclaration> action)
    {
        action.accept(this);
    }

    //@Override
    public ResolvedAnnotationMemberDeclaration resolve()
    {
        return getSymbolResolver().resolveDeclaration(this, typeof(ResolvedAnnotationMemberDeclaration));
    }

    //@Override
    ////@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<AnnotationMemberDeclaration> toAnnotationMemberDeclaration()
    {
        return Optional.of(this);
    }
}
