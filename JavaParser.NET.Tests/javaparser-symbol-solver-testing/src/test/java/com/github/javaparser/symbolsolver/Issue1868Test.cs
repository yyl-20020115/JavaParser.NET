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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;





public class Issue1868Test extends AbstractSymbolResolutionTest {

    @Test
    public void test() {
        
        Path testResources= adaptPath("src/test/resources/issue1868");
        
        CombinedTypeSolver typeSolver = new CombinedTypeSolver();
        typeSolver.add(new ReflectionTypeSolver());
        typeSolver.add(new JavaParserTypeSolver(testResources));

        StaticJavaParser.setConfiguration(new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(typeSolver)));

        String s = 
                "class A {\n" + 
                "    public void foo() {\n" + 
                "        toArray(new String[0]);\n" + 
                "    }\n" + 
                "    public void bar() {\n" + 
                "        B b = null;\n" + 
                "        b.toArray(new String[0]);\n" + 
                "    }\n" + 
                "    public <T> T[] toArray(T[] tArray) {\n" + 
                "        // ...\n" + 
                "    }\n" + 
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        
        List<MethodCallExpr> mces = cu.findAll(MethodCallExpr.class);
        
        assertEquals("toArray(new String[0]) resolved to A.toArray",String.format("%s resolved to %s", mces.get(0), mces.get(0).resolve().getQualifiedName()));
        assertEquals("b.toArray(new String[0]) resolved to B.toArray",String.format("%s resolved to %s", mces.get(1), mces.get(1).resolve().getQualifiedName()));
        
    }

}
