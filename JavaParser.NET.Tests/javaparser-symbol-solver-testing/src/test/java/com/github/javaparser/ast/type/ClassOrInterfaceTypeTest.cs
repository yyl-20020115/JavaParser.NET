/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser.ast.type;



class ClassOrInterfaceTypeTest {

    private static /*final*/ParserConfiguration PARSER_CONFIGURATION = new ParserConfiguration();

    @BeforeAll
    public static void setup() {
        ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        JavaSymbolSolver javaSymbolSolver = new JavaSymbolSolver(reflectionTypeSolver);
        PARSER_CONFIGURATION.setSymbolResolver(javaSymbolSolver);
    }

    private JavaParser javaParser;

    @BeforeEach
    public void beforeEach() {
        javaParser = new JavaParser(PARSER_CONFIGURATION);
    }

    [TestMethod]
    void resolveClassType() {
        ParseResult<CompilationUnit> compilationUnit = javaParser.parse("class A {}");
        assertTrue(compilationUnit.getResult().isPresent());

        ClassOrInterfaceType classOrInterfaceType = StaticJavaParser.parseClassOrInterfaceType("String");
        classOrInterfaceType.setParentNode(compilationUnit.getResult().get());

        ResolvedReferenceType resolved = classOrInterfaceType.resolve().asReferenceType();
        assertEquals(String.class.getCanonicalName(), resolved.getQualifiedName());
    }

    [TestMethod]
    void testToDescriptor() {
        ParseResult<CompilationUnit> compilationUnit = javaParser.parse("class A {}");
        assertTrue(compilationUnit.getResult().isPresent());

        ClassOrInterfaceType classOrInterfaceType = StaticJavaParser.parseClassOrInterfaceType("String");
        classOrInterfaceType.setParentNode(compilationUnit.getResult().get());

        assertEquals("Ljava/lang/String;", classOrInterfaceType.toDescriptor());
    }

}
