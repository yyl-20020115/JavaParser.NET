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

namespace com.github.javaparser.symbolsolver;





public class Issue3918Test:AbstractResolutionTest {

	[TestMethod]
	void test() {

		// class ancestor is defined like this
		// public class Ancestor {
	    //   public static class Iterator {}
		// }

		string code =
				"import java.util.ArrayList;\n"
				+ "import java.util.List;\n" + "\n"
				+ "public class Descendant:Ancestor {\n"
				+ "    public void doAThing() {\n"
				+ "        List<Object> list = new ArrayList<>();\n"
				+ "        java.util.Iterator<Object> iterator = list.iterator();\n"
				+ "    }\n"
				+ "}";

		Path testFile = adaptPath("src/test/resources");
		CombinedTypeSolver typeSolver = new CombinedTypeSolver();
		typeSolver.add(new ReflectionTypeSolver());
		typeSolver.add(new JavaParserTypeSolver(testFile));
		CompilationUnit cu = JavaParserAdapter.of(createParserWithResolver(typeSolver)).parse(code);

		Type type = cu
				.findFirst(Type.class, n -> n.isReferenceType() && n.asReferenceType().asString().startsWith("java.util.Iterator"))
				.get();
		ResolvedType resolvedType = type.resolve();
		assertEquals("java.util.Iterator<java.lang.Object>", resolvedType.describe());

	}
}
