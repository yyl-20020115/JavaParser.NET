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

namespace com.github.javaparser.symbolsolver.resolution.naming;




@SlowTest
class NameLogicTestingJss060Test:AbstractResolutionTest {

    private static /*final*/Path root = adaptPath("src/test/test_sourcecode/javasymbolsolver_0_6_0");
    private static /*final*/Path src = root.resolve("src");

    private void classifyRoles(string projectName, string className){
        Path sourceFile = src.resolve(projectName + "/" + className + ".java");
        CompilationUnit cu = parse(sourceFile);

        List<Node> names = new LinkedList<>();
        names.addAll(cu.findAll(Name.class).stream().filter(Name::isTopLevel).collect(Collectors.toList()));
        names.addAll(cu.findAll(SimpleName.class));
        names.forEach(n -> {
            NameRole role = NameLogic.classifyRole(n);
        });
    }

    private void classifyReferences(string projectName, string className){
        Path sourceFile = src.resolve(projectName + "/" + className + ".java");
        CompilationUnit cu = parse(sourceFile);

        List<Node> names = new LinkedList<>();
        names.addAll(cu.findAll(Name.class).stream().filter(Name::isTopLevel).collect(Collectors.toList()));
        names.addAll(cu.findAll(SimpleName.class));
        names.forEach(n -> {
            if (NameLogic.classifyRole(n) == NameRole.REFERENCE) {
                NameCategory nameCategory = NameLogic.syntacticClassificationAccordingToContext(n);
            }
        });
    }

    [TestMethod]
    void classifyRoleToFileToCoreSourceFileInfoExtractor(){
        classifyRoles("java-symbol-solver-core",
                "com/github/javaparser/symbolsolver/SourceFileInfoExtractor");
    }

    [TestMethod]
    void classifyRolesCoreCoreResolution(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/core/resolution/Context");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/core/resolution/ContextHelper");
    }

    [TestMethod]
    void classifyRolesCoreDeclarationsCommon(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/declarations/common/MethodDeclarationCommonLogic");
    }

    [TestMethod]
    void classifyRolesCoreJavaparserNavigator(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparser/Navigator");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodel(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/DefaultVisitorAdapter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/JavaParserFacade");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/JavaParserFactory");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/LambdaArgumentTypePlaceholder");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/TypeExtractor");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/UnsolvedSymbolException");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodelContexts(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AbstractJavaParserContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AbstractMethodLikeDeclarationContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AnonymousClassDeclarationContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/CatchClauseContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ClassOrInterfaceDeclarationContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/CompilationUnitContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ConstructorContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ContextHelper");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/EnumDeclarationContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/FieldAccessContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ForechStatementContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ForStatementContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/JavaParserTypeDeclarationAdapter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/LambdaExprContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/MethodCallExprContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/MethodContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/StatementContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/SwitchEntryContext");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/TryWithResourceContext");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodelJavaParserAnonymousClassDeclaration(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserAnonymousClassDeclaration");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodelJavaParserInterfaceDeclaration(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserInterfaceDeclaration");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodelDeclarations(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/DefaultConstructorDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/Helper");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserAnnotationDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserClassDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserConstructorDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserEnumConstantDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserEnumDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserFieldDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserMethodDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserParameterDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserSymbolDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeAdapter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeParameter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeVariableDeclaration");
    }

    [TestMethod]
    void classifyRolesCoreJavaparsermodelDeclarators(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/AbstractSymbolDeclarator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/FieldSymbolDeclarator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/NoSymbolDeclarator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/ParameterSymbolDeclarator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/VariableSymbolDeclarator");
    }

    [TestMethod]
    void classifyRolesCoreJavassistmodel(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistClassDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistConstructorDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistEnumDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistFactory");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistFieldDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistInterfaceDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistMethodDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistParameterDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistTypeDeclarationAdapter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistTypeParameter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistUtils");
    }

    [TestMethod]
    void classifyRolesCoreModelTypesystem(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/model/typesystem/LazyType");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/model/typesystem/ReferenceTypeImpl");
    }

    [TestMethod]
    void classifyRolesCoreReflectionmodel(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/MyObjectProvider");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionClassAdapter");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionClassDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionConstructorDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionEnumDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionFactory");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionFieldDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionInterfaceDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionMethodDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionMethodResolutionLogic");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionParameterDeclaration");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionTypeParameter");
    }

    [TestMethod]
    void classifyRolesCoreReflectionmodelComparators(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/ClassComparator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/MethodComparator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/ParameterComparator");
    }

    [TestMethod]
    void classifyRolesCoreResolution(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/ConstructorResolutionLogic");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/MethodResolutionLogic");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/SymbolDeclarator");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/SymbolSolver");
    }

    [TestMethod]
    void classifyRolesCoreResolutionTypesolvers(){
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/CombinedTypeSolver");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/JarTypeSolver");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/JavaParserTypeSolver");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/MemoryTypeSolver");
        classifyRoles("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/ReflectionTypeSolver");
    }

    [TestMethod]
    void classifyRolesLogic(){
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/AbstractClassDeclaration");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/AbstractTypeDeclaration");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/ConfilictingGenericTypesException");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/FunctionalInterfaceLogic");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/InferenceContext");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/InferenceVariableType");
        classifyRoles("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/ObjectProvider");
    }

    [TestMethod]
    void classifyRolesModelDeclarations(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/AccessLevel");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/AnnotationDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ClassDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ConstructorDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/Declaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/EnumDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/FieldDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/HasAccessLevel");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/InterfaceDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodAmbiguityException");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodLikeDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ParameterDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ReferenceTypeDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeParameterDeclaration");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeParametrizable");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ValueDeclaration");
    }

    [TestMethod]
    void classifyRolesModelMethodsMethodUsage(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/methods/MethodUsage");
    }

    [TestMethod]
    void classifyRolesModelResolution(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/SymbolReference");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/TypeSolver");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/UnsolvedSymbolException");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/Value");
    }

    [TestMethod]
    void classifyRolesModelTypesystemReferenceType(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/ReferenceType");
    }

    [TestMethod]
    void classifyRolesModelTypesystem(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/ArrayType");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/LambdaConstraintType");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/NullType");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/PrimitiveType");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/Type");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/TypeTransformer");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/TypeVariable");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/VoidType");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/Wildcard");
    }

    [TestMethod]
    void classifyRolesModelTypesystemParametrization(){
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParametersMap");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParameterValueProvider");
        classifyRoles("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParametrized");
    }

    [TestMethod]
    void classifyReferencesToFileToCoreSourceFileInfoExtractor(){
        classifyReferences("java-symbol-solver-core",
                "com/github/javaparser/symbolsolver/SourceFileInfoExtractor");
    }

    [TestMethod]
    void classifyReferencesCoreCoreResolution(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/core/resolution/Context");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/core/resolution/ContextHelper");
    }

    [TestMethod]
    void classifyReferencesCoreDeclarationsCommon(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/declarations/common/MethodDeclarationCommonLogic");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparserNavigator(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparser/Navigator");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodel(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/DefaultVisitorAdapter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/JavaParserFacade");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/JavaParserFactory");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/LambdaArgumentTypePlaceholder");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/TypeExtractor");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/UnsolvedSymbolException");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodelContexts(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AbstractJavaParserContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AbstractMethodLikeDeclarationContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/AnonymousClassDeclarationContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/CatchClauseContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ClassOrInterfaceDeclarationContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/CompilationUnitContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ConstructorContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ContextHelper");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/EnumDeclarationContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/FieldAccessContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ForechStatementContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/ForStatementContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/JavaParserTypeDeclarationAdapter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/LambdaExprContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/MethodCallExprContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/MethodContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/StatementContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/SwitchEntryContext");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/contexts/TryWithResourceContext");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodelJavaParserAnonymousClassDeclaration(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserAnonymousClassDeclaration");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodelJavaParserInterfaceDeclaration(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserInterfaceDeclaration");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodelDeclarations(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/DefaultConstructorDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/Helper");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserAnnotationDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserClassDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserConstructorDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserEnumConstantDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserEnumDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserFieldDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserMethodDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserParameterDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserSymbolDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeAdapter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeParameter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarations/JavaParserTypeVariableDeclaration");
    }

    [TestMethod]
    void classifyReferencesCoreJavaparsermodelDeclarators(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/AbstractSymbolDeclarator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/FieldSymbolDeclarator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/NoSymbolDeclarator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/ParameterSymbolDeclarator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javaparsermodel/declarators/VariableSymbolDeclarator");
    }

    [TestMethod]
    void classifyReferencesCoreJavassistmodel(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistClassDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistConstructorDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistEnumDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistFactory");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistFieldDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistInterfaceDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistMethodDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistParameterDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistTypeDeclarationAdapter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistTypeParameter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/javassistmodel/JavassistUtils");
    }

    [TestMethod]
    void classifyReferencesCoreModelTypesystem(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/model/typesystem/LazyType");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/model/typesystem/ReferenceTypeImpl");
    }

    [TestMethod]
    void classifyReferencesCoreReflectionmodel(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/MyObjectProvider");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionClassAdapter");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionClassDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionConstructorDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionEnumDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionFactory");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionFieldDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionInterfaceDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionMethodDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionMethodResolutionLogic");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionParameterDeclaration");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/ReflectionTypeParameter");
    }

    [TestMethod]
    void classifyReferencesCoreReflectionmodelComparators(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/ClassComparator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/MethodComparator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/reflectionmodel/comparators/ParameterComparator");
    }

    [TestMethod]
    void classifyReferencesCoreResolution(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/ConstructorResolutionLogic");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/MethodResolutionLogic");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/SymbolDeclarator");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/SymbolSolver");
    }

    [TestMethod]
    void classifyReferencesCoreResolutionTypesolvers(){
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/CombinedTypeSolver");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/JarTypeSolver");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/JavaParserTypeSolver");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/MemoryTypeSolver");
        classifyReferences("java-symbol-solver-core", "com/github/javaparser/symbolsolver/resolution/typesolvers/ReflectionTypeSolver");
    }

    [TestMethod]
    void classifyReferencesLogic(){
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/AbstractClassDeclaration");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/AbstractTypeDeclaration");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/ConfilictingGenericTypesException");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/FunctionalInterfaceLogic");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/InferenceContext");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/InferenceVariableType");
        classifyReferences("java-symbol-solver-logic", "com/github/javaparser/symbolsolver/logic/ObjectProvider");
    }

    [TestMethod]
    void classifyReferencesModelDeclarations(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/AccessLevel");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/AnnotationDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ClassDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ConstructorDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/Declaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/EnumDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/FieldDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/HasAccessLevel");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/InterfaceDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodAmbiguityException");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/MethodLikeDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ParameterDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ReferenceTypeDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeParameterDeclaration");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/TypeParametrizable");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/declarations/ValueDeclaration");
    }

    [TestMethod]
    void classifyReferencesModelMethodsMethodUsage(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/methods/MethodUsage");
    }

    [TestMethod]
    void classifyReferencesModelResolution(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/SymbolReference");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/TypeSolver");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/UnsolvedSymbolException");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/resolution/Value");
    }

    [TestMethod]
    void classifyReferencesModelTypesystemReferenceType(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/ReferenceType");
    }

    [TestMethod]
    void classifyReferencesModelTypesystem(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/ArrayType");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/LambdaConstraintType");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/NullType");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/PrimitiveType");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/Type");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/TypeTransformer");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/TypeVariable");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/VoidType");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/Wildcard");
    }

    [TestMethod]
    void classifyReferencesModelTypesystemParametrization(){
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParametersMap");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParameterValueProvider");
        classifyReferences("java-symbol-solver-model", "com/github/javaparser/symbolsolver/model/typesystem/parametrization/TypeParametrized");
    }
    
}
