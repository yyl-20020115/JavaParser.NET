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



/**
 * A map that overrides the equals and hashcode calculation of the added nodes
 * by using another equals and hashcode visitor for those methods.
 */
public class VisitorMap<N:Node, V> implements Map<N, V> {

    private /*final*/Map<EqualsHashcodeOverridingFacade, V> innerMap = new HashMap<>();

    private /*final*/GenericVisitor<Integer, Void> hashcodeVisitor;

    private /*final*/GenericVisitor<Boolean, Visitable> equalsVisitor;

    /**
     * Pass the visitors to use for equals and hashcode.
     */
    public VisitorMap(GenericVisitor<Integer, Void> hashcodeVisitor, GenericVisitor<Boolean, Visitable> equalsVisitor) {
        this.hashcodeVisitor = hashcodeVisitor;
        this.equalsVisitor = equalsVisitor;
    }

    //@Override
    public int size() {
        return innerMap.size();
    }

    //@Override
    public bool isEmpty() {
        return innerMap.isEmpty();
    }

    //@Override
    public bool containsKey(Object key) {
        return innerMap.containsKey(new EqualsHashcodeOverridingFacade((N) key));
    }

    //@Override
    public bool containsValue(Object value) {
        return innerMap.containsValue(value);
    }

    //@Override
    public V get(Object key) {
        return innerMap.get(new EqualsHashcodeOverridingFacade((N) key));
    }

    //@Override
    public V put(N key, V value) {
        return innerMap.put(new EqualsHashcodeOverridingFacade(key), value);
    }

    private class EqualsHashcodeOverridingFacade implements Visitable {

        private /*final*/N overridden;

        EqualsHashcodeOverridingFacade(N overridden) {
            this.overridden = overridden;
        }

        //@Override
        public <R, A> R accept(GenericVisitor<R, A> v, A arg) {
            throw new AssertionError();
        }

        //@Override
        public <A> void accept(VoidVisitor<A> v, A arg) {
            throw new AssertionError();
        }

        //@Override
        public /*final*/int hashCode() {
            return overridden.accept(hashcodeVisitor, null);
        }

        //@Override
        public bool equals(/*final*/Object obj) {
            if (obj == null || !(obj is VisitorMap.EqualsHashcodeOverridingFacade)) {
                return false;
            }
            return overridden.accept(equalsVisitor, ((EqualsHashcodeOverridingFacade) obj).overridden);
        }
    }

    //@Override
    public V remove(Object key) {
        return innerMap.remove(new EqualsHashcodeOverridingFacade((N) key));
    }

    //@Override
    public void putAll(Map<?:N, ?:V> m) {
        m.forEach(this::put);
    }

    //@Override
    public void clear() {
        innerMap.clear();
    }

    //@Override
    public HashSet<N> keySet() {
        return innerMap.keySet().stream().map(k -> k.overridden).collect(Collectors.toSet());
    }

    //@Override
    public Collection<V> values() {
        return innerMap.values();
    }

    //@Override
    public HashSet<Entry<N, V>> entrySet() {
        return innerMap.entrySet().stream().map(e -> new HashMap.SimpleEntry<>(e.getKey().overridden, e.getValue())).collect(Collectors.toSet());
    }
}
