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

namespace com.github.javaparser.symbolsolver.reflectionmodel;




@interface OuterAnnotation {
  @interface InnerAnnotation {}
}

@interface WithValue {
  string value();
}

@interface WithField {
  int FIELD_DECLARATION = 0;
}

@Inherited
@interface InheritedAnnotation {}

class ReflectionAnnotationDeclarationTest {
  private TypeSolver typeSolver = new ReflectionTypeSolver(false);

  [TestMethod]
  void isClass() {
    ReflectionAnnotationDeclaration
        annotation = (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.OuterAnnotation");
    assertFalse(annotation.isClass());
  }

  [TestMethod]
  void innerIsClass() {
    ReflectionAnnotationDeclaration
        annotation = (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.OuterAnnotation.InnerAnnotation");
    assertFalse(annotation.isClass());
  }

  [TestMethod]
  void getInternalTypes() {
    ReflectionAnnotationDeclaration annotation =
        (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.OuterAnnotation");
    assertEquals(Collections.singleton("InnerAnnotation"),
        annotation.internalTypes().stream().map(ResolvedDeclaration::getName).collect(Collectors.toSet()));
  }

  [TestMethod]
  void solveMethodForAnnotationWithValue() {
    ReflectionAnnotationDeclaration annotation =
        (ReflectionAnnotationDeclaration) typeSolver.solveType(WithValue.class.getCanonicalName());
    /*final*/SymbolReference<ResolvedMethodDeclaration> symbolReference =
        annotation.solveMethod("value", Collections.emptyList(), false);
    assertEquals("value", symbolReference.getCorrespondingDeclaration().getName());
  }

  [TestMethod]
  void getAllFields_shouldReturnTheCorrectFields() {
    ReflectionAnnotationDeclaration annotation =
            (ReflectionAnnotationDeclaration) typeSolver.solveType(
                    "com.github.javaparser.symbolsolver.reflectionmodel.WithField");
    assertEquals(Collections.singleton("FIELD_DECLARATION"),
            annotation.getAllFields().stream().map(ResolvedDeclaration::getName).collect(Collectors.toSet()));
  }
  
  [TestMethod]
  void getClassName_shouldReturnCorrectValue() {
    ReflectionAnnotationDeclaration annotation =
        (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.WithField");
    assertEquals("WithField", annotation.getClassName());
  }
  
  [TestMethod]
  void isAnnotationNotInheritable() {
    ReflectionAnnotationDeclaration
        annotation = (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.OuterAnnotation");
    assertFalse(annotation.isInheritable());
  }
  
  [TestMethod]
  void isAnnotationInheritable() {
    ReflectionAnnotationDeclaration
        annotation = (ReflectionAnnotationDeclaration) typeSolver.solveType(
            "com.github.javaparser.symbolsolver.reflectionmodel.InheritedAnnotation");
    assertTrue(annotation.isInheritable());
  }

}
