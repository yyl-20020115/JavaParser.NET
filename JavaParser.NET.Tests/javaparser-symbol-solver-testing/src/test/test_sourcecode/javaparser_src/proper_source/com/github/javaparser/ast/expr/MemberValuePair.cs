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
public /*final*/class MemberValuePair:Node implements NamedNode {

	private string name;

	private Expression value;

	public MemberValuePair() {
	}

	public MemberValuePair(/*final*/string name, /*final*/Expression value) {
		setName(name);
		setValue(value);
	}

	public MemberValuePair(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/string name, /*final*/Expression value) {
		super(beginLine, beginColumn, endLine, endColumn);
		setName(name);
		setValue(value);
	}

	@Override public <R, A> R accept(/*final*/GenericVisitor<R, A> v, /*final*/A arg) {
		return v.visit(this, arg);
	}

	@Override public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg) {
		v.visit(this, arg);
	}

	public string getName() {
		return name;
	}

	public Expression getValue() {
		return value;
	}

	public void setName(/*final*/string name) {
		this.name = name;
	}

	public void setValue(/*final*/Expression value) {
		this.value = value;
		setAsParentNodeOf(this.value);
	}
}
