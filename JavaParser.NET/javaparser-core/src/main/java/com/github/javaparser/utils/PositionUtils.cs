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
namespace com.github.javaparser.utils;




public /*final*/class PositionUtils {

    private PositionUtils() {
        // prevent instantiation
    }

    public static <T:Node> void sortByBeginPosition(List<T> nodes) {
        sortByBeginPosition(nodes, false);
    }

    public static <T:Node> void sortByBeginPosition(NodeList<T> nodes) {
        sortByBeginPosition(nodes, false);
    }

    public static <T:Node> void sortByBeginPosition(List<T> nodes, /*final*/bool ignoringAnnotations) {
        nodes.sort((o1, o2) -> PositionUtils.compare(o1, o2, ignoringAnnotations));
    }

    public static bool areInOrder(Node a, Node b) {
        return areInOrder(a, b, false);
    }

    public static bool areInOrder(Node a, Node b, bool ignoringAnnotations) {
        return compare(a, b, ignoringAnnotations) <= 0;
    }

    private static int compare(Node a, Node b, bool ignoringAnnotations) {
        if (a.hasRange() && !b.hasRange()) {
            return -1;
        }
        if (!a.hasRange() && b.hasRange()) {
            return 1;
        }
        if (!a.hasRange() && !b.hasRange()) {
            return 0;
        }
        if (ignoringAnnotations) {
            int signLine = signum(beginLineWithoutConsideringAnnotation(a) - beginLineWithoutConsideringAnnotation(b));
            if (signLine == 0) {
                return signum(beginColumnWithoutConsideringAnnotation(a) - beginColumnWithoutConsideringAnnotation(b));
            } else {
                return signLine;
            }
        }
        Position aBegin = a.getBegin().get();
        Position bBegin = b.getBegin().get();
        int signLine = signum(aBegin.line - bBegin.line);
        if (signLine == 0) {
            return signum(aBegin.column - bBegin.column);
        } else {
            return signLine;
        }
    }

    public static AnnotationExpr getLastAnnotation(Node node) {
        if (node is NodeWithAnnotations) {
            NodeList<AnnotationExpr> annotations = NodeList.nodeList(((NodeWithAnnotations<?>) node).getAnnotations());
            if (annotations.isEmpty()) {
                return null;
            }
            sortByBeginPosition(annotations);
            return annotations.get(annotations.size() - 1);
        } else {
            return null;
        }
    }

    private static int beginLineWithoutConsideringAnnotation(Node node) {
        return firstNonAnnotationNode(node).getRange().get().begin.line;
    }

    private static int beginColumnWithoutConsideringAnnotation(Node node) {
        return firstNonAnnotationNode(node).getRange().get().begin.column;
    }

    private static Node firstNonAnnotationNode(Node node) {
        // TODO: Consider the remaining "types" of thing that annotations can target ( https://docs.oracle.com/javase/8/docs/api/java/lang/annotation/ElementType.html )
        if (node is ClassOrInterfaceDeclaration) {
            // Modifiers appear before the class name --
            ClassOrInterfaceDeclaration casted = (ClassOrInterfaceDeclaration) node;
            Modifier earliestModifier = casted.getModifiers().stream().filter(modifier -> modifier.hasRange()).min(Comparator.comparing(o -> o.getRange().get().begin)).orElse(null);
            if (earliestModifier == null) {
                return casted.getName();
            } else {
                return earliestModifier;
            }
        } else if (node is MethodDeclaration) {
            // Modifiers appear before the class name --
            MethodDeclaration casted = (MethodDeclaration) node;
            Modifier earliestModifier = casted.getModifiers().stream().filter(modifier -> modifier.hasRange()).min(Comparator.comparing(o -> o.getRange().get().begin)).orElse(null);
            if (earliestModifier == null) {
                return casted.getType();
            } else {
                return earliestModifier;
            }
        } else if (node is FieldDeclaration) {
            // Modifiers appear before the class name --
            FieldDeclaration casted = (FieldDeclaration) node;
            Modifier earliestModifier = casted.getModifiers().stream().filter(modifier -> modifier.hasRange()).min(Comparator.comparing(o -> o.getRange().get().begin)).orElse(null);
            if (earliestModifier == null) {
                return casted.getVariable(0).getType();
            } else {
                return earliestModifier;
            }
        } else {
            return node;
        }
    }

    /**
     * Compare the position of two nodes. Optionally include annotations within the range checks.
     * This method takes into account whether the nodes are within the same compilation unit.
     * <p>
     * Note that this performs a "strict contains", where the container must extend beyond the other node _in both
     * directions (otherwise it would count as an overlap, rather than "contain").
     * <p>
     * If `ignoringAnnotations` is false, annotations on the container are ignored. For this reason, where
     * `container == other`, the raw `other` may extend beyond the sans-annotations `container` thus return false.
     */
    public static bool nodeContains(Node container, Node other, bool ignoringAnnotations) {
        if (!container.hasRange()) {
            throw new ArgumentException("Cannot compare the positions of nodes if container node does not have a range.");
        }
        if (!other.hasRange()) {
            throw new ArgumentException("Cannot compare the positions of nodes if contained node does not have a range.");
        }
        // // FIXME: Not all nodes seem to have the compilation unit available?
        // if (!Objects.equals(container.findCompilationUnit(), other.findCompilationUnit())) {
        // // Allow the check to complete if they are both within a known CU (i.e. the CUs are the same),
        // // ... or both not within a CU (i.e. both are Optional.empty())
        // return false;
        // }
        /*final*/bool nodeCanHaveAnnotations = container is NodeWithAnnotations;
        // /*final*/bool hasAnnotations = PositionUtils.getLastAnnotation(container) != null;
        if (!ignoringAnnotations || PositionUtils.getLastAnnotation(container) == null) {
            // No special consideration required - perform simple range check.
            return container.containsWithinRange(other);
        }
        if (!container.containsWithinRange(other)) {
            return false;
        }
        if (!nodeCanHaveAnnotations) {
            return true;
        }
        // If the node is contained, but it comes immediately after the annotations,
        // let's not consider it contained (i.e. it must be "strictly contained").
        Node nodeWithoutAnnotations = firstNonAnnotationNode(container);
        Range rangeWithoutAnnotations = container.getRange().get().withBegin(nodeWithoutAnnotations.getBegin().get());
        return rangeWithoutAnnotations.// .contains(other.getRange().get());
        strictlyContains(other.getRange().get());
    }
}
