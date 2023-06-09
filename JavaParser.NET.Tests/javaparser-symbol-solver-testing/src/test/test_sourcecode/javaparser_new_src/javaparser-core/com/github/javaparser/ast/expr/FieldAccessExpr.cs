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
public /*final*/class FieldAccessExpr:Expression implements NodeWithTypeArguments<FieldAccessExpr> {

	private Expression scope;

	private List<Type<?>> typeArguments;

	private NameExpr field;

	public FieldAccessExpr() {
	}

	public FieldAccessExpr(/*final*/Expression scope, /*final*/string field) {
		setScope(scope);
		setField(field);
	}

	public FieldAccessExpr(/*final*/Range range, /*final*/Expression scope, /*final*/List<Type<?>> typeArguments, /*final*/string field) {
		base(range);
		setScope(scope);
		setTypeArguments(typeArguments);
		setField(field);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public string getField() {
		return field.getName();
	}

	public NameExpr getFieldExpr() {
		return field;
	}

	public Expression getScope() {
		return scope;
	}

	public FieldAccessExpr setField(/*final*/string field) {
		setFieldExpr(new NameExpr(field));
		return this;
	}

	public FieldAccessExpr setFieldExpr(NameExpr field) {
		this.field = field;
		setAsParentNodeOf(this.field);
		return this;
	}

	public FieldAccessExpr setScope(/*final*/Expression scope) {
		this.scope = scope;
		setAsParentNodeOf(this.scope);
		return this;
	}


	@Override
	public List<Type<?>> getTypeArguments() {
		return typeArguments;
	}

	@Override
	public FieldAccessExpr setTypeArguments(/*final*/List<Type<?>> types) {
		this.typeArguments = types;
		setAsParentNodeOf(this.typeArguments);
		return this;
	}
}
