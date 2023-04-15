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

namespace com.github.javaparser.utils;




class VisitorListTest {

    [TestMethod]
    void visitorAddAll() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        VisitorList<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.addAll(list);
        for (int i = 0; i < list.size(); i++)
            assertEquals(list.get(i), vList.get(i));
    }

    [TestMethod]
    void visitorAddAllAtIndex() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class Y{}"));
        VisitorList<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.add(parse("class A{}"));
        vList.add(parse("class B{}"));
        vList.addAll(2, list);
        vList.add(parse("class C{}"));
        for (int i = 0; i < list.size(); i++)
            assertEquals(list.get(i), vList.get(2 + i));
    }

    [TestMethod]
    void visitorListContains() {
        CompilationUnit x1 = parse("class X{}");
        VisitorList<CompilationUnit> list = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        list.add(x1);
        assertTrue(list.contains(x1));
    }

    [TestMethod]
    void visitorListContainsAll() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        VisitorList<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.addAll(list);
        assertTrue(vList.size() == 2 && vList.containsAll(list));
    }

    [TestMethod]
    void visitorListIterator() {
        VisitorList<CompilationUnit> list = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        CompilationUnit x1 = parse("class X{}");
        list.add(x1);
        CompilationUnit x2 = parse("class X{}");
        list.add(x2);
        Iterator<CompilationUnit> itr = list.iterator();
        assertEquals(x1, itr.next());
        itr.remove();
        assertEquals(1, list.size());
        assertEquals(x2, itr.next());
        itr.remove();
        assertEquals(0, list.size());
    }

    [TestMethod]
    void visitorListListIterator() {
        VisitorList<CompilationUnit> list = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        CompilationUnit x1 = parse("class X{}");
        list.add(x1);
        CompilationUnit x2 = parse("class X{}");
        list.add(x2);
        Iterator<CompilationUnit> itr = list.listIterator(2);
        assertEquals(x1, itr.next());
        itr.remove();
        assertEquals(3, list.size());
        assertEquals(x2, itr.next());
        itr.remove();
        assertEquals(2, list.size());
    }

    [TestMethod]
    void visitorListRemove() {
        CompilationUnit x1 = parse("class X{}");
        VisitorList<CompilationUnit> list = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        list.add(x1);
        assertTrue(list.remove(x1));
    }

    [TestMethod]
    void visitorListRemoveAll() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        VisitorList<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.addAll(list);
        vList.removeAll(list);
        assertEquals(0, vList.size());
    }

    [TestMethod]
    void visitorListRetainAll() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        VisitorList<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.addAll(list);
        vList.add(parse("class X{}"));
        vList.retainAll(list);
        assertEquals(2, vList.size());
    }

    [TestMethod]
    void visitorListSubList() {
        VisitorList<CompilationUnit> list = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        assertEquals(4, list.size());
        List<CompilationUnit> subLst = list.subList(1, 3);
        assertEquals(2, subLst.size());
        subLst.add(parse("class X{}"));
        assertEquals(3, subLst.size());
        assertEquals(5, list.size());

    }

    [TestMethod]
    void visitorListToArray() {
        List<CompilationUnit> list = new ArrayList<>();
        list.add(parse("class X{}"));
        list.add(parse("class X{}"));
        List<CompilationUnit> vList = new VisitorList<>(new ObjectIdentityHashCodeVisitor(),
                new ObjectIdentityEqualsVisitor());
        vList.addAll(list);
        for (CompilationUnit u : vList.toArray(new CompilationUnit[2]))
            assertTrue(vList.contains(u));
    }

}
