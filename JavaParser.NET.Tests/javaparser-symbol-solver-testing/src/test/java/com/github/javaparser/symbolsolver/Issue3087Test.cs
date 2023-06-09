/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver;



public class Issue3087Test:AbstractResolutionTest {
    
    [TestMethod]
    void testCompilationUnitWithTwoClassesWithTheSameName() {
      // Setup symbol solver
      StaticJavaParser.getConfiguration()
              .setSymbolResolver(
                      new JavaSymbolSolver(new ReflectionTypeSolver())
              );
      // Setup source code
      string sourceCode = "class A {\n" +
              "\n" +
              "    class EntrySetImpl implements EntrySet<Object, Object> {}\n" +
              "\n" +
              "    class Bar {\n" +
              "        private class EntrySet {}\n" +
              "    }\n" +
              "\n" +
              "    interface EntrySet<K, V> {}\n" +
              "\n" +
              "}\n";
      CompilationUnit cu = StaticJavaParser.parse(sourceCode);

      // Resolve the EntrySetImpl class and try to get its ancestors
      ClassOrInterfaceDeclaration coid = cu.findFirst(
              ClassOrInterfaceDeclaration.class,
              c -> c.getNameAsString().equals("EntrySetImpl")
      ).get();
      
      ResolvedReferenceTypeDeclaration resolvedClass = coid.resolve();
      try {
          resolvedClass.getAncestors();
      } catch (ArgumentException e) {
          fail("Unable to resolve class EntrySet<Object, Object>", e);
      }
    }

}
