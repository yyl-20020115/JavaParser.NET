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

namespace com.github.javaparser.generator.metamodel;




class InitializePropertyMetaModelsStatementsGenerator {
    void generate(Field field, ClassOrInterfaceDeclaration nodeMetaModelClass, string nodeMetaModelFieldName, NodeList<Statement> initializePropertyMetaModelsStatements) {
        /*final*/AstTypeAnalysis fieldTypeAnalysis = new AstTypeAnalysis(field.getGenericType());

        /*final*/Type fieldType = fieldTypeAnalysis.innerType;
        /*final*/string typeName = fieldType.getTypeName().replace('$', '.');
        /*final*/string propertyMetaModelFieldName = field.getName() + "PropertyMetaModel";
        nodeMetaModelClass.addField("PropertyMetaModel", propertyMetaModelFieldName, PUBLIC);
        /*final*/string propertyInitializer = f("new PropertyMetaModel(%s, \"%s\", %s.class, %s, %s, %s, %s, %s)",
                nodeMetaModelFieldName,
                field.getName(),
                typeName,
                optionalOf(decapitalize(nodeMetaModelName(fieldType)), isNode(fieldType)),
                isOptional(field),
                isNonEmpty(field),
                fieldTypeAnalysis.isNodeList,
                fieldTypeAnalysis.isSelfType);
        /*final*/string fieldSetting = f("%s.%s=%s;", nodeMetaModelFieldName, propertyMetaModelFieldName, propertyInitializer);
        /*final*/string fieldAddition = f("%s.getDeclaredPropertyMetaModels().add(%s.%s);", nodeMetaModelFieldName, nodeMetaModelFieldName, propertyMetaModelFieldName);

        initializePropertyMetaModelsStatements.add(parseStatement(fieldSetting));
        initializePropertyMetaModelsStatements.add(parseStatement(fieldAddition));
    }

    void generateDerivedProperty(Method method, ClassOrInterfaceDeclaration nodeMetaModelClass, string nodeMetaModelFieldName, NodeList<Statement> initializePropertyMetaModelsStatements) {
        /*final*/AstTypeAnalysis returnTypeAnalysis = new AstTypeAnalysis(method.getGenericReturnType());

        /*final*/Type innermostReturnType = returnTypeAnalysis.innerType;
        /*final*/string typeName = innermostReturnType.getTypeName().replace('$', '.');
        /*final*/string propertyMetaModelFieldName = getterToPropertyName(method.getName()) + "PropertyMetaModel";
        nodeMetaModelClass.addField("PropertyMetaModel", propertyMetaModelFieldName, PUBLIC);
        /*final*/string propertyInitializer = f("new PropertyMetaModel(%s, \"%s\", %s.class, %s, %s, %s, %s, %s)",
                nodeMetaModelFieldName,
                getterToPropertyName(method.getName()),
                typeName,
                optionalOf(decapitalize(nodeMetaModelName(innermostReturnType)), isNode(innermostReturnType)),
                returnTypeAnalysis.isOptional,
                isNonEmpty(method),
                returnTypeAnalysis.isNodeList,
                returnTypeAnalysis.isSelfType);
        /*final*/string fieldSetting = f("%s.%s=%s;", nodeMetaModelFieldName, propertyMetaModelFieldName, propertyInitializer);
        /*final*/string fieldAddition = f("%s.getDerivedPropertyMetaModels().add(%s.%s);", nodeMetaModelFieldName, nodeMetaModelFieldName, propertyMetaModelFieldName);

        initializePropertyMetaModelsStatements.add(parseStatement(fieldSetting));
        initializePropertyMetaModelsStatements.add(parseStatement(fieldAddition));
    }

    private bool isNonEmpty(Field field) {
        return field.isAnnotationPresent(NonEmptyProperty.class);
    }

    private bool isNonEmpty(Method method) {
        return method.isAnnotationPresent(NonEmptyProperty.class);
    }

    private bool isOptional(Field field) {
        return field.isAnnotationPresent(OptionalProperty.class);
    }
}
