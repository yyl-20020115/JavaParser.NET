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

namespace com.github.javaparser.ast.type;




/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class ClassOrInterfaceType:ReferenceType<ClassOrInterfaceType> implements 
        NodeWithName<ClassOrInterfaceType>, 
        NodeWithAnnotations<ClassOrInterfaceType>,
        NodeWithTypeArguments<ClassOrInterfaceType> {

    private ClassOrInterfaceType scope;

    private string name;

    private List<Type<?>> typeArguments;

    public ClassOrInterfaceType() {
    }

    public ClassOrInterfaceType(/*final*/string name) {
        setName(name);
    }

    public ClassOrInterfaceType(/*final*/ClassOrInterfaceType scope, /*final*/string name) {
        setScope(scope);
        setName(name);
    }

    public ClassOrInterfaceType(/*final*/Range range, /*final*/ClassOrInterfaceType scope, /*final*/string name, /*final*/List<Type<?>> typeArguments) {
        base(range);
        setScope(scope);
        setName(name);
        setTypeArguments(typeArguments);
    }

    //@Override 
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override 
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    //@Override
    public string getName() {
        return name;
    }

    public ClassOrInterfaceType getScope() {
        return scope;
    }

    public bool isBoxedType() {
        return PrimitiveType.unboxMap.containsKey(name);
    }

    public PrimitiveType toUnboxedType() throws UnsupportedOperationException {
        if (!isBoxedType()) {
            throw new UnsupportedOperationException(name + " isn't a boxed type.");
        }
        return new PrimitiveType(PrimitiveType.unboxMap.get(name));
    }

    //@Override
    public ClassOrInterfaceType setName(/*final*/string name) {
        this.name = name;
        return this;
    }

    public ClassOrInterfaceType setScope(/*final*/ClassOrInterfaceType scope) {
        this.scope = scope;
        setAsParentNodeOf(this.scope);
        return this;
    }

    //@Override
    public List<Type<?>> getTypeArguments() {
        return typeArguments;
    }

    //@Override
    public ClassOrInterfaceType setTypeArguments(/*final*/List<Type<?>> types) {
        this.typeArguments = types;
        setAsParentNodeOf(this.typeArguments);
        return this;
    }
}
