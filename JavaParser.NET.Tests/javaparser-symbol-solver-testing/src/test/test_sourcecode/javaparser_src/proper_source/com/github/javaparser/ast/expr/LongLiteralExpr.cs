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
public class LongLiteralExpr:StringLiteralExpr {

	private static /*final*/string UNSIGNED_MIN_VALUE = "9223372036854775808";

	protected static /*final*/string MIN_VALUE = "-" + UNSIGNED_MIN_VALUE + "L";

	public LongLiteralExpr() {
	}

	public LongLiteralExpr(/*final*/string value) {
		base(value);
	}

	public LongLiteralExpr(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/string value) {
		base(beginLine, beginColumn, endLine, endColumn, value);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public /*final*/bool isMinValue() {
		return value != null && //
				value.length() == 20 && //
				value.startsWith(UNSIGNED_MIN_VALUE) && //
				(value.charAt(19) == 'L' || value.charAt(19) == 'l');
	}
}
