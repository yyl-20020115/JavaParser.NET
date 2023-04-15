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




public class TypeCastingGenerator:NodeGenerator {
    private /*final*/Set<BaseNodeMetaModel> baseNodes = set(
            JavaParserMetaModel.statementMetaModel,
            JavaParserMetaModel.expressionMetaModel,
            JavaParserMetaModel.typeMetaModel,
            JavaParserMetaModel.moduleDirectiveMetaModel,
            JavaParserMetaModel.bodyDeclarationMetaModel,
            JavaParserMetaModel.commentMetaModel
    );

    public TypeCastingGenerator(SourceRoot sourceRoot) {
        super(sourceRoot);
    }

    @Override
    protected void generateNode(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, ClassOrInterfaceDeclaration nodeCoid) {
        Pair<CompilationUnit, ClassOrInterfaceDeclaration> baseCode = null;
        for (BaseNodeMetaModel baseNode : baseNodes) {
            if(nodeMetaModel == baseNode) {
                // We adjust the base models from the child nodes,
                // so we don't do anything when we *are* the base model.
                return;
            }
            if (nodeMetaModel.isInstanceOfMetaModel(baseNode)) {
                baseCode = parseNode(baseNode);
            }
        }

        if (baseCode == null) {
            // Node is not a child of one of the base nodes, so we don't want to generate this method for it.
            return;
        }

        /*final*/string typeName = nodeMetaModel.getTypeName();
        /*final*/ClassOrInterfaceDeclaration baseCoid = baseCode.b;
        /*final*/CompilationUnit baseCu = baseCode.a;

        generateIsType(nodeMetaModel, baseCu, nodeCoid, baseCoid, typeName);
        generateAsType(nodeMetaModel, baseCu, nodeCoid, baseCoid, typeName);
        generateToType(nodeMetaModel, nodeCu, baseCu, nodeCoid, baseCoid, typeName);
        generateIfType(nodeMetaModel, nodeCu, baseCu, nodeCoid, baseCoid, typeName);
    }

    private void generateAsType(BaseNodeMetaModel nodeMetaModel, CompilationUnit baseCu, ClassOrInterfaceDeclaration nodeCoid, ClassOrInterfaceDeclaration baseCoid, string typeName) {
        baseCu.addImport("com.github.javaparser.utils.CodeGenerationUtils.f", true, false);

        /*final*/MethodDeclaration asTypeBaseMethod = (MethodDeclaration) parseBodyDeclaration(f("public %s as%s() { throw new IllegalStateException(f(\"%%s is not %s, it is %%s\", this, this.getClass().getSimpleName())); }", typeName, typeName, typeName));
        /*final*/MethodDeclaration asTypeNodeMethod = (MethodDeclaration) parseBodyDeclaration(f("@Override public %s as%s() { return this; }", typeName, typeName));

        annotateWhenOverridden(nodeMetaModel, asTypeNodeMethod);

        addOrReplaceWhenSameSignature(baseCoid, asTypeBaseMethod);
        addOrReplaceWhenSameSignature(nodeCoid, asTypeNodeMethod);
    }

    private void generateToType(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, CompilationUnit baseCu, ClassOrInterfaceDeclaration nodeCoid, ClassOrInterfaceDeclaration baseCoid, string typeName) {
        baseCu.addImport(Optional.class);
        nodeCu.addImport(Optional.class);

        /*final*/MethodDeclaration toTypeBaseMethod = (MethodDeclaration) parseBodyDeclaration(f("public Optional<%s> to%s() { return Optional.empty(); }", typeName, typeName, typeName));
        /*final*/MethodDeclaration toTypeNodeMethod = (MethodDeclaration) parseBodyDeclaration(f("@Override public Optional<%s> to%s() { return Optional.of(this); }", typeName, typeName));

        annotateWhenOverridden(nodeMetaModel, toTypeNodeMethod);

        addOrReplaceWhenSameSignature(baseCoid, toTypeBaseMethod);
        addOrReplaceWhenSameSignature(nodeCoid, toTypeNodeMethod);
    }

    private void generateIfType(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, CompilationUnit baseCu, ClassOrInterfaceDeclaration nodeCoid, ClassOrInterfaceDeclaration baseCoid, string typeName) {
        baseCu.addImport(Consumer.class);
        nodeCu.addImport(Consumer.class);

        /*final*/MethodDeclaration ifTypeBaseMethod = (MethodDeclaration) parseBodyDeclaration(f("public void if%s(Consumer<%s> action) { }", typeName, typeName));
        /*final*/MethodDeclaration ifTypeNodeMethod = (MethodDeclaration) parseBodyDeclaration(f("public void if%s(Consumer<%s> action) { action.accept(this); }", typeName, typeName));

        annotateWhenOverridden(nodeMetaModel, ifTypeNodeMethod);

        addOrReplaceWhenSameSignature(baseCoid, ifTypeBaseMethod);
        addOrReplaceWhenSameSignature(nodeCoid, ifTypeNodeMethod);
    }

    private void generateIsType(BaseNodeMetaModel nodeMetaModel, CompilationUnit baseCu, ClassOrInterfaceDeclaration nodeCoid, ClassOrInterfaceDeclaration baseCoid, string typeName) {
        /*final*/MethodDeclaration baseIsTypeMethod = (MethodDeclaration) parseBodyDeclaration(f("public boolean is%s() { return false; }", typeName));
        /*final*/MethodDeclaration overriddenIsTypeMethod = (MethodDeclaration) parseBodyDeclaration(f("@Override public boolean is%s() { return true; }", typeName));

        annotateWhenOverridden(nodeMetaModel, overriddenIsTypeMethod);

        addOrReplaceWhenSameSignature(nodeCoid, overriddenIsTypeMethod);
        addOrReplaceWhenSameSignature(baseCoid, baseIsTypeMethod);
    }
}
