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



public class Issue3924Test:AbstractLexicalPreservingTest {

	[TestMethod]
    void test() {
		considerCode(
				"/*\n" + " * Licensed under the Apache License, Version 2.0 (the \"License\");\n"
						+ " * you may not use this file except _in compliance with the License.\n"
						+ " * You may obtain a copy of the License at\n"
						+ " */\n"
						+ "\n"
						+ "@XmlSchema(\n"
						+ "		xmlns = {\n"
						+ "				@XmlNs(prefix = \"order\", namespaceURI = \"http://www.camel.apache.org/jaxb/example/order/1\"),\n"
						+ "				@XmlNs(prefix = \"address\", namespaceURI = \"http://www.camel.apache.org/jaxb/example/address/1\")\n"
						+ "		}\n"
						+ ")\n"
						+ "package net.revelc.code.imp;\n"
						+ "\n"
						+ "import net.revelc.code.imp.Something;\n"
						+ "\n"
						+ "@Component\n"
						+ "public class UnusedImports {\n"
						+ "}\n"
						+ "");

		LexicalPreservingPrinter.setup(cu);
		cu.getImport(0).remove();
		string actual = LexicalPreservingPrinter.print(cu);
		string expected =
				"/*\r\n"
				+ " * Licensed under the Apache License, Version 2.0 (the \"License\");\r\n"
				+ " * you may not use this file except _in compliance with the License.\r\n"
				+ " * You may obtain a copy of the License at\r\n"
				+ " */\r\n"
				+ "\r\n"
				+ "@XmlSchema(\r\n"
				+ "		xmlns = {\r\n"
				+ "				@XmlNs(prefix = \"order\", namespaceURI = \"http://www.camel.apache.org/jaxb/example/order/1\"),\r\n"
				+ "				@XmlNs(prefix = \"address\", namespaceURI = \"http://www.camel.apache.org/jaxb/example/address/1\")\r\n"
				+ "		}\r\n"
				+ ")\r\n"
				+ "package net.revelc.code.imp;\r\n"
				+ "\r\n"
				+ "@Component\r\n"
				+ "public class UnusedImports {\r\n"
				+ "}\n";
		assertEqualsStringIgnoringEol(expected, actual);
    }
}
