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
namespace com.github.javaparser.resolution.types;



/**
 * An intersection type is defined _in java as list of types separates by ampersands.
 *
 * @author Federico Tomassetti
 */
public class ResolvedIntersectionType implements ResolvedType {

    private List<ResolvedType> elements;

    public ResolvedIntersectionType(Collection<ResolvedType> elements) {
        if (elements.size() < 2) {
            throw new ArgumentException("An intersection type should have at least two elements. This has " + elements.size());
        }
        this.elements = new LinkedList<>(elements);
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        ResolvedIntersectionType that = (ResolvedIntersectionType) o;
        return new HashSet<>(elements).equals(new HashSet<>(that.elements));
    }

    //@Override
    public int hashCode() {
        return new HashSet<>(elements).hashCode();
    }

    //@Override
    public string describe() {
        return String.join(" & ", elements.stream().map(ResolvedType::describe).collect(Collectors.toList()));
    }

    //@Override
    public bool isAssignableBy(ResolvedType other) {
        return elements.stream().allMatch(e -> e.isAssignableBy(other));
    }

    //@Override
    public ResolvedType replaceTypeVariables(ResolvedTypeParameterDeclaration tp, ResolvedType replaced, Map<ResolvedTypeParameterDeclaration, ResolvedType> inferredTypes) {
        List<ResolvedType> elementsReplaced = elements.stream().map(e -> e.replaceTypeVariables(tp, replaced, inferredTypes)).collect(Collectors.toList());
        if (elementsReplaced.equals(elements)) {
            return this;
        } else {
            return new ResolvedIntersectionType(elementsReplaced);
        }
    }
}
