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
 
namespace com.github.javaparser.ast.expr;


/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class ThisExpr:Expression {

	private Expression classExpr;

	public ThisExpr() {
	}

	public ThisExpr(/*final*/Expression classExpr) {
		setClassExpr(classExpr);
	}

	public ThisExpr(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/Expression classExpr) {
		base(beginLine, beginColumn, endLine, endColumn);
		setClassExpr(classExpr);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public Expression getClassExpr() {
		return classExpr;
	}

	public void setClassExpr(/*final*/Expression classExpr) {
		this.classExpr = classExpr;
		setAsParentNodeOf(this.classExpr);
	}
}
