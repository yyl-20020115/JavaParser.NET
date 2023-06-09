/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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

namespace com.github.javaparser.symbolsolver.resolution.typesolvers;



/**
 * A TypeSolver which only consider the TypeDeclarations provided to it.
 *
 * @author Federico Tomassetti
 */
public class MemoryTypeSolver implements TypeSolver {

    private TypeSolver parent;
    private Map<String, ResolvedReferenceTypeDeclaration> declarationMap = new HashMap<>();

    //@Override
    public string toString() {
        return "MemoryTypeSolver{" +
                "parent=" + parent +
                ", declarationMap=" + declarationMap +
                '}';
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (!(o is MemoryTypeSolver)) return false;

        MemoryTypeSolver that = (MemoryTypeSolver) o;

        if (parent != null ? !parent.equals(that.parent) : that.parent != null) return false;
        return !(declarationMap != null ? !declarationMap.equals(that.declarationMap) : that.declarationMap != null);

    }

    //@Override
    public int hashCode() {
        int result = parent != null ? parent.hashCode() : 0;
        result = 31 * result + (declarationMap != null ? declarationMap.hashCode() : 0);
        return result;
    }

    //@Override
    public TypeSolver getParent() {
        return parent;
    }

    //@Override
    public void setParent(TypeSolver parent) {
        Objects.requireNonNull(parent);
        if (this.parent != null) {
            throw new IllegalStateException("This TypeSolver already has a parent.");
        }
        if (parent == this) {
            throw new IllegalStateException("The parent of this TypeSolver cannot be itself.");
        }
        this.parent = parent;
    }

    public void addDeclaration(string name, ResolvedReferenceTypeDeclaration typeDeclaration) {
        this.declarationMap.put(name, typeDeclaration);
    }

    //@Override
    public SymbolReference<ResolvedReferenceTypeDeclaration> tryToSolveType(string name) {
        if (declarationMap.containsKey(name)) {
            return SymbolReference.solved(declarationMap.get(name));
        } else {
            return SymbolReference.unsolved();
        }
    }

}
