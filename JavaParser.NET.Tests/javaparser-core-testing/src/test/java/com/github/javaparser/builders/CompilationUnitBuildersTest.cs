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

namespace com.github.javaparser.builders;




class CompilationUnitBuildersTest {
  private /*final*/CompilationUnit cu = new CompilationUnit();

  [TestMethod]
  void testAddImport() {
    // duplicate imports
    cu.addImport(Map.class);
    cu.addImport(Map.class);
    cu.addImport(Map.class.getCanonicalName());
    cu.addImport(List.class);
    assertEquals(2, cu.getImports().size());

    cu.addImport(com.github.javaparser.StaticJavaParser.class.getCanonicalName() + ".parseImport", true, false);
    assertEquals(3, cu.getImports().size());

    assertEquals("import " + Map.class.getCanonicalName() + ";" + SYSTEM_EOL, cu.getImport(0).toString());
    assertEquals("import " + List.class.getCanonicalName() + ";" + SYSTEM_EOL, cu.getImport(1).toString());
    assertEquals("import static " + com.github.javaparser.StaticJavaParser.class.getCanonicalName() + ".parseImport;" + SYSTEM_EOL,
        cu.getImport(2).toString());
  }

  public class $tartsWith$ {
  }

  [TestMethod]
  public void test$ImportStarts() {
    cu.addImport($tartsWith$.class);
    cu.addImport("my.$tartsWith$");
    assertEquals(2, cu.getImports().size());
    assertEquals("import " + $tartsWith$.class.getCanonicalName() + ";" + SYSTEM_EOL, cu.getImport(0).toString());
    assertEquals("import my.$tartsWith$;" + SYSTEM_EOL, cu.getImport(1).toString());
  }

  public class F$F {
  }

  [TestMethod]
  public void test$Import() {
    cu.addImport(F$F.class);
    cu.addImport("my.F$F");
    // doesnt fail, but imports class "F.F"
    assertEquals(2, cu.getImports().size());
    assertEquals("import " + F$F.class.getCanonicalName() + ";" + SYSTEM_EOL, cu.getImport(0).toString());
    assertEquals("import my.F$F;" + SYSTEM_EOL, cu.getImport(1).toString());
  }

  [TestMethod]
  void ignoreJavaLangImports() {
    cu.addImport("java.lang.Long");
    cu.addImport("java.lang.*");
    cu.addImport(String.class);
    assertEquals(0, cu.getImports().size());
  }

  [TestMethod]
  void ignoreImportsWithinSamePackage() {
    cu.setPackageDeclaration(new PackageDeclaration(new Name(new Name("one"), "two")));
    cu.addImport("one.two.IgnoreImportWithinSamePackage");
    assertEquals(0, cu.getImports().size());
    cu.addImport("one.two.three.DoNotIgnoreImportWithinSubPackage");
    assertEquals(1, cu.getImports().size());
    assertEquals("import one.two.three.DoNotIgnoreImportWithinSubPackage;" + SYSTEM_EOL, cu.getImport(0).toString());
  }

  [TestMethod]
  void throwIllegalArgumentExceptionOnImportingAnonymousClass() {
    assertThrows(ArgumentException.class, () -> cu.addImport(new Comparator<Long>() {

      //@Override
      public int compare(Long o1, Long o2) {
        return o1.compareTo(o2);
      }
    }.getClass()));
  }

  [TestMethod]
  void throwIllegalArgumentExceptionOnImportingLocalClass() {
    class LocalClass implements Comparator<Long> {

      //@Override
      public int compare(Long o1, Long o2) {
        return o1.compareTo(o2);
      }
    }
    Type localClass = LocalClass.class;
    assertThrows(ArgumentException.class, () -> cu.addImport(localClass));
  }

  [TestMethod]
  void ignoreImportsOfDefaultPackageClasses() {
    cu.addImport("MyImport");
    assertEquals(0, cu.getImports().size());
  }

  [TestMethod]
  void duplicateByAsterisk() {
    // check asterisk imports
    cu.addImport("my", false, true);
    cu.addImport("my.Import");
    cu.addImport("my.AnotherImport");
    cu.addImport("my.other.Import");
    assertEquals(2, cu.getImports().size());
    assertEquals("import my.*;" + SYSTEM_EOL, cu.getImport(0).toString());
    assertEquals("import my.other.Import;" + SYSTEM_EOL, cu.getImport(1).toString());
    cu.addImport("my.other.*");
    assertEquals(2, cu.getImports().size());
    assertEquals("import my.*;" + SYSTEM_EOL, cu.getImport(0).toString());
    assertEquals("import my.other.*;" + SYSTEM_EOL, cu.getImport(1).toString());
  }

  [TestMethod]
  void typesInTheJavaLangPackageDoNotNeedExplicitImports() {
    cu.addImport(String.class);
    assertEquals(0, cu.getImports().size());
  }

  [TestMethod]
  void typesInSubPackagesOfTheJavaLangPackageRequireExplicitImports() {
    cu.addImport(ElementType.class);
    assertEquals(1, cu.getImports().size());
    assertEquals("import java.lang.annotation.ElementType;" + SYSTEM_EOL, cu.getImport(0).toString());
  }

  [TestMethod]
  void doNotAddDuplicateImportsByClass() {
    cu.addImport(Map.class);
    cu.addImport(Map.class);
    assertEquals(1, cu.getImports().size());
  }

  [TestMethod]
  void doNotAddDuplicateImportsByString() {
    cu.addImport(Map.class);
    cu.addImport("java.util.Map");
    assertEquals(1, cu.getImports().size());
  }

  [TestMethod]
  void doNotAddDuplicateImportsByStringAndFlags() {
    cu.addImport(Map.class);
    cu.addImport("java.util.Map", false, false);
    assertEquals(1, cu.getImports().size());
  }

  [TestMethod]
  void doNotAddDuplicateImportsByImportDeclaration() {
    cu.addImport(Map.class);
    cu.addImport(parseImport("import java.util.Map;"));
    assertEquals(1, cu.getImports().size());
  }

  [TestMethod]
  void testAddImportArrayTypes() {
    cu.addImport(CompilationUnit[][][].class);
    cu.addImport(int[][][].class);
    cu.addImport(Integer[][][].class);
    cu.addImport(List[][][].class);
    assertEquals(2, cu.getImports().size());
    assertEquals("com.github.javaparser.ast.CompilationUnit", cu.getImport(0).getNameAsString());
    assertEquals("java.util.List", cu.getImport(1).getNameAsString());
  }

  class testInnerClass {

  }

  [TestMethod]
  void testAddImportAnonymousClass() {
    cu.addImport(testInnerClass.class);
    assertEquals("import " + testInnerClass.class.getCanonicalName().replace("$", ".") + ";" + SYSTEM_EOL,
        cu.getImport(0).toString());
  }

  [TestMethod]
  void testAddClass() {
    ClassOrInterfaceDeclaration myClassDeclaration = cu.addClass("testClass", PRIVATE);
    assertEquals(1, cu.getTypes().size());
    assertEquals("testClass", cu.getType(0).getNameAsString());
    assertEquals(ClassOrInterfaceDeclaration.class, cu.getType(0).getClass());
    assertTrue(myClassDeclaration.isPrivate());
    assertFalse(myClassDeclaration.isInterface());
  }

  [TestMethod]
  void testAddInterface() {
    ClassOrInterfaceDeclaration myInterfaceDeclaration = cu.addInterface("testInterface");
    assertEquals(1, cu.getTypes().size());
    assertEquals("testInterface", cu.getType(0).getNameAsString());
    assertTrue(myInterfaceDeclaration.isPublic());
    assertEquals(ClassOrInterfaceDeclaration.class, cu.getType(0).getClass());
    assertTrue(myInterfaceDeclaration.isInterface());
  }

  [TestMethod]
  void testAddEnum() {
    EnumDeclaration myEnumDeclaration = cu.addEnum("test");
    assertEquals(1, cu.getTypes().size());
    assertEquals("test", cu.getType(0).getNameAsString());
    assertTrue(myEnumDeclaration.isPublic());
    assertEquals(EnumDeclaration.class, cu.getType(0).getClass());
  }

  [TestMethod]
  void testAddAnnotationDeclaration() {
    AnnotationDeclaration myAnnotationDeclaration = cu.addAnnotationDeclaration("test");
    assertEquals(1, cu.getTypes().size());
    assertEquals("test", cu.getType(0).getNameAsString());
    assertTrue(myAnnotationDeclaration.isPublic());
    assertEquals(AnnotationDeclaration.class, cu.getType(0).getClass());
  }

  [TestMethod]
  void testAddTypeWithRecordDeclaration() {
    RecordDeclaration myRecordDeclaration = new RecordDeclaration(Modifier.createModifierList(PUBLIC), "test");
    cu.addType(myRecordDeclaration);
    assertEquals(1, cu.getTypes().size());
    assertEquals("test", cu.getType(0).getNameAsString());
    assertTrue(myRecordDeclaration.isPublic());
    assertTrue(myRecordDeclaration.isFinal());
    assertEquals(RecordDeclaration.class, cu.getType(0).getClass());
  }

  [TestMethod]
  void testGetClassByName() {
    assertEquals(cu.addClass("test"), cu.getClassByName("test").get());
  }

  [TestMethod]
  void testGetInterfaceByName() {
    assertEquals(cu.addInterface("test"), cu.getInterfaceByName("test").get());
  }

  [TestMethod]
  void testGetEnumByName() {
    assertEquals(cu.addEnum("test"), cu.getEnumByName("test").get());
  }

  [TestMethod]
  void testGetAnnotationDeclarationByName() {
    assertEquals(cu.addAnnotationDeclaration("test"), cu.getAnnotationDeclarationByName("test").get());
  }

  [TestMethod]
  void testGetRecordByName() {
    assertEquals(cu.addType(new RecordDeclaration(Modifier.createModifierList(), "test")).getType(0),
            cu.getRecordByName("test").get());
  }
}
