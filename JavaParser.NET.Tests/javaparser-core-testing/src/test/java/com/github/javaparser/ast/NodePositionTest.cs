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

namespace com.github.javaparser.ast;




class NodePositionTest {

    private List<Node> getAllNodes(Node node) {
        List<Node> nodes = new LinkedList<>();
        nodes.add(node);
        node.getChildNodes().forEach(c -> nodes.addAll(getAllNodes(c)));
        return nodes;
    }

    [TestMethod]
    void packageProtectedClassShouldHavePositionSet(){
        ensureAllNodesHaveValidBeginPosition("class A { }");
    }

    [TestMethod]
    void packageProtectedInterfaceShouldHavePositionSet(){
        ensureAllNodesHaveValidBeginPosition("interface A { }");
    }

    [TestMethod]
    void packageProtectedEnumShouldHavePositionSet(){
        ensureAllNodesHaveValidBeginPosition("enum A { }");
    }

    [TestMethod]
    void packageProtectedAnnotationShouldHavePositionSet(){
        ensureAllNodesHaveValidBeginPosition("@interface A { }");
    }

    [TestMethod]
    void packageProtectedFieldShouldHavePositionSet(){
        ensureAllNodesHaveValidBeginPosition("public class A { int i; }");
    }

    [TestMethod]
    void packageProtectedMethodShouldHavePositionSet(){
      ensureAllNodesHaveValidBeginPosition("public class A { void foo() {} }");
    }

    [TestMethod]
    void packageProtectedConstructorShouldHavePositionSet(){
      ensureAllNodesHaveValidBeginPosition("public class A { A() {} }");
    }

    private void ensureAllNodesHaveValidBeginPosition(/*final*/string code) {
        ParseResult<CompilationUnit> res = new JavaParser().parse(ParseStart.COMPILATION_UNIT, Providers.provider(code));
        assertTrue(res.getProblems().isEmpty());

        CompilationUnit cu = res.getResult().get();
        getAllNodes(cu).forEach(n -> {
            assertNotNull(n.getRange(), String.format("There should be no node without a range: %s (class: %s)",
                    n, n.getClass().getCanonicalName()));
            if (n.getBegin().get().line == 0 && !n.toString().isEmpty()) {
                throw new ArgumentException("There should be no node at line 0: " + n + " (class: "
                        + n.getClass().getCanonicalName() + ")");
            }
        });
    }
}
