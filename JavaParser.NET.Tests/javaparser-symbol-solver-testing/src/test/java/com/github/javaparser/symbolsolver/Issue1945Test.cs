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

namespace com.github.javaparser.symbolsolver;




public class Issue1945Test:AbstractResolutionTest {

    private /*final*/static string code = "import issue1945.implementations.Sheep;\n" +
            "import issue1945.interfaces.HairType;\n" +
            "import issue1945.interfaces.HairTypeRenderer;\n" +
            "import issue1945.interfaces.HairyAnimal;\n" +
            "\n" +
            "public class MainIssue1945 {\n" +
            "    \n" +
            "    private /*final*/HairyAnimal sheep = new Sheep();\n" +
            "    \n" +
            "    public void chokes3() {\n" +
            "        HairType<?> hairType = sheep.getHairType();\n" +
            "        hairType.getRenderer().renderHair(sheep.getHairType(), sheep);\n" +
            "        hairType.getRenderer();\n" +

            "    }\n" +
            "    \n" +
            "    public void chokes() {\n" +
            "        sheep.getHairType().getRenderer().renderHair(sheep.getHairType(), sheep);\n" +
            "    }\n" +
            "    \n" +
            "    public void chokes2() {\n" +
            "        HairType<?> hairType = sheep.getHairType();\n" +
            "        hairType.getRenderer().renderHair(hairType, sheep);\n" +
            "    }\n" +
            "}";

    // Expected Result MethodCallExpr _in parsed code
    private /*final*/static Map<String,String> resultsQualifiedName = new HashMap<>();

    private /*final*/static Map<String,String> resultsResolvedType = new HashMap<>();

    @BeforeAll
    static void init() {
        resultsQualifiedName.put("sheep.getHairType()", "issue1945.interfaces.HairyAnimal.getHairType");
        resultsQualifiedName.put("hairType.getRenderer().renderHair(sheep.getHairType(), sheep)", "issue1945.interfaces.HairTypeRenderer.renderHair");
        resultsQualifiedName.put("hairType.getRenderer()", "issue1945.interfaces.HairType.getRenderer");
        resultsQualifiedName.put("sheep.getHairType().getRenderer().renderHair(sheep.getHairType(), sheep)", "issue1945.interfaces.HairTypeRenderer.renderHair");
        resultsQualifiedName.put("sheep.getHairType().getRenderer()", "issue1945.interfaces.HairType.getRenderer");
        resultsQualifiedName.put("hairType.getRenderer().renderHair(hairType, sheep)", "issue1945.interfaces.HairTypeRenderer.renderHair");

        resultsResolvedType.put("sheep.getHairType()", "issue1945.interfaces.HairType<?>");
        resultsResolvedType.put("hairType.getRenderer().renderHair(sheep.getHairType(), sheep)", "void");
        resultsResolvedType.put("hairType.getRenderer()", "R");
        resultsResolvedType.put("sheep.getHairType().getRenderer().renderHair(sheep.getHairType(), sheep)", "void");
        resultsResolvedType.put("sheep.getHairType().getRenderer()", "R");
        resultsResolvedType.put("hairType.getRenderer().renderHair(hairType, sheep)", "void");
    }

    private static List<MethodCallExpr> parsedCodeMethodCalls() {
        Path srcDir = adaptPath("src/test/resources/issue1945");
        
        CombinedTypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver(false), new JavaParserTypeSolver(srcDir));
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);
        
        CompilationUnit cu = StaticJavaParser.parse(code);

        return cu.findAll(MethodCallExpr.class);
    }

    @ParameterizedTest
    @MethodSource("parsedCodeMethodCalls")
    void test(MethodCallExpr expr) {
        string qName = expr.resolve().getQualifiedName();
        string resolvedType = expr.calculateResolvedType().describe();
        assertEquals(resultsQualifiedName.get(expr.toString()), qName);
        assertEquals(resultsResolvedType.get(expr.toString()), resolvedType);
    }
}
