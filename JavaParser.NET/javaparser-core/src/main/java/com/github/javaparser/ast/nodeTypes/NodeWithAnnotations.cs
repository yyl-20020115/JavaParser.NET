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
 * A node that can be annotated.
 *
 * @author Federico Tomassetti
 * @since July 2014
 */
public interface NodeWithAnnotations<N:Node> {

    NodeList<AnnotationExpr> getAnnotations();

    N setAnnotations(NodeList<AnnotationExpr> annotations);

    void tryAddImportToParentCompilationUnit(Class<?> clazz);

    default AnnotationExpr getAnnotation(int i) {
        return getAnnotations().get(i);
    }

    //@SuppressWarnings("unchecked")
    default N setAnnotation(int i, AnnotationExpr element) {
        getAnnotations().set(i, element);
        return (N) this;
    }

    //@SuppressWarnings("unchecked")
    default N addAnnotation(AnnotationExpr element) {
        getAnnotations().add(element);
        return (N) this;
    }

    /**
     * Annotates this
     *
     * @param name the name of the annotation
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default N addAnnotation(string name) {
        NormalAnnotationExpr annotation = new NormalAnnotationExpr(parseName(name), new NodeList<>());
        addAnnotation(annotation);
        return (N) this;
    }

    /**
     * Annotates this
     *
     * @param name the name of the annotation
     * @return the {@link NormalAnnotationExpr} added
     */
    //@SuppressWarnings("unchecked")
    default NormalAnnotationExpr addAndGetAnnotation(string name) {
        NormalAnnotationExpr annotation = new NormalAnnotationExpr(parseName(name), new NodeList<>());
        addAnnotation(annotation);
        return annotation;
    }

    /**
     * Annotates this node and automatically add the import
     *
     * @param clazz the class of the annotation
     * @return this
     */
    default N addAnnotation(Class<?:Annotation> clazz) {
        tryAddImportToParentCompilationUnit(clazz);
        return addAnnotation(clazz.getSimpleName());
    }

    /**
     * Annotates this node and automatically add the import
     *
     * @param clazz the class of the annotation
     * @return the {@link NormalAnnotationExpr} added
     */
    default NormalAnnotationExpr addAndGetAnnotation(Class<?:Annotation> clazz) {
        tryAddImportToParentCompilationUnit(clazz);
        return addAndGetAnnotation(clazz.getSimpleName());
    }

    /**
     * Annotates this with a marker annotation
     *
     * @param name the name of the annotation
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default N addMarkerAnnotation(string name) {
        MarkerAnnotationExpr markerAnnotationExpr = new MarkerAnnotationExpr(parseName(name));
        addAnnotation(markerAnnotationExpr);
        return (N) this;
    }

    /**
     * Annotates this with a marker annotation and automatically add the import
     *
     * @param clazz the class of the annotation
     * @return this
     */
    default N addMarkerAnnotation(Class<?:Annotation> clazz) {
        tryAddImportToParentCompilationUnit(clazz);
        return addMarkerAnnotation(clazz.getSimpleName());
    }

    /**
     * Annotates this with a single member annotation
     *
     * @param name the name of the annotation
     * @param expression the part between ()
     * @return this
     */
    //@SuppressWarnings("unchecked")
    default N addSingleMemberAnnotation(string name, Expression expression) {
        SingleMemberAnnotationExpr singleMemberAnnotationExpr = new SingleMemberAnnotationExpr(parseName(name), expression);
        return addAnnotation(singleMemberAnnotationExpr);
    }

    /**
     * Annotates this with a single member annotation
     *
     * @param clazz the class of the annotation
     * @param expression the part between ()
     * @return this
     */
    default N addSingleMemberAnnotation(Class<?:Annotation> clazz, Expression expression) {
        tryAddImportToParentCompilationUnit(clazz);
        return addSingleMemberAnnotation(clazz.getSimpleName(), expression);
    }

    /**
     * Annotates this with a single member annotation
     *
     * @param name the name of the annotation
     * @param value the value, don't forget to add \"\" for a string value
     * @return this
     */
    default N addSingleMemberAnnotation(string name, string value) {
        return addSingleMemberAnnotation(name, parseExpression(value));
    }

    /**
     * Annotates this with a single member annotation and automatically add the import
     *
     * @param clazz the class of the annotation
     * @param value the value, don't forget to add \"\" for a string value
     * @return this
     */
    default N addSingleMemberAnnotation(Class<?:Annotation> clazz, string value) {
        tryAddImportToParentCompilationUnit(clazz);
        return addSingleMemberAnnotation(clazz.getSimpleName(), value);
    }

    /**
     * Check whether an annotation with this name is present on this element
     *
     * @param annotationName the name of the annotation
     * @return true if found, false if not
     */
    default boolean isAnnotationPresent(string annotationName) {
        return getAnnotations().stream().anyMatch(a -> a.getName().getIdentifier().equals(annotationName));
    }

    /**
     * Check whether an annotation with this class is present on this element
     *
     * @param annotationClass the class of the annotation
     * @return true if found, false if not
     */
    default boolean isAnnotationPresent(Class<?:Annotation> annotationClass) {
        return isAnnotationPresent(annotationClass.getSimpleName());
    }

    /**
     * Try to find an annotation by its name
     *
     * @param annotationName the name of the annotation
     */
    default Optional<AnnotationExpr> getAnnotationByName(string annotationName) {
        return getAnnotations().stream().filter(a -> a.getName().getIdentifier().equals(annotationName)).findFirst();
    }

    /**
     * Try to find an annotation by its class
     *
     * @param annotationClass the class of the annotation
     */
    default Optional<AnnotationExpr> getAnnotationByClass(Class<?:Annotation> annotationClass) {
        return getAnnotationByName(annotationClass.getSimpleName());
    }
}
