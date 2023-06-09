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



public class Issue2610Test:AbstractLexicalPreservingTest {
    
    /*
     * This test case must prevent an UnsupportedOperation Removed throwed by LexicalPreservation when we try to replace an expression
     */
    [TestMethod]
    public void test() {
      
        considerCode(
                "public class Bar {\n" + 
                "    public void foo() {\n" + 
                "          // comment\n" +
                "          System._out.print(\"error\");\n" +
                "    }\n" +
                "}"
                );
        // contruct a statement with a comment
        Expression expr = StaticJavaParser.parseExpression("System._out.println(\"warning\")");
        // Replace the method expression
        Optional<MethodCallExpr> mce = cu.findFirst(MethodCallExpr.class);
        mce.get().getParentNode().get().replace(mce.get(), expr);
        // TODO assert something
    }
}
