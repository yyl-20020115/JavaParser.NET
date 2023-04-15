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




class LexicalDifferenceCalculatorTest:AbstractLexicalPreservingTest {

    [TestMethod]
    void compilationUnitExampleOriginal() {
        considerCode("class A {}");
        CsmElement element = ConcreteSyntaxModel.forClass(cu.getClass());
        LexicalDifferenceCalculator.CalculatedSyntaxModel csmOriginal = new LexicalDifferenceCalculator().calculatedSyntaxModelForNode(element, cu);
        assertEquals(2, csmOriginal.elements.size());
        assertEquals(new CsmChild(cu.getType(0)), csmOriginal.elements.get(0));
        assertEquals(new CsmToken(eolTokenKind()), csmOriginal.elements.get(1));
    }

    [TestMethod]
    void compilationUnitExampleWithPackageSet() {
        considerCode("class A {}");
        CsmElement element = ConcreteSyntaxModel.forClass(cu.getClass());
        PackageDeclaration packageDeclaration = new PackageDeclaration(new Name(new Name("foo"), "bar"));
        LexicalDifferenceCalculator.CalculatedSyntaxModel csmChanged = new LexicalDifferenceCalculator().calculatedSyntaxModelAfterPropertyChange(element, cu, ObservableProperty.PACKAGE_DECLARATION, null, packageDeclaration);
        assertEquals(3, csmChanged.elements.size());
        assertEquals(new CsmChild(packageDeclaration), csmChanged.elements.get(0));
        assertEquals(new CsmChild(cu.getType(0)), csmChanged.elements.get(1));
        assertEquals(new CsmToken(eolTokenKind()), csmChanged.elements.get(2));
    }

    [TestMethod]
    void annotationDeclarationModifiersExampleOriginal(){
        considerExample("AnnotationDeclaration_Example1_original");
        AnnotationDeclaration annotationDeclaration = (AnnotationDeclaration)cu.getType(0);
        CsmElement element = ConcreteSyntaxModel.forClass(annotationDeclaration.getClass());
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelForNode(element, annotationDeclaration);
        csm.removeIndentationElements();
        int i = 0;
        assertEquals(new CsmToken(GeneratedJavaParserConstants.AT), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.INTERFACE), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getName()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.LBRACE), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(0)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(1)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(2)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(3)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(4)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(5)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.RBRACE), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void annotationDeclarationModifiersExampleModified(){
        considerExample("AnnotationDeclaration_Example1_original");
        AnnotationDeclaration annotationDeclaration = (AnnotationDeclaration)cu.getType(0);
        CsmElement element = ConcreteSyntaxModel.forClass(annotationDeclaration.getClass());
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelAfterPropertyChange(element, annotationDeclaration, ObservableProperty.MODIFIERS, new NodeList<>(), createModifierList(PUBLIC));
        csm.removeIndentationElements();
        int i = 0;
        assertEquals(new CsmChild(Modifier.publicModifier()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.AT), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.INTERFACE), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getName()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.LBRACE), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(0)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(1)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(2)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(3)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(4)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(5)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.RBRACE), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void annotationDeclarationNameExampleModified(){
        considerExample("AnnotationDeclaration_Example1_original");
        AnnotationDeclaration annotationDeclaration = (AnnotationDeclaration)cu.getType(0);
        CsmElement element = ConcreteSyntaxModel.forClass(annotationDeclaration.getClass());
        SimpleName newName = new SimpleName("NewName");
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelAfterPropertyChange(element, annotationDeclaration, ObservableProperty.NAME,
                annotationDeclaration.getName(), newName);
        csm.removeIndentationElements();
        int i = 0;
        assertEquals(new CsmToken(GeneratedJavaParserConstants.AT), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.INTERFACE), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(newName), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.LBRACE), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(0)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(1)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(2)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(3)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(4)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(5)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.RBRACE), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void annotationDeclarationJavadocExampleOriginal(){
        considerExample("AnnotationDeclaration_Example3_original");
        AnnotationDeclaration annotationDeclaration = (AnnotationDeclaration)cu.getType(0);
        CsmElement element = ConcreteSyntaxModel.forClass(annotationDeclaration.getClass());
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelForNode(element, annotationDeclaration);
        csm.removeIndentationElements();
        int i = 0;
        assertEquals(new CsmChild(Modifier.publicModifier()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.AT), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.INTERFACE), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getName()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.LBRACE), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(0)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(1)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(2)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(3)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(4)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(5)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.RBRACE), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void annotationDeclarationJavadocExampleAddingJavadoc(){
        considerExample("AnnotationDeclaration_Example3_original");
        AnnotationDeclaration annotationDeclaration = (AnnotationDeclaration)cu.getType(0);
        CsmElement element = ConcreteSyntaxModel.forClass(annotationDeclaration.getClass());
        JavadocComment comment = new JavadocComment("Cool this annotation!");
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelAfterPropertyChange(element, annotationDeclaration, ObservableProperty.COMMENT, null, comment);
        csm.removeIndentationElements();
        int i = 0;
        assertEquals(new CsmChild(Modifier.publicModifier()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.AT), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.INTERFACE), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getName()), csm.elements.get(i++));
        assertEquals(new CsmToken(spaceTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.LBRACE), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(0)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(1)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(2)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(3)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(4)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmChild(annotationDeclaration.getMember(5)), csm.elements.get(i++));
        assertEquals(new CsmToken(eolTokenKind()), csm.elements.get(i++));
        assertEquals(new CsmToken(GeneratedJavaParserConstants.RBRACE), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void simpleEnumConstantDeclaration() {
        EnumConstantDeclaration ecd = considerEcd("A");
        LexicalDifferenceCalculator.CalculatedSyntaxModel csm = new LexicalDifferenceCalculator().calculatedSyntaxModelForNode(ecd);

        int i = 0;
        assertEquals(new CsmChild(ecd.getName()), csm.elements.get(i++));
        assertEquals(i, csm.elements.size());
    }

    [TestMethod]
    void csmModelAfterAddingStatementToEmptyBlock(){
        LexicalDifferenceCalculator ldc = new LexicalDifferenceCalculator();
        considerExample("ASimpleClassWithMoreFormatting_step3");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        Statement assignStatement = new ExpressionStmt(
                new AssignExpr(
                        new FieldAccessExpr(new ThisExpr(),"aField"),
                        new NameExpr("aField"),
                        AssignExpr.Operator.ASSIGN
                ));
        LexicalDifferenceCalculator.CalculatedSyntaxModel calculatedSyntaxModel =
                ldc.calculatedSyntaxModelAfterListAddition(
                        ConcreteSyntaxModel.forClass(BlockStmt.class),
                        ObservableProperty.STATEMENTS,
                        setter.getBody().get().getStatements(),
                        0,
                        assignStatement);
        int index = 0;
        assertEquals(CsmElement.token(GeneratedJavaParserConstants.LBRACE), calculatedSyntaxModel.elements.get(index++));
        assertEquals(CsmElement.newline(), calculatedSyntaxModel.elements.get(index++));
        assertEquals(CsmElement.indent(), calculatedSyntaxModel.elements.get(index++));
        assertTrue(isChild(calculatedSyntaxModel.elements.get(index++), ExpressionStmt.class));
        assertEquals(CsmElement.newline(), calculatedSyntaxModel.elements.get(index++));
        assertEquals(CsmElement.unindent(), calculatedSyntaxModel.elements.get(index++));
        assertEquals(CsmElement.token(GeneratedJavaParserConstants.RBRACE), calculatedSyntaxModel.elements.get(index++));
        assertEquals(index, calculatedSyntaxModel.elements.size());
    }

    [TestMethod]
    void differenceAfterddingStatementToEmptyBlock(){
        LexicalDifferenceCalculator ldc = new LexicalDifferenceCalculator();
        considerExample("ASimpleClassWithMoreFormatting_step3");

        MethodDeclaration setter = cu.getClassByName("MyRenamedClass").get()
                .getMethodsByName("setAField").get(0);
        Statement assignStatement = new ExpressionStmt(
                new AssignExpr(
                        new FieldAccessExpr(new ThisExpr(),"aField"),
                        new NameExpr("aField"),
                        AssignExpr.Operator.ASSIGN
                ));
        List<DifferenceElement> differenceElements = ldc.calculateListAdditionDifference(
                ObservableProperty.STATEMENTS,
                setter.getBody().get().getStatements(),
                0,
                assignStatement);
        int index = 0;
        assertEquals(DifferenceElement.kept(CsmElement.token(GeneratedJavaParserConstants.LBRACE)), differenceElements.get(index++));
        assertEquals(DifferenceElement.kept(CsmElement.newline()), differenceElements.get(index++));
        assertEquals(DifferenceElement.added(CsmElement.indent()), differenceElements.get(index++));
        assertTrue(isAddedChild(differenceElements.get(index++), ExpressionStmt.class));
        CsmElement cmsElement = differenceElements.get(index++).getElement();
        // all end of line tokens are not equal so it's safer to check if it's a token and then a end of line token
        assertTrue(CsmToken.class.isAssignableFrom(cmsElement.getClass()) && ((CsmToken)cmsElement).isNewLine());
        assertEquals(DifferenceElement.added(CsmElement.unindent()), differenceElements.get(index++));
        assertEquals(DifferenceElement.kept(CsmElement.token(GeneratedJavaParserConstants.RBRACE)), differenceElements.get(index++));
        assertEquals(index, differenceElements.size());
    }

    private bool isAddedChild(DifferenceElement element, Class<?:Node> childClass) {
        return element.isAdded() && isChild(element.getElement(), childClass);
    }

    private bool isChild(CsmElement element, Class<?:Node> childClass) {
        return element is CsmChild && childClass.isInstance(((CsmChild)element).getChild());
    }

    protected EnumConstantDeclaration considerEcd(string code) {
        considerCode("enum A { " + code + " }");
        return ((EnumDeclaration)cu.getType(0)).getEntries().get(0);
    }
}
