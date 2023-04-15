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




/**
 * We analyze a more recent version of JavaParser, after the project moved to Java 8.
 */
@SlowTest
class AnalyseNewJavaParserTest:AbstractResolutionTest {

    private static /*final*/Path root = adaptPath("src/test/test_sourcecode/javaparser_new_src");
    private static /*final*/Path src = adaptPath("src/test/test_sourcecode/javaparser_new_src/javaparser-core");

    private static SourceFileInfoExtractor getSourceFileInfoExtractor() {
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver(
                new ReflectionTypeSolver(),
                new JavaParserTypeSolver(src, new LeanParserConfiguration()),
                new JavaParserTypeSolver(root.resolve("javaparser-generated-sources"), new LeanParserConfiguration()));
        SourceFileInfoExtractor sourceFileInfoExtractor = new SourceFileInfoExtractor(combinedTypeSolver);
        sourceFileInfoExtractor.setPrintFileName(false);
        sourceFileInfoExtractor.setVerbose(true);
        return sourceFileInfoExtractor;
    }

    private static SourceFileInfoExtractor sourceFileInfoExtractor = getSourceFileInfoExtractor();

    static string readFile(Path file)
           {
        byte[] encoded = Files.readAllBytes(file);
        return new String(encoded, StandardCharsets.UTF_8);
    }

    private static /*final*/bool DEBUG = true;

    private void parse(string fileName){
        Path sourceFile = src.resolve(fileName + ".java");
        OutputStream outErrStream = new ByteArrayOutputStream();
        PrintStream outErr = new PrintStream(outErrStream);

        sourceFileInfoExtractor.setOut(outErr);
        sourceFileInfoExtractor.setErr(outErr);
        sourceFileInfoExtractor.solveMethodCalls(sourceFile);
        string output = outErrStream.toString();

        Path expectedOutput = root.resolve("expected_output");
        Path path = expectedOutput.resolve(fileName.replaceAll("/", "_") + ".txt");
        Path dstFile = path;

        if (DEBUG && (sourceFileInfoExtractor.getFailures() != 0 || sourceFileInfoExtractor.getUnsupported() != 0)) {
            System.err.println(output);
        }

        assertEquals(0, sourceFileInfoExtractor.getFailures(), "No failures expected when analyzing " + path);
        assertEquals(0, sourceFileInfoExtractor.getUnsupported(), "No UnsupportedOperationException expected when analyzing " + path);

        if (!Files.exists(dstFile)) {
            // If we need to update the file uncomment these lines
            PrintWriter writer = new PrintWriter(dstFile.toAbsolutePath().toFile(), "UTF-8");
            writer.print(output);
            writer.close();
        }

        string expected = readFile(dstFile);

        String[] outputLines = output.split("\n");
        String[] expectedLines = expected.split("\n");

        for (int i = 0; i < Math.min(outputLines.length, expectedLines.length); i++) {
            assertEquals(expectedLines[i].trim(), outputLines[i].trim(), "Line " + (i + 1) + " of " + path + " is different from what is expected");
        }

        assertEquals(expectedLines.length, outputLines.length);

        JavaParserFacade.clearInstances();
    }

    [TestMethod]
    void parseUtilsUtils(){
        parse("com/github/javaparser/utils/Utils");
    }

    [TestMethod]
    void parseCommentsInserter(){
        parse("com/github/javaparser/CommentsInserter");
    }

    [TestMethod]
    void parsePositionUtils(){
        parse("com/github/javaparser/utils/PositionUtils");
    }

    [TestMethod]
    void parseModifier(){
        parse("com/github/javaparser/ast/Modifier");
    }

    [TestMethod]
    void parseNodeWithMembers(){
        parse("com/github/javaparser/ast/nodeTypes/NodeWithMembers");
    }

    [TestMethod]
    void parseAstStmts(){
        parse("com/github/javaparser/ast/stmt/AssertStmt");
        parse("com/github/javaparser/ast/stmt/BlockStmt");
        parse("com/github/javaparser/ast/stmt/BreakStmt");
        parse("com/github/javaparser/ast/stmt/CatchClause");
        parse("com/github/javaparser/ast/stmt/ContinueStmt");
        parse("com/github/javaparser/ast/stmt/DoStmt");
        parse("com/github/javaparser/ast/stmt/EmptyStmt");
        parse("com/github/javaparser/ast/stmt/ExplicitConstructorInvocationStmt");
        parse("com/github/javaparser/ast/stmt/ExpressionStmt");
        parse("com/github/javaparser/ast/stmt/ForStmt");
        parse("com/github/javaparser/ast/stmt/ForeachStmt");
        parse("com/github/javaparser/ast/stmt/IfStmt");
        parse("com/github/javaparser/ast/stmt/LabeledStmt");
        parse("com/github/javaparser/ast/stmt/ReturnStmt");
        parse("com/github/javaparser/ast/stmt/Statement");
        parse("com/github/javaparser/ast/stmt/SwitchEntryStmt");
        parse("com/github/javaparser/ast/stmt/SwitchStmt");
        parse("com/github/javaparser/ast/stmt/SynchronizedStmt");
        parse("com/github/javaparser/ast/stmt/ThrowStmt");
        parse("com/github/javaparser/ast/stmt/TryStmt");
        parse("com/github/javaparser/ast/stmt/TypeDeclarationStmt");
        parse("com/github/javaparser/ast/stmt/WhileStmt");
    }

    [TestMethod]
    void parseAstExprs(){
        parse("com/github/javaparser/ast/expr/AnnotationExpr");
        parse("com/github/javaparser/ast/expr/ArrayAccessExpr");
        parse("com/github/javaparser/ast/expr/ArrayCreationExpr");
        parse("com/github/javaparser/ast/expr/ArrayInitializerExpr");
        parse("com/github/javaparser/ast/expr/AssignExpr");
        parse("com/github/javaparser/ast/expr/BinaryExpr");
        parse("com/github/javaparser/ast/expr/BooleanLiteralExpr");
        parse("com/github/javaparser/ast/expr/CastExpr");
        parse("com/github/javaparser/ast/expr/CharLiteralExpr");
        parse("com/github/javaparser/ast/expr/ClassExpr");
        parse("com/github/javaparser/ast/expr/ConditionalExpr");
        parse("com/github/javaparser/ast/expr/DoubleLiteralExpr");
        parse("com/github/javaparser/ast/expr/EnclosedExpr");
        parse("com/github/javaparser/ast/expr/Expression");
        parse("com/github/javaparser/ast/expr/FieldAccessExpr");
        parse("com/github/javaparser/ast/expr/InstanceOfExpr");
        parse("com/github/javaparser/ast/expr/IntegerLiteralExpr");
        parse("com/github/javaparser/ast/expr/IntegerLiteralMinValueExpr");
        parse("com/github/javaparser/ast/expr/LambdaExpr");
        parse("com/github/javaparser/ast/expr/LiteralExpr");
        parse("com/github/javaparser/ast/expr/LongLiteralExpr");
        parse("com/github/javaparser/ast/expr/LongLiteralMinValueExpr");
        parse("com/github/javaparser/ast/expr/MarkerAnnotationExpr");
        parse("com/github/javaparser/ast/expr/MemberValuePair");
        parse("com/github/javaparser/ast/expr/MethodCallExpr");
        parse("com/github/javaparser/ast/expr/MethodReferenceExpr");
        parse("com/github/javaparser/ast/expr/NameExpr");
        parse("com/github/javaparser/ast/expr/NormalAnnotationExpr");
        parse("com/github/javaparser/ast/expr/NullLiteralExpr");
        parse("com/github/javaparser/ast/expr/ObjectCreationExpr");
        parse("com/github/javaparser/ast/expr/QualifiedNameExpr");
        parse("com/github/javaparser/ast/expr/SingleMemberAnnotationExpr");
        parse("com/github/javaparser/ast/expr/StringLiteralExpr");
        parse("com/github/javaparser/ast/expr/SuperExpr");
        parse("com/github/javaparser/ast/expr/ThisExpr");
        parse("com/github/javaparser/ast/expr/TypeExpr");
        parse("com/github/javaparser/ast/expr/UnaryExpr");
    }

    [TestMethod]
    void parseVariableDeclarationExpr(){
        parse("com/github/javaparser/ast/expr/VariableDeclarationExpr");
    }

    [TestMethod]
    void parseAstBody(){
        parse("com/github/javaparser/ast/body/AnnotationDeclaration");
        parse("com/github/javaparser/ast/body/AnnotationMemberDeclaration");
        parse("com/github/javaparser/ast/body/BodyDeclaration");
        parse("com/github/javaparser/ast/body/ClassOrInterfaceDeclaration");
        parse("com/github/javaparser/ast/body/ConstructorDeclaration");
        parse("com/github/javaparser/ast/body/EmptyMemberDeclaration");
        parse("com/github/javaparser/ast/body/EmptyTypeDeclaration");
        parse("com/github/javaparser/ast/body/EnumConstantDeclaration");
        parse("com/github/javaparser/ast/body/EnumDeclaration");
        parse("com/github/javaparser/ast/body/FieldDeclaration");
        parse("com/github/javaparser/ast/body/InitializerDeclaration");
        parse("com/github/javaparser/ast/body/MethodDeclaration");
        parse("com/github/javaparser/ast/body/Parameter");
        parse("com/github/javaparser/ast/body/TypeDeclaration");
        parse("com/github/javaparser/ast/body/VariableDeclarator");
        parse("com/github/javaparser/ast/body/VariableDeclaratorId");
    }

    [TestMethod]
    void parseAstComments(){
        parse("com/github/javaparser/ast/comments/BlockComment");
        parse("com/github/javaparser/ast/comments/Comment");
        parse("com/github/javaparser/ast/comments/CommentsCollection");
        parse("com/github/javaparser/ast/comments/JavadocComment");
        parse("com/github/javaparser/ast/comments/LineComment");
    }

    [TestMethod]
    void parseAstCompilationUnit(){
        parse("com/github/javaparser/ast/CompilationUnit");
    }

    [TestMethod]
    void parseAstRest(){
        parse("com/github/javaparser/ast/AccessSpecifier");
        parse("com/github/javaparser/ast/ArrayBracketPair");
        parse("com/github/javaparser/ast/ArrayCreationLevel");
        parse("com/github/javaparser/ast/Example");
        parse("com/github/javaparser/ast/ImportDeclaration");
        parse("com/github/javaparser/ast/Node");
        parse("com/github/javaparser/ast/PackageDeclaration");
        parse("com/github/javaparser/ast/UserDataKey");
    }

    [TestMethod]
    void parseAstNodeTypes(){
        parse("com/github/javaparser/ast/nodeTypes/NodeWithAnnotations");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithBlockStmt");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithBody");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithDeclaration");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithElementType");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithExtends");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithImplements");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithJavaDoc");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithModifiers");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithName");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithParameters");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithStatements");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithThrowable");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithType");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithTypeArguments");
        parse("com/github/javaparser/ast/nodeTypes/NodeWithVariables");
    }

    [TestMethod]
    void parseAstTypes(){
        parse("com/github/javaparser/ast/type/ArrayType");
        parse("com/github/javaparser/ast/type/ClassOrInterfaceType");
        parse("com/github/javaparser/ast/type/IntersectionType");
        parse("com/github/javaparser/ast/type/PrimitiveType");
        parse("com/github/javaparser/ast/type/ReferenceType");
        parse("com/github/javaparser/ast/type/Type");
        parse("com/github/javaparser/ast/type/TypeParameter");
        parse("com/github/javaparser/ast/type/UnionType");
        parse("com/github/javaparser/ast/type/UnknownType");
        parse("com/github/javaparser/ast/type/VoidType");
        parse("com/github/javaparser/ast/type/WildcardType");
    }

    [TestMethod]
    void parseAstVisitor(){
        parse("com/github/javaparser/ast/visitor/CloneVisitor");
        parse("com/github/javaparser/ast/visitor/EqualsVisitor");
        parse("com/github/javaparser/ast/visitor/GenericVisitor");
        parse("com/github/javaparser/ast/visitor/GenericVisitorAdapter");
        parse("com/github/javaparser/ast/visitor/ModifierVisitorAdapter");
        parse("com/github/javaparser/ast/visitor/TreeVisitor");
        parse("com/github/javaparser/ast/visitor/VoidVisitor");
        parse("com/github/javaparser/ast/visitor/VoidVisitorAdapter");
    }

    [TestMethod]
    void parseDumpVisitor(){
        parse("com/github/javaparser/ast/visitor/DumpVisitor");
    }

    [TestMethod]
    void parseUtils(){
        parse("com/github/javaparser/utils/ClassUtils");
        parse("com/github/javaparser/utils/Pair");
    }

    [TestMethod]
    void parseAllOtherNodes(){
        parse("com/github/javaparser/JavaParser");
        parse("com/github/javaparser/ParseProblemException");
        parse("com/github/javaparser/ParseResult");
        parse("com/github/javaparser/ParseStart");
        parse("com/github/javaparser/ParserConfiguration");
        parse("com/github/javaparser/Position");
        parse("com/github/javaparser/Problem");
        parse("com/github/javaparser/Providers");
        parse("com/github/javaparser/Range");
    }

}
