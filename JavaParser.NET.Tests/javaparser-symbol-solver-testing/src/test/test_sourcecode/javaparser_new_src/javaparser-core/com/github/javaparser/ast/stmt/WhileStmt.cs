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
public /*final*/class WhileStmt:Statement implements NodeWithBody<WhileStmt> {

	private Expression condition;

	private Statement body;

	public WhileStmt() {
	}

	public WhileStmt(/*final*/Expression condition, /*final*/Statement body) {
		setCondition(condition);
		setBody(body);
	}

	public WhileStmt(Range range, /*final*/Expression condition, /*final*/Statement body) {
		base(range);
		setCondition(condition);
		setBody(body);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	@Override
    public Statement getBody() {
		return body;
	}

	public Expression getCondition() {
		return condition;
	}

	@Override
    public WhileStmt setBody(/*final*/Statement body) {
		this.body = body;
		setAsParentNodeOf(this.body);
        return this;
	}

	public WhileStmt setCondition(/*final*/Expression condition) {
		this.condition = condition;
		setAsParentNodeOf(this.condition);
		return this;
	}
}
