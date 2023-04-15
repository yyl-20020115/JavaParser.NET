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
 * Lambda expressions. 
 * @author Raquel Pau
 *
 */
public class LambdaExpr:Expression {

	private List<Parameter> parameters;

	private bool parametersEnclosed;

	private Statement body;

	public LambdaExpr() {
	}

	public LambdaExpr(int beginLine, int beginColumn, int endLine,
                      int endColumn, List<Parameter> parameters, Statement body,
                      bool parametersEnclosed) {

		base(beginLine, beginColumn, endLine, endColumn);
		setParameters(parameters);
		setBody(body);
        setParametersEnclosed(parametersEnclosed);
	}

	public List<Parameter> getParameters() {
		return parameters;
	}

	public void setParameters(List<Parameter> parameters) {
		this.parameters = parameters;
		setAsParentNodeOf(this.parameters);
	}

	public Statement getBody() {
		return body;
	}

	public void setBody(Statement body) {
		this.body = body;
		setAsParentNodeOf(this.body);
	}

	@Override
	public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override
	public <A> void accept(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public bool isParametersEnclosed() {
		return parametersEnclosed;
	}

	public void setParametersEnclosed(bool parametersEnclosed) {
		this.parametersEnclosed = parametersEnclosed;
	}

}
