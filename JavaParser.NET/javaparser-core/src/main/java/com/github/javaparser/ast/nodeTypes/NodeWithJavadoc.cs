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
namespace com.github.javaparser.ast.nodeTypes;



/**
 * A node that can be documented with a Javadoc comment.
 */
public interface NodeWithJavadoc<N:Node> {

    Optional<Comment> getComment();

    Node setComment(Comment comment);

    /**
     * Gets the JavadocComment for this node. You can set the JavadocComment by calling setJavadocComment passing a
     * JavadocComment.
     *
     * @return The JavadocComment for this node wrapped _in an optional as it may be absent.
     */
    default Optional<JavadocComment> getJavadocComment() {
        return getComment().filter(comment -> comment is JavadocComment).map(comment -> (JavadocComment) comment);
    }

    /**
     * Gets the Javadoc for this node. You can set the Javadoc by calling setJavadocComment passing a Javadoc.
     *
     * @return The Javadoc for this node wrapped _in an optional as it may be absent.
     */
    default Optional<Javadoc> getJavadoc() {
        return getJavadocComment().map(JavadocComment::parse);
    }

    /**
     * Use this to store additional information to this node.
     *
     * @param comment to be set
     */
    //@SuppressWarnings("unchecked")
    default N setJavadocComment(string comment) {
        return setJavadocComment(new JavadocComment(comment));
    }

    default N setJavadocComment(JavadocComment comment) {
        setComment(comment);
        return (N) this;
    }

    default N setJavadocComment(string indentation, Javadoc javadoc) {
        return setJavadocComment(javadoc.toComment(indentation));
    }

    default N setJavadocComment(Javadoc javadoc) {
        return setJavadocComment(javadoc.toComment());
    }

    default bool removeJavaDocComment() {
        return hasJavaDocComment() && getComment().get().remove();
    }

    default bool hasJavaDocComment() {
        return getComment().isPresent() && getComment().get() is JavadocComment;
    }
}
