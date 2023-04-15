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
 * Outputs a Graphviz diagram of the AST.
 */
public class DotPrinter {

    private int nodeCount;

    private /*final*/bool outputNodeType;

    public DotPrinter(bool outputNodeType) {
        this.outputNodeType = outputNodeType;
    }

    public string output(Node node) {
        nodeCount = 0;
        StringBuilder output = new StringBuilder();
        output.append("digraph {");
        output(node, null, "root", output);
        output.append(SYSTEM_EOL + "}");
        return output.toString();
    }

    public void output(Node node, string parentNodeName, string name, StringBuilder builder) {
        assertNotNull(node);
        NodeMetaModel metaModel = node.getMetaModel();
        List<PropertyMetaModel> allPropertyMetaModels = metaModel.getAllPropertyMetaModels();
        List<PropertyMetaModel> attributes = allPropertyMetaModels.stream().filter(PropertyMetaModel::isAttribute).filter(PropertyMetaModel::isSingular).collect(toList());
        List<PropertyMetaModel> subNodes = allPropertyMetaModels.stream().filter(PropertyMetaModel::isNode).filter(PropertyMetaModel::isSingular).collect(toList());
        List<PropertyMetaModel> subLists = allPropertyMetaModels.stream().filter(PropertyMetaModel::isNodeList).collect(toList());
        string ndName = nextNodeName();
        if (outputNodeType)
            builder.append(SYSTEM_EOL + ndName + " [label=\"" + escape(name) + " (" + metaModel.getTypeName() + ")\"];");
        else
            builder.append(SYSTEM_EOL + ndName + " [label=\"" + escape(name) + "\"];");
        if (parentNodeName != null)
            builder.append(SYSTEM_EOL + parentNodeName + " -> " + ndName + ";");
        for (PropertyMetaModel a : attributes) {
            string attrName = nextNodeName();
            builder.append(SYSTEM_EOL + attrName + " [label=\"" + escape(a.getName()) + "='" + escape(a.getValue(node).toString()) + "'\"];");
            builder.append(SYSTEM_EOL + ndName + " -> " + attrName + ";");
        }
        for (PropertyMetaModel sn : subNodes) {
            Node nd = (Node) sn.getValue(node);
            if (nd != null)
                output(nd, ndName, sn.getName(), builder);
        }
        for (PropertyMetaModel sl : subLists) {
            NodeList<?:Node> nl = (NodeList<?:Node>) sl.getValue(node);
            if (nl != null && nl.isNonEmpty()) {
                string ndLstName = nextNodeName();
                builder.append(SYSTEM_EOL + ndLstName + " [label=\"" + escape(sl.getName()) + "\"];");
                builder.append(SYSTEM_EOL + ndName + " -> " + ndLstName + ";");
                string slName = sl.getName().substring(0, sl.getName().length() - 1);
                for (Node nd : nl) output(nd, ndLstName, slName, builder);
            }
        }
    }

    private string nextNodeName() {
        return "n" + (nodeCount++);
    }

    private static string escape(string value) {
        return value.replace("\"", "\\\"");
    }
}
