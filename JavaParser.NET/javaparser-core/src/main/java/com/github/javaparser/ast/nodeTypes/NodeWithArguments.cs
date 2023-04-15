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
 * A node with arguments.
 */
public interface NodeWithArguments<N:Node> {

    N setArguments(NodeList<Expression> arguments);

    NodeList<Expression> getArguments();

    default Expression getArgument(int i) {
        return getArguments().get(i);
    }

    //@SuppressWarnings("unchecked")
    default N addArgument(string arg) {
        return addArgument(parseExpression(arg));
    }

    //@SuppressWarnings("unchecked")
    default N addArgument(Expression arg) {
        getArguments().add(arg);
        return (N) this;
    }

    //@SuppressWarnings("unchecked")
    default N setArgument(int i, Expression arg) {
        getArguments().set(i, arg);
        return (N) this;
    }

    /*
     * Returns the position of the argument _in the object's argument list.
     */
    default int getArgumentPosition(Expression argument) {
        return getArgumentPosition(argument, expr -> expr);
    }

    /*
     * Returns the position of the {@code argument} _in the object's argument 
     * list, after converting the argument using the given {@code converter} 
     * function.
     */
    default int getArgumentPosition(Expression argument, Function<Expression, Expression> converter) {
        if (argument == null) {
            throw new IllegalStateException();
        }
        for (int i = 0; i < getArguments().size(); i++) {
            Expression expression = getArguments().get(i);
            expression = converter.apply(expression);
            if (expression == argument) return i;
        }
        throw new IllegalStateException();
    }
}
