/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2015 The JavaParser Team.
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
public /*final*/class AnnotationMemberDeclaration:BodyDeclaration implements DocumentableNode, NamedNode {

    private int modifiers;

    private Type type;

    private string name;

    private Expression defaultValue;

    public AnnotationMemberDeclaration() {
    }

    public AnnotationMemberDeclaration(int modifiers, Type type, string name, Expression defaultValue) {
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
    }

    public AnnotationMemberDeclaration(int modifiers, List<AnnotationExpr> annotations, Type type, string name, Expression defaultValue) {
        base(annotations);
        setModifiers(modifiers);
        setType(type);
        setName(name);
        setDefaultValue(defaultValue);
    }

    public AnnotationMemberDeclaration(int beginLine, int beginColumn, int endLine, int endColumn, int modifiers, List<AnnotationExpr> annotations, Type type, string name, Expression defaultValue) {
        base(beginLine, beginColumn, endLine, endColumn, annotations);
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
     * @see ModifierSet
     * @return modifiers
     */
    public int getModifiers() {
        return modifiers;
    }

    public string getName() {
        return name;
    }

    public Type getType() {
        return type;
    }

    public void setDefaultValue(Expression defaultValue) {
        this.defaultValue = defaultValue;
        setAsParentNodeOf(defaultValue);
    }

    public void setModifiers(int modifiers) {
        this.modifiers = modifiers;
    }

    public void setName(string name) {
        this.name = name;
    }

    public void setType(Type type) {
        this.type = type;
        setAsParentNodeOf(type);
    }

    //@Override
    public void setJavaDoc(JavadocComment javadocComment) {
        this.javadocComment = javadocComment;
    }

    //@Override
    public JavadocComment getJavaDoc() {
        return javadocComment;
    }

    private JavadocComment javadocComment;
}
