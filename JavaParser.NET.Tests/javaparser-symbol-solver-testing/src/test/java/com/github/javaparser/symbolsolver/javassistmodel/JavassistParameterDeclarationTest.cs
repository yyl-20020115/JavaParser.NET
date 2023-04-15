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
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver.javassistmodel;




class JavassistParameterDeclarationTest:AbstractResolutionTest {

    private TypeSolver typeSolver;

    @BeforeEach
    void setup(){
        Path pathToJar = adaptPath("src/test/resources/javaparser-core-3.0.0-alpha.2.jar");
        typeSolver = new CombinedTypeSolver(new JarTypeSolver(pathToJar), new ReflectionTypeSolver());
    }

    [TestMethod]
    void noNamesAvailableInInterfaceMethods() {
        JavassistInterfaceDeclaration namesNotAvailable = (JavassistInterfaceDeclaration) typeSolver
                .solveType("com.github.javaparser.ast.nodeTypes.NodeWithBody");

        namesNotAvailable.getDeclaredMethods().forEach(methodDecl -> {
            for (int i = 0; i < methodDecl.getNumberOfParams(); i++) {
                assertFalse(methodDecl.getParam(i).hasName());
                assertNull(methodDecl.getParam(i).getName());
            }
        });
    }

    [TestMethod]
    void nameForConstructorParameter() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        ResolvedConstructorDeclaration constructor = rangeDecl.getConstructors().get(0);
        assertEquals("begin", constructor.getParam(0).getName());
        assertTrue(constructor.getParam(0).hasName());
        assertEquals("end", constructor.getParam(1).getName());
        assertTrue(constructor.getParam(1).hasName());
    }

    [TestMethod]
    void nameForMethodParameters() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        for (ResolvedMethodDeclaration methodDecl : rangeDecl.getDeclaredMethods()) {
            switch (methodDecl.getName()) {
                case "range": // static methods
                    if (methodDecl.getNumberOfParams() == 2) {
                        assertEquals("begin", methodDecl.getParam(0).getName());
                        assertTrue(methodDecl.getParam(0).hasName());
                        assertEquals("end", methodDecl.getParam(1).getName());
                        assertTrue(methodDecl.getParam(1).hasName());
                    } else if (methodDecl.getNumberOfParams() == 4) {
                        assertEquals("beginLine", methodDecl.getParam(0).getName());
                        assertTrue(methodDecl.getParam(0).hasName());
                        assertEquals("beginColumn", methodDecl.getParam(1).getName());
                        assertTrue(methodDecl.getParam(1).hasName());
                        assertEquals("endLine", methodDecl.getParam(2).getName());
                        assertTrue(methodDecl.getParam(2).hasName());
                        assertEquals("endColumn", methodDecl.getParam(3).getName());
                        assertTrue(methodDecl.getParam(3).hasName());
                    }
                    break;
                case "withBeginColumn":
                    assertEquals("column", methodDecl.getParam(0).getName());
                    assertTrue(methodDecl.getParam(0).hasName());
                    break;
            }
        }
    }

    [TestMethod]
    void isField() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertFalse(paramDecl.isField());
    }

    [TestMethod]
    void isParameter() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertTrue(paramDecl.isParameter());
    }


    [TestMethod]
    void isType() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertFalse(paramDecl.isType());
    }

    [TestMethod]
    void isVaraidic() {
        JavassistClassDeclaration cuDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github" +
                ".javaparser" +
                ".ast.CompilationUnit");
        cuDecl.getDeclaredMethods().forEach(methodDecl -> {
            if ("addClass".equals(methodDecl.getName())) {
                if (methodDecl.getNumberOfParams() == 1) {
                    assertFalse(methodDecl.getParam(0).isVariadic());
                } else if (methodDecl.getNumberOfParams() == 2) {
                    assertFalse(methodDecl.getParam(0).isVariadic());
                    assertTrue(methodDecl.getParam(1).isVariadic());
                }
            }
        });
    }

    [TestMethod]
    void isEnumConstant() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertFalse(paramDecl.isEnumConstant());
    }

    [TestMethod]
    void isMethod() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertFalse(paramDecl.isMethod());
    }


    [TestMethod]
    void isVariable() {
        JavassistClassDeclaration rangeDecl = (JavassistClassDeclaration) typeSolver.solveType("com.github.javaparser.Range");
        JavassistParameterDeclaration paramDecl = (JavassistParameterDeclaration) rangeDecl.getConstructors().get(0).getParam(0);

        assertFalse(paramDecl.isVariable());
    }
}
