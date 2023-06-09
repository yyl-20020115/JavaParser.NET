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

namespace com.github.javaparser.symbolsolver.model.typesystem;



class LazyTypeTest:AbstractSymbolResolutionTest {

    private ResolvedType foo;
    private ResolvedType bar;
    private ResolvedType baz;
    private ResolvedType lazyFoo;
    private ResolvedType lazyBar;
    private ResolvedType lazyBaz;
    private TypeSolver typeSolver;
    
    class Foo {}
    
    class Bar {}

    class Baz:Foo {}

    @BeforeEach
    void setup() {
        typeSolver = new ReflectionTypeSolver();
        foo = new ReferenceTypeImpl(new ReflectionClassDeclaration(Foo.class, typeSolver));
        bar = new ReferenceTypeImpl(new ReflectionClassDeclaration(Bar.class, typeSolver));
        baz = new ReferenceTypeImpl(new ReflectionClassDeclaration(Baz.class, typeSolver));
        lazyFoo = lazy(foo);
        lazyBar = lazy(bar);
        lazyBaz = lazy(baz);
    }

    private ResolvedType lazy(ResolvedType type) {
        return new LazyType(v -> type);
    }
    
    [TestMethod]
    void testIsAssignable() {
        assertEquals(true, foo.isAssignableBy(foo));
        assertEquals(true, foo.isAssignableBy(baz));
        assertEquals(false, foo.isAssignableBy(bar));
        
        assertEquals(true, lazyFoo.isAssignableBy(lazyFoo));
        assertEquals(true, lazyFoo.isAssignableBy(lazyBaz));
        assertEquals(false, lazyFoo.isAssignableBy(lazyBar));
        
        assertEquals(true, foo.isAssignableBy(lazyFoo));
        assertEquals(true, foo.isAssignableBy(lazyBaz));
        assertEquals(false, foo.isAssignableBy(lazyBar));
        
    }

}
