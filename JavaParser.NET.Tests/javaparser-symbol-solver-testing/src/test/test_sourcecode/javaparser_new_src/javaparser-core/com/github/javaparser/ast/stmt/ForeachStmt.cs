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
public /*final*/class ForeachStmt:Statement implements NodeWithBody<ForeachStmt> {

	private VariableDeclarationExpr var;

	private Expression iterable;

	private Statement body;

	public ForeachStmt() {
	}

	public ForeachStmt(/*final*/VariableDeclarationExpr var,
			/*final*/Expression iterable, /*final*/Statement body) {
		setVariable(var);
		setIterable(iterable);
		setBody(body);
	}

	public ForeachStmt(Range range,
	                   /*final*/VariableDeclarationExpr var, /*final*/Expression iterable,
	                   /*final*/Statement body) {
		base(range);
		setVariable(var);
		setIterable(iterable);
		setBody(body);
	}

    /**
     * Will create a {@link NameExpr} with the iterable param
     * 
     * @param var
     * @param iterable
     * @param body
     */
    public ForeachStmt(VariableDeclarationExpr var, string iterable, BlockStmt body) {
        setVariable(var);
        setIterable(new NameExpr(iterable));
        setBody(body);
    }

    //@Override
	public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override
	public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	@Override
    public Statement getBody() {
		return body;
	}

	public Expression getIterable() {
		return iterable;
	}

	public VariableDeclarationExpr getVariable() {
		return var;
	}

	@Override
    public ForeachStmt setBody(/*final*/Statement body) {
		this.body = body;
		setAsParentNodeOf(this.body);
        return this;
	}

	public ForeachStmt setIterable(/*final*/Expression iterable) {
		this.iterable = iterable;
		setAsParentNodeOf(this.iterable);
		return this;
	}

	public ForeachStmt setVariable(/*final*/VariableDeclarationExpr var) {
		this.var = var;
		setAsParentNodeOf(this.var);
		return this;
	}
}
