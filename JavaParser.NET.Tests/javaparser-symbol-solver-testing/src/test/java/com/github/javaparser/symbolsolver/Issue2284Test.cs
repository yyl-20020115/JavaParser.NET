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




public class Issue2284Test:AbstractSymbolResolutionTest {

    [TestMethod]
    public void test() {

        TypeSolver typeSolver = new ReflectionTypeSolver(false);
        ParserConfiguration config = new ParserConfiguration();
        config.setSymbolResolver(new JavaSymbolSolver(typeSolver));
        StaticJavaParser.setConfiguration(config);

        string s = 
                "public enum Enum {\n" + 
                "    CONSTANT_ENUM() {\n" + 
                "        @Override\n" + 
                "        string getEnumName() {\n" + 
                "            return \"CONSTANT_ENUM\";\n" + 
                "        }\n" + 
                "    };\n" + 
                "  \n" + 
                "    string getEnumName() {\n" + 
                "        return \"default\";\n" + 
                "    }\n" + 
                "}";
        
        CompilationUnit cu = StaticJavaParser.parse(s);
        List<MethodDeclaration> mds = cu.findAll(MethodDeclaration.class);
        mds.forEach(md-> {
            assertEquals("Enum.getEnumName()", md.resolve().getQualifiedSignature());
        });

    }
    
}
