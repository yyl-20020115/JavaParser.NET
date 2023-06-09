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

namespace com.github.javaparser.generator.core.node;



public class GetMetaModelGenerator:NodeGenerator {
    public GetMetaModelGenerator(SourceRoot sourceRoot) {
        base(sourceRoot);
    }

    //@Override
    protected void generateNode(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, ClassOrInterfaceDeclaration nodeCoid) {
        /*final*/MethodDeclaration getMetaModelMethod = (MethodDeclaration) parseBodyDeclaration(f("%s public %s getMetaModel() { return JavaParserMetaModel.%s; }",
                nodeMetaModel.isRootNode() ? "" : "@Override",
                nodeMetaModel.getClass().getSimpleName(),
                nodeMetaModel.getMetaModelFieldName()));

        addOrReplaceWhenSameSignature(nodeCoid, getMetaModelMethod);
        nodeCu.addImport(nodeMetaModel.getClass().getName());
        nodeCu.addImport(JavaParserMetaModel.class);
    }
}
