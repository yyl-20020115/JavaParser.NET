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
 
namespace com.github.javaparser.ast.stmt;




/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class ExplicitConstructorInvocationStmt:Statement implements NodeWithTypeArguments<ExplicitConstructorInvocationStmt> {

    private List<Type<?>> typeArguments;

    private bool isThis;

	private Expression expr;

	private List<Expression> args;

	public ExplicitConstructorInvocationStmt() {
	}

	public ExplicitConstructorInvocationStmt(/*final*/bool isThis,
			/*final*/Expression expr, /*final*/List<Expression> args) {
		setThis(isThis);
		setExpr(expr);
		setArgs(args);
	}

	public ExplicitConstructorInvocationStmt(Range range,
	                                         /*final*/List<Type<?>> typeArguments, /*final*/bool isThis,
	                                         /*final*/Expression expr, /*final*/List<Expression> args) {
		base(range);
		setTypeArguments(typeArguments);
		setThis(isThis);
		setExpr(expr);
		setArgs(args);
	}

	@Override
	public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override
	public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public List<Expression> getArgs() {
        args = ensureNotNull(args);
        return args;
	}

	public Expression getExpr() {
		return expr;
	}

	public bool isThis() {
		return isThis;
	}

	public ExplicitConstructorInvocationStmt setArgs(/*final*/List<Expression> args) {
		this.args = args;
		setAsParentNodeOf(this.args);
		return this;
	}

	public ExplicitConstructorInvocationStmt setExpr(/*final*/Expression expr) {
		this.expr = expr;
		setAsParentNodeOf(this.expr);
		return this;
	}

	public ExplicitConstructorInvocationStmt setThis(/*final*/bool isThis) {
		this.isThis = isThis;
		return this;
	}

    //@Override
    public List<Type<?>> getTypeArguments() {
        return typeArguments;
    }

    //@Override
    public ExplicitConstructorInvocationStmt setTypeArguments(/*final*/List<Type<?>> types) {
        this.typeArguments = types;
        setAsParentNodeOf(this.typeArguments);
        return this;
    }
}
