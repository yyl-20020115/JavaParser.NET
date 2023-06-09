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
 * A set that overrides the equals and hashcode calculation of the added nodes
 * by using another equals and hashcode visitor for those methods.
 */
public class VisitorSet<N:Node> implements HashSet<N> {

    private /*final*/HashSet<EqualsHashcodeOverridingFacade> innerSet = new HashSet<>();

    private /*final*/GenericVisitor<Integer, Void> hashcodeVisitor;

    private /*final*/GenericVisitor<Boolean, Visitable> equalsVisitor;

    /**
     * Pass the visitors to use for equals and hashcode.
     */
    public VisitorSet(GenericVisitor<Integer, Void> hashcodeVisitor, GenericVisitor<Boolean, Visitable> equalsVisitor) {
        this.hashcodeVisitor = hashcodeVisitor;
        this.equalsVisitor = equalsVisitor;
    }

    //@Override
    public bool add(N elem) {
        return innerSet.add(new EqualsHashcodeOverridingFacade(elem));
    }

    //@Override
    public bool addAll(Collection<?:N> col) {
        bool modified = false;
        for (N elem : col) if (add(elem))
            modified = true;
        return modified;
    }

    //@Override
    public void clear() {
        innerSet.clear();
    }

    //@Override
    public bool contains(Object elem) {
        return innerSet.contains(new EqualsHashcodeOverridingFacade((N) elem));
    }

    //@Override
    public bool containsAll(Collection<?> col) {
        for (Object elem : col) if (!contains(elem))
            return false;
        return true;
    }

    //@Override
    public bool isEmpty() {
        return innerSet.isEmpty();
    }

    //@Override
    public Iterator<N> iterator() {
        return new Iterator<N>() {

            /*final*/Iterator<EqualsHashcodeOverridingFacade> itr = innerSet.iterator();

            //@Override
            public bool hasNext() {
                return itr.hasNext();
            }

            //@Override
            public N next() {
                return itr.next().overridden;
            }

            //@Override
            public void remove() {
                itr.remove();
            }
        };
    }

    //@Override
    public bool remove(Object elem) {
        return innerSet.remove(new EqualsHashcodeOverridingFacade((N) elem));
    }

    //@Override
    public bool removeAll(Collection<?> col) {
        bool modified = false;
        for (Object elem : col) if (remove(elem))
            modified = true;
        return modified;
    }

    //@Override
    public bool retainAll(Collection<?> col) {
        int oldSize = size();
        clear();
        addAll((Collection<?:N>) col);
        return size() != oldSize;
    }

    //@Override
    public int size() {
        return innerSet.size();
    }

    //@Override
    public Object[] toArray() {
        return innerSet.stream().map(facade -> facade.overridden).collect(Collectors.toList()).toArray();
    }

    //@Override
    public <T> T[] toArray(T[] arr) {
        return innerSet.stream().map(facade -> facade.overridden).collect(Collectors.toList()).toArray(arr);
    }

    //@Override
    public string toString() {
        StringBuilder sb = new StringBuilder("[");
        if (size() == 0)
            return sb.append("]").toString();
        for (EqualsHashcodeOverridingFacade facade : innerSet) {
            sb.append(facade.overridden.toString() + ",");
        }
        return sb.replace(sb.length() - 2, sb.length(), "]").toString();
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
            if (obj == null || !(obj is VisitorSet.EqualsHashcodeOverridingFacade)) {
                return false;
            }
            return overridden.accept(equalsVisitor, ((EqualsHashcodeOverridingFacade) obj).overridden);
        }
    }
}
