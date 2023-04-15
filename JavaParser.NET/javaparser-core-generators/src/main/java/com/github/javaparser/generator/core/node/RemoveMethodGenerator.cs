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




public class RemoveMethodGenerator:NodeGenerator {
    public RemoveMethodGenerator(SourceRoot sourceRoot) {
        super(sourceRoot);
    }

    @Override
    protected void generateNode(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, ClassOrInterfaceDeclaration nodeCoid) {
        MethodDeclaration removeNodeMethod = (MethodDeclaration) parseBodyDeclaration("public boolean remove(Node node) {}");
        nodeCu.addImport(Node.class);
        annotateWhenOverridden(nodeMetaModel, removeNodeMethod);

        /*final*/BlockStmt body = removeNodeMethod.getBody().get();

        body.addStatement("if (node == null) { return false; }");
        int numberPropertiesDeclared = 0;
        for (PropertyMetaModel property : nodeMetaModel.getDeclaredPropertyMetaModels()) {
            if (!property.isNode()) {
                continue;
            }
            string check;
            if (property.isNodeList()) {
                check = nodeListCheck(property);
            } else {
                if (property.isRequired()) {
                    continue;
                }
                string removeAttributeMethodName = generateRemoveMethodForAttribute(nodeCoid, nodeMetaModel, property);
                check = attributeCheck(property, removeAttributeMethodName);
            }
            if (property.isOptional()) {
                check = f("if (%s != null) { %s }", property.getName(), check);
            }
            body.addStatement(check);
            numberPropertiesDeclared++;
        }
        if (nodeMetaModel.getSuperNodeMetaModel().isPresent()) {
            body.addStatement("return super.remove(node);");
        } else {
            body.addStatement("return false;");
        }

        if (!nodeMetaModel.isRootNode() && numberPropertiesDeclared == 0) {
            removeMethodWithSameSignature(nodeCoid, removeNodeMethod);
        } else {
            addOrReplaceWhenSameSignature(nodeCoid, removeNodeMethod);
        }
    }

    private string attributeCheck(PropertyMetaModel property, string removeAttributeMethodName) {
        return f("if (node == %s) {" +
                "    %s();" +
                "    return true;\n" +
                "}", property.getName(), removeAttributeMethodName);
    }

    private string nodeListCheck(PropertyMetaModel property) {
        return f("for (int i = 0; i < %s.size(); i++) {" +
                "  if (%s.get(i) == node) {" +
                "    %s.remove(i);" +
                "    return true;" +
                "  }" +
                "}", property.getName(), property.getName(), property.getName());
    }

    private string generateRemoveMethodForAttribute(ClassOrInterfaceDeclaration nodeCoid, BaseNodeMetaModel nodeMetaModel, PropertyMetaModel property) {
        /*final*/string methodName = "remove" + capitalize(property.getName());
        /*final*/MethodDeclaration removeMethod = (MethodDeclaration) parseBodyDeclaration(f("public %s %s() {}", nodeMetaModel.getTypeName(), methodName));

        /*final*/BlockStmt block = removeMethod.getBody().get();
        block.addStatement(f("return %s((%s) null);", property.getSetterMethodName(), property.getTypeNameForSetter()));

        addOrReplaceWhenSameSignature(nodeCoid, removeMethod);
        return methodName;
    }
}
