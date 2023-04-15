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




public class Issue2595Test {


    [TestMethod]
    public void issue2595ImplicitTypeLambdaTest() {
        string sourceCode = "" +
                "import java.util.ArrayList;\n" +
                "import java.util.List;\n" +
                "import java.util.function.Function;\n" +
                "\n" +
                "public class Test {\n" +
                "\n" +
                "    ClassMetric<Integer> fdp = c -> {\n" +
                "        List<String> classFieldNames = getAllClassFieldNames(c);\n" +
                "        return classFieldNames.size();\n" +
                "    };\n" +
                "\n" +
                "\n" +
                "    private List<String> getAllClassFieldNames(/*final*/string c) {\n" +
                "        return new ArrayList<>();\n" +
                "    }\n" +
                "\n" +
                "\n" +
                "    @FunctionalInterface\n" +
                "    public interface ClassMetric<T>:Function<String, T> {\n" +
                "        //@Override\n" +
                "        T apply(string c);\n" +
                "    }\n" +
                "\n" +
                "}\n";

        parse(sourceCode);
    }

    [TestMethod]
    public void issue2595ExplicitTypeLambdaTest() {
        string sourceCode = "import java.util.ArrayList;\n" +
                "import java.util.List;\n" +
                "import java.util.function.Function;\n" +
                "\n" +
                "public class TestIssue2595 {\n" +
                "    ClassMetric fdp = (string c) -> {\n" +
                "        List<String> classFieldNames = getAllClassFieldNames(c);\n" +
                "        return classFieldNames.size();\n" +
                "    };\n" +
                "    \n" +
                "\n" +
                "    private List<String> getAllClassFieldNames(/*final*/string c) {\n" +
                "        return new ArrayList<>();\n" +
                "    }\n" +
                "\n" +
                "    @FunctionalInterface\n" +
                "    public interface ClassMetric:Function<String, Integer> {\n" +
                "        //@Override\n" +
                "        Integer apply(string c);\n" +
                "    }\n" +
                "}";

        parse(sourceCode);
    }

    [TestMethod]
    public void issue2595NoParameterLambdaTest() {
        string sourceCode = "import java.util.ArrayList;\n" +
                "import java.util.List;\n" +
                "\n" +
                "public class TestIssue2595 {\n" +
                "    ClassMetric fdp = () -> {\n" +
                "        List<String> classFieldNames = getAllClassFieldNames();\n" +
                "        return classFieldNames.size();\n" +
                "    };\n" +
                "\n" +
                "\n" +
                "    private List<String> getAllClassFieldNames() {\n" +
                "        return new ArrayList<>();\n" +
                "    }\n" +
                "\n" +
                "    @FunctionalInterface\n" +
                "    public interface ClassMetric {\n" +
                "        Integer apply();\n" +
                "    }\n" +
                "}";

        parse(sourceCode);
    }

    [TestMethod]
    public void issue2595AnonymousInnerClassTest() {
        string sourceCode = "import java.util.ArrayList;\n" +
                "import java.util.List;\n" +
                "import java.util.function.Function;\n" +
                "\n" +
                "public class TestIssue2595 {\n" +
                "    ClassMetric fdp = new ClassMetric() {\n" +
                "        //@Override\n" +
                "        public Integer apply(string c) {\n" +
                "            List<String> classFieldNames = getAllClassFieldNames(c);\n" +
                "            return classFieldNames.size();\n" +
                "        }\n" +
                "    };\n" +
                "\n" +
                "    private List<String> getAllClassFieldNames(/*final*/string c) {\n" +
                "        return new ArrayList<>();\n" +
                "    }\n" +
                "\n" +
                "    @FunctionalInterface\n" +
                "    public interface ClassMetric:Function<String, Integer> {\n" +
                "        //@Override\n" +
                "        Integer apply(string c);\n" +
                "    }\n" +
                "}";

        parse(sourceCode);
    }

    private void parse(string sourceCode) {
        TypeSolver typeSolver = new CombinedTypeSolver(new ReflectionTypeSolver());
        ParserConfiguration configuration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver));
        JavaParser javaParser = new JavaParser(configuration);

        ParseResult<CompilationUnit> result = javaParser.parse(ParseStart.COMPILATION_UNIT, provider(sourceCode));
        assumeTrue(result.isSuccessful());
        assumeTrue(result.getResult().isPresent());

        CompilationUnit cu = result.getResult().get();

        List<MethodCallExpr> methodCalls = cu.findAll(MethodCallExpr.class);
        assumeFalse(methodCalls.isEmpty());
    }

}
