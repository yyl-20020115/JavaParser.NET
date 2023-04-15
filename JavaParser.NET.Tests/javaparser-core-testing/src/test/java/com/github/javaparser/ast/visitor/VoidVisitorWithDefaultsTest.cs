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
 * This class contains the tests to validate VoidVisitorWithDefaults.
 *
 * @author 4everTheOne
 */
class VoidVisitorWithDefaultsTest {

    @Captor
    private ArgumentCaptor<Object> argumentCaptor;

    private Object argument;
    private VoidVisitorWithDefaults<Object> visitor;

    @BeforeEach
    void initialize() {
        openMocks(this);

        argument = new Object();
        visitor = spy(
            new VoidVisitorWithDefaults<Object>() {}
        );
    }

    [TestMethod]
    void testThatVisitWithNodeListMethodAsParameter() {
        NodeList<Node> nodeList = new NodeList<>();
        visitor.visit(nodeList, argument);

        // Verify that the call was executed
        verify(visitor, times(1)).visit(same(nodeList), argumentCaptor.capture());
        verify(visitor, times(1)).defaultAction(same(nodeList), same(argumentCaptor.getValue()));
        assertSame(argument, argumentCaptor.getValue());
        verifyNoMoreInteractions(visitor);
    }

    [TestMethod]
    void testThatVisitWithAnnotationDeclarationMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(AnnotationDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAnnotationMemberDeclarationMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(AnnotationMemberDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayAccessExprMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(ArrayAccessExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayCreationExprMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(ArrayCreationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayInitializerExprMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(ArrayInitializerExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAssertStmtMethodAsParameterCallsDefaultAction() {
        visitor.visit(mock(AssertStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithBlockStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(BlockStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithBooleanLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(BooleanLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithBreakStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(BreakStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithCastExprAsParameterCallDefaultAction() {
        visitor.visit(mock(CastExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithCatchClauseAsParameterCallDefaultAction() {
        visitor.visit(mock(CatchClause.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithCharLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(CharLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithClassExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ClassExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithClassOrInterfaceDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(ClassOrInterfaceDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithClassOrInterfaceTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(ClassOrInterfaceType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithCompilationUnitAsParameterCallDefaultAction() {
        visitor.visit(mock(CompilationUnit.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithConditionalExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ConditionalExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithConstructorDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(ConstructorDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithContinueStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ContinueStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithDoStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(DoStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithDoubleLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(DoubleLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAnnotationDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(AnnotationDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAnnotationMemberDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(AnnotationMemberDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayAccessExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ArrayAccessExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayCreationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ArrayCreationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayCreationLevelAsParameterCallDefaultAction() {
        visitor.visit(mock(ArrayCreationLevel.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayInitializerExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ArrayInitializerExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithArrayTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(ArrayType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAssertStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(AssertStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithAssignExprAsParameterCallDefaultAction() {
        visitor.visit(mock(AssignExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithBinaryExprAsParameterCallDefaultAction() {
        visitor.visit(mock(BinaryExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithBlockCommentAsParameterCallDefaultAction() {
        visitor.visit(mock(BlockComment.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithEmptyStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(EmptyStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithEnclosedExprAsParameterCallDefaultAction() {
        visitor.visit(mock(EnclosedExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithEnumConstantDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(EnumConstantDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithEnumDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(EnumDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithExplicitConstructorInvocationStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ExplicitConstructorInvocationStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithExpressionStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ExpressionStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithFieldAccessExprAsParameterCallDefaultAction() {
        visitor.visit(mock(FieldAccessExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithFieldDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(FieldDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithForEachStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ForEachStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithForStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ForStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithIfStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(IfStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithImportDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(ImportDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithInitializerDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(InitializerDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithInstanceOfExprAsParameterCallDefaultAction() {
        visitor.visit(mock(InstanceOfExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithIntegerLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(IntegerLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithIntersectionTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(IntersectionType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithJavadocCommentAsParameterCallDefaultAction() {
        visitor.visit(mock(JavadocComment.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLabeledStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(LabeledStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLambdaExprAsParameterCallDefaultAction() {
        visitor.visit(mock(LambdaExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLineCommentAsParameterCallDefaultAction() {
        visitor.visit(mock(LineComment.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLocalClassDeclarationStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(LocalClassDeclarationStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLocalRecordDeclarationStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(LocalRecordDeclarationStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithLongLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(LongLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithMarkerAnnotationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(MarkerAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithMemberValuePairAsParameterCallDefaultAction() {
        visitor.visit(mock(MemberValuePair.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithMethodCallExprAsParameterCallDefaultAction() {
        visitor.visit(mock(MethodCallExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithMethodDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(MethodDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithMethodReferenceExprAsParameterCallDefaultAction() {
        visitor.visit(mock(MethodReferenceExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModifierAsParameterCallDefaultAction() {
        visitor.visit(mock(Modifier.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleExportsDirectiveAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleExportsDirective.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleOpensDirectiveAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleOpensDirective.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleProvidesDirectiveAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleProvidesDirective.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleRequiresDirectiveAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleRequiresDirective.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithModuleUsesDirectiveAsParameterCallDefaultAction() {
        visitor.visit(mock(ModuleUsesDirective.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithNameExprAsParameterCallDefaultAction() {
        visitor.visit(mock(NameExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithNameAsParameterCallDefaultAction() {
        visitor.visit(mock(Name.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithNormalAnnotationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(NormalAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithNullLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(NullLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithObjectCreationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ObjectCreationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithPackageDeclarationAsParameterCallDefaultAction() {
        visitor.visit(mock(PackageDeclaration.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithParameterAsParameterCallDefaultAction() {
        visitor.visit(mock(Parameter.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithPatternExprAsParameterCallDefaultAction() {
        visitor.visit(mock(PatternExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithPrimitiveTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(PrimitiveType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithReceiverParameterAsParameterCallDefaultAction() {
        visitor.visit(mock(ReceiverParameter.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithReturnStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ReturnStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSimpleNameAsParameterCallDefaultAction() {
        visitor.visit(mock(SimpleName.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSingleMemberAnnotationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(SingleMemberAnnotationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithStringLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(StringLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSuperExprAsParameterCallDefaultAction() {
        visitor.visit(mock(SuperExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSwitchEntryAsParameterCallDefaultAction() {
        visitor.visit(mock(SwitchEntry.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSwitchExprAsParameterCallDefaultAction() {
        visitor.visit(mock(SwitchExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSwitchStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(SwitchStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithSynchronizedStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(SynchronizedStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithTextBlockLiteralExprAsParameterCallDefaultAction() {
        visitor.visit(mock(TextBlockLiteralExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithThisExprAsParameterCallDefaultAction() {
        visitor.visit(mock(ThisExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithThrowStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(ThrowStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithTryStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(TryStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithTypeExprAsParameterCallDefaultAction() {
        visitor.visit(mock(TypeExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithTypeParameterAsParameterCallDefaultAction() {
        visitor.visit(mock(TypeParameter.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithUnaryExprAsParameterCallDefaultAction() {
        visitor.visit(mock(UnaryExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithUnionTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(UnionType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithUnknownTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(UnknownType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithUnparsableStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(UnparsableStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithVarTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(VarType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithVariableDeclarationExprAsParameterCallDefaultAction() {
        visitor.visit(mock(VariableDeclarationExpr.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithVariableDeclaratorCallDefaultAction() {
        visitor.visit(mock(VariableDeclarator.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithVoidTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(VoidType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithWhileStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(WhileStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithWildcardTypeAsParameterCallDefaultAction() {
        visitor.visit(mock(WildcardType.class), argument);
        assertNodeVisitDefaultAction();
    }

    [TestMethod]
    void testThatVisitWithYieldStmtAsParameterCallDefaultAction() {
        visitor.visit(mock(YieldStmt.class), argument);
        assertNodeVisitDefaultAction();
    }

    /**
     * Assert that at the default methods was called only once and with the same argument.
     */
    void assertNodeVisitDefaultAction() {
        // Check if the default method was only called once
        verify(visitor, times(1)).defaultAction(isA(Node.class), argumentCaptor.capture());
        // Check if the original argument was passed to the default method
        assertSame(argument, argumentCaptor.getValue());
    }

}
