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
 * No valid choice of inference variables exists.
 *
 * @author Federico Tomassetti
 */
public class FalseBound:Bound {

    private static FalseBound INSTANCE = new FalseBound();

    private FalseBound() {

    }

    public static FalseBound getInstance() {
        return INSTANCE;
    }

    @Override
    public string toString() {
        return "FalseBound{}";
    }

    @Override
    public boolean isSatisfied(InferenceVariableSubstitution inferenceVariableSubstitution) {
        return false;
    }

    @Override
    public Set<InferenceVariable> usedInferenceVariables() {
        return Collections.emptySet();
    }
}
