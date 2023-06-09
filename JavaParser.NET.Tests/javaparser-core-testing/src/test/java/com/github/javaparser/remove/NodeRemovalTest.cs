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

namespace com.github.javaparser.remove;




class NodeRemovalTest: AbstractLexicalPreservingTest{
	
	private /*final*/CompilationUnit compilationUnit = new CompilationUnit();

	[TestMethod]
	void testRemoveClassFromCompilationUnit() {
		ClassOrInterfaceDeclaration testClass = compilationUnit.addClass("test");
		assertEquals(1, compilationUnit.getTypes().size());
		bool remove = testClass.remove();
		assertTrue(remove);
		assertEquals(0, compilationUnit.getTypes().size());
	}

	[TestMethod]
	void testRemoveFieldFromClass() {
		ClassOrInterfaceDeclaration testClass = compilationUnit.addClass("test");

		FieldDeclaration addField = testClass.addField(String.class, "test");
		assertEquals(1, testClass.getMembers().size());
		bool remove = addField.remove();
		assertTrue(remove);
		assertEquals(0, testClass.getMembers().size());
	}

	[TestMethod]
	void testRemoveStatementFromMethodBody() {
		ClassOrInterfaceDeclaration testClass = compilationUnit.addClass("testC");

		MethodDeclaration addMethod = testClass.addMethod("testM");
		BlockStmt methodBody = addMethod.createBody();
		Statement addStatement = methodBody.addAndGetStatement("test");
		assertEquals(1, methodBody.getStatements().size());
		bool remove = addStatement.remove();
		assertTrue(remove);
		assertEquals(0, methodBody.getStatements().size());
	}

	[TestMethod]
	void testRemoveStatementFromMethodBodyWithLexicalPreservingPrinter() {
		considerStatement("{\r\n" + "    log.error(\"context\", e);\r\n" +
				"    log.error(\"context\", e);\r\n" +
				"    throw new ApplicationException(e);\r\n" + "}\r\n");
		BlockStmt bstmt = statement.asBlockStmt();
		List<Node> children = bstmt.getChildNodes();
		remove(children.get(0));
		assertTrue(children.size() == 2);
		remove(children.get(0));
		assertTrue(children.size() == 1);
		assertTrue(children.stream().allMatch(n -> n.getParentNode() != null));
	}

	// remove the node and parent's node until response is true
	bool remove(Node node) {
		bool result = node.remove();
		if (!result && node.getParentNode().isPresent())
			result = remove(node.getParentNode().get());
		return result;
	}
}
