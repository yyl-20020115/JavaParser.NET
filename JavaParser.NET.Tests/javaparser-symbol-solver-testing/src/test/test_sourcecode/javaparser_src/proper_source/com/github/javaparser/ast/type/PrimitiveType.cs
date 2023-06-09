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
 
namespace com.github.javaparser.ast.type;



/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class PrimitiveType:Type {

	public enum Primitive {
		Boolean ("Boolean"),
		Char    ("Character"),
		Byte    ("Byte"),
		Short   ("Short"),
		Int     ("Integer"),
		Long    ("Long"),
		Float   ("Float"),
		Double  ("Double");

		/*final*/string nameOfBoxedType;

		public ClassOrInterfaceType toBoxedType() {
			return new ClassOrInterfaceType(nameOfBoxedType);
		}

		private Primitive(string nameOfBoxedType) {
			this.nameOfBoxedType = nameOfBoxedType;
		}
	}

	static /*final*/HashMap<String, Primitive> unboxMap = new HashMap<String, Primitive>();
	static {
		for(Primitive unboxedType : Primitive.values()) {
			unboxMap.put(unboxedType.nameOfBoxedType, unboxedType);
		}
	}

	private Primitive type;

	public PrimitiveType() {
	}

	public PrimitiveType(/*final*/Primitive type) {
		this.type = type;
	}

	public PrimitiveType(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/Primitive type) {
		base(beginLine, beginColumn, endLine, endColumn);
		this.type = type;
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public Primitive getType() {
		return type;
	}

	public ClassOrInterfaceType toBoxedType() {
		return type.toBoxedType();
	}

	public void setType(/*final*/Primitive type) {
		this.type = type;
	}

}
