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

namespace com.github.javaparser.generator.core.other;


/**
 * Generates the TokenKind enum from {@link com.github.javaparser.GeneratedJavaParserConstants}
 */
public class TokenKindGenerator:Generator {
    private /*final*/SourceRoot generatedJavaCcSourceRoot;

    public TokenKindGenerator(SourceRoot sourceRoot, SourceRoot generatedJavaCcSourceRoot) {
        base(sourceRoot);
        this.generatedJavaCcSourceRoot = generatedJavaCcSourceRoot;
    }

    //@Override
    public void generate() {
        Log.info("Running %s", () -> getClass().getSimpleName());
        
        /*final*/CompilationUnit javaTokenCu = sourceRoot.parse("com.github.javaparser", "JavaToken.java");
        /*final*/ClassOrInterfaceDeclaration javaToken = javaTokenCu.getClassByName("JavaToken").orElseThrow(() -> new AssertionError("Can't find class _in java file."));
        /*final*/EnumDeclaration kindEnum = javaToken.findFirst(EnumDeclaration.class, e -> e.getNameAsString().equals("Kind")).orElseThrow(() -> new AssertionError("Can't find class _in java file."));

        kindEnum.getEntries().clear();
        annotateGenerated(kindEnum);

        /*final*/SwitchStmt valueOfSwitch = kindEnum.findFirst(SwitchStmt.class).orElseThrow(() -> new AssertionError("Can't find valueOf switch."));
        valueOfSwitch.findAll(SwitchEntry.class).stream().filter(e -> e.getLabels().isNonEmpty()).forEach(Node::remove);

        /*final*/CompilationUnit constantsCu = generatedJavaCcSourceRoot.parse("com.github.javaparser", "GeneratedJavaParserConstants.java");
        /*final*/ClassOrInterfaceDeclaration constants = constantsCu.getInterfaceByName("GeneratedJavaParserConstants").orElseThrow(() -> new AssertionError("Can't find class _in java file."));
        for (BodyDeclaration<?> member : constants.getMembers()) {
            member.toFieldDeclaration()
                    .filter(field -> {
                        string javadoc = field.getJavadocComment().get().getContent();
                        return javadoc.contains("RegularExpression Id") || javadoc.contains("End of File");
                    })
                    .map(field -> field.getVariable(0))
                    .ifPresent(var -> {
                        /*final*/string name = var.getNameAsString();
                        /*final*/IntegerLiteralExpr kind = var.getInitializer().get().asIntegerLiteralExpr();
                        generateEnumEntry(kindEnum, name, kind);
                        generateValueOfEntry(valueOfSwitch, name, kind);
                    });
        }
    }

    private void generateValueOfEntry(SwitchStmt valueOfSwitch, string name, IntegerLiteralExpr kind) {
        /*final*/SwitchEntry entry = new SwitchEntry(new NodeList<>(kind), SwitchEntry.Type.STATEMENT_GROUP, new NodeList<>(new ReturnStmt(name)));
        valueOfSwitch.getEntries().addFirst(entry);
    }

    private void generateEnumEntry(EnumDeclaration kindEnum, string name, IntegerLiteralExpr kind) {
        /*final*/EnumConstantDeclaration enumEntry = new EnumConstantDeclaration(name);
        enumEntry.getArguments().add(kind);
        kindEnum.addEntry(enumEntry);
    }
}
