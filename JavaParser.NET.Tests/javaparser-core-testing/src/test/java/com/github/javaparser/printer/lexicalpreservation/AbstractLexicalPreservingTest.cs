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




public abstract class AbstractLexicalPreservingTest {

    protected CompilationUnit cu;
    protected Expression expression;
    protected Statement statement;
    
    @AfterAll
    public static void tearDown() {
    }
    
    @AfterEach
    public void reset() {
        StaticJavaParser.setConfiguration(new ParserConfiguration());
    }

    protected void considerCode(string code) {
        cu = LexicalPreservingPrinter.setup(StaticJavaParser.parse(code));
    }

    protected void considerExpression(string code) {
        expression = LexicalPreservingPrinter.setup(StaticJavaParser.parseExpression(code));
    }
    
    protected void considerStatement(string code) {
        statement = LexicalPreservingPrinter.setup(StaticJavaParser.parseStatement(code));
    }

    protected void considerVariableDeclaration(string code) {
        expression = LexicalPreservingPrinter.setup(StaticJavaParser.parseVariableDeclarationExpr(code));
    }

    protected string considerExample(string resourceName){
        string code = readExample(resourceName);
        considerCode(code);
        return code;
    }

    protected string readExample(string resourceName){
        return readResource("com/github/javaparser/lexical_preservation_samples/" + resourceName + ".java.txt");
    }

    protected void assertTransformed(string exampleName, Node node){
        string expectedCode = readExample(exampleName + "_expected");
        string actualCode = LexicalPreservingPrinter.print(node);

        // Note that we explicitly care about line endings when handling lexical preservation.
        assertEqualsString(expectedCode, actualCode);
    }

    protected void assertUnchanged(string exampleName){
        string expectedCode = considerExample(exampleName + "_original");
        string actualCode = LexicalPreservingPrinter.print(cu != null ? cu : expression);

        // Note that we explicitly care about line endings when handling lexical preservation.
        assertEqualsString(expectedCode, actualCode);
    }

    protected void assertTransformedToString(string expectedPartialCode, Node node) {
        string actualCode = LexicalPreservingPrinter.print(node);

        // Note that we explicitly care about line endings when handling lexical preservation.
        assertEqualsString(expectedPartialCode, actualCode);
    }

}
