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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;



class JavaParserAnonymousClassDeclarationTest:AbstractResolutionTest {

  [TestMethod]
  void anonymousClassAsMethodArgument() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar1");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "of").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
        JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertThat(methodUsage.getDeclaration().getQualifiedSignature(),
            is("AnonymousClassDeclarations.ParDo.of(AnonymousClassDeclarations.DoFn<I, O>)"));
    assertThat(methodUsage.getQualifiedSignature(),
               is("AnonymousClassDeclarations.ParDo.of(AnonymousClassDeclarations.DoFn<java.lang.Integer, java.lang.Long>)"));
  }

  [TestMethod]
  void callingSuperClassInnerClassMethod() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar2");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "innerClassMethod").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
        JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertThat(methodUsage.getQualifiedSignature(),
               is("AnonymousClassDeclarations.DoFn.ProcessContext.innerClassMethod()"));
  }

  [TestMethod]
  void callingAnonymousClassInnerMethod() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar3");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "callAnnonClassInnerMethod").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
        JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertTrue(methodUsage.getQualifiedSignature().startsWith("AnonymousClassDeclarations"));
    assertTrue(methodUsage.getQualifiedSignature().contains("Anonymous-"));
    assertTrue(methodUsage.getQualifiedSignature().endsWith("callAnnonClassInnerMethod()"));
  }

  [TestMethod]
  void usingAnonymousSuperClassInnerType() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar4");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "toString").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
        JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertThat(methodUsage.getQualifiedSignature(), is("java.lang.Enum.toString()"));
  }

  [TestMethod]
  void usingAnonymousClassInnerType() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar5");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "toString").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
        JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertThat(methodUsage.getQualifiedSignature(), is("java.lang.Enum.toString()"));
  }

  [TestMethod]
  void callingScopedAnonymousClassInnerMethod() {
    CompilationUnit cu = parseSample("AnonymousClassDeclarations");
    ClassOrInterfaceDeclaration aClass = Navigator.demandClass(cu, "AnonymousClassDeclarations");
    MethodDeclaration method = Navigator.demandMethod(aClass, "fooBar6");
    MethodCallExpr methodCall = Navigator.findMethodCall(method, "innerClassMethod").get();

    CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
    combinedTypeSolver.add(new ReflectionTypeSolver());
    MethodUsage methodUsage =
            JavaParserFacade.get(combinedTypeSolver).solveMethodAsUsage(methodCall);

    assertThat(methodUsage.getQualifiedSignature(), is("AnonymousClassDeclarations.DoFn.ProcessContext.innerClassMethod()"));
  }
}
