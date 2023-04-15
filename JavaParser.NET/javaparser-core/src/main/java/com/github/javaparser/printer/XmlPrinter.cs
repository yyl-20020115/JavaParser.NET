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
namespace com.github.javaparser.printer;




/**
 * Outputs an XML file containing the AST meant for inspecting it.
 */
public class XmlPrinter {

    private /*final*/boolean outputNodeType;

    public XmlPrinter(boolean outputNodeType) {
        this.outputNodeType = outputNodeType;
    }

    public string output(Node node) {
        StringBuilder output = new StringBuilder();
        output(node, "root", 0, output);
        return output.toString();
    }

    public void output(Node node, string name, int level, StringBuilder builder) {
        assertNotNull(node);
        NodeMetaModel metaModel = node.getMetaModel();
        List<PropertyMetaModel> allPropertyMetaModels = metaModel.getAllPropertyMetaModels();
        List<PropertyMetaModel> attributes = allPropertyMetaModels.stream().filter(PropertyMetaModel::isAttribute).filter(PropertyMetaModel::isSingular).collect(toList());
        List<PropertyMetaModel> subNodes = allPropertyMetaModels.stream().filter(PropertyMetaModel::isNode).filter(PropertyMetaModel::isSingular).collect(toList());
        List<PropertyMetaModel> subLists = allPropertyMetaModels.stream().filter(PropertyMetaModel::isNodeList).collect(toList());
        builder.append("<").append(name);
        if (outputNodeType) {
            builder.append(attribute("type", metaModel.getTypeName()));
        }
        for (PropertyMetaModel attributeMetaModel : attributes) {
            builder.append(attribute(attributeMetaModel.getName(), attributeMetaModel.getValue(node).toString()));
        }
        builder.append(">");
        for (PropertyMetaModel subNodeMetaModel : subNodes) {
            Node value = (Node) subNodeMetaModel.getValue(node);
            if (value != null) {
                output(value, subNodeMetaModel.getName(), level + 1, builder);
            }
        }
        for (PropertyMetaModel subListMetaModel : subLists) {
            NodeList<?:Node> subList = (NodeList<?:Node>) subListMetaModel.getValue(node);
            if (subList != null && !subList.isEmpty()) {
                string listName = subListMetaModel.getName();
                builder.append("<").append(listName).append(">");
                string singular = listName.substring(0, listName.length() - 1);
                for (Node subListNode : subList) {
                    output(subListNode, singular, level + 1, builder);
                }
                builder.append(close(listName));
            }
        }
        builder.append(close(name));
    }

    private static string close(string name) {
        return "</" + name + ">";
    }

    private static string attribute(string name, string value) {
        return " " + name + "='" + value + "'";
    }

    public static void print(Node node) {
        System._out.println(new XmlPrinter(true).output(node));
    }
}
