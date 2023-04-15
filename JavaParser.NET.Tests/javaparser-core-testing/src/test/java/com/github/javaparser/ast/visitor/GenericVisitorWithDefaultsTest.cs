/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General License as published by
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
 * GNU Lesser General License for more details.
 */

namespace com.github.javaparser.ast.visitor;



/**
 * This class contains the tests to validate GenericVisitorWithDefaults.
 *
 * @author 4everTheOne
 */
class GenericVisitorWithDefaultsTest {

    @Captor
    private ArgumentCaptor<Object> argumentCaptor;

    private Object argument;
    private GenericVisitorWithDefaults<Node, Object> visitor;

    @BeforeEach
    void initialize() {
        openMocks(this);

        argument = new Object();
        visitor = spy(
            new GenericVisitorWithDefaults<Node, Object>() {
                //@Override
                public Node defaultAction(Node n, Object arg) {
                    super.defaultAction(n, arg);
                    return n;
                }
            }
        );
    }

    [TestMethod]
    void testThatVisitWithNodeListMethodAsParameter() {
        NodeList<Node> nodeList = new NodeList<>();
        Node node = visitor.visit(nodeList, argument);
        assertNull(node);
    }

    [TestMethod]
    void testThatVisitWithAnnotationDeclarationMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(AnnotationDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAnnotationMemberDeclarationMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(AnnotationMemberDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayAccessExprMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(ArrayAccessExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayCreationExprMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(ArrayCreationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayInitializerExprMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(ArrayInitializerExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAssertStmtMethodAsParameterCallsDefaultAction() {
        Node node = visitor.visit(mock(AssertStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithBlockStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(BlockStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithBooleanLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(BooleanLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithBreakStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(BreakStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithCastExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(CastExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithCatchClauseAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(CatchClause.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithCharLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(CharLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithClassExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ClassExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithClassOrInterfaceDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ClassOrInterfaceDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithClassOrInterfaceTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ClassOrInterfaceType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithCompilationUnitAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(CompilationUnit.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithConditionalExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ConditionalExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithConstructorDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ConstructorDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithContinueStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ContinueStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithDoStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(DoStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithDoubleLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(DoubleLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAnnotationDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(AnnotationDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAnnotationMemberDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(AnnotationMemberDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayAccessExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ArrayAccessExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayCreationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ArrayCreationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayCreationLevelAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ArrayCreationLevel.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayInitializerExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ArrayInitializerExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithArrayTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ArrayType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAssertStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(AssertStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithAssignExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(AssignExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithBinaryExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(BinaryExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithBlockCommentAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(BlockComment.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithEmptyStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(EmptyStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithEnclosedExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(EnclosedExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithEnumConstantDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(EnumConstantDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithEnumDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(EnumDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithExplicitConstructorInvocationStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ExplicitConstructorInvocationStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithExpressionStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ExpressionStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithFieldAccessExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(FieldAccessExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithFieldDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(FieldDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithForEachStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ForEachStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithForStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ForStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithIfStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(IfStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithImportDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ImportDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithInitializerDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(InitializerDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithInstanceOfExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(InstanceOfExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithIntegerLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(IntegerLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithIntersectionTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(IntersectionType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithJavadocCommentAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(JavadocComment.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLabeledStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LabeledStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLambdaExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LambdaExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLineCommentAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LineComment.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLocalClassDeclarationStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LocalClassDeclarationStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLocalRecordDeclarationStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LocalRecordDeclarationStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithLongLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(LongLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithMarkerAnnotationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(MarkerAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithMemberValuePairAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(MemberValuePair.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithMethodCallExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(MethodCallExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithMethodDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(MethodDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithMethodReferenceExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(MethodReferenceExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModifierAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(Modifier.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleExportsDirectiveAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleExportsDirective.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleOpensDirectiveAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleOpensDirective.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleProvidesDirectiveAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleProvidesDirective.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleRequiresDirectiveAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleRequiresDirective.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithModuleUsesDirectiveAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ModuleUsesDirective.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithNameExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(NameExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithNameAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(Name.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithNormalAnnotationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(NormalAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithNullLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(NullLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithObjectCreationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ObjectCreationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithPackageDeclarationAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(PackageDeclaration.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithParameterAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(Parameter.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithPatternExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(PatternExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithPrimitiveTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(PrimitiveType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithReceiverParameterAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ReceiverParameter.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithReturnStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ReturnStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSimpleNameAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SimpleName.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSingleMemberAnnotationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SingleMemberAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithStringLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(StringLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSuperExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SuperExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSwitchEntryAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SwitchEntry.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSwitchExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SwitchExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSwitchStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SwitchStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithSynchronizedStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(SynchronizedStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithTextBlockLiteralExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(TextBlockLiteralExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithThisExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ThisExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithThrowStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(ThrowStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithTryStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(TryStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithTypeExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(TypeExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithTypeParameterAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(TypeParameter.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithUnaryExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(UnaryExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithUnionTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(UnionType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithUnknownTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(UnknownType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithUnparsableStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(UnparsableStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithVarTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(VarType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithVariableDeclarationExprAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(VariableDeclarationExpr.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithVariableDeclaratorCallDefaultAction() {
        Node node = visitor.visit(mock(VariableDeclarator.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithVoidTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(VoidType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithWhileStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(WhileStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithWildcardTypeAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(WildcardType.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    [TestMethod]
    void testThatVisitWithYieldStmtAsParameterCallDefaultAction() {
        Node node = visitor.visit(mock(YieldStmt.class), argument);
        assertNodeVisitDefaultAction(node);
    }

    /**
     * Assert that at the default methods was called only once and with the same argument.
     */
    void assertNodeVisitDefaultAction(Node node) {
        // Check if the default method was only called once
        verify(visitor, times(1)).defaultAction(same(node), argumentCaptor.capture());
        // Check if the original argument was passed to the default method
        assertSame(argument, argumentCaptor.getValue());
    }

}
