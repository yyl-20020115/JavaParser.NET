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
public /*final*/class SynchronizedStmt:Statement implements NodeWithBlockStmt<SynchronizedStmt> {

    private Expression expr;

    private BlockStmt block;

    public SynchronizedStmt() {
    }

    public SynchronizedStmt(/*final*/Expression expr, /*final*/BlockStmt block) {
        setExpr(expr);
        setBlock(block);
    }

    public SynchronizedStmt(Range range, /*final*/Expression expr,
                            /*final*/BlockStmt block) {
        base(range);
        setExpr(expr);
        setBlock(block);
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
     * @deprecated use {@link #getBody()}
     * @return
     */
    //@Deprecated
    public BlockStmt getBlock() {
        return block;
    }

    public Expression getExpr() {
        return expr;
    }

    /**
     * @deprecated Use {@link #setBody(BlockStmt)} instead
     * @param block
     */
    //@Deprecated
    public SynchronizedStmt setBlock(/*final*/BlockStmt block) {
        this.block = block;
        setAsParentNodeOf(this.block);
        return this;
    }

    public SynchronizedStmt setExpr(/*final*/Expression expr) {
        this.expr = expr;
        setAsParentNodeOf(this.expr);
        return this;
    }

    //@Override
    public BlockStmt getBody() {
        return block;
    }

    //@Override
    public SynchronizedStmt setBody(BlockStmt block) {
        this.block = block;
        setAsParentNodeOf(this.block);
        return this;
    }
}
