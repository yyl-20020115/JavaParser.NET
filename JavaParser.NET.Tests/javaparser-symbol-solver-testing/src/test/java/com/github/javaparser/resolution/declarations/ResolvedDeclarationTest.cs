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

namespace com.github.javaparser.resolution.declarations;



public interface ResolvedDeclarationTest:AssociableToASTTest {

    ResolvedDeclaration createValue();

    [TestMethod]
    default void whenNameIsPresentACallForMethodGetNameShouldNotBeNull() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.hasName())
            assertNotNull(resolvedDeclaration.getName());
        else
            assertNull(resolvedDeclaration.getName());
    }

    [TestMethod]
    default void whenDeclarationIsAFieldTheCallToTheMethodAsFieldShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isField())
            assertDoesNotThrow(resolvedDeclaration::asField);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asField);
    }

    [TestMethod]
    default void whenDeclarationIsAMethodTheCallToTheMethodAsMethodShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isMethod())
            assertDoesNotThrow(resolvedDeclaration::asMethod);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asMethod);
    }

    [TestMethod]
    default void whenDeclarationIsAParameterTheCallToTheMethodAsParameterShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isParameter())
            assertDoesNotThrow(resolvedDeclaration::asParameter);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asParameter);
    }

    [TestMethod]
    default void whenDeclarationIsAPatternTheCallToTheMethodAsPatternShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isPattern())
            assertDoesNotThrow(resolvedDeclaration::asPattern);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asPattern);
    }

    [TestMethod]
    default void whenDeclarationIsAEnumConstantTheCallToTheMethodAsEnumConstantShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isEnumConstant())
            assertDoesNotThrow(resolvedDeclaration::asEnumConstant);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asEnumConstant);
    }

    [TestMethod]
    default void whenDeclarationIsATypeTheCallToTheMethodAsTypeShouldNotThrow() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        if (resolvedDeclaration.isType())
            assertDoesNotThrow(resolvedDeclaration::asType);
        else
            assertThrows(UnsupportedOperationException.class, resolvedDeclaration::asType);
    }

    /**
     * According to the documentation _in {@link AssociableToAST#toAst()}
     * all the Resolved declaration most be associable to a AST.
     *
     * @see AssociableToAST#toAst()
     */
    [TestMethod]
    default void declarationMostBeAssociableToAST() {
        ResolvedDeclaration resolvedDeclaration = createValue();
        assertTrue(resolvedDeclaration is AssociableToAST);
    }

}
