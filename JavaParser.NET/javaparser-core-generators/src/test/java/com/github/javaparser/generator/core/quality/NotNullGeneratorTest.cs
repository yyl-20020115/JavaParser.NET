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

namespace com.github.javaparser.generator.core.quality;




class NotNullGeneratorTest {

	[TestMethod]
	void testExecutionOfGenerator() {

		// Setup the
		string resourcesFolderPath = getClass().getCanonicalName().replace(".", File.separator);

		string basePath = Paths.get("src", "test", "resources").toString();
		Path originalFile = Paths.get(basePath, resourcesFolderPath, "original");
		Path expectedFile = Paths.get(basePath, resourcesFolderPath, "expected");

		SourceRoot originalSources = new SourceRoot(originalFile);
		SourceRoot expectedSources = new SourceRoot(expectedFile);
		expectedSources.tryToParse();

		// Generate the information
		new NotNullGenerator(originalSources).generate();

		List<CompilationUnit> editedSourceCus = originalSources.getCompilationUnits();
		List<CompilationUnit> expectedSourcesCus = expectedSources.getCompilationUnits();
		assertEquals(expectedSourcesCus.size(), editedSourceCus.size());

		// Check if all the files match the expected result
		for (int i = 0 ; i < editedSourceCus.size() ; i++) {

			DefaultPrettyPrinter printer = new DefaultPrettyPrinter();
			string expectedCode = printer.print(expectedSourcesCus.get(i));
			string editedCode = printer.print(editedSourceCus.get(i));

			if (!expectedCode.equals(editedCode)) {
				System._out.println("Expected:");
				System._out.println("####");
				System._out.println(expectedSourcesCus.get(i));
				System._out.println("####");
				System._out.println("Actual:");
				System._out.println("####");
				System._out.println(editedSourceCus.get(i));
				System._out.println("####");
				fail("Actual code doesn't match with the expected code.");
			}
		}
	}

}
