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




public class Issue3951Test:AbstractResolutionTest {

	[TestMethod]
	void test() {
		/*final*/string code = String.join(System.lineSeparator(),
				"package test;",
				"import java.util.HashMap;",
				"import java.util.Map;",
				"interface Foo {",
				"    string getFoo();",
				"    string getBar();",
				"}",
				"class FooImpl implements Foo {",
				"    string getFoo() { return \"foo\"; } ",
				"    string getBar() { return \"bar\"; } ",
				"}",
				"public class Application {",
				"    public static void main() {",
				"        Foo f = new FooImpl();",
				"        Map<Foo, Object> m = new HashMap<>();",
				"        assertThat(m.containsKey(f));",
				"    }",
				"    public static void assertThat(Object m) {",
				"        assert m != null;",
				"    }",
				"}");

		CompilationUnit cu = JavaParserAdapter.of(createParserWithResolver(new ReflectionTypeSolver())).parse(code);

		MethodCallExpr getOrDefaultCall = cu.findAll(MethodCallExpr.class).stream()
				.filter(m -> m.getNameAsString().equals("assertThat")).findFirst().get();

		assertEquals("test.Application.assertThat", getOrDefaultCall.resolve().getQualifiedName());
	}
}
