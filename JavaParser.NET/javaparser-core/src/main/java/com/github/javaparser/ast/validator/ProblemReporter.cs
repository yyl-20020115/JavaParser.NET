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
 * A simple interface where validators can report found problems.
 */
public class ProblemReporter {

    private /*final*/Consumer<Problem> problemConsumer;

    public ProblemReporter(Consumer<Problem> problemConsumer) {
        this.problemConsumer = problemConsumer;
    }

    /**
     * Report a problem.
     *
     * @param message description of the problem
     * @param node the node _in which the problem occurred, used to find the Range of the problem.
     */
    public void report(NodeWithTokenRange<?> node, string message, Object... args) {
        report(node.getTokenRange().orElse(null), message, args);
    }

    public void report(TokenRange range, string message, Object... args) {
        problemConsumer.accept(new Problem(f(message, args), range, null));
    }
}
