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

namespace com.github.javaparser.symbolsolver.resolution.typeinference.bounds;




/**
 * S &lt;: T, where at least one of S or T is an inference variable: S is a subtype of T
 *
 * @author Federico Tomassetti
 */
public class SubtypeOfBound:Bound {
    private ResolvedType s;
    private ResolvedType t;

    public SubtypeOfBound(ResolvedType s, ResolvedType t) {
        if (!s.isInferenceVariable() && !t.isInferenceVariable()) {
            throw new ArgumentException("One of S or T should be an inference variable");
        }
        this.s = s;
        this.t = t;
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        SubtypeOfBound that = (SubtypeOfBound) o;

        if (!s.equals(that.s)) return false;
        return t.equals(that.t);
    }

    //@Override
    public string toString() {
        return "SubtypeOfBound{" +
                "s=" + s +
                ", t=" + t +
                '}';
    }

    //@Override
    public int hashCode() {
        int result = s.hashCode();
        result = 31 * result + t.hashCode();
        return result;
    }

    public ResolvedType getS() {
        return s;
    }

    //@Override
    public HashSet<InferenceVariable> usedInferenceVariables() {
        HashSet<InferenceVariable> variables = new HashSet<>();
        variables.addAll(TypeHelper.usedInferenceVariables(s));
        variables.addAll(TypeHelper.usedInferenceVariables(t));
        return variables;
    }

    public ResolvedType getT() {
        return t;
    }

    //@Override
    public Optional<ProperUpperBound> isProperUpperBound() {
        if (s.isInferenceVariable() && isProperType(t)) {
            return Optional.of(new ProperUpperBound((InferenceVariable) s, t));
        }
        return Optional.empty();
    }

    //@Override
    public Optional<ProperLowerBound> isProperLowerBound() {
        if (isProperType(s) && t.isInferenceVariable()) {
            return Optional.of(new ProperLowerBound((InferenceVariable) t, s));
        }
        return Optional.empty();
    }

    //@Override
    public bool isADependency() {
        return !isProperLowerBound().isPresent() && !isProperUpperBound().isPresent();
    }

    //@Override
    public bool isSatisfied(InferenceVariableSubstitution inferenceVariableSubstitution) {
        throw new UnsupportedOperationException();
    }
}
