/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver.resolution;




class ExprResolutionTest:AbstractResolutionTest {

    private TypeSolver ts;
    private ResolvedType stringType;

    @BeforeEach
    void setup() {
        ts = new ReflectionTypeSolver();
        stringType = new ReferenceTypeImpl(ts.solveType(String.class.getCanonicalName()));
    }

    // JLS 5.6.2. Binary Numeric Promotion
    // Widening primitive conversion (§5.1.2) is applied to convert either or both operands as specified by the
    // following rules:
    //
    // * If either operand is of type double, the other is converted to double.
    // * Otherwise, if either operand is of type float, the other is converted to float.
    // * Otherwise, if either operand is of type long, the other is converted to long.
    // * Otherwise, both operands are converted to type int.

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsDoubleAndByte() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  double d = 0.0; "
                        + "  System._out.println( d + b );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsByteAndDouble() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  double d = 0.0; "
                        + "  System._out.println( b + d );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsDoubleAndChar() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  char c = 'a'; "
                        + "  double d = 0.0; "
                        + "  System._out.println( d + c );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsCharAndDouble() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  char c = 'a'; "
                        + "  double d = 0.0; "
                        + "  System._out.println( c + d );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsDoubleAndInt() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  int i = 0; "
                        + "  double d = 0.0; "
                        + "  System._out.println( d + i );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsIntAndDouble() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  int i = 0; "
                        + "  double d = 0.0; "
                        + "  System._out.println( i + d );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsfloatAndByte() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( f + b );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsByteAndfloat() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( b + f );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsfloatAndChar() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  char c = 'a'; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( f + c );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsCharAndfloat() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  char c = 'a'; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( c + f );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsfloatAndInt() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  int i = 0; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( f + i );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsIntAndfloat() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  int i = 0; "
                        + "  float f = 0.0f; "
                        + "  System._out.println( i + f );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(FLOAT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsDoubleAndFloat() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  float f = 0.0f; "
                        + "  double d = 0.0; "
                        + "  System._out.println( d + f );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsFloatAndDouble() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  float f = 0.0f; "
                        + "  double d = 0.0; "
                        + "  System._out.println( f + d );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(DOUBLE, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1589
    [TestMethod]
    void typeOfPlusExpressionsByteAndChar() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  char c = 'a'; "
                        + "  System._out.println( b + c );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(INT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    [TestMethod]
    void typeOfPlusExpressionsCharAndByte() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  byte b = (byte)0; "
                        + "  char c = 'a'; "
                        + "  System._out.println( c + b );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(INT, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsDoubleAndString() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( 1.0 + \"a_text\" );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsIntAndString() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( 1 + s1 );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsCharAndString() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( s1.charAt(2) + s1 );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsStringAndDouble() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( \"a_text\" + 1.0 );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsStringAndInt() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( s1 + 1 );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

    // Related to issue 1731
    [TestMethod]
    void typeOfPlusExpressionsStringAndChar() {
        CompilationUnit compilationUnit = parse(
                "public class Class1 {"
                        + " public void method1() {"
                        + "  string s1 = \"string1\";"
                        + "  System._out.println( s1 + s1.charAt(2) );"
                        + " }"
                        + "}");

        List<BinaryExpr> bExprs = compilationUnit.findAll(BinaryExpr.class);
        assertEquals(1, bExprs.size());
        assertEquals(stringType, JavaParserFacade.get(ts).getType(bExprs.get(0)));
    }

}
