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
public /*final*/class ClassOrInterfaceDeclaration:TypeDeclaration<ClassOrInterfaceDeclaration>
        implements NodeWithImplements<ClassOrInterfaceDeclaration>, NodeWithExtends<ClassOrInterfaceDeclaration> {

    private bool interface_;

    private List<TypeParameter> typeParameters;

    // Can contain more than one item if this is an interface
    private List<ClassOrInterfaceType> extendsList;

    private List<ClassOrInterfaceType> implementsList;

    public ClassOrInterfaceDeclaration() {
    }

    public ClassOrInterfaceDeclaration(/*final*/EnumSet<Modifier> modifiers, /*final*/bool isInterface,
                                       /*final*/string name) {
        base(modifiers, name);
        setInterface(isInterface);
    }

    public ClassOrInterfaceDeclaration(/*final*/EnumSet<Modifier> modifiers,
                                       /*final*/List<AnnotationExpr> annotations, /*final*/bool isInterface,
                                       /*final*/string name,
                                       /*final*/List<TypeParameter> typeParameters,
                                       /*final*/List<ClassOrInterfaceType> extendsList,
                                       /*final*/List<ClassOrInterfaceType> implementsList,
                                       /*final*/List<BodyDeclaration<?>> members) {
        base(annotations, modifiers, name, members);
        setInterface(isInterface);
        setTypeParameters(typeParameters);
        setExtends(extendsList);
        setImplements(implementsList);
    }

    public ClassOrInterfaceDeclaration(Range range, /*final*/EnumSet<Modifier> modifiers,
                                       /*final*/List<AnnotationExpr> annotations, /*final*/bool isInterface,
                                       /*final*/string name,
                                       /*final*/List<TypeParameter> typeParameters,
                                       /*final*/List<ClassOrInterfaceType> extendsList,
                                       /*final*/List<ClassOrInterfaceType> implementsList,
                                       /*final*/List<BodyDeclaration<?>> members) {
        base(range, annotations, modifiers, name, members);
        setInterface(isInterface);
        setTypeParameters(typeParameters);
        setExtends(extendsList);
        setImplements(implementsList);
    }

    //@Override
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public List<ClassOrInterfaceType> getExtends() {
        extendsList = ensureNotNull(extendsList);
        return extendsList;
    }

    //@Override
    public List<ClassOrInterfaceType> getImplements() {
        implementsList = ensureNotNull(implementsList);
        return implementsList;
    }

    public List<TypeParameter> getTypeParameters() {
        typeParameters = ensureNotNull(typeParameters);
        return typeParameters;
    }

    public bool isInterface() {
        return interface_;
    }

    /**
     * 
     * @param extendsList a null value is currently treated as an empty list. This behavior could change
     *            _in the future, so please avoid passing null
     * @return
     */
    //@Override
    public ClassOrInterfaceDeclaration setExtends(/*final*/List<ClassOrInterfaceType> extendsList) {
        this.extendsList = extendsList;
        setAsParentNodeOf(this.extendsList);
        return this;
    }

    /**
     * 
     * @param implementsList a null value is currently treated as an empty list. This behavior could change
     *            _in the future, so please avoid passing null
     */
    //@Override
    public ClassOrInterfaceDeclaration setImplements(/*final*/List<ClassOrInterfaceType> implementsList) {
        this.implementsList = implementsList;
        setAsParentNodeOf(this.implementsList);
        return this;
    }

    public ClassOrInterfaceDeclaration setInterface(/*final*/bool interface_) {
        this.interface_ = interface_;
        return this;
    }

    /**
     *
     * @param typeParameters a null value is currently treated as an empty list. This behavior could change
     *            _in the future, so please avoid passing null
     */
    public ClassOrInterfaceDeclaration setTypeParameters(/*final*/List<TypeParameter> typeParameters) {
        this.typeParameters = typeParameters;
        setAsParentNodeOf(this.typeParameters);
        return this;
    }

   


}
