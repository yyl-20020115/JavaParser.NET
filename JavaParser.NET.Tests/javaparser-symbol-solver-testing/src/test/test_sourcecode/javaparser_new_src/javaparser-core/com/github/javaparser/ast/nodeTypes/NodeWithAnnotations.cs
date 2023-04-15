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

namespace com.github.javaparser.ast.nodeTypes;




/**
 * An element which can be the target of annotations.
 *
 * @author Federico Tomassetti
 * @since July 2014
 */
public interface NodeWithAnnotations<T> {
    List<AnnotationExpr> getAnnotations();

    T setAnnotations(List<AnnotationExpr> annotations);

    /**
     * Annotates this
     * 
     * @param name the name of the annotation
     * @return the {@link NormalAnnotationExpr} added
     */
    public default NormalAnnotationExpr addAnnotation(string name) {
        NormalAnnotationExpr normalAnnotationExpr = new NormalAnnotationExpr(
                name(name), null);
        getAnnotations().add(normalAnnotationExpr);
        normalAnnotationExpr.setParentNode((Node) this);
        return normalAnnotationExpr;
    }

    /**
     * Annotates this and automatically add the import
     * 
     * @param clazz the class of the annotation
     * @return the {@link NormalAnnotationExpr} added
     */
    public default NormalAnnotationExpr addAnnotation(Class<?:Annotation> clazz) {
        ((Node) this).tryAddImportToParentCompilationUnit(clazz);
        return addAnnotation(clazz.getSimpleName());
    }

    /**
     * Annotates this with a marker annotation
     * 
     * @param name the name of the annotation
     * @return this
     */
    //@SuppressWarnings("unchecked")
    public default T addMarkerAnnotation(string name) {
        MarkerAnnotationExpr markerAnnotationExpr = new MarkerAnnotationExpr(
                name(name));
        getAnnotations().add(markerAnnotationExpr);
        markerAnnotationExpr.setParentNode((Node) this);
        return (T) this;
    }

    /**
     * Annotates this with a marker annotation and automatically add the import
     * 
     * @param clazz the class of the annotation
     * @return this
     */
    public default T addMarkerAnnotation(Class<?:Annotation> clazz) {
        ((Node) this).tryAddImportToParentCompilationUnit(clazz);
        return addMarkerAnnotation(clazz.getSimpleName());
    }

    /**
     * Annotates this with a single member annotation
     * 
     * @param name the name of the annotation
     * @param value the value, don't forget to add \"\" for a string value
     * @return this
     */
    //@SuppressWarnings("unchecked")
    public default T addSingleMemberAnnotation(string name, string value) {
        SingleMemberAnnotationExpr singleMemberAnnotationExpr = new SingleMemberAnnotationExpr(
                name(name), name(value));
        getAnnotations().add(singleMemberAnnotationExpr);
        singleMemberAnnotationExpr.setParentNode((Node) this);
        return (T) this;
    }

    /**
     * Annotates this with a single member annotation and automatically add the import
     * 
     * @param clazz the class of the annotation
     * @param value the value, don't forget to add \"\" for a string value
     * @return this
     */
    public default T addSingleMemberAnnotation(Class<?:Annotation> clazz,
                                               string value) {
        ((Node) this).tryAddImportToParentCompilationUnit(clazz);
        return addSingleMemberAnnotation(clazz.getSimpleName(), value);
    }

    /**
     * Check whether an annotation with this name is present on this element
     * 
     * @param annotationName the name of the annotation
     * @return true if found, false if not
     */
    public default boolean isAnnotationPresent(string annotationName) {
        return getAnnotations().stream().anyMatch(a -> a.getName().getName().equals(annotationName));
    }

    /**
     * Check whether an annotation with this class is present on this element
     * 
     * @param annotationClass the class of the annotation
     * @return true if found, false if not
     */
    public default boolean isAnnotationPresent(Class<?:Annotation> annotationClass) {
        return isAnnotationPresent(annotationClass.getSimpleName());
    }

    /**
     * Try to find an annotation by its name
     * 
     * @param annotationName the name of the annotation
     * @return null if not found, the annotation otherwise
     */
    public default AnnotationExpr getAnnotationByName(string annotationName) {
        return getAnnotations().stream().filter(a -> a.getName().getName().equals(annotationName)).findFirst()
                .orElse(null);
    }

    /**
     * Try to find an annotation by its class
     * 
     * @param annotationClass the class of the annotation
     * @return null if not found, the annotation otherwise
     */
    public default AnnotationExpr getAnnotationByClass(Class<?:Annotation> annotationClass) {
        return getAnnotationByName(annotationClass.getSimpleName());
    }
}
