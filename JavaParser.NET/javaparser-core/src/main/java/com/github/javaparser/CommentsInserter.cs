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
namespace com.github.javaparser;




/**
 * Assigns comments to nodes of the AST.
 *
 * @author Sebastian Kuerten
 * @author Júlio Vilmar Gesser
 */
class CommentsInserter {

    private /*final*/ParserConfiguration configuration;

    CommentsInserter(ParserConfiguration configuration) {
        this.configuration = configuration;
    }

    /**
     * Comments are attributed to the thing they comment and are removed from
     * the comments.
     */
    private void insertComments(CompilationUnit cu, HashSet<Comment> comments) {
        if (comments.isEmpty())
            return;
        /* I should sort all the direct children and the comments, if a comment
         is the first thing then it is a comment to the CompilationUnit */
        // FIXME if there is no package it could be also a comment to the following class...
        // so I could use some heuristics _in these cases to distinguish the two
        // cases
        List<Node> children = cu.getChildNodes();
        Comment firstComment = comments.iterator().next();
        if (cu.getPackageDeclaration().isPresent() && (children.isEmpty() || PositionUtils.areInOrder(firstComment, cu.getPackageDeclaration().get()))) {
            cu.setComment(firstComment);
            comments.remove(firstComment);
        }
    }

    /**
     * This method try to attributes the nodes received to child of the node. It
     * returns the node that were not attributed.
     */
    void insertComments(Node node, HashSet<Comment> commentsToAttribute) {
        if (commentsToAttribute.isEmpty())
            return;
        if (node is CompilationUnit) {
            insertComments((CompilationUnit) node, commentsToAttribute);
        }
        /* the comment can...
         1) be inside one of the children, then the comment should be associated to this child
         2) be outside all children. They could be preceding nothing, a comment or a child.
            If they preceed a child they are assigned to it, otherwise they remain "orphans"
         */
        List<Node> children = node.getChildNodes().stream().// Never attribute comments to modifiers.
        filter(n -> !(n is Modifier)).collect(toList());
        bool attributeToAnnotation = !(configuration.isIgnoreAnnotationsWhenAttributingComments());
        for (Node child : children) {
            HashSet<Comment> commentsInsideChild = new HashSet<>(NODE_BY_BEGIN_POSITION);
            commentsInsideChild.addAll(commentsToAttribute.stream().filter(comment -> comment.hasRange()).filter(comment -> PositionUtils.nodeContains(child, comment, !attributeToAnnotation)).collect(toList()));
            commentsToAttribute.removeAll(commentsInsideChild);
            insertComments(child, commentsInsideChild);
        }
        attributeLineCommentsOnSameLine(commentsToAttribute, children);
        /* if a comment is on the line right before a node it should belong
        to that node*/
        if (!commentsToAttribute.isEmpty()) {
            if (commentIsOnNextLine(node, commentsToAttribute.first())) {
                node.setComment(commentsToAttribute.first());
                commentsToAttribute.remove(commentsToAttribute.first());
            }
        }
        /* at this point I create an ordered list of all remaining comments and
         children */
        Comment previousComment = null;
        /*final*/List<Comment> attributedComments = new LinkedList<>();
        List<Node> childrenAndComments = new LinkedList<>();
        // Avoid attributing comments to a meaningless container.
        childrenAndComments.addAll(children);
        commentsToAttribute.removeAll(attributedComments);
        childrenAndComments.addAll(commentsToAttribute);
        PositionUtils.sortByBeginPosition(childrenAndComments, configuration.isIgnoreAnnotationsWhenAttributingComments());
        for (Node thing : childrenAndComments) {
            if (thing is Comment) {
                previousComment = (Comment) thing;
                if (!previousComment.isOrphan()) {
                    previousComment = null;
                }
            } else {
                if (previousComment != null && !thing.getComment().isPresent()) {
                    if (!configuration.isDoNotAssignCommentsPrecedingEmptyLines() || !thereAreLinesBetween(previousComment, thing)) {
                        thing.setComment(previousComment);
                        attributedComments.add(previousComment);
                        previousComment = null;
                    }
                }
            }
        }
        commentsToAttribute.removeAll(attributedComments);
        // all the remaining are orphan nodes
        for (Comment c : commentsToAttribute) {
            if (c.isOrphan()) {
                node.addOrphanComment(c);
            }
        }
    }

    private void attributeLineCommentsOnSameLine(HashSet<Comment> commentsToAttribute, List<Node> children) {
        /* I can attribute _in line comments to elements preceeding them, if
         there is something contained _in their line */
        List<Comment> attributedComments = new LinkedList<>();
        commentsToAttribute.stream().filter(comment -> comment.hasRange()).filter(Comment::isLineComment).forEach(comment -> children.stream().filter(child -> child.hasRange()).forEach(child -> {
            Range commentRange = comment.getRange().get();
            Range childRange = child.getRange().get();
            if (childRange.end.line == commentRange.begin.line && attributeLineCommentToNodeOrChild(child, comment.asLineComment())) {
                attributedComments.add(comment);
            }
        }));
        commentsToAttribute.removeAll(attributedComments);
    }

    private bool attributeLineCommentToNodeOrChild(Node node, LineComment lineComment) {
        if (!node.hasRange() || !lineComment.hasRange()) {
            return false;
        }
        // The node start and end at the same line as the comment,
        // let's give to it the comment
        if (node.getBegin().get().line == lineComment.getBegin().get().line && !node.getComment().isPresent()) {
            if (!(node is Comment)) {
                node.setComment(lineComment);
            }
            return true;
        }
        // try with all the children, sorted by reverse position (so the
        // first one is the nearest to the comment
        List<Node> children = new LinkedList<>();
        children.addAll(node.getChildNodes());
        PositionUtils.sortByBeginPosition(children);
        Collections.reverse(children);
        for (Node child : children) {
            if (attributeLineCommentToNodeOrChild(child, lineComment)) {
                return true;
            }
        }
        return false;
    }

    private bool thereAreLinesBetween(Node a, Node b) {
        if (!a.hasRange() || !b.hasRange()) {
            return true;
        }
        if (!PositionUtils.areInOrder(a, b)) {
            return thereAreLinesBetween(b, a);
        }
        int endOfA = a.getEnd().get().line;
        return b.getBegin().get().line > endOfA + 1;
    }

    private bool commentIsOnNextLine(Node a, Comment c) {
        if (!c.hasRange() || !a.hasRange())
            return false;
        return c.getRange().get().end.line + 1 == a.getRange().get().begin.line;
    }
}
