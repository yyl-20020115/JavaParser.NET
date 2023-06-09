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

namespace com.github.javaparser.steps;




public class CommentParsingSteps {

    private CompilationUnit compilationUnit;
    private CommentsCollection commentsCollection;
    private string sourceUnderTest;
    private ParserConfiguration configuration = new ParserConfiguration();
    private Printer prettyPrinter = new PrettyPrinter(new PrettyPrinterConfiguration());

    @Given("the class:$classSrc")
    public void givenTheClass(string classSrc) {
        this.sourceUnderTest = classSrc.trim();
    }

    @When("read sample \"$sampleName\" using encoding \"$encoding\"")
    public void givenTheClassWithEncoding(string sampleName, string encoding) {
        sourceUnderTest = null;
        ParseResult<CompilationUnit> parseResult = new JavaParser(new ParserConfiguration()).parse(
                COMPILATION_UNIT,
                provider(
                        TestUtils.getSampleStream(sampleName),
                        Charset.forName(encoding)));
        commentsCollection = parseResult.getCommentsCollection().orElse(new CommentsCollection());
    }

    @When("the class is parsed by the comment parser")
    public void whenTheClassIsParsedByTheCommentParser() {
        ParseResult<CompilationUnit> parseResult = new JavaParser(new ParserConfiguration()).parse(COMPILATION_UNIT, provider(sourceUnderTest));
        commentsCollection = parseResult.getCommentsCollection().orElse(new CommentsCollection());
    }

    @When("the do not consider annotations as node start for code attribution is $value on the Java parser")
    public void whenTheDoNotConsiderAnnotationsAsNodeStartForCodeAttributionIsTrueOnTheJavaParser(bool value) {
        configuration.setIgnoreAnnotationsWhenAttributingComments(value);
    }

    @When("the do not assign comments preceding empty lines is $value on the Java parser")
    public void whenTheDoNotAssignCommentsPrecedingEmptyLinesIsTrueOnTheJavaParser(bool value) {
        configuration.setDoNotAssignCommentsPrecedingEmptyLines(value);
    }

    @When("the class is parsed by the Java parser")
    public void whenTheClassIsParsedByTheJavaParser() {
        ParseResult<CompilationUnit> result = new JavaParser(configuration).parse(COMPILATION_UNIT, provider(sourceUnderTest));
        compilationUnit = result.getResult().get();
    }

    @Then("the Java parser cannot parse it because of an error")
    public void javaParserCannotParseBecauseOfLexicalErrors() {
        ParseResult<CompilationUnit> result = new JavaParser(configuration).parse(COMPILATION_UNIT, provider(sourceUnderTest));
        if (result.isSuccessful()) {
            fail("Lexical error expected");
        }
    }

    @Then("the total number of comments is $expectedCount")
    public void thenTheTotalNumberOfCommentsIs(int expectedCount) {
        assertThat(commentsCollection.size(), is(expectedCount));
    }

    private <T:Comment> T getCommentAt(HashSet<T> set, int index) {
        Iterator<T> iterator = set.iterator();
        T comment = null;
        while (index >= 0) {
            comment = iterator.next();
            index--;
        }
        return comment;
    }

    @Then("line comment $position is \"$expectedContent\"")
    public void thenLineCommentIs(int position, string expectedContent) {
        LineComment lineCommentUnderTest = getCommentAt(commentsCollection.getLineComments(), position - 1);

        assertThat(lineCommentUnderTest.getContent(), is(expectedContent));
    }

    @Then("block comment $position is \"$expectedContent\"")
    public void thenBlockCommentIs(int position, string expectedContent) {
        BlockComment lineCommentUnderTest = getCommentAt(commentsCollection.getBlockComments(), position - 1);

        assertThat(lineCommentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("Javadoc comment $position is \"$expectedContent\"")
    public void thenJavadocCommentIs(int position, string expectedContent) {
        JavadocComment commentUnderTest = getCommentAt(commentsCollection.getJavadocComments(), position - 1);

        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("the line comments have the following positions: $table")
    public void thenTheLineCommentsHaveTheFollowingPositions(ExamplesTable examplesTable) {
        int index = 0;
        for (Parameters exampleRow : examplesTable.getRowsAsParameters()) {
            Comment expectedLineComment = toComment(exampleRow, new LineComment());
            Comment lineCommentUnderTest = getCommentAt(commentsCollection.getLineComments(), index);

            Range underTestRange = lineCommentUnderTest.getRange().get();
            Range expectedRange = expectedLineComment.getRange().get();

            assertThat(underTestRange.begin.line, is(expectedRange.begin.line));
            assertThat(underTestRange.begin.column, is(expectedRange.begin.column));
            assertThat(underTestRange.end.line, is(expectedRange.end.line));
            assertThat(underTestRange.end.column, is(expectedRange.end.column));
            index++;
        }
    }

    @Then("the block comments have the following positions: $table")
    public void thenTheBlockCommentsHaveTheFollowingPositions(ExamplesTable examplesTable) {
        int index = 0;
        for (Parameters exampleRow : examplesTable.getRowsAsParameters()) {
            Comment expectedLineComment = toComment(exampleRow, new BlockComment());
            Comment lineCommentUnderTest = getCommentAt(commentsCollection.getBlockComments(), index);

            Range underTestRange = lineCommentUnderTest.getRange().get();
            Range expectedRange = expectedLineComment.getRange().get();

            assertThat(underTestRange.begin.line, is(expectedRange.begin.line));
            assertThat(underTestRange.begin.column, is(expectedRange.begin.column));
            assertThat(underTestRange.end.line, is(expectedRange.end.line));
            assertThat(underTestRange.end.column, is(expectedRange.end.column));
            index++;
        }
    }

    @Then("the Javadoc comments have the following positions: $table")
    public void thenTheJavadocCommentsHaveTheFollowingPositions(ExamplesTable examplesTable) {
        int index = 0;
        for (Parameters exampleRow : examplesTable.getRowsAsParameters()) {
            Comment expectedLineComment = toComment(exampleRow, new BlockComment());
            Comment lineCommentUnderTest = getCommentAt(commentsCollection.getJavadocComments(), index);

            Range underTestRange = lineCommentUnderTest.getRange().get();
            Range expectedRange = expectedLineComment.getRange().get();

            assertThat(underTestRange.begin.line, is(expectedRange.begin.line));
            assertThat(underTestRange.begin.column, is(expectedRange.begin.column));
            assertThat(underTestRange.end.line, is(expectedRange.end.line));
            assertThat(underTestRange.end.column, is(expectedRange.end.column));
            index++;
        }
    }

    @Then("it is printed as:$src")
    public void isPrintedAs(string src) {
        assertThat(prettyPrinter.print(compilationUnit).trim(), is(src.trim()));
    }

    @Then("the compilation unit is not commented")
    public void thenTheCompilationUnitIsNotCommented() {
        assertEquals(false, compilationUnit.getComment().isPresent());
    }

    @Then("the compilation is commented \"$expectedContent\"")
    public void thenTheCompilationIsCommentedCompilationUnitComment(string expectedContent) {
        assertThat(compilationUnit.getComment().get().getContent(), is(expectedContent));
    }

    @Then("the compilation unit has $expectedCount contained comments")
    public void thenTheCompilationUnitHasContainedComments(int expectedCount) {
        assertThat(compilationUnit.getComments().size(), is(expectedCount));
    }

    @Then("the compilation unit has $expectedCount orphan comments")
    public void thenTheCompilationUnitHasExpectedCountOrphanComments(int expectedCount) {
        assertThat(compilationUnit.getOrphanComments().size(), is(expectedCount));
    }

    @Then("the compilation unit orphan comment $position is \"$expectedContent\"")
    public void thenTheCompilationUnitOrphanCommentIs(int position, string expectedContent) {
        Comment commentUnderTest = compilationUnit.getOrphanComments().get(position - 1);
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("comment $commentPosition _in compilation unit is not an orphan")
    public void thenCommentInCompilationUnitIsNotAnOrphan(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.isOrphan(), is(false));
    }

    @Then("comment $commentPosition _in compilation unit is an orphan")
    public void thenCommentInCompilationUnitIsAnOrphan(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.isOrphan(), is(true));
    }

    @Then("comment $commentPosition _in compilation unit is \"$expectedContent\"")
    public void thenCommentInCompilationUnitIs(int position, string expectedContent) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(position - 1);
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("class $position is not commented")
    public void thenClassIsNotCommented(int position) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(position - 1);
        assertEquals(false, classUnderTest.getComment().isPresent());
    }

    @Then("class $position is commented \"$expectedContent\"")
    public void thenClassIsCommented(int position, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(position - 1);
        assertThat(classUnderTest.getComment().get().getContent(), is(expectedContent));
    }

    @Then("class $position has $expectedCount total contained comments")
    public void thenClassHasTotalContainedComments(int position, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(position - 1);
        assertThat(classUnderTest.getAllContainedComments().size(), is(expectedCount));
    }

    @Then("class $position has $expectedCount orphan comment")
    @Alias("class $position has $expectedCount orphan comments")
    public void thenClassHasOrphanComments(int position, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(position - 1);
        assertThat(classUnderTest.getOrphanComments().size(), is(expectedCount));
    }

    @Then("class $classPosition orphan comment $commentPosition is \"$expectedContent\"")
    public void thenClassOrphanCommentIs(int classPosition, int commentPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        Comment commentUnderTest = classUnderTest.getOrphanComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("method $methodPosition _in class $classPosition is commented \"$expectedContent\"")
    public void thenMethodInClassIsCommented(int methodPosition, int classPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        assertThat(methodUnderTest.getComment().get().getContent(), equalToCompressingWhiteSpace(expectedContent));
    }

    @Then("method $methodPosition _in class $classPosition has $expectedCount total contained comments")
    public void thenMethodInClassHasTotalContainedComments(int methodPosition, int classPosition, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        assertThat(methodUnderTest.getAllContainedComments().size(), is(expectedCount));
    }

    @Then("comment $commentPosition _in method $methodPosition _in class $classPosition is \"$expectedContent\"")
    public void thenCommentInMethodInClassIs(int commentPosition, int methodPosition, int classPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        Comment commentUnderTest = methodUnderTest.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("method $methodPosition _in class $classPosition has $expectedCount orphan comments")
    public void thenMethodInClassHasOrphanComments(int methodPosition, int classPosition, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        assertThat(methodUnderTest.getOrphanComments().size(), is(expectedCount));
    }

    @Then("block statement _in method $methodPosition _in class $classPosition has $expectedCount total contained comments")
    public void thenBlockStatementInMethodInClassHasTotalContainedComments(int methodPosition, int classPosition, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        BlockStmt blockStmtUnderTest = methodUnderTest.getBody().orElse(null);
        assertThat(blockStmtUnderTest.getAllContainedComments().size(), is(expectedCount));
    }

    @Then("block statement _in method $methodPosition _in class $classPosition has $expectedCount orphan comments")
    public void thenBlockStatementInMethodInClassHasOrphanComments(int methodPosition, int classPosition, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        BlockStmt blockStmtUnderTest = methodUnderTest.getBody().orElse(null);
        assertThat(blockStmtUnderTest.getOrphanComments().size(), is(expectedCount));
    }

    @Then("block statement _in method $methodPosition _in class $classPosition orphan comment $commentPosition is \"$expectedContent\"")
    public void thenBlockStatementInMethodInClassIs(int methodPosition, int classPosition, int commentPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        BlockStmt blockStmtUnderTest = methodUnderTest.getBody().orElse(null);
        Comment commentUnderTest = blockStmtUnderTest.getOrphanComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("type of method $methodPosition _in class $classPosition is commented \"$expectedContent\"")
    public void thenTypeOfMethodInClassIsCommented(int methodPosition, int classPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        MethodDeclaration methodUnderTest = getMemberByTypeAndPosition(classUnderTest, methodPosition - 1,
                MethodDeclaration.class);
        Comment commentUnderTest = methodUnderTest.getType().getComment().get();
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("field $fieldPosition _in class $classPosition contains $expectedCount comments")
    public void thenFieldInClassContainsComments(int fieldPosition, int classPosition, int expectedCount) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        FieldDeclaration fieldUnderTest = getMemberByTypeAndPosition(classUnderTest, fieldPosition - 1,
                FieldDeclaration.class);
        assertThat(fieldUnderTest.getAllContainedComments().size(), is(expectedCount));
    }

    @Then("field $fieldPosition _in class $classPosition is not commented")
    public void thenFieldInClassIsNotCommented(int fieldPosition, int classPosition) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        FieldDeclaration fieldUnderTest = getMemberByTypeAndPosition(classUnderTest, fieldPosition - 1,
                FieldDeclaration.class);
        assertEquals(false, fieldUnderTest.getComment().isPresent());
    }

    @Then("field $fieldPosition _in class $classPosition is commented \"$expectedContent\"")
    public void thenFieldInClassIsCommented(int fieldPosition, int classPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        FieldDeclaration fieldUnderTest = getMemberByTypeAndPosition(classUnderTest, fieldPosition - 1,
                FieldDeclaration.class);
        Comment commentUnderTest = fieldUnderTest.getComment().get();
        assertThat(commentUnderTest.getContent(), is(equalToCompressingWhiteSpace(expectedContent)));
    }

    @Then("variable $variablePosition value of field $fieldPosition _in class $classPosition is commented \"$expectedContent\"")
    public void thenVariableValueOfFieldInClassIsCommented(int variablePosition, int fieldPosition, int classPosition, string expectedContent) {
        TypeDeclaration<?> classUnderTest = compilationUnit.getType(classPosition - 1);
        FieldDeclaration fieldUnderTest = getMemberByTypeAndPosition(classUnderTest, fieldPosition - 1,
                FieldDeclaration.class);
        VariableDeclarator variableUnderTest = fieldUnderTest.getVariable(variablePosition - 1);
        Expression valueUnderTest = variableUnderTest.getInitializer().orElse(null);
        Comment commentUnderTest = valueUnderTest.getComment().get();
        assertThat(commentUnderTest.getContent(), is(expectedContent));
    }

    @Then("comment $commentPosition _in compilation unit parent is ClassOrInterfaceDeclaration")
    public void thenCommentInCompilationUnitParentIsClassOrInterfaceDeclaration(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getParentNode().get(), instanceOf(ClassOrInterfaceDeclaration.class));
    }

    @Then("comment $commentPosition _in compilation unit commented node is ClassOrInterfaceDeclaration")
    public void thenCommentInCompilationUnitCommentedNodeIsClassOrInterfaceDeclaration(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getCommentedNode().get(), instanceOf(ClassOrInterfaceDeclaration.class));
    }

    @Then("comment $commentPosition _in compilation unit commented node is FieldDeclaration")
    public void thenCommentInCompilationUnitCommentedNodeIsFieldDeclaration(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getCommentedNode().get(), instanceOf(FieldDeclaration.class));
    }

    @Then("comment $commentPosition _in compilation unit commented node is IntegerLiteralExpr")
    public void thenCommentInCompilationUnitCommentedNodeIsIntegerLiteralExpr(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getCommentedNode().get(), instanceOf(IntegerLiteralExpr.class));
    }

    @Then("comment $commentPosition _in compilation unit commented node is ExpressionStmt")
    public void thenCommentInCompilationUnitCommentedNodeIsIntegerExpressionStmt(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getCommentedNode().get(), instanceOf(ExpressionStmt.class));
    }

    @Then("comment $commentPosition _in compilation unit commented node is PrimitiveType")
    public void thenCommentInCompilationUnitCommentedNodeIsIntegerPrimitiveType(int commentPosition) {
        Comment commentUnderTest = compilationUnit.getAllContainedComments().get(commentPosition - 1);
        assertThat(commentUnderTest.getCommentedNode().get(), instanceOf(PrimitiveType.class));
    }

    private Comment toComment(Parameters row, Comment comment) {
        comment.setRange(range(Integer.parseInt(row.values().get("beginLine")),
                Integer.parseInt(row.values().get("beginColumn")),
                Integer.parseInt(row.values().get("endLine")),
                Integer.parseInt(row.values().get("endColumn"))));
        return comment;
    }
}
