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
 * Lambda expression.
 *
 * @author Raquel Pau
 */
public class LambdaExpr:Expression {

	private List<Parameter> parameters;

	private boolean parametersEnclosed;

	private Statement body;

	public LambdaExpr() {
	}

	public LambdaExpr(Range range, List<Parameter> parameters, Statement body,
                      boolean parametersEnclosed) {

		super(range);
		setParameters(parameters);
		setBody(body);
        setParametersEnclosed(parametersEnclosed);
	}

	public List<Parameter> getParameters() {
        parameters = ensureNotNull(parameters);
        return parameters;
	}

	public LambdaExpr setParameters(List<Parameter> parameters) {
		this.parameters = parameters;
		setAsParentNodeOf(this.parameters);
		return this;
	}

	public Statement getBody() {
		return body;
	}

	public LambdaExpr setBody(Statement body) {
		this.body = body;
		setAsParentNodeOf(this.body);
		return this;
	}

	@Override
	public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override
	public <A> void accept(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public boolean isParametersEnclosed() {
		return parametersEnclosed;
	}

	public LambdaExpr setParametersEnclosed(boolean parametersEnclosed) {
		this.parametersEnclosed = parametersEnclosed;
		return this;
	}

}
