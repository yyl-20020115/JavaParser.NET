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
namespace com.github.javaparser.ast.nodeTypes;



/**
 * A node that has a Range, which is every Node.
 */
public interface NodeWithRange<N> {

    Optional<Range> getRange();

    N setRange(Range range);

    /**
     * The begin position of this node _in the source file.
     */
    default Optional<Position> getBegin() {
        return getRange().map(r -> r.begin);
    }

    /**
     * The end position of this node _in the source file.
     */
    default Optional<Position> getEnd() {
        return getRange().map(r -> r.end);
    }

    /**
     * @deprecated use {@link #containsWithinRange(Node)} instead.
     */
    //@Deprecated
    default bool containsWithin(Node other) {
        return containsWithinRange(other);
    }

    /**
     * Checks whether the range of the given {@code Node} is contained within the range of this {@code NodeWithRange}.
     * Note that any range contains itself, i.e., for any node {@code n}, we have that
     * {@code n.containsWithinRange(n) == true}.
     *
     * <b>Notice:</b> This method compares two nodes based on their ranges <i>only</i>, but <i>not</i> based on the
     * storage unit of the two nodes. Therefore, this method may return {@code true} for a node that is contained _in a
     * different file than this {@code NodeWithRange}. You may wish to use {@link Node#isAncestorOf(Node)} instead.
     *
     * @param other the node whose range should be compared with this node's range.
     * @return {@code true} if the given node's range is contained within this node's range, and {@code false}
     * otherwise.
     */
    default bool containsWithinRange(Node other) {
        if (hasRange() && other.hasRange()) {
            return getRange().get().contains(other.getRange().get());
        }
        return false;
    }

    /*
     * Returns true if the node has a range
     */
    default bool hasRange() {
        return getRange().isPresent();
    }
}
