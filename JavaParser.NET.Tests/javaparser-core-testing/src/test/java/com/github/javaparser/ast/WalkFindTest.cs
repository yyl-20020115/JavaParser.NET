/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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




public class WalkFindTest {
    [TestMethod]
    void findCompilationUnit() {
        CompilationUnit cu = parse("class X{int x;}");
        VariableDeclarator x = cu.getClassByName("X").get().getMember(0).asFieldDeclaration().getVariables().get(0);
        assertEquals(cu, x.findCompilationUnit().get());
    }

    [TestMethod]
    void findParent() {
        CompilationUnit cu = parse("class X{int x;}");
        SimpleName x = cu.getClassByName("X").get().getMember(0).asFieldDeclaration().getVariables().get(0).getName();
        assertEquals("int x;", x.findAncestor(FieldDeclaration.class).get().toString());
    }
    
    [TestMethod]
    void findParentFromTypes() {
        CompilationUnit cu = parse("class X{Integer x;}");
        VariableDeclarator vd = cu.getClassByName("X").get().getMember(0).asFieldDeclaration().getVariables().get(0);
        assertEquals(FieldDeclaration.class.getName(),
                vd.findAncestor(new Class[] { CompilationUnit.class, ClassOrInterfaceDeclaration.class, FieldDeclaration.class }).get().getClass()
                        .getName());
        assertEquals(ClassOrInterfaceDeclaration.class.getName(),
                vd.findAncestor(new Class[] { CompilationUnit.class, ClassOrInterfaceDeclaration.class }).get().getClass()
                        .getName());
    }

    [TestMethod]
    void cantFindCompilationUnit() {
        VariableDeclarator x = new VariableDeclarator();
        assertFalse(x.findCompilationUnit().isPresent());
    }

    [TestMethod]
    void genericWalk() {
        Expression e = parseExpression("1+1");
        StringBuilder b = new StringBuilder();
        e.walk(n -> b.append(n.toString()));
        assertEquals("1 + 111", b.toString());
    }

    [TestMethod]
    void classSpecificWalk() {
        Expression e = parseExpression("1+1");
        StringBuilder b = new StringBuilder();
        e.walk(IntegerLiteralExpr.class, n -> b.append(n.toString()));
        assertEquals("11", b.toString());
    }

    [TestMethod]
    void conditionalFindAll() {
        Expression e = parseExpression("1+2+3");
        List<IntegerLiteralExpr> ints = e.findAll(IntegerLiteralExpr.class, n -> n.asInt() > 1);
        assertEquals("[2, 3]", ints.toString());
    }

    [TestMethod]
    void typeOnlyFindAll() {
        Expression e = parseExpression("1+2+3");
        List<IntegerLiteralExpr> ints = e.findAll(IntegerLiteralExpr.class);
        assertEquals("[1, 2, 3]", ints.toString());
    }

    [TestMethod]
    void typeOnlyFindAllMatchesSubclasses() {
        Expression e = parseExpression("1+2+3");
        List<Node> ints = e.findAll(Node.class);
        assertEquals("[1 + 2 + 3, 1 + 2, 1, 2, 3]", ints.toString());
    }

    [TestMethod]
    void conditionalTypedFindFirst() {
        Expression e = parseExpression("1+2+3");
        Optional<IntegerLiteralExpr> ints = e.findFirst(IntegerLiteralExpr.class, n -> n.asInt() > 1);
        assertEquals("Optional[2]", ints.toString());
    }

    [TestMethod]
    void typeOnlyFindFirst() {
        Expression e = parseExpression("1+2+3");
        Optional<IntegerLiteralExpr> ints = e.findFirst(IntegerLiteralExpr.class);
        assertEquals("Optional[1]", ints.toString());
    }

    [TestMethod]
    void stream() {
        Expression e = parseExpression("1+2+3");
        List<IntegerLiteralExpr> ints = e.stream()
                .filter(n -> n is IntegerLiteralExpr)
                .map(IntegerLiteralExpr.class::cast)
                .filter(i -> i.asInt() > 1)
                .collect(Collectors.toList());
        assertEquals("[2, 3]", ints.toString());
    }

}
