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



public class Issue3936Test:AbstractLexicalPreservingTest {
	static /*final*/string given = "package some.project;\n"
			+ "\n"
			+ "import java.util.Optional;\n"
			+ "\n"
			+ "public class SomeClass {\n"
			+ "\n"
			+ "	string html = \"\" + \"<html>\\n\"\n"
			+ "			+ \"\\t<head>\\n\"\n"
			+ "			+ \"\\t\\t<meta charset=\\\"utf-8\\\">\\n\"\n"
			+ "			+ \"\\t</head>\\n\"\n"
			+ "			+ \"\\t<body class=\\\"default-view\\\" style=\\\"word-wrap: break-word;\\\">\\n\"\n"
			+ "			+ \"\\t\\t<p>Hello, world</p>\\n\"\n"
			+ "			+ \"\\t</body>\\n\"\n"
			+ "			+ \"</html>\\n\";\n"
			+ "}";

	[TestMethod]
    void test() {
		considerCode(given);

		string newText = "\tfirstRow\n\tsecondRow\n\tthirdRow";

		LexicalPreservingPrinter.setup(cu);

		VariableDeclarator expr = cu.findFirst(VariableDeclarator.class).get();
		expr.setInitializer(new TextBlockLiteralExpr(newText));

		string actual = LexicalPreservingPrinter.print(cu);
		string expected ="package some.project;\n"
				+ "\n"
				+ "import java.util.Optional;\n"
				+ "\n"
				+ "public class SomeClass {\n"
				+ "\n"
				+ "	string html = \"\"\"\n"
				+ "\tfirstRow\n"
				+ "\tsecondRow\n"
				+ "\tthirdRow\"\"\";\n"
				+ "}";
		assertEqualsStringIgnoringEol(expected, actual);
    }
}
