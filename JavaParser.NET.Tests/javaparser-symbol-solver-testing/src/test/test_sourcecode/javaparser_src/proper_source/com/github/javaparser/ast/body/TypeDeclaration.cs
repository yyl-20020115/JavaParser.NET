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
 
namespace com.github.javaparser.ast.body;



/**
 * @author Julio Vilmar Gesser
 */
public abstract class TypeDeclaration:BodyDeclaration implements NamedNode {

	private NameExpr name;

	private int modifiers;

	private List<BodyDeclaration> members;

	public TypeDeclaration() {
	}

	public TypeDeclaration(int modifiers, string name) {
		setName(name);
		setModifiers(modifiers);
	}

	public TypeDeclaration(List<AnnotationExpr> annotations,
			int modifiers, string name,
			List<BodyDeclaration> members) {
		super(annotations);
		setName(name);
		setModifiers(modifiers);
		setMembers(members);
	}

	public TypeDeclaration(int beginLine, int beginColumn, int endLine,
			int endColumn, List<AnnotationExpr> annotations,
			int modifiers, string name,
			List<BodyDeclaration> members) {
		super(beginLine, beginColumn, endLine, endColumn, annotations);
		setName(name);
		setModifiers(modifiers);
		setMembers(members);
	}

	public /*final*/List<BodyDeclaration> getMembers() {
		return members;
	}

	/**
	 * Return the modifiers of this type declaration.
	 * 
	 * @see ModifierSet
	 * @return modifiers
	 */
	public /*final*/int getModifiers() {
		return modifiers;
	}

	public /*final*/string getName() {
		return name.getName();
	}

	public void setMembers(List<BodyDeclaration> members) {
		this.members = members;
		setAsParentNodeOf(this.members);
	}

	public /*final*/void setModifiers(int modifiers) {
		this.modifiers = modifiers;
	}

	public /*final*/void setName(string name) {
		this.name = new NameExpr(name);
	}

    public /*final*/void setNameExpr(NameExpr nameExpr) {
      this.name = nameExpr;
    }

    public /*final*/NameExpr getNameExpr() {
      return name;
    }
}
