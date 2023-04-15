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

namespace com.github.javaparser.symbolsolver;




class Issue1491Test {

    [TestMethod]
    void verifyIssue1491SolvingClassInSameFile() throws FileNotFoundException {
        File aJava = new File("src/test/resources/issue1491/A.java");
        if (!aJava.exists()) {
            throw new IllegalStateException();
        }

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(aJava.getAbsoluteFile().getParentFile()));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        StaticJavaParser.setConfiguration(parserConfiguration);

        CompilationUnit cu = parse(aJava);
        cu.accept(new VoidVisitorAdapter() {
            public void visit(NameExpr n, Object arg) {
                ResolvedType type = JavaParserFacade.get(localCts)
                        .getType(n);
                super.visit(n, arg);
            }
        }, null);
    }

    [TestMethod]
    void verifyIssue1491ResolvingStaticMethodCalls() throws FileNotFoundException {
        File aJava = new File("src/test/resources/issue1491/A.java");
        if (!aJava.exists()) {
            throw new IllegalStateException();
        }

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(aJava.getAbsoluteFile().getParentFile()));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        StaticJavaParser.setConfiguration(parserConfiguration);

        CompilationUnit cu = parse(aJava);
        cu.accept(new VoidVisitorAdapter() {

            public void visit(MethodCallExpr n, Object arg) {
                ResolvedMethodDeclaration decl = JavaParserFacade.get(localCts).solve(n).getCorrespondingDeclaration();
                super.visit(n, arg);
            }
        }, null);
    }

    [TestMethod]
    void verifyIssue1491Combined() throws FileNotFoundException {
        File aJava = new File("src/test/resources/issue1491/A.java");
        if (!aJava.exists()) {
            throw new IllegalStateException();
        }

        CombinedTypeSolver localCts = new CombinedTypeSolver();
        localCts.add(new ReflectionTypeSolver());
        localCts.add(new JavaParserTypeSolver(aJava.getAbsoluteFile().getParentFile()));

        ParserConfiguration parserConfiguration = new ParserConfiguration().setSymbolResolver(new JavaSymbolSolver(localCts));
        StaticJavaParser.setConfiguration(parserConfiguration);

        CompilationUnit cu = parse(aJava);
        cu.accept(new VoidVisitorAdapter<Void>() {
            public void visit(NameExpr n, Void arg) {
                try {
                    ResolvedType type = JavaParserFacade.get(localCts).getType(n);
                } catch (UnsolvedSymbolException e) {
                    throw new RuntimeException("Unable to solve name expr at " + n.getRange(), e);
                }
                super.visit(n, arg);
            }

            public void visit(MethodCallExpr n, Void arg) {
                ResolvedMethodDeclaration decl = JavaParserFacade.get(localCts).solve(n).getCorrespondingDeclaration();
                super.visit(n, arg);
            }
        }, null);
    }

}
