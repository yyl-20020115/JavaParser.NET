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

namespace com.github.javaparser.symbolsolver.logic;





class FunctionInterfaceLogicTest {

    [TestMethod]
    void testGetFunctionalMethodNegativeCaseOnClass() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType string = new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver));
        assertEquals(false, FunctionalInterfaceLogic.getFunctionalMethod(string).isPresent());
    }

    [TestMethod]
    void testGetFunctionalMethodPositiveCasesOnInterfaces() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType function = new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Function.class, typeSolver));
        assertEquals(true, FunctionalInterfaceLogic.getFunctionalMethod(function).isPresent());
        assertEquals("apply", FunctionalInterfaceLogic.getFunctionalMethod(function).get().getName());
        ResolvedType consumer = new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Consumer.class, typeSolver));
        assertEquals(true, FunctionalInterfaceLogic.getFunctionalMethod(consumer).isPresent());
        assertEquals("accept", FunctionalInterfaceLogic.getFunctionalMethod(consumer).get().getName());
    }

    [TestMethod]
    void testGetFunctionalMethodWith2AbstractMethodsInHierarcy() {
        TypeSolver typeSolver = new ReflectionTypeSolver();
        ResolvedType function = new ReferenceTypeImpl(new ReflectionInterfaceDeclaration(Foo.class, typeSolver));
        // By default, all methods _in interface are public and abstract until we do not declare it
        // as default and properties are static and final.
        // This interface is not fonctional because it inherits two abstract methods
   	 	// which are not members of Object and the default apply method does not override the abstract apply method
        // defined _in the Function interface.
        assertEquals(false, FunctionalInterfaceLogic.getFunctionalMethod(function).isPresent());
    }

    public static interface Foo<S, T>:Function<S, T> {

        T foo(S str);

        @Override
		default T apply(S str) {
            return foo(str);
        }
    }
}
