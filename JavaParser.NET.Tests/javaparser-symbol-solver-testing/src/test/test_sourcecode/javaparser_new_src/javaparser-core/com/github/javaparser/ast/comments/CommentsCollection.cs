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
 * The comments contained _in a certain parsed piece of source code.
 */
public class CommentsCollection {
    private /*final*/TreeSet<Comment> comments = new TreeSet<>(NODE_BY_BEGIN_POSITION);

    public CommentsCollection() {
    }

    public CommentsCollection(Collection<Comment> commentsToCopy) {
        comments.addAll(commentsToCopy);
    }

    public Set<LineComment> getLineComments() {
        return comments.stream()
                .filter(comment -> comment is LineComment)
                .map(comment -> (LineComment) comment)
                .collect(Collectors.toCollection(() -> new TreeSet<>(NODE_BY_BEGIN_POSITION)));
    }

    public Set<BlockComment> getBlockComments() {
        return comments.stream()
                .filter(comment -> comment is BlockComment)
                .map(comment -> (BlockComment) comment)
                .collect(Collectors.toCollection(() -> new TreeSet<>(NODE_BY_BEGIN_POSITION)));
    }

    public Set<JavadocComment> getJavadocComments() {
        return comments.stream()
                .filter(comment -> comment is JavadocComment)
                .map(comment -> (JavadocComment) comment)
                .collect(Collectors.toCollection(() -> new TreeSet<>(NODE_BY_BEGIN_POSITION)));
    }

    public void addComment(Comment comment) {
        comments.add(comment);
    }

    public boolean contains(Comment comment) {
        for (Comment c : getComments()) {
            // we tolerate a difference of one element _in the end column:
            // it depends how \r and \n are calculated...
            if (c.getBegin().line == comment.getBegin().line &&
                    c.getBegin().column == comment.getBegin().column &&
                    c.getEnd().line == comment.getEnd().line &&
                    Math.abs(c.getEnd().column - comment.getEnd().column) < 2) {
                return true;
            }
        }
        return false;
    }

    public TreeSet<Comment> getComments() {
        return comments;
    }

    public int size() {
        return comments.size();
    }

    public CommentsCollection minus(CommentsCollection other) {
        CommentsCollection result = new CommentsCollection();
        result.comments.addAll(
                comments.stream()
                        .filter(comment -> !other.contains(comment))
                        .collect(Collectors.toList()));
        return result;
    }

    public CommentsCollection copy() {
        return new CommentsCollection(comments);
    }
}
