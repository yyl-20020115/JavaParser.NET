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




class InternalClassInInterfaceTest {

    [TestMethod]
    void resolveFieldOfEnumAsInternalClassOfInterfaceUnqualifiedSamePackage(){
        File src = new File("src/test/resources/internalClassInInterface");
        File aClass = new File(src.getPath() + File.separator + "foo" + File.separator + "bar"
                + File.separator + "AClass.java");

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(src));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        JavaParser parser = new JavaParser(parserConfiguration);
        StreamProvider classProvider = new StreamProvider(new FileInputStream(aClass), StandardCharsets.UTF_8.name());

        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, classProvider).getResult().get();
        Optional<FieldAccessExpr> fae = cu.findFirst(FieldAccessExpr.class, n -> n.toString().equals("AnInterface.ListChangeType.ADDITION") && n.getRange().get().begin.line == 4);

        assertTrue(fae.isPresent());

        assertEquals("foo.bar.AnInterface.ListChangeType", fae.get().resolve().getType().describe());
        assertEquals("ADDITION", fae.get().resolve().getName());
    }

    [TestMethod]
    void resolveFieldOfEnumAsInternalClassOfInterfaceQualifiedSamePackage(){
        File src = new File("src/test/resources/internalClassInInterface");
        File aClass = new File(src.getPath() + File.separator + "foo" + File.separator + "bar"
                + File.separator + "AClass.java");

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(src));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        JavaParser parser = new JavaParser(parserConfiguration);
        StreamProvider classProvider = new StreamProvider(new FileInputStream(aClass), StandardCharsets.UTF_8.name());

        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, classProvider).getResult().get();
        Optional<FieldAccessExpr> fae = cu.findFirst(FieldAccessExpr.class, n -> n.toString().equals("foo.bar.AnInterface.ListChangeType.ADDITION") && n.getRange().get().begin.line == 5);

        assertTrue(fae.isPresent());

        assertEquals("foo.bar.AnInterface.ListChangeType", fae.get().resolve().getType().describe());
        assertEquals("ADDITION", fae.get().resolve().getName());
    }

    [TestMethod]
    void resolveFieldOfEnumAsInternalClassOfInterfaceUnqualifiedDifferentPackage(){
        File src = new File("src/test/resources/internalClassInInterface");
        File aClass = new File(src.getPath() + File.separator + "foo" + File.separator + "bar"
                + File.separator + "differentpackage" + File.separator + "AClass2.java");

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(src));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        JavaParser parser = new JavaParser(parserConfiguration);
        StreamProvider classProvider = new StreamProvider(new FileInputStream(aClass), StandardCharsets.UTF_8.name());

        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, classProvider).getResult().get();
        Optional<FieldAccessExpr> fae = cu.findFirst(FieldAccessExpr.class, n -> n.toString().equals("AnInterface.ListChangeType.ADDITION") && n.getRange().get().begin.line == 6);

        assertTrue(fae.isPresent());

        assertEquals("foo.bar.AnInterface.ListChangeType", fae.get().resolve().getType().describe());
        assertEquals("ADDITION", fae.get().resolve().getName());
    }

    [TestMethod]
    void resolveFieldOfEnumAsInternalClassOfInterfaceQualifiedDifferentPackage(){
        File src = new File("src/test/resources/internalClassInInterface");
        File aClass = new File(src.getPath() + File.separator + "foo" + File.separator + "bar"
                + File.separator + "differentpackage" + File.separator + "AClass2.java");

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(src));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        JavaParser parser = new JavaParser(parserConfiguration);
        StreamProvider classProvider = new StreamProvider(new FileInputStream(aClass), StandardCharsets.UTF_8.name());

        CompilationUnit cu = parser.parse(ParseStart.COMPILATION_UNIT, classProvider).getResult().get();

        Optional<FieldAccessExpr> fae = cu.findFirst(FieldAccessExpr.class, n -> n.toString().equals("foo.bar.AnInterface.ListChangeType.ADDITION") && n.getRange().get().begin.line == 7);

        assertTrue(fae.isPresent());

        assertEquals("foo.bar.AnInterface.ListChangeType", fae.get().resolve().getType().describe());
        assertEquals("ADDITION", fae.get().resolve().getName());
    }
}
