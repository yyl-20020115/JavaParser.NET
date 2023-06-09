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

namespace com.github.javaparser.printer.lexicalpreservation.changes;



class NoChangeTest {

    @BeforeAll
    static void setUpBeforeClass() {
    }

    @AfterAll
    static void tearDownAfterClass() {
    }

    @BeforeEach
    void setUp() {
    }

    @AfterEach
    void tearDown() {
    }
    
    [TestMethod]
    void getValue_WithMethodFound() {
        Object o = new NoChange().getValue(ObservableProperty.ANNOTATIONS, new ClassOrInterfaceType());
        assertNotNull(o);
    }

    [TestMethod]
    void getValue_WithNoMethodFound() {
        assertThrows(RuntimeException.class, () -> {
            new NoChange().getValue(ObservableProperty.IMPORTS, new ClassOrInterfaceType());
        }, "RuntimeException was expected");
    }
    
}
