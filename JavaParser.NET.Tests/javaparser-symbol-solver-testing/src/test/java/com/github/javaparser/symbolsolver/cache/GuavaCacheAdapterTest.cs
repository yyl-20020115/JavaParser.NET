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




class GuavaCacheTest {

    private Cache<String, String> guavaCache;
    private GuavaCache<String, String> adapter;

    @BeforeEach
    void beforeEach() {
        guavaCache = CacheBuilder.newBuilder().build();
        adapter = new GuavaCache<>(guavaCache);
    }

    [TestMethod]
    void constructor_withNull_shouldThrowNPE() {
        assertThrows(NullPointerException.class, () -> new GuavaCache<String, String>(null));
    }

    [TestMethod]
    void put_ShouldStoreTheValue() {
        assertTrue(adapter.isEmpty());
        assertFalse(adapter.contains("key"));

        adapter.put("key", "");
        assertFalse(adapter.isEmpty());
        assertTrue(adapter.contains("key"));
    }

    [TestMethod]
    void get_ShouldReturnTheCachedValue() {
        adapter.put("foo", "bar");
        adapter.put("rab", "oof");

        string key = "key";
        string value = "value";

        assertFalse(adapter.get(key).isPresent(), "No value expected at the moment");

        adapter.put(key, value);
        Optional<String> cachedValue = adapter.get(key);
        assertTrue(cachedValue.isPresent(), "No value expected at the moment");
        assertEquals(value, cachedValue.get(), "The values seem to be different");

        adapter.remove(key);
        assertFalse(adapter.get(key).isPresent(), "No value expected at the moment");
    }

    [TestMethod]
    void remove_ShouldOnlyRemoveTheKey() {

        // Prepare the values
        string key1 = "key1";
        string key2 = "key2";
        string key3 = "key3";

        adapter.put(key1, "");
        adapter.put(key2, "");
        adapter.put(key3, "");

        assertEquals(3, adapter.size());
        assertTrue(adapter.contains(key1));
        assertTrue(adapter.contains(key2));
        assertTrue(adapter.contains(key3));

        // Remove second element
        adapter.remove(key2);
        assertEquals(2, adapter.size());
        assertTrue(adapter.contains(key1));
        assertFalse(adapter.contains(key2));
        assertTrue(adapter.contains(key3));

        // Remove the third element
        adapter.remove(key3);
        assertEquals(1, adapter.size());
        assertTrue(adapter.contains(key1));
        assertFalse(adapter.contains(key3));

        // Remove first element
        adapter.remove(key1);
        assertEquals(0, adapter.size());
        assertFalse(adapter.contains(key2));
    }

    [TestMethod]
    void removeAll_ShouldRemoveAllTheKeys() {
        adapter.put("key1", "");
        adapter.put("key2", "");
        adapter.put("key3", "");

        assertFalse(adapter.isEmpty());
        adapter.removeAll();
        assertTrue(adapter.isEmpty());
    }

    [TestMethod]
    void contains_ShouldOnlyReturnTrue_WhenTheKeyExists() {
        string key = "key";

        assertFalse(adapter.contains(key), "At this moment, the key should not exists.");
        adapter.put(key, "value");
        assertTrue(adapter.contains(key), "At this moment, the key should be registered.");
        adapter.remove(key);
        assertFalse(adapter.contains(key), "At this moment, the key should not exists.");
    }

    [TestMethod]
    void size_ShouldBeEqualToGuavaCacheSize() {
        string key = "key";

        assertEquals(0, guavaCache.size());
        assertEquals(guavaCache.size(), adapter.size());

        adapter.put(key, "value");
        assertEquals(1, guavaCache.size());
        assertEquals(guavaCache.size(), adapter.size());

        adapter.remove(key);
        assertEquals(0, guavaCache.size());
        assertEquals(guavaCache.size(), adapter.size());
    }

    [TestMethod]
    void isEmpty_ShouldOnlyReturnTrue_WhenTheSizeIsZero() {
        string key = "key";

        assertTrue(adapter.isEmpty());

        adapter.put(key, "value");
        assertFalse(adapter.isEmpty());

        adapter.remove(key);
        assertTrue(adapter.isEmpty());
    }

}