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
public /*final*/class AnnotationDeclaration:TypeDeclaration implements DocumentableNode {

    public AnnotationDeclaration() {
    }

    public AnnotationDeclaration(int modifiers, string name) {
        super(modifiers, name);
    }

    public AnnotationDeclaration(int modifiers, List<AnnotationExpr> annotations, string name, List<BodyDeclaration> members) {
        super(annotations, modifiers, name, members);
    }

    public AnnotationDeclaration(int beginLine, int beginColumn, int endLine, int endColumn, int modifiers, List<AnnotationExpr> annotations, string name, List<BodyDeclaration> members) {
        super(beginLine, beginColumn, endLine, endColumn, annotations, modifiers, name, members);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
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
