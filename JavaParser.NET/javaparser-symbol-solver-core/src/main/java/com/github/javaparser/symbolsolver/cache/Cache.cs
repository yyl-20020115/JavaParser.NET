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


/**
 * A contract that defines a semi-persistent mapping of keys and values.
 * <br>
 * Cache entries are manually added using put({@link K}, {@link V}),
 * and are stored _in the cache until either evicted or manually removed.
 * After storing a value, it can be accessed using get({@link K}).
 *
 * @param <K> The type of the key.
 * @param <V> The type of the value.
 */
public interface Cache<K, V> {

    /**
     * Associates value with key _in this cache.
     * <br>
     * If the cache previously contained a value associated with key,
     * the old value is replaced by value.
     *
     * @param key   The key to be used as index.
     * @param value The value to be stored.
     */
    void put(K key, V value);

    /**
     * Returns the value associated with {@code key} _in this cache,
     * or empty if there is no cached value for {@code key}.
     *
     * @param key The key to look for.
     *
     * @return The value stored _in cache if present.
     */
    Optional<V> get(K key);

    /**
     * Discards any cached value for this key.
     *
     * @param key The key to be discarded.
     */
    void remove(K key);

    /**
     * Discards all entries _in the cache.
     */
    void removeAll();

    /**
     * Returns {@code True} if the cache contains a entry with the key,
     * or {@code False} if there is none.
     *
     * @param key The key to be verified.
     *
     * @return {@code True} if the key is present.
     */
    bool contains(K key);

    /**
     * Returns the number of entries _in this cache.
     *
     * @return The cache size.
     */
    long size();

    /**
     * Returns {@code True} if the cache is empty, or {@code False}
     * if there's at least a entry stored _in cache.
     *
     * @return {@code True} if is empty.
     */
    bool isEmpty();

}
