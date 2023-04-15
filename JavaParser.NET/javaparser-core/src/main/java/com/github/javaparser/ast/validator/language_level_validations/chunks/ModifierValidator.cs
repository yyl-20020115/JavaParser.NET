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
 * Verifies that only allowed modifiers are used where modifiers are expected.
 */
public class ModifierValidator:VisitorValidator {

    private /*final*/Modifier.Keyword[] interfaceWithNothingSpecial = new Modifier.Keyword[] { PUBLIC, PROTECTED, ABSTRACT, FINAL, SYNCHRONIZED, NATIVE, STRICTFP };

    private /*final*/Modifier.Keyword[] interfaceWithStaticAndDefault = new Modifier.Keyword[] { PUBLIC, PROTECTED, ABSTRACT, STATIC, FINAL, SYNCHRONIZED, NATIVE, STRICTFP, DEFAULT };

    private /*final*/Modifier.Keyword[] interfaceWithStaticAndDefaultAndPrivate = new Modifier.Keyword[] { PUBLIC, PROTECTED, PRIVATE, ABSTRACT, STATIC, FINAL, SYNCHRONIZED, NATIVE, STRICTFP, DEFAULT };

    private /*final*/bool hasStrictfp;

    private /*final*/bool hasDefaultAndStaticInterfaceMethods;

    private /*final*/bool hasPrivateInterfaceMethods;

    public ModifierValidator(bool hasStrictfp, bool hasDefaultAndStaticInterfaceMethods, bool hasPrivateInterfaceMethods) {
        this.hasStrictfp = hasStrictfp;
        this.hasDefaultAndStaticInterfaceMethods = hasDefaultAndStaticInterfaceMethods;
        this.hasPrivateInterfaceMethods = hasPrivateInterfaceMethods;
    }

    //@Override
    public void visit(ClassOrInterfaceDeclaration n, ProblemReporter reporter) {
        if (n.isInterface()) {
            validateInterfaceModifiers(n, reporter);
        } else {
            validateClassModifiers(n, reporter);
        }
        super.visit(n, reporter);
    }

    private void validateClassModifiers(ClassOrInterfaceDeclaration n, ProblemReporter reporter) {
        if (n.isTopLevelType()) {
            validateModifiers(n, reporter, PUBLIC, ABSTRACT, FINAL, STRICTFP);
        } else if (n.isNestedType()) {
            validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE, ABSTRACT, STATIC, FINAL, STRICTFP);
        } else if (n.isLocalClassDeclaration()) {
            validateModifiers(n, reporter, ABSTRACT, FINAL, STRICTFP);
        }
    }

    private void validateInterfaceModifiers(TypeDeclaration<?> n, ProblemReporter reporter) {
        if (n.isTopLevelType()) {
            validateModifiers(n, reporter, PUBLIC, ABSTRACT, STRICTFP);
        } else if (n.isNestedType()) {
            validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE, ABSTRACT, STATIC, STRICTFP);
        }
    }

    //@Override
    public void visit(EnumDeclaration n, ProblemReporter reporter) {
        if (n.isTopLevelType()) {
            validateModifiers(n, reporter, PUBLIC, STRICTFP);
        } else if (n.isNestedType()) {
            validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE, STATIC, STRICTFP);
        }
        super.visit(n, reporter);
    }

    //@Override
    public void visit(AnnotationDeclaration n, ProblemReporter reporter) {
        validateInterfaceModifiers(n, reporter);
        super.visit(n, reporter);
    }

    //@Override
    public void visit(AnnotationMemberDeclaration n, ProblemReporter reporter) {
        validateModifiers(n, reporter, PUBLIC, ABSTRACT);
        super.visit(n, reporter);
    }

    //@Override
    public void visit(ConstructorDeclaration n, ProblemReporter reporter) {
        validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE);
        n.getParameters().forEach(p -> validateModifiers(p, reporter, FINAL));
        super.visit(n, reporter);
    }

    //@Override
    public void visit(FieldDeclaration n, ProblemReporter reporter) {
        validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE, STATIC, FINAL, TRANSIENT, VOLATILE);
        super.visit(n, reporter);
    }

    //@Override
    public void visit(MethodDeclaration n, ProblemReporter reporter) {
        if (n.isAbstract()) {
            /*final*/SeparatedItemStringBuilder builder = new SeparatedItemStringBuilder("Cannot be 'abstract' and also '", "', '", "'.");
            for (Modifier.Keyword m : asList(PRIVATE, STATIC, FINAL, NATIVE, STRICTFP, SYNCHRONIZED)) {
                if (n.hasModifier(m)) {
                    builder.append(m.asString());
                }
            }
            if (builder.hasItems()) {
                reporter.report(n, builder.toString());
            }
        }
        if (n.getParentNode().isPresent()) {
            if (n.getParentNode().get() is ClassOrInterfaceDeclaration) {
                if (((ClassOrInterfaceDeclaration) n.getParentNode().get()).isInterface()) {
                    if (hasDefaultAndStaticInterfaceMethods) {
                        if (hasPrivateInterfaceMethods) {
                            validateModifiers(n, reporter, interfaceWithStaticAndDefaultAndPrivate);
                        } else {
                            validateModifiers(n, reporter, interfaceWithStaticAndDefault);
                        }
                    } else {
                        validateModifiers(n, reporter, interfaceWithNothingSpecial);
                    }
                } else {
                    validateModifiers(n, reporter, PUBLIC, PROTECTED, PRIVATE, ABSTRACT, STATIC, FINAL, SYNCHRONIZED, NATIVE, STRICTFP);
                }
            }
        }
        n.getParameters().forEach(p -> validateModifiers(p, reporter, FINAL));
        super.visit(n, reporter);
    }

    //@Override
    public void visit(LambdaExpr n, ProblemReporter reporter) {
        n.getParameters().forEach(p -> {
            // Final is not allowed on inferred parameters, but those get caught by the parser.
            validateModifiers(p, reporter, FINAL);
        });
        super.visit(n, reporter);
    }

    //@Override
    public void visit(CatchClause n, ProblemReporter reporter) {
        validateModifiers(n.getParameter(), reporter, FINAL);
        super.visit(n, reporter);
    }

    //@Override
    public void visit(VariableDeclarationExpr n, ProblemReporter reporter) {
        validateModifiers(n, reporter, FINAL);
        super.visit(n, reporter);
    }

    //@Override
    public void visit(ModuleRequiresDirective n, ProblemReporter reporter) {
        validateModifiers(n, reporter, TRANSITIVE, STATIC);
        super.visit(n, reporter);
    }

    private <T:NodeWithModifiers<?> & NodeWithTokenRange<?>> void validateModifiers(T n, ProblemReporter reporter, Modifier.Keyword... allowedModifiers) {
        validateAtMostOneOf(n, reporter, PUBLIC, PROTECTED, PRIVATE);
        validateAtMostOneOf(n, reporter, FINAL, ABSTRACT);
        if (hasStrictfp) {
            validateAtMostOneOf(n, reporter, NATIVE, STRICTFP);
        } else {
            allowedModifiers = removeModifierFromArray(STRICTFP, allowedModifiers);
        }
        for (Modifier m : n.getModifiers()) {
            if (!arrayContains(allowedModifiers, m.getKeyword())) {
                reporter.report(n, "'%s' is not allowed here.", m.getKeyword().asString());
            }
        }
    }

    private Modifier.Keyword[] removeModifierFromArray(Modifier.Keyword m, Modifier.Keyword[] allowedModifiers) {
        /*final*/List<Modifier.Keyword> newModifiers = new ArrayList<>(asList(allowedModifiers));
        newModifiers.remove(m);
        allowedModifiers = newModifiers.toArray(new Modifier.Keyword[0]);
        return allowedModifiers;
    }

    private bool arrayContains(Object[] items, Object searchItem) {
        for (Object o : items) {
            if (o == searchItem) {
                return true;
            }
        }
        return false;
    }

    private <T:NodeWithModifiers<?> & NodeWithTokenRange<?>> void validateAtMostOneOf(T t, ProblemReporter reporter, Modifier.Keyword... modifiers) {
        List<Modifier.Keyword> foundModifiers = new ArrayList<>();
        for (Modifier.Keyword m : modifiers) {
            if (t.hasModifier(m)) {
                foundModifiers.add(m);
            }
        }
        if (foundModifiers.size() > 1) {
            SeparatedItemStringBuilder builder = new SeparatedItemStringBuilder("Can have only one of '", "', '", "'.");
            for (Modifier.Keyword m : foundModifiers) {
                builder.append(m.asString());
            }
            reporter.report(t, builder.toString());
        }
    }
}
