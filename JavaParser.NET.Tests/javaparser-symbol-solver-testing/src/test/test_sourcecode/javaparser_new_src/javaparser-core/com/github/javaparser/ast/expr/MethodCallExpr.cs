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

namespace com.github.javaparser.ast.expr;




/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class MethodCallExpr:Expression implements NodeWithTypeArguments<MethodCallExpr> {

    private Expression scope;

    private List<Type<?>> typeArguments;

    private NameExpr name;

    private List<Expression> args;

    public MethodCallExpr() {
    }

    public MethodCallExpr(/*final*/Expression scope, /*final*/string name) {
        setScope(scope);
        setName(name);
    }

    public MethodCallExpr(/*final*/Expression scope, /*final*/string name, /*final*/List<Expression> args) {
        setScope(scope);
        setName(name);
        setArgs(args);
    }

	public MethodCallExpr(/*final*/Range range, /*final*/Expression scope, /*final*/List<Type<?>> typeArguments, /*final*/string name, /*final*/List<Expression> args) {
		base(range);
		setScope(scope);
		setTypeArguments(typeArguments);
		setName(name);
		setArgs(args);
	}

    /**
     * Adds the given argument to the method call.
     *
     * @param arg
     *            argument value
     */
    public MethodCallExpr addArgument(Expression arg) {
        getArgs().add(arg);
        arg.setParentNode(this);
        return this;
    }

    public void addArgument(string arg) {
        addArgument(new NameExpr(arg));
    }

    //@Override
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    public List<Expression> getArgs() {
        args = ensureNotNull(args);
        return args;
    }

    public string getName() {
        return name.getName();
    }

    public NameExpr getNameExpr() {
        return name;
    }

    public Expression getScope() {
        return scope;
    }

	public void setArgs(/*final*/List<Expression> args) {
		this.args = args;
		setAsParentNodeOf(this.args);
	}

    public MethodCallExpr setName(/*final*/string name) {
        setNameExpr(new NameExpr(name));
        return this;
    }

    public MethodCallExpr setNameExpr(NameExpr name) {
        this.name = name;
        setAsParentNodeOf(this.name);
        return this;
    }

    public MethodCallExpr setScope(/*final*/Expression scope) {
        this.scope = scope;
        setAsParentNodeOf(this.scope);
        return this;
    }

    //@Override
    public List<Type<?>> getTypeArguments() {
        return typeArguments;
    }

    //@Override
    public MethodCallExpr setTypeArguments(/*final*/List<Type<?>> types) {
        this.typeArguments = types;
        setAsParentNodeOf(this.typeArguments);
        return this;
    }
}
