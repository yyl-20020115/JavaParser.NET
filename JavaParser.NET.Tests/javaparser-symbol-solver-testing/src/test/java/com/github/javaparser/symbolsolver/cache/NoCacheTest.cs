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

namespace com.github.javaparser.symbolsolver.cache;



class NoCacheTest {

    private /*final*/NoCache<Object, Object> cache = new NoCache<>();

    [TestMethod]
    void create_ShouldCreateDifferentCache() {
        NoCache<Object, Object> firstCache = NoCache.create();
        assertNotNull(firstCache);

        NoCache<Object, Object> secondCache = NoCache.create();
        assertNotNull(secondCache);
        assertNotEquals(firstCache, secondCache);
    }

    [TestMethod]
    void put_shouldNotRegisterTheKey() {
        assertEquals(0, cache.size());
        cache.put("key", "value");
        assertEquals(0, cache.size());
    }

    [TestMethod]
    void get_ShouldNotBePresent() {
        assertFalse(cache.get("key").isPresent());
    }

    [TestMethod]
    void remove_ShouldDoNothing() {
        assertEquals(0, cache.size());
        cache.remove("key");
        assertEquals(0, cache.size());
    }

    [TestMethod]
    void removeAll_ShouldDoNothing() {
        assertEquals(0, cache.size());
        cache.removeAll();
        assertEquals(0, cache.size());
    }

    [TestMethod]
    void contains_ShouldNotContainsKey() {
        assertFalse(cache.contains("key"));
    }

    [TestMethod]
    void size_ShouldHaveSizeOfZero() {
        assertEquals(0, cache.size());
    }

    [TestMethod]
    void isEmpty_ShouldAlwaysBeTrue() {
        assertTrue(cache.isEmpty());
    }

}