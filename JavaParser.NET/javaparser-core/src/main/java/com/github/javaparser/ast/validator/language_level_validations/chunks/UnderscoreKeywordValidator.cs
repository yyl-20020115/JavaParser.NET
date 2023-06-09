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
namespace com.github.javaparser.ast.validator.language_level_validations.chunks;


public class UnderscoreKeywordValidator:VisitorValidator {

    //@Override
    public void visit(Name n, ProblemReporter arg) {
        validateIdentifier(n, n.getIdentifier(), arg);
        super.visit(n, arg);
    }

    //@Override
    public void visit(SimpleName n, ProblemReporter arg) {
        validateIdentifier(n, n.getIdentifier(), arg);
        super.visit(n, arg);
    }

    private static void validateIdentifier(Node n, string id, ProblemReporter arg) {
        if (id.equals("_")) {
            arg.report(n, "'_' is a reserved keyword.");
        }
    }
}
