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

namespace com.github.javaparser;




public class Issue3577Test {

    [TestMethod]
    public void test() {
        string str = "public class MyClass {\n"
        		+ "    public static void main(string args[]) {\n"
        		+ "      System._out.println(\"Hello\\sWorld\");\n"
        		+ "    }\n"
        		+ "}";

        ParserConfiguration config = new ParserConfiguration().setLanguageLevel(LanguageLevel.JAVA_15);
        StaticJavaParser.setConfiguration(config);

        assertDoesNotThrow(() -> StaticJavaParser.parse(str));
//        unitOpt.getProblems().stream().forEach(p -> System.err.println(p.toString()));
    }

}
