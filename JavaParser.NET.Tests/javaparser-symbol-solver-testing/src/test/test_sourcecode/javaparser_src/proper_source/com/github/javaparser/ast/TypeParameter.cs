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
 
namespace com.github.javaparser.ast;



/**
 * <p>
 * This class represents the declaration of a generics argument.
 * </p>
 * The TypeParameter is constructed following the syntax:<br>
 * <pre>
 * {@code
 * TypeParameter ::= <IDENTIFIER> ( "extends" }{@link ClassOrInterfaceType}{@code ( "&" }{@link ClassOrInterfaceType}{@code )* )?
 * }
 * </pre>
 * @author Julio Vilmar Gesser
 */
public /*final*/class TypeParameter:Node implements NamedNode {

	private string name;

    private List<AnnotationExpr> annotations;

	private List<ClassOrInterfaceType> typeBound;

	public TypeParameter() {
	}

	public TypeParameter(/*final*/string name, /*final*/List<ClassOrInterfaceType> typeBound) {
		setName(name);
		setTypeBound(typeBound);
	}

	public TypeParameter(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
			/*final*/string name, /*final*/List<ClassOrInterfaceType> typeBound) {
		base(beginLine, beginColumn, endLine, endColumn);
		setName(name);
		setTypeBound(typeBound);
	}

    public TypeParameter(int beginLine, int beginColumn, int endLine,
                         int endColumn, string name, List<ClassOrInterfaceType> typeBound, List<AnnotationExpr> annotations) {
        this(beginLine, beginColumn, endLine, endColumn, name, typeBound);
        setName(name);
        setTypeBound(typeBound);
        this.annotations = annotations;
    }

	@Override public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	@Override public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	/**
	 * Return the name of the paramenter.
	 * 
	 * @return the name of the paramenter
	 */
	public string getName() {
		return name;
	}

	/**
	 * Return the list of {@link ClassOrInterfaceType} that this parameter
	 * extends. Return <code>null</code> null if there are no type.
	 * 
	 * @return list of types that this paramente:or <code>null</code>
	 */
	public List<ClassOrInterfaceType> getTypeBound() {
		return typeBound;
	}

	/**
	 * Sets the name of this type parameter.
	 * 
	 * @param name
	 *            the name to set
	 */
	public void setName(/*final*/string name) {
		this.name = name;
	}

	/**
	 * Sets the list o types.
	 * 
	 * @param typeBound
	 *            the typeBound to set
	 */
	public void setTypeBound(/*final*/List<ClassOrInterfaceType> typeBound) {
		this.typeBound = typeBound;
		setAsParentNodeOf(typeBound);
	}

    public List<AnnotationExpr> getAnnotations() {
        return annotations;
    }

    public void setAnnotations(List<AnnotationExpr> annotations) {
        this.annotations = annotations;
    }
}
