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

namespace com.github.javaparser.issues;



public class Issue3255Test {

    private static /*final*/string EOL = LineSeparator.SYSTEM.asRawString();

    [TestMethod]
    public void test() {
        JavaParser javaParser = new JavaParser();
        ParseResult<CompilationUnit> parseResult = javaParser.parse("class Test {" + EOL +
                "    private void bad() {" + EOL +
                "        string record = \"\";" + EOL +
                "        record.getBytes();" + EOL +
                "    }" + EOL +
                "}");

        assertEquals(0, parseResult.getProblems().size());

        CompilationUnit compilationUnit = parseResult.getResult().get();
    }

    [TestMethod]
    public void test2() {
        JavaParser javaParser = new JavaParser();
        ParseResult<CompilationUnit> parseResult = javaParser.parse("class Test {" + EOL +
                "    private void bad() {" + EOL +
                "        string record2 = \"\";" + EOL +
                "        record2.getBytes();" + EOL +
                "    }" + EOL +
                "}");

        assertEquals(0, parseResult.getProblems().size());

        CompilationUnit compilationUnit = parseResult.getResult().get();
    }

    [TestMethod]
    void recordIsAValidVariableNameWhenParsingAStatement() {
        parseStatement("Object record;");
    }

    [TestMethod]
    public void recordIsAValidVariableNameWhenUsedInAClass() {
        JavaParser javaParser = new JavaParser();
        ParseResult<CompilationUnit> parseResult = javaParser.parse("class Test {" + EOL +
                "    private void goodInJava16() {" + EOL +
                "        Object record;" + EOL +
                "    }" + EOL +
                "}");

        assertEquals(0, parseResult.getProblems().size());

        CompilationUnit compilationUnit = parseResult.getResult().get();
    }

}
