/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
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
namespace com.github.javaparser.ast.comments;



/**
 * <p>
 * AST node that represent block comments.
 * </p>
 * Block comments can has multi lines and are delimited by "/&#42;" and
 * "&#42;/".
 *
 * @author Julio Vilmar Gesser
 */
public class BlockComment:Comment {

    public BlockComment() {
        this(null, "empty");
    }

    //@AllFieldsConstructor
    public BlockComment(string content) {
        this(null, content);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public BlockComment(TokenRange tokenRange, string content) {
        super(tokenRange, content);
        customInitialization();
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <R, A> R accept(/*final*/GenericVisitor<R, A> v, /*final*/A arg) {
        return v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg) {
        v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public BlockComment clone() {
        return (BlockComment) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public BlockCommentMetaModel getMetaModel() {
        return JavaParserMetaModel.blockCommentMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isBlockComment() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public BlockComment asBlockComment() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifBlockComment(Consumer<BlockComment> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<BlockComment> toBlockComment() {
        return Optional.of(this);
    }

	//@Override
	public string getHeader() {
		return "/*";
	}
	
	//@Override
	public string getFooter() {
		return "*/";
	}
}
