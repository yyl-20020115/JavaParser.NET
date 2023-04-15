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



public class Issue546Test {
    [TestMethod]
    void switchWithTabs() {
        Statement cu = parseStatement("switch(x){ case 1: return y; case 2: return z;}");

        string printed = new PrettyPrinter(new PrettyPrinterConfiguration())
                .print(cu);
        assertEqualsStringIgnoringEol("switch(x) {\n" +
                "    case 1:\n" +
                "        return y;\n" +
                "    case 2:\n" +
                "        return z;\n" +
                "}", printed);
    }
    [TestMethod]
    void switchWithoutTabs() {
        Statement cu = parseStatement("switch(x){ case 1: return y; case 2: return z;}");

        string printed = new PrettyPrinter(new PrettyPrinterConfiguration().setIndentCaseInSwitch(false))
                .print(cu);
        assertEqualsStringIgnoringEol("switch(x) {\n" +
                "case 1:\n" +
                "    return y;\n" +
                "case 2:\n" +
                "    return z;\n" +
                "}", printed);
    }

    public static void assertEqualsStringIgnoringEol(string expected, string actual) {
        assertEquals(
                normalizeEolInTextBlock(expected, LineSeparator.ARBITRARY),
                normalizeEolInTextBlock(actual, LineSeparator.ARBITRARY)
        );
    }
}
