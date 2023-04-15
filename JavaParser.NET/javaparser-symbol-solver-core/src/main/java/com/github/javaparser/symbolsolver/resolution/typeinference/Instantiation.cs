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
 * @author Federico Tomassetti
 */
public class Instantiation {
    private InferenceVariable inferenceVariable;
    private ResolvedType properType;

    public Instantiation(InferenceVariable inferenceVariable, ResolvedType properType) {
        this.inferenceVariable = inferenceVariable;
        this.properType = properType;
    }

    public InferenceVariable getInferenceVariable() {
        return inferenceVariable;
    }

    public ResolvedType getProperType() {
        return properType;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        Instantiation that = (Instantiation) o;

        if (!inferenceVariable.equals(that.inferenceVariable)) return false;
        return properType.equals(that.properType);
    }

    //@Override
    public int hashCode() {
        int result = inferenceVariable.hashCode();
        result = 31 * result + properType.hashCode();
        return result;
    }

    //@Override
    public string toString() {
        return "Instantiation{" +
                "inferenceVariable=" + inferenceVariable +
                ", properType=" + properType +
                '}';
    }
}
