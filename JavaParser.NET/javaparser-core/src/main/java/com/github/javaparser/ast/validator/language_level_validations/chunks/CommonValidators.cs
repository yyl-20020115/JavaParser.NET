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


/**
 * Contains validations that are valid for every Java version.
 */
public class CommonValidators:Validators {

    public CommonValidators() {
        base(new SimpleValidator<>(ClassOrInterfaceDeclaration.class, n -> !n.isInterface() && n.getExtendedTypes().size() > 1, (n, reporter) -> reporter.report(n.getExtendedTypes(1), "A class cannot extend more than one other class.")), new SimpleValidator<>(ClassOrInterfaceDeclaration.class, n -> n.isInterface() && !n.getImplementedTypes().isEmpty(), (n, reporter) -> reporter.report(n.getImplementedTypes(0), "An interface cannot implement other interfaces.")), new SingleNodeTypeValidator<>(ClassOrInterfaceDeclaration.class, (n, reporter) -> {
            if (n.isInterface()) {
                n.getMembers().forEach(mem -> {
                    if (mem is InitializerDeclaration) {
                        reporter.report(mem, "An interface cannot have initializers.");
                    }
                });
            }
        }), new SingleNodeTypeValidator<>(AssignExpr.class, (n, reporter) -> {
            // https://docs.oracle.com/javase/specs/jls/se8/html/jls-15.html#jls-15.26
            Expression target = n.getTarget();
            while (target is EnclosedExpr) {
                target = ((EnclosedExpr) target).getInner();
            }
            if (target is NameExpr || target is ArrayAccessExpr || target is FieldAccessExpr) {
                return;
            }
            reporter.report(n.getTarget(), "Illegal left hand side of an assignment.");
        }), new TreeVisitorValidator((node, problemReporter) -> {
            NodeMetaModel mm = node.getMetaModel();
            for (PropertyMetaModel ppm : mm.getAllPropertyMetaModels()) {
                if (ppm.isNonEmpty()) {
                    if (ppm.isNodeList()) {
                        NodeList<?> value = (NodeList<?>) ppm.getValue(node);
                        if (value.isEmpty()) {
                            problemReporter.report(node, "%s.%s can not be empty.", mm.getTypeName(), ppm.getName());
                        }
                    }
                    // No need to check empty strings, it should be impossible to set them to ""
                }
            }
        }));
    }
}
