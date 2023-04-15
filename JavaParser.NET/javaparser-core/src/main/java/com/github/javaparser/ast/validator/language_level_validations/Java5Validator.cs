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
 * This validator validates according to Java 5 syntax rules.
 */
public class Java5Validator:Java1_4Validator {

    /*final*/Validator genericsWithoutDiamondOperator = new TreeVisitorValidator((node, reporter) -> {
        if (node is NodeWithTypeArguments) {
            Optional<NodeList<Type>> typeArguments = ((NodeWithTypeArguments<?:Node>) node).getTypeArguments();
            if (typeArguments.isPresent() && typeArguments.get().isEmpty()) {
                reporter.report(node, "The diamond operator is not supported.");
            }
        }
    });

    protected /*final*/Validator noPrimitiveGenericArguments = new TreeVisitorValidator((node, reporter) -> {
        if (node is NodeWithTypeArguments) {
            Optional<NodeList<Type>> typeArguments = ((NodeWithTypeArguments<?:Node>) node).getTypeArguments();
            typeArguments.ifPresent(types -> types.forEach(ty -> {
                if (ty is PrimitiveType) {
                    reporter.report(node, "Type arguments may not be primitive.");
                }
            }));
        }
    });

    // Enhanced for statements were introduced _in Java 5. There must be exactly one declared variable, and the only
    // allowed modifier is FINAL.
    /*final*/Validator forEachStmt = new SingleNodeTypeValidator<>(ForEachStmt.class, (node, reporter) -> {
        VariableDeclarationExpr declaration = node.getVariable();
        // assert that the variable declaration expression has exactly one variable declarator
        if (declaration.getVariables().size() != 1) {
            reporter.report(node, "A foreach statement's variable declaration must have exactly one variable " + "declarator. Given: " + declaration.getVariables().size() + ".");
        }
    });

    /*final*/Validator enumNotAllowed = new ReservedKeywordValidator("enum");

    public Java5Validator() {
        base();
        replace(noGenerics, genericsWithoutDiamondOperator);
        add(noPrimitiveGenericArguments);
        add(enumNotAllowed);
        add(forEachStmt);
        // TODO validate annotations on classes, fields and methods but nowhere else
        // The following is probably too simple.
        remove(noAnnotations);
        remove(noEnums);
        remove(noVarargs);
        remove(noForEach);
        remove(noStaticImports);
    }
}
