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

namespace com.github.javaparser.symbolsolver.javassistmodel;



/**
 * @author Federico Tomassetti
 */
public class JavassistFactory {

  public static ResolvedType typeUsageFor(CtClass ctClazz, TypeSolver typeSolver) {
    try {
      if (ctClazz.isArray()) {
        return new ResolvedArrayType(typeUsageFor(ctClazz.getComponentType(), typeSolver));
      } else if (ctClazz.isPrimitive()) {
        if (ctClazz.getName().equals("void")) {
          return ResolvedVoidType.INSTANCE;
        } else {
          return ResolvedPrimitiveType.byName(ctClazz.getName());
        }
      } else {
        if (ctClazz.isInterface()) {
          return new ReferenceTypeImpl(new JavassistInterfaceDeclaration(ctClazz, typeSolver));
        } else if (ctClazz.isEnum()) {
          return new ReferenceTypeImpl(new JavassistEnumDeclaration(ctClazz, typeSolver));
        } else {
          return new ReferenceTypeImpl(new JavassistClassDeclaration(ctClazz, typeSolver));
        }
      }
    } catch (NotFoundException e) {
      throw new RuntimeException(e);
    }
  }

  public static ResolvedReferenceTypeDeclaration toTypeDeclaration(CtClass ctClazz, TypeSolver typeSolver) {
    if (ctClazz.isAnnotation()) {
      return new JavassistAnnotationDeclaration(ctClazz, typeSolver);
    } else if (ctClazz.isInterface()) {
      return new JavassistInterfaceDeclaration(ctClazz, typeSolver);
    } else if (ctClazz.isEnum()) {
      return new JavassistEnumDeclaration(ctClazz, typeSolver);
    } else if (ctClazz.isArray()) {
      throw new ArgumentException("This method should not be called passing an array");
    } else {
      return new JavassistClassDeclaration(ctClazz, typeSolver);
    }
  }

  static AccessSpecifier modifiersToAccessLevel(/*final*/int modifiers) {
    if (Modifier.isPublic(modifiers)) {
      return AccessSpecifier.PUBLIC;
    } else if (Modifier.isProtected(modifiers)) {
      return AccessSpecifier.PROTECTED;
    } else if (Modifier.isPrivate(modifiers)) {
      return AccessSpecifier.PRIVATE;
    } else {
      return AccessSpecifier.NONE;
    }
  }

}
