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
namespace com.github.javaparser.ast.validator.language_level_validations;


/**
 * This validator validates according to Java 1.1 syntax rules.
 */
public class Java1_1Validator:Java1_0Validator {

    /*final*/Validator innerClasses = new SingleNodeTypeValidator<>(ClassOrInterfaceDeclaration.class, (n, reporter) -> n.getParentNode().ifPresent(p -> {
        if (p is LocalClassDeclarationStmt && n.isInterface())
            reporter.report(n, "There is no such thing as a local interface.");
    }));

    public Java1_1Validator() {
        super();
        replace(noInnerClasses, innerClasses);
        remove(noReflection);
    }
}
