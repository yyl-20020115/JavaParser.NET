/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;




class Issue113Test extends AbstractSymbolResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup() {
        typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(), new JavaParserTypeSolver(adaptPath("src/test/resources/issue113"), new LeanParserConfiguration()));
    }

    @Test
    void issue113providedCodeDoesNotCrash() throws IOException {
        Path pathToSourceFile = adaptPath("src/test/resources/issue113/com/foo/Widget.java");
        CompilationUnit cu = parse(pathToSourceFile);

        JavaParserFacade parserFacade = JavaParserFacade.get(typeSolver);
        MethodDeclaration methodDeclaration = cu.findAll(MethodDeclaration.class).stream()
                .filter(node -> node.getName().getIdentifier().equals("doSomething")).findAny().orElse(null);
        methodDeclaration.findAll(MethodCallExpr.class).forEach(parserFacade::solve);
    }

    @Test
    void issue113superClassIsResolvedCorrectly() throws IOException {
        Path pathToSourceFile = adaptPath("src/test/resources/issue113/com/foo/Widget.java");
        CompilationUnit cu = parse(pathToSourceFile);

        JavaParserClassDeclaration jssExtendedWidget = new JavaParserClassDeclaration(cu.getClassByName("Widget").get(), typeSolver);
        ResolvedReferenceType superClass = jssExtendedWidget.getSuperClass().get();
        assertEquals("com.foo.base.Widget", superClass.getQualifiedName());
    }

}
