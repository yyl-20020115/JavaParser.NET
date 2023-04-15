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

namespace com.github.javaparser.symbolsolver.resolution.javaparser;



class VarTypeTest {
    private /*final*/TypeSolver typeSolver = new ReflectionTypeSolver();
    private /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration()
            .setLanguageLevel(JAVA_10)
            .setSymbolResolver(new JavaSymbolSolver(typeSolver)));

    [TestMethod]
    void resolveAPrimitive() {
        CompilationUnit ast = javaParser.parse(ParseStart.COMPILATION_UNIT, provider("class X{void x(){var abc = 1;}}")).getResult().get();
        VarType varType = ast.findFirst(VarType.class).get();

        ResolvedType resolvedType = varType.resolve();

        assertEquals(ResolvedPrimitiveType.INT, resolvedType);
    }

    [TestMethod]
    void resolveAReferenceType() {
        CompilationUnit ast = javaParser.parse(ParseStart.COMPILATION_UNIT, provider("class X{void x(){var abc = \"\";}}")).getResult().get();
        VarType varType = ast.findFirst(VarType.class).get();

        ResolvedType resolvedType = varType.resolve();

        assertEquals(new ReferenceTypeImpl(new ReflectionClassDeclaration(String.class, typeSolver)), resolvedType);
    }

    [TestMethod]
    void failResolveNoInitializer() {
        assertThrows(IllegalStateException.class, () -> {
            CompilationUnit ast = javaParser.parse(ParseStart.COMPILATION_UNIT, provider("class X{void x(){var abc;}}")).getResult().get();
        VarType varType = ast.findFirst(VarType.class).get();
        varType.resolve();
    });
        
}

    [TestMethod]
    void failResolveWrongLocation() {
        assertThrows(IllegalStateException.class, () -> {
            CompilationUnit ast = javaParser.parse(ParseStart.COMPILATION_UNIT, provider("class X{void x(var x){};}")).getResult().get();
        VarType varType = ast.findFirst(VarType.class).get();
        varType.resolve();
    });
        
}
}
