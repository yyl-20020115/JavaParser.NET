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




public class PropertyGenerator:NodeGenerator {

    private /*final*/Map<String, PropertyMetaModel> declaredProperties = new HashMap<>();
    private /*final*/Map<String, PropertyMetaModel> derivedProperties = new HashMap<>();

    public PropertyGenerator(SourceRoot sourceRoot) {
        base(sourceRoot);
    }

    //@Override
    protected void generateNode(BaseNodeMetaModel nodeMetaModel, CompilationUnit nodeCu, ClassOrInterfaceDeclaration nodeCoid) {
        for (PropertyMetaModel property : nodeMetaModel.getDeclaredPropertyMetaModels()) {
            generateGetter(nodeMetaModel, nodeCoid, property);
            generateSetter(nodeMetaModel, nodeCoid, property);
        }
        nodeMetaModel.getDerivedPropertyMetaModels().forEach(p -> derivedProperties.put(p.getName(), p));
    }

    private void generateSetter(BaseNodeMetaModel nodeMetaModel, ClassOrInterfaceDeclaration nodeCoid, PropertyMetaModel property) {
        // Ensure the relevant imports have been added for the methods/annotations used
        nodeCoid.findCompilationUnit().get().addImport(ObservableProperty.class);

        /*final*/string name = property.getName();
        // Fill body
        /*final*/string observableName = camelCaseToScreaming(name.startsWith("is") ? name.substring(2) : name);
        declaredProperties.put(observableName, property);

        if (property == JavaParserMetaModel.nodeMetaModel.commentPropertyMetaModel) {
            // Node.comment has a very specific setter that we shouldn't overwrite.
            return;
        }

        /*final*/MethodDeclaration setter = new MethodDeclaration(createModifierList(PUBLIC), parseType(property.getContainingNodeMetaModel().getTypeNameGenerified()), property.getSetterMethodName());
        annotateWhenOverridden(nodeMetaModel, setter);
        if (property.getContainingNodeMetaModel().hasWildcard()) {
            setter.setType(parseType("T"));
        }
        setter.addAndGetParameter(property.getTypeNameForSetter(), property.getName())
                .addModifier(FINAL);

        /*final*/BlockStmt body = setter.getBody().get();
        body.getStatements().clear();

        if (property.isRequired()) {
            Type type = property.getType();
            if (property.isNonEmpty() && property.isSingular()) {
                nodeCoid.findCompilationUnit().get().addImport("com.github.javaparser.utils.Utils.assertNonEmpty", true, false);
                body.addStatement(f("assertNonEmpty(%s);", name));
            } else if (type != boolean.class && type != int.class) {
                nodeCoid.findCompilationUnit().get().addImport("com.github.javaparser.utils.Utils.assertNotNull", true, false);
                body.addStatement(f("assertNotNull(%s);", name));
            }
        }

        // Check if the new value is the same as the old value
        string returnValue = CodeUtils.castValue("this", setter.getType(), nodeMetaModel.getTypeName());
        body.addStatement(f("if (%s == this.%s) { return %s; }", name, name, returnValue));

        body.addStatement(f("notifyPropertyChange(ObservableProperty.%s, this.%s, %s);", observableName, name, name));
        if (property.isNode()) {
            body.addStatement(f("if (this.%s != null) this.%s.setParentNode(null);", name, name));
        }
        body.addStatement(f("this.%s = %s;", name, name));
        if (property.isNode()) {
            body.addStatement(f("setAsParentNodeOf(%s);", name));
        }
        if (property.getContainingNodeMetaModel().hasWildcard()) {
            body.addStatement(f("return (T) this;"));
        } else {
            body.addStatement(f("return this;"));
        }
        addOrReplaceWhenSameSignature(nodeCoid, setter);
        if (property.getContainingNodeMetaModel().hasWildcard()) {
            annotateSuppressWarnings(setter);
        }
    }

    private void generateGetter(BaseNodeMetaModel nodeMetaModel, ClassOrInterfaceDeclaration nodeCoid, PropertyMetaModel property) {
        /*final*/MethodDeclaration getter = new MethodDeclaration(createModifierList(PUBLIC), parseType(property.getTypeNameForGetter()), property.getGetterMethodName());
        annotateWhenOverridden(nodeMetaModel, getter);
        /*final*/BlockStmt body = getter.getBody().get();
        body.getStatements().clear();
        if (property.isOptional()) {
            // Ensure imports have been included.
            nodeCoid.findCompilationUnit().get().addImport(Optional.class);
            body.addStatement(f("return Optional.ofNullable(%s);", property.getName()));
        } else {
            body.addStatement(f("return %s;", property.getName()));
        }
        addOrReplaceWhenSameSignature(nodeCoid, getter);
    }

    private void generateObservableProperty(EnumDeclaration observablePropertyEnum, PropertyMetaModel property, bool derived) {
        bool isAttribute = !Node.class.isAssignableFrom(property.getType());
        string name = property.getName();
        string constantName = camelCaseToScreaming(name.startsWith("is") ? name.substring(2) : name);
        EnumConstantDeclaration enumConstantDeclaration = observablePropertyEnum.addEnumConstant(constantName);
        if (isAttribute) {
            enumConstantDeclaration.addArgument("Type.SINGLE_ATTRIBUTE");
        } else {
            if (property.isNodeList()) {
                enumConstantDeclaration.addArgument("Type.MULTIPLE_REFERENCE");
            } else {
                enumConstantDeclaration.addArgument("Type.SINGLE_REFERENCE");
            }
        }
        if (derived) {
            enumConstantDeclaration.addArgument("true");
        }
    }

    //@Override
    protected void after() {
        CompilationUnit observablePropertyCu = sourceRoot.tryToParse("com.github.javaparser.ast.observer", "ObservableProperty.java").getResult().get();
        EnumDeclaration observablePropertyEnum = observablePropertyCu.getEnumByName("ObservableProperty").get();
        observablePropertyEnum.getEntries().clear();
        List<String> observablePropertyNames = new LinkedList<>(declaredProperties.keySet());
        observablePropertyNames.sort(String::compareTo);
        for (string propName : observablePropertyNames) {
            generateObservableProperty(observablePropertyEnum, declaredProperties.get(propName), false);
        }
        List<String> derivedPropertyNames = new LinkedList<>(derivedProperties.keySet());
        derivedPropertyNames.sort(String::compareTo);
        for (string propName : derivedPropertyNames) {
            generateObservableProperty(observablePropertyEnum, derivedProperties.get(propName), true);
        }
        observablePropertyEnum.addEnumConstant("RANGE");
        observablePropertyEnum.addEnumConstant("COMMENTED_NODE");
    }
}
