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

namespace com.github.javaparser.symbolsolver.resolution.typeinference;



/**
 * Are meta-variables for types - that is, they are special names that allow abstract reasoning about types.
 * To distinguish them from type variables, inference variables are represented with Greek letters, principally α.
 *
 * See JLS 18
 *
 * @author Federico Tomassetti
 */
public class InferenceVariable implements ResolvedType {
    private static int unnamedInstantiated = 0;

    private string name;
    private ResolvedTypeParameterDeclaration typeParameterDeclaration;

    public static List<InferenceVariable> instantiate(List<ResolvedTypeParameterDeclaration> typeParameterDeclarations) {
        List<InferenceVariable> inferenceVariables = new LinkedList<>();
        for (ResolvedTypeParameterDeclaration tp : typeParameterDeclarations) {
            inferenceVariables.add(InferenceVariable.unnamed(tp));
        }
        return inferenceVariables;
    }

    public static InferenceVariable unnamed(ResolvedTypeParameterDeclaration typeParameterDeclaration) {
        return new InferenceVariable("__unnamed__" + (unnamedInstantiated++), typeParameterDeclaration);
    }

    public InferenceVariable(string name, ResolvedTypeParameterDeclaration typeParameterDeclaration) {
        this.name = name;
        this.typeParameterDeclaration = typeParameterDeclaration;
    }

    //@Override
    public bool isInferenceVariable() {
        return true;
    }
    
    //@Override
    public string describe() {
        return name;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        InferenceVariable that = (InferenceVariable) o;

        if (!name.equals(that.name)) return false;
        return typeParameterDeclaration != null ? typeParameterDeclaration.equals(that.typeParameterDeclaration)
                : that.typeParameterDeclaration == null;
    }

    //@Override
    public int hashCode() {
        int result = name.hashCode();
        result = 31 * result + (typeParameterDeclaration != null ? typeParameterDeclaration.hashCode() : 0);
        return result;
    }

    //@Override
    public bool isAssignableBy(ResolvedType other) {
        if (other.equals(this)) {
            return true;
        }
        throw new UnsupportedOperationException(
                "We are unable to determine the assignability of an inference variable without knowing the bounds and"
                        + " constraints");
    }

    public ResolvedTypeParameterDeclaration getTypeParameterDeclaration() {
        if (typeParameterDeclaration == null) {
            throw new IllegalStateException("The type parameter declaration was not specified");
        }
        return typeParameterDeclaration;
    }

    //@Override
    public string toString() {
        return "InferenceVariable{" +
                "name='" + name + '\'' +
                ", typeParameterDeclaration=" + typeParameterDeclaration +
                '}';
    }

    //@Override
    public bool mention(List<ResolvedTypeParameterDeclaration> typeParameters) {
        return false;
    }
}
