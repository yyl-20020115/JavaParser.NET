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
public /*final*/class ObjectCreationExpr:Expression {

	private Expression scope;

	private ClassOrInterfaceType type;

	private List<Type> typeArgs;

	private List<Expression> args;

	private List<BodyDeclaration> anonymousClassBody;

	public ObjectCreationExpr() {
	}

	public ObjectCreationExpr(/*final*/Expression scope, /*final*/ClassOrInterfaceType type, /*final*/List<Expression> args) {
		setScope(scope);
		setType(type);
		setArgs(args);
	}

	public ObjectCreationExpr(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/Expression scope, /*final*/ClassOrInterfaceType type, /*final*/List<Type> typeArgs,
			/*final*/List<Expression> args, /*final*/List<BodyDeclaration> anonymousBody) {
		base(beginLine, beginColumn, endLine, endColumn);
		setScope(scope);
		setType(type);
		setTypeArgs(typeArgs);
		setArgs(args);
		setAnonymousClassBody(anonymousBody);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public List<BodyDeclaration> getAnonymousClassBody() {
		return anonymousClassBody;
	}

	public List<Expression> getArgs() {
		return args;
	}

	public Expression getScope() {
		return scope;
	}

	public ClassOrInterfaceType getType() {
		return type;
	}

	public List<Type> getTypeArgs() {
		return typeArgs;
	}

	public void setAnonymousClassBody(/*final*/List<BodyDeclaration> anonymousClassBody) {
		this.anonymousClassBody = anonymousClassBody;
		setAsParentNodeOf(this.anonymousClassBody);
	}

	public void setArgs(/*final*/List<Expression> args) {
		this.args = args;
		setAsParentNodeOf(this.args);
	}

	public void setScope(/*final*/Expression scope) {
		this.scope = scope;
		setAsParentNodeOf(this.scope);
	}

	public void setType(/*final*/ClassOrInterfaceType type) {
		this.type = type;
		setAsParentNodeOf(this.type);
	}

	public void setTypeArgs(/*final*/List<Type> typeArgs) {
		this.typeArgs = typeArgs;
		setAsParentNodeOf(this.typeArgs);
	}
}
