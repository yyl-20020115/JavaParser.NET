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
 
namespace com.github.javaparser.ast.stmt;



/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class CatchClause:Node {

    private MultiTypeParameter except;

    private BlockStmt catchBlock;

    public CatchClause() {
    }

    public CatchClause(/*final*/MultiTypeParameter except, /*final*/BlockStmt catchBlock) {
        setExcept(except);
        setCatchBlock(catchBlock);
    }
	
    public CatchClause(int exceptModifier, List<AnnotationExpr> exceptAnnotations, List<Type> exceptTypes, VariableDeclaratorId exceptId, BlockStmt catchBlock) {
        this(new MultiTypeParameter(exceptModifier, exceptAnnotations, exceptTypes, exceptId), catchBlock);
    }

    public CatchClause(/*final*/int beginLine, /*final*/int beginColumn, /*final*/int endLine, /*final*/int endColumn,
    	    /*final*/int exceptModifier, /*final*/List<AnnotationExpr> exceptAnnotations, /*final*/List<Type> exceptTypes, 
    	    /*final*/VariableDeclaratorId exceptId, /*final*/BlockStmt catchBlock) {
        base(beginLine, beginColumn, endLine, endColumn);
        setExcept(new MultiTypeParameter(beginLine, beginColumn, endLine, endColumn, exceptModifier, exceptAnnotations, exceptTypes, exceptId));
        setCatchBlock(catchBlock);
    }

	//@Override 
	public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	//@Override
	public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

	public BlockStmt getCatchBlock() {
		return catchBlock;
	}

	public MultiTypeParameter getExcept() {
		return except;
	}

	public void setCatchBlock(/*final*/BlockStmt catchBlock) {
		this.catchBlock = catchBlock;
		setAsParentNodeOf(this.catchBlock);
	}

	public void setExcept(/*final*/MultiTypeParameter except) {
		this.except = except;
		setAsParentNodeOf(this.except);
	}
}
