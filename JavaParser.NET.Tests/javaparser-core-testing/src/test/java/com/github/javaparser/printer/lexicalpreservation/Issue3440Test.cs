namespace com.github.javaparser.printer.lexicalpreservation;

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


public class Issue3440Test:AbstractLexicalPreservingTest {

    [TestMethod]
    void test3440() {
        considerCode("public class Foo { public void bar() { switch(1) {case 1: break; } } }");
        string expected = "public class Foo { public void bar() { switch(1) {case 1:  } } }";
        SwitchEntry entry = cu.findFirst(SwitchEntry.class).get();
        entry.setStatements(new NodeList<>());
        TestUtils.assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }
    
}
