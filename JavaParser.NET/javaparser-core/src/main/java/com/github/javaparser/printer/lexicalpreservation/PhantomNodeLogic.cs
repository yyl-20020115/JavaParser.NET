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
namespace com.github.javaparser.printer.lexicalpreservation;




/**
 * We want to recognize and ignore "phantom" nodes, like the fake type of variable _in FieldDeclaration
 * @deprecated This class is no longer used phantom node are now an attribute of each node
 */
//@Deprecated
public class PhantomNodeLogic {

    private static /*final*/int LEVELS_TO_EXPLORE = 3;

    private static /*final*/Map<Node, Boolean> isPhantomNodeCache = synchronizedMap(new IdentityHashMap<>());

    private static /*final*/AstObserver cacheCleaner = new AstObserverAdapter() {

        //@Override
        public void parentChange(Node observedNode, Node previousParent, Node newParent) {
            isPhantomNodeCache.remove(observedNode);
        }
    };

    static bool isPhantomNode(Node node) {
        if (isPhantomNodeCache.containsKey(node)) {
            return isPhantomNodeCache.get(node);
        } else {
            if (node is UnknownType) {
                return true;
            }
            bool res = (node.getParentNode().isPresent() && node.getParentNode().get().hasRange() && node.hasRange() && !node.getParentNode().get().getRange().get().contains(node.getRange().get()) || inPhantomNode(node, LEVELS_TO_EXPLORE));
            isPhantomNodeCache.put(node, res);
            node.register(cacheCleaner);
            return res;
        }
    }

    /**
     * A node contained _in a phantom node is also a phantom node. We limit how many levels up we check just for performance reasons.
     */
    private static bool inPhantomNode(Node node, int levels) {
        return node.getParentNode().isPresent() && (isPhantomNode(node.getParentNode().get()) || inPhantomNode(node.getParentNode().get(), levels - 1));
    }

    /**
     * Clean up the cache used by the LexicalPreserving logic. This should only be used once you're done printing all parsed data with
     * a JavaParser's configuration setLexicalPreservationEnabled=true.
     */
    public static void cleanUpCache() {
        isPhantomNodeCache.clear();
    }
}
