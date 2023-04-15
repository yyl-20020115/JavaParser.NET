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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */
namespace com.github.javaparser.ast.body;



/**
 * An annotation type declaration.<br>{@code @interface X { ... }}
 *
 * @author Julio Vilmar Gesser
 */
public class AnnotationDeclaration : TypeDeclaration<AnnotationDeclaration>, NodeWithAbstractModifier<AnnotationDeclaration>, Resolvable<ResolvedAnnotationDeclaration>
{

    public AnnotationDeclaration()
    : this(null, new NodeList<>(), new NodeList<>(), new SimpleName(), new NodeList<>())
    {
    }

    public AnnotationDeclaration(NodeList<Modifier> modifiers, String name)
    : this(null, modifiers, new NodeList<>(), new SimpleName(name), new NodeList<>())
    {

    }

    //@AllFieldsConstructor
    public AnnotationDeclaration(NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, SimpleName name, NodeList<BodyDeclaration<?>> members)
    : this(null, modifiers, annotations, name, members)
    {
        ;
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public AnnotationDeclaration(TokenRange tokenRange, NodeList<Modifier> modifiers, NodeList<AnnotationExpr> annotations, SimpleName name, NodeList<BodyDeclaration<?>> members)
    : base(tokenRange, modifiers, annotations, name, members)
    {
        ;
        customInitialization();
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public R accept<R, A>(GenericVisitor<R, A> v, A arg)
    {
        return v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public void accept<A>(VoidVisitor<A> v, A arg)
    {
        v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public AnnotationDeclaration clone()
    {
        return (AnnotationDeclaration)accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public AnnotationDeclarationMetaModel getMetaModel()
    {
        return JavaParserMetaModel.annotationDeclarationMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isAnnotationDeclaration()
    {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public AnnotationDeclaration asAnnotationDeclaration()
    {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifAnnotationDeclaration(Consumer<AnnotationDeclaration> action)
    {
        action.accept(this);
    }

    //@Override
    public ResolvedAnnotationDeclaration resolve()
    {
        return getSymbolResolver().resolveDeclaration(this, ResolvedAnnotationDeclaration.class);
    }

//@Override
//@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
public Optional<AnnotationDeclaration> toAnnotationDeclaration()
{
    return Optional.of(this);
}

//@Override
public FieldDeclaration addField(Type type, String name, params Modifier.Keyword[] modifiers)
{
    throw new IllegalStateException("Cannot add a field to an annotation declaration.");
}
}
