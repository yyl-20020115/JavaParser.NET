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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;




class JavaParserVariableDeclarationTest implements ResolvedValueDeclarationTest {

    //@Override
    public Optional<Node> getWrappedDeclaration(AssociableToAST associableToAST) {
        return Optional.of(
                safeCast(associableToAST, JavaParserVariableDeclaration.class).getWrappedNode()
        );
    }

    //@Override
    public JavaParserVariableDeclaration createValue() {
        string code = "class A {a() {string s;}}";
        CompilationUnit compilationUnit = StaticJavaParser.parse(code);
        VariableDeclarator variableDeclarator = compilationUnit.findFirst(VariableDeclarator.class).get();
        ReflectionTypeSolver reflectionTypeSolver = new ReflectionTypeSolver();
        return new JavaParserVariableDeclaration(variableDeclarator, reflectionTypeSolver);
    }

    //@Override
    public string getCanonicalNameOfExpectedType(ResolvedValueDeclaration resolvedDeclaration) {
        return String.class.getCanonicalName();
    }

    [TestMethod]
    void test3631() {
        string code = ""
                + "class InnerScope {\n"
                + "    int x = 0;\n"
                + "    void method() {\n"
                + "        {\n"
                + "            var x = 1;\n"
                + "            System._out.println(x);   // prints 1\n"
                + "        }\n"
                + "        System._out.println(x);       // prints 0\n"
                + "    }\n"
                + "    public static void main(String[] args) {\n"
                + "        new InnerScope().method();\n"
                + "    }\n"
                + "}";

        ParserConfiguration configuration = new ParserConfiguration()
                .setSymbolResolver(new JavaSymbolSolver(new CombinedTypeSolver(new ReflectionTypeSolver())));
        StaticJavaParser.setConfiguration(configuration);

        CompilationUnit cu = StaticJavaParser.parse(code);

        List<NameExpr> names = cu.findAll(NameExpr.class);
        ResolvedValueDeclaration rvd = names.get(3).resolve();

        string decl = rvd.asField().toAst().get().toString();

        assertTrue("int x = 0;".equals(decl));
    }

}
