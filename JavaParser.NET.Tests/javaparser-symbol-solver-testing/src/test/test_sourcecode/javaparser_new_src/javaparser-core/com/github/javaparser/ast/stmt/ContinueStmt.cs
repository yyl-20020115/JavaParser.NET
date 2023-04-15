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
public /*final*/class ContinueStmt:Statement {

	private string id;

	public ContinueStmt() {
	}

	public ContinueStmt(/*final*/string id) {
		this.id = id;
	}

	public ContinueStmt(Range range, /*final*/string id) {
		super(range);
		this.id = id;
	}

	@Override public <R, A> R accept(/*final*/GenericVisitor<R, A> v, /*final*/A arg) {
		return v.visit(this, arg);
	}

	@Override public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg) {
		v.visit(this, arg);
	}

	public string getId() {
		return id;
	}

	public ContinueStmt setId(/*final*/string id) {
		this.id = id;
		return this;
	}
}
