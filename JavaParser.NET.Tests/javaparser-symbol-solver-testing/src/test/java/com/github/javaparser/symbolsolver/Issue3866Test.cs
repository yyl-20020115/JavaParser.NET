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





public class Issue3866Test:AbstractResolutionTest {

	[TestMethod]
	void test() {

		string code =
				"public interface MyActivity {\n"
				+ "  class MyTimestamps {}\n"
				+ "    MyTimestamps getTimestamps();\n"
				+ "  }\n"
				+ "\n"
				+ "  public interface MyRichPresence:MyActivity { }\n"
				+ "\n"
				+ "  class MyActivityImpl implements MyActivity {\n"
				+ "    MyActivity.MyTimestamps timestamps;\n"
				+ "    //@Override\n"
				+ "    public MyActivity.MyTimestamps getTimestamps() {\n"
				+ "      return timestamps;\n"
				+ "  }\n"
				+ "}";

		/*final*/JavaSymbolSolver solver = new JavaSymbolSolver(new ReflectionTypeSolver(false));
		StaticJavaParser.getParserConfiguration().setSymbolResolver(solver);
        /*final*/CompilationUnit compilationUnit = StaticJavaParser.parse(code);

        /*final*/List<String> returnTypes = compilationUnit.findAll(MethodDeclaration.class)
                .stream()
                .map(md -> md.resolve())
                .map(rmd -> rmd.getReturnType().describe())
                .collect(Collectors.toList());

        returnTypes.forEach(type -> assertEquals("MyActivity.MyTimestamps", type));
	}
}
