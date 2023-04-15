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

namespace com.github.javaparser.ast.expr;



class MethodReferenceExprTest {

    [TestMethod]
    void methodReferenceExprHasAlwaysAScope() {
        assertNotNull(new MethodReferenceExpr().getScope());
    }

    [TestMethod]
    void reference1() {
        assertExpressionValid("String::length");
    }
        
    [TestMethod]
    void reference2() {
        assertExpressionValid("System::currentTimeMillis // static method");
    }
        
    [TestMethod]
    void reference3() {
        assertExpressionValid("List<String>::size // explicit type arguments for generic type");
    }
        
    [TestMethod]
    void reference4() {
        assertExpressionValid("List::size // inferred type arguments for generic type");
    }
        
    [TestMethod]
    void reference5() {
        assertExpressionValid("int[]::clone");
    }
        
    [TestMethod]
    void reference6() {
        assertExpressionValid("T::tvarMember");
    }
        
    [TestMethod]
    void reference7() {
        assertExpressionValid("System._out::println");
    }
        
    [TestMethod]
    void reference8() {
        assertExpressionValid("\"abc\"::length");
    }
        
    [TestMethod]
    void reference9() {
        assertExpressionValid("foo[x]::bar");
    }
        
    [TestMethod]
    void reference10() {
        assertExpressionValid("(test ? list.replaceAll(String::trim) : list) :: iterator");
    }
        
    [TestMethod]
    void reference10Annotated1() {
        assertExpressionValid("(test ? list.replaceAll(@A String::trim) : list) :: iterator");
    }
        
    [TestMethod]
    void reference11() {
        assertExpressionValid("String::valueOf // overload resolution needed");
    }
        
    [TestMethod]
    void reference12() {
        assertExpressionValid("Arrays::sort // type arguments inferred from context");
    }
        
    [TestMethod]
    void reference13() {
        assertExpressionValid("Arrays::<String>sort // explicit type arguments");
    }
        
    [TestMethod]
    void reference14() {
        assertExpressionValid("ArrayList<String>::new // constructor for parameterized type");
    }
        
    [TestMethod]
    void reference15() {
        assertExpressionValid("ArrayList::new // inferred type arguments");
    }
        
    [TestMethod]
    void reference16() {
        assertExpressionValid("Foo::<Integer>new // explicit type arguments");
    }
        
    [TestMethod]
    void reference17() {
        assertExpressionValid("Bar<String>::<Integer>new // generic class, generic constructor");
    }
        
    [TestMethod]
    void reference18() {
        assertExpressionValid("Outer.Inner::new // inner class constructor");
    }
        
    [TestMethod]
    void reference19() {
        assertExpressionValid("int[]::new // array creation");
    }
}
