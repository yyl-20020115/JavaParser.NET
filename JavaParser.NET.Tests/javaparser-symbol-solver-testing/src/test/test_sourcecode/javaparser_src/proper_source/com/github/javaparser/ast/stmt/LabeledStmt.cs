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
 
namespace com.github.javaparser.ast.stmt;


/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class LabeledStmt:Statement {

	private string label;

	private Statement stmt;

	public LabeledStmt() {
	}

	public LabeledStmt(/*final*/string label, /*final*/Statement stmt) {
		setLabel(label);
		setStmt(stmt);
	}

	public LabeledStmt(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/string label, /*final*/Statement stmt) {
		base(beginLine, beginColumn, endLine, endColumn);
		setLabel(label);
		setStmt(stmt);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public string getLabel() {
		return label;
	}

	public Statement getStmt() {
		return stmt;
	}

	public void setLabel(/*final*/string label) {
		this.label = label;
	}

	public void setStmt(/*final*/Statement stmt) {
		this.stmt = stmt;
		setAsParentNodeOf(this.stmt);
	}
}
