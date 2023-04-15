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
 
namespace com.github.javaparser.ast.comments;


/**
 * Abstract class for all AST nodes that represent comments.
 * 
 * @see BlockComment
 * @see LineComment
 * @see JavadocComment
 * @author Julio Vilmar Gesser
 */
public abstract class Comment:Node {

    private string content;
    private Node commentedNode;

    public Comment() {
    }

    public Comment(string content) {
        this.content = content;
    }

    public Comment(Range range, string content) {
        super(range);
        this.content = content;
    }

    /**
     * Return the text of the comment.
     * 
     * @return text of the comment
     */
    public /*final*/string getContent() {
        return content;
    }

    /**
     * Sets the text of the comment.
     * 
     * @param content
     *            the text of the comment to set
     */
    public Comment setContent(string content) {
        this.content = content;
        return this;
    }

    public boolean isLineComment()
    {
        return false;
    }

    public LineComment asLineComment() {
        if (isLineComment()) {
            return (LineComment) this;
        } else {
            throw new UnsupportedOperationException("Not a line comment");
        }
    }

    public Node getCommentedNode()
    {
        return this.commentedNode;
    }

    public Comment setCommentedNode(Node commentedNode)
    {
        if (commentedNode==null) {
            this.commentedNode = null;
            return this;
        }
        if (commentedNode==this) {
            throw new IllegalArgumentException();
        }
        if (commentedNode is Comment) {
            throw new IllegalArgumentException();
        }
        this.commentedNode = commentedNode;
        return this;
    }

    public boolean isOrphan()
    {
        return this.commentedNode == null;
    }
}
