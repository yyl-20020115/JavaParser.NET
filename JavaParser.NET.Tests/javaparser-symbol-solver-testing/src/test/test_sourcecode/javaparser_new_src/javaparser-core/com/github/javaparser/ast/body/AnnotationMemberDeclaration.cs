/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2016 The JavaParser Team.
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
 * @author Julio Vilmar Gesser
 */
public /*final*/class AnnotationMemberDeclaration:BodyDeclaration<AnnotationMemberDeclaration>
        implements NodeWithJavaDoc<AnnotationMemberDeclaration>, NodeWithName<AnnotationMemberDeclaration>,
        NodeWithType<AnnotationMemberDeclaration>, NodeWithModifiers<AnnotationMemberDeclaration> {

    private EnumSet<Modifier> modifiers = EnumSet.noneOf(Modifier.class);

    private Type type;

    private string name;

    private Expression defaultValue;

    public AnnotationMemberDeclaration() {
    }

    public AnnotationMemberDeclaration(EnumSet<Modifier> modifiers, Type type, string name, Expression defaultValue) {
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
    }

    public AnnotationMemberDeclaration(EnumSet<Modifier> modifiers, List<AnnotationExpr> annotations, Type type, string name,
                                       Expression defaultValue) {
        super(annotations);
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
    }

    public AnnotationMemberDeclaration(Range range, EnumSet<Modifier> modifiers, List<AnnotationExpr> annotations, Type type,
                                       string name, Expression defaultValue) {
        super(range, annotations);
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public Expression getDefaultValue() {
        return defaultValue;
    }

    /**
     * Return the modifiers of this member declaration.
     * 
     * @see Modifier
     * @return modifiers
     */
    //@Override
    public EnumSet<Modifier> getModifiers() {
        return modifiers;
    }

    //@Override
    public string getName() {
        return name;
    }

    //@Override
    public Type getType() {
        return type;
    }

    public AnnotationMemberDeclaration setDefaultValue(Expression defaultValue) {
        this.defaultValue = defaultValue;
        setAsParentNodeOf(defaultValue);
        return this;
    }

    //@Override
    public AnnotationMemberDeclaration setModifiers(EnumSet<Modifier> modifiers) {
        this.modifiers = modifiers;
        return this;
    }

    //@Override
    public AnnotationMemberDeclaration setName(string name) {
        this.name = name;
        return this;
    }

    //@Override
    public AnnotationMemberDeclaration setType(Type type) {
        this.type = type;
        setAsParentNodeOf(type);
        return this;
    }

    //@Override
    public JavadocComment getJavaDoc() {
        if (getComment() is JavadocComment) {
            return (JavadocComment) getComment();
        }
        return null;
    }
}
