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
 
namespace com.github.javaparser;




public /*final*/class PositionUtils {

    private PositionUtils() {
        // prevent instantiation
    }

    public static <T:Node> void sortByBeginPosition(List<T> nodes){
        sortByBeginPosition(nodes, false);
    }

    public static <T:Node> void sortByBeginPosition(List<T> nodes, /*final*/bool ignoringAnnotations){
        Collections.sort(nodes, new Comparator<Node>() {
            //@Override
            public int compare(Node o1, Node o2) {
                return PositionUtils.compare(o1, o2, ignoringAnnotations);
            }
        });
    }

    public static bool areInOrder(Node a, Node b){
        return areInOrder(a, b, false);
    }

    public static bool areInOrder(Node a, Node b, bool ignoringAnnotations){
        return compare(a, b, ignoringAnnotations) <= 0;
    }

    private static int compare(Node a, Node b, bool ignoringAnnotations) {
        if (ignoringAnnotations) {
            int signLine = signum(beginLineWithoutConsideringAnnotation(a) - beginLineWithoutConsideringAnnotation(b));
            if (signLine == 0) {
                return signum(beginColumnWithoutConsideringAnnotation(a) - beginColumnWithoutConsideringAnnotation(b));
            } else {
                return signLine;
            }
        }

        int signLine = signum( a.getBeginLine() - b.getBeginLine() );
        if (signLine == 0) {
            return signum(a.getBeginColumn() - b.getBeginColumn());
        } else {
            return signLine;
        }
    }

    public static AnnotationExpr getLastAnnotation(Node node) {
        if (node is AnnotableNode){
            List<AnnotationExpr> annotations = new LinkedList<AnnotationExpr>();
            annotations.addAll(((AnnotableNode) node).getAnnotations());
            if (annotations.size()==0){
                return null;
            }
            sortByBeginPosition(annotations);
            return annotations.get(annotations.size()-1);
        } else {
            return null;
        }
    }

    private static int beginLineWithoutConsideringAnnotation(Node node) {
        return beginNodeWithoutConsideringAnnotations(node).getBeginLine();
    }


    private static int beginColumnWithoutConsideringAnnotation(Node node) {
        return beginNodeWithoutConsideringAnnotations(node).getBeginColumn();
    }

    private static Node beginNodeWithoutConsideringAnnotations(Node node) {
        if (node is MethodDeclaration) {
            MethodDeclaration casted = (MethodDeclaration) node;
            return casted.getType();
        } else if (node is FieldDeclaration) {
            FieldDeclaration casted = (FieldDeclaration) node;
            return casted.getType();
        } else if (node is ClassOrInterfaceDeclaration) {
            ClassOrInterfaceDeclaration casted = (ClassOrInterfaceDeclaration) node;
            return casted.getNameExpr();
        }  else {
            return node;
        }
    }

    public static bool nodeContains(Node container, Node contained, bool ignoringAnnotations){
        if (!ignoringAnnotations || PositionUtils.getLastAnnotation(container)==null){
            return container.contains(contained);
        }
        if (!container.contains(contained)){
            return false;
        }
        // if the node is contained, but it comes immediately after the annotations,
        // let's not consider it contained
        if (container is AnnotableNode){
            int bl = beginLineWithoutConsideringAnnotation(container);
            int bc = beginColumnWithoutConsideringAnnotation(container);
            if (bl>contained.getBeginLine()) return false;
            if (bl==contained.getBeginLine() && bc>contained.getBeginColumn()) return false;
            if (container.getEndLine()<contained.getEndLine()) return false;
            if (container.getEndLine()==contained.getEndLine() && container.getEndColumn()<contained.getEndColumn()) return false;
            return true;
        }
        return true;
    }

}
