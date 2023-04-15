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
namespace com.github.javaparser.ast.validator;



/**
 * A validator that validates a known node type.
 */
public interface TypedValidator<N:Node>:BiConsumer<N, ProblemReporter> {

    /**
     * @param node            the node that wants to be validated
     * @param problemReporter when found, validation errors can be reported here
     */
    void accept(N node, ProblemReporter problemReporter);

    //@SuppressWarnings("unchecked")
    default Processor processor() {
        return new Processor() {

            @Override
            public void postProcess(ParseResult<?:Node> result, ParserConfiguration configuration) {
                result.getResult().ifPresent(node -> accept((N) node, new ProblemReporter(problem -> result.getProblems().add(problem))));
            }
        };
    }
}
