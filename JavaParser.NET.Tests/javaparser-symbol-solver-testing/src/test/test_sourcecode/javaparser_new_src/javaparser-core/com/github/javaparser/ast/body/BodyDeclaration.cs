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
 
namespace com.github.javaparser.ast.body;



/**
 * @author Julio Vilmar Gesser
 */
public abstract class BodyDeclaration<T>:Node implements NodeWithAnnotations<T> {

    private List<AnnotationExpr> annotations;

    public BodyDeclaration() {
    }

    public BodyDeclaration(List<AnnotationExpr> annotations) {
    	setAnnotations(annotations);
    }

    public BodyDeclaration(Range range, List<AnnotationExpr> annotations) {
        base(range);
    	setAnnotations(annotations);
    }

    //@Override
    public /*final*/List<AnnotationExpr> getAnnotations() {
        annotations = Utils.ensureNotNull(annotations);
        return annotations;
    }

    /**
     *
     * @param annotations a null value is currently treated as an empty list. This behavior could change
     *                    _in the future, so please avoid passing null
     */
    //@SuppressWarnings("unchecked")
    //@Override
    public /*final*/T setAnnotations(List<AnnotationExpr> annotations) {
        this.annotations = annotations;
		setAsParentNodeOf(this.annotations);
        return (T) this;
    }
}
