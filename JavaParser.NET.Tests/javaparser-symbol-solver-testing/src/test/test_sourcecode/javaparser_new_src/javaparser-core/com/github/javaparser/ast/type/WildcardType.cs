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
 
namespace com.github.javaparser.ast.type;


/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class WildcardType:Type<WildcardType> implements NodeWithAnnotations<WildcardType> {

	private ReferenceType ext;

	private ReferenceType sup;

	public WildcardType() {
	}

	public WildcardType(/*final*/ReferenceType ext) {
		setExtends(ext);
	}

	public WildcardType(/*final*/ReferenceType ext, /*final*/ReferenceType sup) {
		setExtends(ext);
		setSuper(sup);
	}

	public WildcardType(/*final*/Range range,
			/*final*/ReferenceType ext, /*final*/ReferenceType sup) {
		base(range);
		setExtends(ext);
		setSuper(sup);
	}

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public ReferenceType getExtends() {
		return ext;
	}

	public ReferenceType getSuper() {
		return sup;
	}

	public WildcardType setExtends(/*final*/ReferenceType ext) {
		this.ext = ext;
		setAsParentNodeOf(this.ext);
		return this;
	}

	public WildcardType setSuper(/*final*/ReferenceType sup) {
		this.sup = sup;
		setAsParentNodeOf(this.sup);
		return this;
	}

}
