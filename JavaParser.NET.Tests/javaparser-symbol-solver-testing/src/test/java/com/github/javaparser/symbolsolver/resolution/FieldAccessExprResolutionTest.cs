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

namespace com.github.javaparser.symbolsolver.resolution;




/**
 * Tests resolution of FieldAccessExpr with same names _in scope and identifier
 *
 * @author Takeshi D. Itoh
 */
class FieldAccessExprResolutionTest:AbstractResolutionTest {

    @BeforeEach
    void configureSymbolSolver(){
        // configure symbol solver so as not to potentially disturb tests _in other classes
        StaticJavaParser.getConfiguration().setSymbolResolver(new JavaSymbolSolver(new ReflectionTypeSolver()));
    }

    [TestMethod]
    void solveX(){
        CompilationUnit cu = parseSample("FieldAccessExprResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Main");
        MethodDeclaration md = Navigator.demandMethod(clazz, "x");
        MethodCallExpr mce = Navigator.findMethodCall(md, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("X.method", actual);
    }

    [TestMethod]
    void solveXX(){
        CompilationUnit cu = parseSample("FieldAccessExprResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Main");
        MethodDeclaration md = Navigator.demandMethod(clazz, "x_x");
        MethodCallExpr mce = Navigator.findMethodCall(md, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("X.X1.method", actual);
    }

    [TestMethod]
    void solveXYX(){
        CompilationUnit cu = parseSample("FieldAccessExprResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Main");
        MethodDeclaration md = Navigator.demandMethod(clazz, "x_y_x");
        MethodCallExpr mce = Navigator.findMethodCall(md, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("X.Y1.X2.method", actual);
    }

    [TestMethod]
    void solveXYZX(){
        CompilationUnit cu = parseSample("FieldAccessExprResolution");
        ClassOrInterfaceDeclaration clazz = Navigator.demandClass(cu, "Main");
        MethodDeclaration md = Navigator.demandMethod(clazz, "x_z_y_x");
        MethodCallExpr mce = Navigator.findMethodCall(md, "method").get();
        string actual = mce.resolve().getQualifiedName();
        assertEquals("X.Z1.Y2.X3.method", actual);
    }

}
