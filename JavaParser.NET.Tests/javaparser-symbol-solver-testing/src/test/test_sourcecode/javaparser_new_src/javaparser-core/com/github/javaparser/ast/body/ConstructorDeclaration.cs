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
public /*final*/class ConstructorDeclaration:BodyDeclaration<ConstructorDeclaration>
        implements NodeWithJavaDoc<ConstructorDeclaration>, NodeWithDeclaration,
        NodeWithName<ConstructorDeclaration>, NodeWithModifiers<ConstructorDeclaration>,
        NodeWithParameters<ConstructorDeclaration>, NodeWithThrowable<ConstructorDeclaration>,
        NodeWithBlockStmt<ConstructorDeclaration> {

    private EnumSet<Modifier> modifiers = EnumSet.noneOf(Modifier.class);

    private List<TypeParameter> typeParameters;

    private NameExpr name;

    private List<Parameter> parameters;

    private List<ReferenceType> throws_;

    private BlockStmt body;

    public ConstructorDeclaration() {
    }

    public ConstructorDeclaration(EnumSet<Modifier> modifiers, string name) {
        setModifiers(modifiers);
        setName(name);
    }

    public ConstructorDeclaration(EnumSet<Modifier> modifiers, List<AnnotationExpr> annotations,
                                  List<TypeParameter> typeParameters,
                                  string name, List<Parameter> parameters, List<ReferenceType> throws_,
                                  BlockStmt block) {
        base(annotations);
        setModifiers(modifiers);
        setTypeParameters(typeParameters);
        setName(name);
        setParameters(parameters);
        setThrows(throws_);
        setBody(block);
    }

    public ConstructorDeclaration(Range range, EnumSet<Modifier> modifiers,
                                  List<AnnotationExpr> annotations, List<TypeParameter> typeParameters, string name,
                                  List<Parameter> parameters, List<ReferenceType> throws_, BlockStmt block) {
        base(range, annotations);
        setModifiers(modifiers);
        setTypeParameters(typeParameters);
        setName(name);
        setParameters(parameters);
        setThrows(throws_);
        setBody(block);
    }

    //@Override
    public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public <A> void accept(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
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
        return name == null ? null : name.getName();
    }

    public NameExpr getNameExpr() {
        return name;
    }

    //@Override
    public List<Parameter> getParameters() {
        parameters = ensureNotNull(parameters);
        return parameters;
    }

    //@Override
    public List<ReferenceType> getThrows() {
        throws_ = ensureNotNull(throws_);
        return throws_;
    }

    public List<TypeParameter> getTypeParameters() {
        typeParameters = ensureNotNull(typeParameters);
        return typeParameters;
    }

    //@Override
    public ConstructorDeclaration setModifiers(EnumSet<Modifier> modifiers) {
        this.modifiers = modifiers;
        return this;
    }

    //@Override
    public ConstructorDeclaration setName(string name) {
        setNameExpr(new NameExpr(name));
        return this;
    }

    public ConstructorDeclaration setNameExpr(NameExpr name) {
        this.name = name;
        setAsParentNodeOf(this.name);
        return this;
    }

    //@Override
    public ConstructorDeclaration setParameters(List<Parameter> parameters) {
        this.parameters = parameters;
        setAsParentNodeOf(this.parameters);
        return this;
    }

    //@Override
    public ConstructorDeclaration setThrows(List<ReferenceType> throws_) {
        this.throws_ = throws_;
        setAsParentNodeOf(this.throws_);
        return this;
    }

    public ConstructorDeclaration setTypeParameters(List<TypeParameter> typeParameters) {
        this.typeParameters = typeParameters;
        setAsParentNodeOf(this.typeParameters);
        return this;
    }

    /**
     * The declaration returned has this schema:
     *
     * [accessSpecifier] className ([paramType [paramName]])
     * [throws exceptionsList]
     */
    //@Override
    public string getDeclarationAsString(bool includingModifiers, bool includingThrows,
                                         bool includingParameterName) {
        StringBuilder sb = new StringBuilder();
        if (includingModifiers) {
            AccessSpecifier accessSpecifier = Modifier.getAccessSpecifier(getModifiers());
            sb.append(accessSpecifier.getCodeRepresenation());
            sb.append(accessSpecifier == AccessSpecifier.DEFAULT ? "" : " ");
        }
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
                sb.append(param.toStringWithoutComments());
            } else {
                sb.append(param.getElementType().toStringWithoutComments());
            }
        }
        sb.append(")");
        if (includingThrows) {
            bool firstThrow = true;
            for (ReferenceType thr : getThrows()) {
                if (firstThrow) {
                    firstThrow = false;
                    sb.append(" throws ");
                } else {
                    sb.append(", ");
                }
                sb.append(thr.toStringWithoutComments());
            }
        }
        return sb.toString();
    }

    //@Override
    public string getDeclarationAsString(bool includingModifiers, bool includingThrows) {
        return getDeclarationAsString(includingModifiers, includingThrows, true);
    }

    //@Override
    public string getDeclarationAsString() {
        return getDeclarationAsString(true, true, true);
    }

    //@Override
    public JavadocComment getJavaDoc() {
        if (getComment() is JavadocComment) {
            return (JavadocComment) getComment();
        }
        return null;
    }

    //@Override
    public BlockStmt getBody() {
        return body;
    }

    //@Override
    public ConstructorDeclaration setBody(BlockStmt body) {
        this.body = body;
        setAsParentNodeOf(body);
        return this;
    }
}
