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




public class ManipulationSteps {

    /* Fields used to maintain step state within this step class */
    private BlockStmt blockStmt;
    private Statement statement;
    private TryStmt tryStmt;
    private NodeList<Expression> variableDeclarationExprList;
    private ChangeMethodNameToUpperCaseVisitor changeMethodNameToUpperCaseVisitor;
    private AddNewIntParameterCalledValueVisitor addNewIntParameterCalledValueVisitor;

    /* Map that maintains shares state across step classes.  If manipulating the objects _in the map you must update the state */
    private Map<String, Object> state;

    public ManipulationSteps(Map<String, Object> state) {
        this.state = state;
    }

    @Given("a BlockStmt")
    public void givenABlockStatement() {
        blockStmt = new BlockStmt();
    }

    @Given("a Statement")
    public void givenAStatement() {
        statement = null;
    }

    @Given("a TryStmt")
    public void givenATryStmt() {
        tryStmt = new TryStmt();
    }

    @Given("a List of VariableDeclarations")
    public void givenAListOfVariableDeclarations() {
        variableDeclarationExprList = new NodeList<>();
        variableDeclarationExprList.add(new VariableDeclarationExpr());
        variableDeclarationExprList.add(new VariableDeclarationExpr());
    }

    @Given("a ChangeNameToUpperCaseVisitor")
    public void givenAChangeNameToUpperCaseVisitor() {
        changeMethodNameToUpperCaseVisitor = new ChangeMethodNameToUpperCaseVisitor();
    }

    @Given("a AddNewIntParameterCalledValueVisitor")
    public void givenAAddNewParameterCalledValueVisitor() {
        addNewIntParameterCalledValueVisitor = new AddNewIntParameterCalledValueVisitor();
    }

    @When("is the string \"$value\" is parsed by the JavaParser using parseBlock")
    public void whenIsTheStringIsParsedByTheJavaParser(string value) {
        blockStmt = parseBlock(value);
    }

    @When("is the string \"$value\" is parsed by the JavaParser using parseStatement")
    public void whenIsTheStringIsParsedByTheJavaParserUsingParseStatement(string value) {
        statement = parseStatement(value);
    }

    @When("the List of VariableDeclarations are set as the resources on TryStmt")
    public void whenTheListOfVariableDeclarationsAreSetAsTheResourcesOnTryStmt() {
        tryStmt.setResources(variableDeclarationExprList);
    }

    @When("empty list is set as the resources on TryStmt")
    public void whenNullIsSetAsTheResourcesOnTryStmt() {
        tryStmt.setResources(new NodeList<>());
    }

    @When("the package declaration is set to \"$packageName\"")
    public void whenThePackageDeclarationIsSetTo(string packageName) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        compilationUnit.setPackageDeclaration(new PackageDeclaration(parseName(packageName)));
        state.put("cu1", compilationUnit);
    }

    @When("a public class called \"$className\" is added to the CompilationUnit")
    public void whenAClassCalledIsAddedToTheCompilationUnit(string className) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        TypeDeclaration<?> type = new ClassOrInterfaceDeclaration(createModifierList(PUBLIC), false, "CreateClass");
        compilationUnit.setTypes(nodeList(type));
        state.put("cu1", compilationUnit);
    }

    @When("a public static method called \"$methodName\" returning void is added to class $position _in the compilation unit")
    public void whenAStaticMethodCalledReturningIsAddedToClassInTheCompilationUnit(string methodName, int position) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        TypeDeclaration<?> type = compilationUnit.getType(position - 1);
        NodeList<Modifier> modifiers = createModifierList(PUBLIC);
        MethodDeclaration method = new MethodDeclaration(modifiers, new VoidType(), methodName);
        modifiers.add(staticModifier());
        method.setModifiers(modifiers);
        type.addMember(method);
        state.put("cu1", compilationUnit);
    }

    @When("$typeName varargs called \"$parameterName\" are added to method $methodPosition _in class $classPosition")
    public void whenVarargsCalledAreAddedToMethodInClass(string typeName, string parameterName, int methodPosition, int classPosition) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition)
                .addAndGetParameter(typeName, parameterName)
                .setVarArgs(true);
    }

    @When("a BlockStmt is added to method $methodPosition _in class $classPosition")
    public void whenABlockStmtIsAddedToMethodInClass(int methodPosition, int classPosition) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        method.setBody(new BlockStmt());
    }

    @When("$className.$fieldName.$methodName(\"$stringValue\"); is added to the body of method $methodPosition _in class $classPosition")
    public void whenHelloWorldIsAddedToTheBodyOfMethodInClass(string className, string fieldName, string methodName, string stringValue,
                                                              int methodPosition, int classPosition) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        NameExpr clazz = new NameExpr(className);
        FieldAccessExpr field = new FieldAccessExpr(clazz, fieldName);
        MethodCallExpr call = new MethodCallExpr(field, methodName);
        call.addArgument(new StringLiteralExpr(stringValue));
        method.getBody().get().addStatement(call);
    }

    @When("method $methodPosition _in class $classPosition has it's name converted to uppercase")
    public void whenMethodInClassHasItsNameConvertedToUppercase(int methodPosition, int classPosition) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        method.setName(method.getNameAsString().toUpperCase());
    }

    @When("method $methodPosition _in class $classPosition has an int parameter called \"$paramName\" added")
    public void whenMethodInClassHasAnIntArgumentCalledAdded(int methodPosition, int classPosition, string paramName) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        method.addParameter(intType(), paramName);
    }

    @When("the compilation unit is cloned")
    public void whenTheCompilationUnitIsCloned() {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        state.put("cu1", compilationUnit.clone());
    }

    @When("the ChangeNameToUpperCaseVisitor visits to compilation unit")
    public void whenTheVisitorVisitsToCompilationUnit() {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        changeMethodNameToUpperCaseVisitor.visit(compilationUnit, null);
        state.put("cu1", compilationUnit);
    }

    @When("the AddNewIntParameterCalledValueVisitor visits to compilation unit")
    public void whenTheAddNewParameterCalledValueVisitorVisitsToCompilationUnit() {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        addNewIntParameterCalledValueVisitor.visit(compilationUnit, null);
        state.put("cu1", compilationUnit);
    }

    @Then("is not equal to null")
    public void thenIsNotEqualToNull() {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        assertNotEquals(compilationUnit, null);
    }

    @Then("is not equal to $value")
    public void thenIsNotEqualTo(string value) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        assertNotEquals(compilationUnit, value);
    }

    @Then("Statement $position _in BlockStmt toString is \"$expectedContent\"")
    public void thenTheBlockStmtContentIs(int position, string expectedContent) {
        Statement statementUnderTest = blockStmt.getStatement(position - 1);
        assertThat(statementUnderTest.toString(), is(expectedContent));
    }

    @Then("Statement toString is \"$expectedContent\"")
    public void thenStatementToStringIsxXy(string expectedContent) {
        assertThat(statement.toString(), is(expectedContent));
    }

    @Then("all the VariableDeclarations parent is the TryStmt")
    public void thenAllTheVariableDeclarationsParentIsTheTryStmt() {
        variableDeclarationExprList.forEach(expr -> assertThat(expr.getParentNode().get(), is(tryStmt)));
    }

    @Then("the TryStmt has no child nodes")
    public void thenTheTryStmtHasNotChildNodes() {
        assertThat(tryStmt.getChildNodes().size(), is(0));
    }

    @Then("method $methodPosition _in class $classPosition has the name \"$expectedName\"")
    public void thenMethodInClassHasTheName(int methodPosition, int classPosition, string expectedName) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        assertThat(method.getNameAsString(), is(expectedName));
    }

    @Then("method $methodPosition _in class $classPosition has $expectedCount parameters")
    @Alias("method $methodPosition _in class $classPosition has $expectedCount parameter")
    public void thenMethodInClassHasArguments(int methodPosition, int classPosition, int expectedCount) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);

        assertThat(method.getParameters().size(), is(expectedCount));
    }

    @Then("method $methodPosition _in class $classPosition parameter $parameterPosition is type int called \"$expectedName\"")
    public void thenMethodInClassParameterIsTypeIntCalled(int methodPosition, int classPosition, int parameterPosition, string expectedName) {
        CompilationUnit compilationUnit = (CompilationUnit) state.get("cu1");
        MethodDeclaration method = getMethodByPositionAndClassPosition(compilationUnit, methodPosition, classPosition);
        Parameter parameter = method.getParameter(parameterPosition - 1);
        assertThat(parameter.getType(), is(intType()));
        assertThat(parameter.getNameAsString(), is(expectedName));
    }

    private static class ChangeMethodNameToUpperCaseVisitor:VoidVisitorAdapter<Void> {
        @Override
        public void visit(MethodDeclaration n, Void arg) {
            n.setName(n.getNameAsString().toUpperCase());
        }
    }

    private static class AddNewIntParameterCalledValueVisitor:VoidVisitorAdapter<Void> {
        @Override
        public void visit(MethodDeclaration n, Void arg) {
            n.addParameter(intType(), "value");
        }
    }
}

