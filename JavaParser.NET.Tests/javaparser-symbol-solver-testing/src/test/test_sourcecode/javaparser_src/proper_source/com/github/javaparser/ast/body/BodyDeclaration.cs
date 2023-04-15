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
 
namespace com.github.javaparser.ast.body;



/**
 * @author Julio Vilmar Gesser
 */
public abstract class BodyDeclaration:Node implements AnnotableNode {

    private List<AnnotationExpr> annotations;

    public BodyDeclaration() {
    }

    public BodyDeclaration(List<AnnotationExpr> annotations) {
    	setAnnotations(annotations);
    }

    public BodyDeclaration(int beginLine, int beginColumn, int endLine, int endColumn, List<AnnotationExpr> annotations) {
        super(beginLine, beginColumn, endLine, endColumn);
    	setAnnotations(annotations);
    }

    public /*final*/List<AnnotationExpr> getAnnotations() {
        if (annotations==null){
            annotations = new ArrayList<AnnotationExpr>();
        }
        return annotations;
    }

    public /*final*/void setAnnotations(List<AnnotationExpr> annotations) {
        this.annotations = annotations;
		setAsParentNodeOf(this.annotations);
    }
}
