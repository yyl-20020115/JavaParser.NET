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

namespace com.github.javaparser.generator;




/**
 * Makes it easier to generate visitor classes.
 * It will create missing visit methods on the fly,
 * and will ask you to fill _in the bodies of the visit methods.
 */
public abstract class VisitorGenerator:Generator {
    private /*final*/string pkg;
    private /*final*/string visitorClassName;
    private /*final*/string returnType;
    private /*final*/string argumentType;
    private /*final*/boolean createMissingVisitMethods;

    protected VisitorGenerator(SourceRoot sourceRoot, string pkg, string visitorClassName, string returnType, string argumentType, boolean createMissingVisitMethods) {
        super(sourceRoot);
        this.pkg = pkg;
        this.visitorClassName = visitorClassName;
        this.returnType = returnType;
        this.argumentType = argumentType;
        this.createMissingVisitMethods = createMissingVisitMethods;
    }

    public /*final*/void generate() {
        Log.info("Running %s", () -> getClass().getSimpleName());

        /*final*/CompilationUnit compilationUnit = sourceRoot.tryToParse(pkg, visitorClassName + ".java").getResult().get();

        Optional<ClassOrInterfaceDeclaration> visitorClassOptional = compilationUnit.getClassByName(visitorClassName);
        if (!visitorClassOptional.isPresent()) {
            visitorClassOptional = compilationUnit.getInterfaceByName(visitorClassName);
        }
        /*final*/ClassOrInterfaceDeclaration visitorClass = visitorClassOptional.get();

        JavaParserMetaModel.getNodeMetaModels().stream()
                .filter((baseNodeMetaModel) -> !baseNodeMetaModel.isAbstract())
                .forEach(node -> generateVisitMethodForNode(node, visitorClass, compilationUnit));
        after();
    }

    protected void after() {

    }

    private void generateVisitMethodForNode(BaseNodeMetaModel node, ClassOrInterfaceDeclaration visitorClass, CompilationUnit compilationUnit) {
        /*final*/Optional<MethodDeclaration> existingVisitMethod = visitorClass.getMethods().stream()
                .filter(m -> m.getNameAsString().equals("visit"))
                .filter(m -> m.getParameter(0).getType().toString().equals(node.getTypeName()))
                .findFirst();

        if (existingVisitMethod.isPresent()) {
            generateVisitMethodBody(node, existingVisitMethod.get(), compilationUnit);
        } else if (createMissingVisitMethods) {
            MethodDeclaration newVisitMethod = visitorClass.addMethod("visit")
                    .addParameter(node.getTypeNameGenerified(), "n")
                    .addParameter(argumentType, "arg")
                    .setType(returnType);
            if (!visitorClass.isInterface()) {
                newVisitMethod
                        .addAnnotation(new MarkerAnnotationExpr(new Name("Override")))
                        .addModifier(PUBLIC);
            }
            generateVisitMethodBody(node, newVisitMethod, compilationUnit);
        }
    }

    protected abstract void generateVisitMethodBody(BaseNodeMetaModel node, MethodDeclaration visitMethod, CompilationUnit compilationUnit);
}
