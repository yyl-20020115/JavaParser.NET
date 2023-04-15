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




public class Issue1774Test:AbstractResolutionTest {

    [TestMethod]
    public void test() {
        StaticJavaParser.setConfiguration(new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver(false))));

        string str = 
                  "public class A { "
                + "  string s1 = false + \"str\";"
                + "  string s2 = 'a' + \"str\";"
                + "  string s3 = 1 + \"foo\";"
                + "  float f = 3 % 2.71f;"
                + "  double d = 3 % 2.0;"
                + "  Integer i1 = 'G' & 6;"
                + "  long l1 = 'z' & 1L;"
                + "  long l2 = 0x01 & 2L;"
                + "  Integer i2 = 'G' & 6;"
                + "  long l3 = 'z' & 1L;"
                + "  long l4 = 0x01 & 2L;"
                + "  int i10 = 'B' << 1;"
                + "  byte b = 8;"
                + "  int i11 = b >> 2;"
                + "  short s = 0x0f;"
                + "  int i12 = 'B' << 1;"
                + "}";
        CompilationUnit cu = StaticJavaParser.parse(str);
        List<BinaryExpr> exprs = cu.findAll(BinaryExpr.class);
        assertEquals("java.lang.String", exprs.get(0).calculateResolvedType().describe());
        assertEquals("java.lang.String", exprs.get(1).calculateResolvedType().describe());
        assertEquals("java.lang.String", exprs.get(2).calculateResolvedType().describe());
        assertEquals("float", exprs.get(3).calculateResolvedType().describe());
        assertEquals("double", exprs.get(4).calculateResolvedType().describe());
        assertEquals("int", exprs.get(5).calculateResolvedType().describe());
        assertEquals("long", exprs.get(6).calculateResolvedType().describe());
        assertEquals("long", exprs.get(7).calculateResolvedType().describe());
        assertEquals("int", exprs.get(8).calculateResolvedType().describe());
        assertEquals("long", exprs.get(9).calculateResolvedType().describe());
        assertEquals("long", exprs.get(10).calculateResolvedType().describe());
        
        // unary primitve promotion
        assertEquals("int", exprs.get(11).calculateResolvedType().describe());
        assertEquals("int", exprs.get(12).calculateResolvedType().describe());
        assertEquals("int", exprs.get(13).calculateResolvedType().describe());
        
    }

}
