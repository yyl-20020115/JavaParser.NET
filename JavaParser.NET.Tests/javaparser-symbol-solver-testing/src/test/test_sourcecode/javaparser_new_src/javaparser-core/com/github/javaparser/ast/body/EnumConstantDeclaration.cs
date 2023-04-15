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
public /*final*/class EnumConstantDeclaration:BodyDeclaration<EnumConstantDeclaration>
        implements NodeWithJavaDoc<EnumConstantDeclaration>, NodeWithName<EnumConstantDeclaration> {

    private string name;

    private List<Expression> args;

    private List<BodyDeclaration<?>> classBody;

    public EnumConstantDeclaration() {
    }

    public EnumConstantDeclaration(string name) {
        setName(name);
    }

    public EnumConstantDeclaration(List<AnnotationExpr> annotations, string name, List<Expression> args,
                                   List<BodyDeclaration<?>> classBody) {
        base(annotations);
        setName(name);
        setArgs(args);
        setClassBody(classBody);
    }

    public EnumConstantDeclaration(Range range, List<AnnotationExpr> annotations, string name, List<Expression> args,
                                   List<BodyDeclaration<?>> classBody) {
        base(range, annotations);
        setName(name);
        setArgs(args);
        setClassBody(classBody);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public List<Expression> getArgs() {
        args = ensureNotNull(args);
        return args;
    }

    public List<BodyDeclaration<?>> getClassBody() {
        classBody = ensureNotNull(classBody);
        return classBody;
    }

    //@Override
    public string getName() {
        return name;
    }

    public EnumConstantDeclaration setArgs(List<Expression> args) {
        this.args = args;
		setAsParentNodeOf(this.args);
        return this;
    }

    public EnumConstantDeclaration setClassBody(List<BodyDeclaration<?>> classBody) {
        this.classBody = classBody;
		setAsParentNodeOf(this.classBody);
        return this;
    }

    //@Override
    public EnumConstantDeclaration setName(string name) {
        this.name = name;
        return this;
    }

    //@Override
    public JavadocComment getJavaDoc() {
        if(getComment() is JavadocComment){
            return (JavadocComment) getComment();
        }
        return null;
    }

    public EnumConstantDeclaration addArgument(string valueExpr) {
        getArgs().add(name(valueExpr));
        return this;
    }
}
