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
 
namespace com.github.javaparser.ast.stmt;



/**
 * @author Julio Vilmar Gesser
 */
public /*final*/class CatchClause:Node implements NodeWithBlockStmt<CatchClause> {

    private Parameter param;

    private BlockStmt catchBlock;

    public CatchClause() {
    }

    public CatchClause(/*final*/Parameter param, /*final*/BlockStmt catchBlock) {
        setParam(param);
        setBody(catchBlock);
    }

    public CatchClause(/*final*/Range range,
                       /*final*/EnumSet<Modifier> exceptModifier, 
                       /*final*/List<AnnotationExpr> exceptAnnotations,
                       /*final*/Type exceptType,
                       /*final*/VariableDeclaratorId exceptId, 
                       /*final*/BlockStmt catchBlock) {
        base(range);
        setParam(new Parameter(range, exceptModifier, exceptAnnotations, exceptType, null, false, exceptId));
        setBody(catchBlock);
    }

	//@Override
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
		return v.visit(this, arg);
	}

	//@Override 
    public void accept<A>(VoidVisitor<A> v, A arg) {
		v.visit(this, arg);
	}

    /**
     * Use {@link #getBody()} instead
     */
    //@Deprecated
	public BlockStmt getCatchBlock() {
		return catchBlock;
	}

	/**
	 * Note that the type of the Parameter can be a UnionType. In this case, any annotations found at the start of the catch(@X A a |...)
	 * are found directly _in the Parameter. Annotations that are on the second or later type - catch(A a | @X B b ...) are found on those types.
	 */
	public Parameter getParam() {
		return param;
	}

    /**
     * Use {@link #setBody(BlockStmt)} instead
     * 
     * @param catchBlock
     */
    //@Deprecated
	public CatchClause setCatchBlock(/*final*/BlockStmt catchBlock) {
		this.catchBlock = catchBlock;
		setAsParentNodeOf(this.catchBlock);
        return this;
	}

	public CatchClause setParam(/*final*/Parameter param) {
		this.param = param;
		setAsParentNodeOf(this.param);
        return this;
	}

    //@Override
    public BlockStmt getBody() {
        return catchBlock;
    }

    //@Override
    public CatchClause setBody(BlockStmt block) {
        this.catchBlock = block;
        setAsParentNodeOf(this.catchBlock);
        return this;
    }
}
