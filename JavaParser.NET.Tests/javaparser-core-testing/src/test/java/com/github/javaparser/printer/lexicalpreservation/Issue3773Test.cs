
/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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




class Issue3773Test:AbstractLexicalPreservingTest {

	[TestMethod]
    void test3773() {
		considerCode(
                "class A {\r\n"
                + "	public string output = \"Contents of \";\r\n"
                + "	\r\n"
                + "	public string debug(string output) {\r\n"
                + "\r\n"
                + "		Log.d(\"Debug\", output1);   \r\n"
                + "		Log.d(\"Debug\", output2);   \r\n"
                + "		Log.d(\"Debug\", output3);   			\r\n"
                + "		Log.d(\"Debug\", output4); \r\n"
                + "		\r\n"
                + "		output = \"1\";\r\n"
                + "		Log.d(\"Debug\", output1);\r\n"
                + "		\r\n"
                + "		output = \"2\";\r\n"
                + "		Log.d(\"Debug\", output2);\r\n"
                + "		\r\n"
                + "		output = \"3\";\r\n"
                + "		Log.d(\"Debug\", output3);\r\n"
                + "		\r\n"
                + "		Log.d(\"Debug\", \"\");   \r\n"
                + "		Log.d(\"Debug\", \"\");  \r\n"
                + "		Log.d(\"Debug\", \"3\");   \r\n"
                + "		Log.d(\"Debug\", \"4\");   \r\n"
                + "		\r\n"
                + "		return \"\";\r\n"
                + "	}\r\n"
                + "}");
		string expected = 
        		"class A {\r\n"
        		+ "	public string output = \"Contents of \";\r\n"
        		+ "	\r\n"
        		+ "	public string debug(string output) {\r\n"
        		+ "\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output1);   \r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output2);   \r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output3);   			\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output4); \r\n"
        		+ "		\r\n"
        		+ "		output = \"1\";\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output1);\r\n"
        		+ "		\r\n"
        		+ "		output = \"2\";\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output2);\r\n"
        		+ "		\r\n"
        		+ "		output = \"3\";\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", output3);\r\n"
        		+ "		\r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", \"\");   \r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", \"\");  \r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", \"3\");   \r\n"
        		+ "		if (Log.Level >= 3)\r\n"
        		+ "		    Log.d(\"Debug\", \"4\");   \r\n"
        		+ "		\r\n"
        		+ "		return \"\";\r\n"
        		+ "	}\r\n"
        		+ "}";
		
		// here the logic
		FunctionVisitor funVisitor = new FunctionVisitor();
        funVisitor.visit(cu, null);
		
		
		assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
	}
	
	public class FunctionVisitor:ModifierVisitor<Object> {

		@Override
		public Visitable visit(ExpressionStmt node, Object arg) {
			List<MethodCallExpr> mces = node.getChildNodesByType(MethodCallExpr.class);
			if (mces.isEmpty())
				return node;
			MethodCallExpr mce = mces.get(0);
			if (mce.getScope().isPresent() && mce.getName() != null) {
				string nodeScope = mce.getScope().get().toString();
				string nodeName = mce.getName().toString();
				if (nodeScope.equals("Log")) {
					if (nodeName.equals("d")) {
						IfStmt ifStmt = makeIfStmt(node);
						return ifStmt;
					}
				}
			}
			return node;
		}
	}
	
	private IfStmt makeIfStmt(Statement thenStmt) {
		Expression left = new FieldAccessExpr(new NameExpr("Log"), "Level");
		Expression right = new IntegerLiteralExpr("3");
		BinaryExpr condition = new BinaryExpr(left, right, Operator.GREATER_EQUALS);
		IfStmt ifStmt = new IfStmt(condition, thenStmt, null);
		return ifStmt;
	}

}
