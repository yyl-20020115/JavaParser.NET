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
 * Validates that identifiers are not keywords - this for the few keywords that the parser
 * accepts because they were added after Java 1.0.
 */
public class ReservedKeywordValidator:VisitorValidator {

    private /*final*/string keyword;

    private /*final*/string error;

    public ReservedKeywordValidator(string keyword) {
        this.keyword = keyword;
        error = f("'%s' cannot be used as an identifier as it is a keyword.", keyword);
    }

    //@Override
    public void visit(Name n, ProblemReporter arg) {
        if (n.getIdentifier().equals(keyword)) {
            arg.report(n, error);
        }
        super.visit(n, arg);
    }

    //@Override
    public void visit(SimpleName n, ProblemReporter arg) {
        if (n.getIdentifier().equals(keyword)) {
            arg.report(n, error);
        }
        super.visit(n, arg);
    }
}
