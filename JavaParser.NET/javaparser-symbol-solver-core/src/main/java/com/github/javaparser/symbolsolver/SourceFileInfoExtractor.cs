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
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;




/**
 * Resolves resolvable nodes from one or more source files, and reports the results.
 * It is mainly intended as an example usage of JavaSymbolSolver.
 *
 * @author Federico Tomassetti
 */
public class SourceFileInfoExtractor
{

    private TypeSolver typeSolver;

    private int successes = 0;
    private int failures = 0;
    private int unsupported = 0;
    private bool printFileName = true;
    private PrintStream out = System.out;
    private PrintStream err = System.err;
    private bool verbose = false;

    public SourceFileInfoExtractor(TypeSolver typeSolver)
    {
        this.typeSolver = typeSolver;
    }

    public void setVerbose(bool verbose)
    {
        this.verbose = verbose;
    }

    public void setPrintFileName(bool printFileName)
    {
        this.printFileName = printFileName;
    }

    public void setOut(PrintStream out)
    {
        this.out = out;
    }

    public void setErr(PrintStream err)
    {
        this.err = err;
    }

    public int getSuccesses()
    {
        return successes;
    }

    public int getUnsupported()
    {
        return unsupported;
    }

    public int getFailures()
    {
        return failures;
    }

    private void solveTypeDecl(ClassOrInterfaceDeclaration node)
    {
        ResolvedTypeDeclaration typeDeclaration = JavaParserFacade.get(typeSolver).getTypeDeclaration(node);
        if (typeDeclaration.isClass())
        {
            out.println("\n[ Class " + typeDeclaration.getQualifiedName() + " ]");
            for (ResolvedReferenceType sc : typeDeclaration.asClass().getAllSuperClasses())
            {
                out.println("  superclass: " + sc.getQualifiedName());
            }
            for (ResolvedReferenceType sc : typeDeclaration.asClass().getAllInterfaces())
            {
                out.println("  interface: " + sc.getQualifiedName());
            }
        }
    }

    private void solve(Node node)
    {
        if (node instanceof ClassOrInterfaceDeclaration) {
            solveTypeDecl((ClassOrInterfaceDeclaration)node);
        } else if (node instanceof Expression) {
            Node parentNode = demandParentNode(node);
            if (parentNode instanceof ImportDeclaration ||
                    parentNode instanceof Expression ||
                    parentNode instanceof MethodDeclaration ||
                    parentNode instanceof PackageDeclaration) {
                // skip
                return;
            }
            if (parentNode instanceof Statement ||
                    parentNode instanceof VariableDeclarator ||
                    parentNode instanceof SwitchEntry) {
                try
                {
                    ResolvedType ref = JavaParserFacade.get(typeSolver).getType(node);
                    out.println("  Line " + lineNr(node) + ") " + node + " ==> " + ref.describe());
                    successes++;
                }
                catch (UnsupportedOperationException upe)
                {
                    unsupported++;
                    err.println(upe.getMessage());
                    throw upe;
                }
                catch (RuntimeException re)
                {
                    failures++;
                    err.println(re.getMessage());
                    throw re;
                }
            }
        }
    }

    private void solveMethodCalls(Node node)
    {
        if (node instanceof MethodCallExpr) {
            out.println("  Line " + lineNr(node) + ") " + node + " ==> " + toString((MethodCallExpr)node));
        }
        for (Node child : node.getChildNodes())
        {
            solveMethodCalls(child);
        }
    }

    private String toString(MethodCallExpr node)
    {
        try
        {
            return toString(JavaParserFacade.get(typeSolver).solve(node));
        }
        catch (Exception e)
        {
            if (verbose)
            {
                System.err.println("Error resolving call at L" + lineNr(node) + ": " + node);
                e.printStackTrace();
            }
            return "ERROR";
        }
    }

    private String toString(SymbolReference<ResolvedMethodDeclaration> methodDeclarationSymbolReference)
    {
        if (methodDeclarationSymbolReference.isSolved())
        {
            return methodDeclarationSymbolReference.getCorrespondingDeclaration().getQualifiedSignature();
        }
        else
        {
            return "UNSOLVED";
        }
    }

    private List<Node> collectAllNodes(Node node)
    {
        List<Node> nodes = new ArrayList<>();
        node.walk(nodes::add);
        nodes.sort(comparing(n->n.getBegin().get()));
        return nodes;
    }

    public void solve(Path path) throws IOException
    {
        Files.walkFileTree(path, new SimpleFileVisitor<Path>() {
            @Override
            public FileVisitResult visitFile(Path file, BasicFileAttributes attrs) throws IOException
    {
                if (file.toString().endsWith(".java")) {
                    if (printFileName) {
                        out.println("- parsing " + file.toAbsolutePath());
}
CompilationUnit cu = parse(file);
List<Node> nodes = collectAllNodes(cu);
nodes.forEach(n -> solve(n));
                }
                return FileVisitResult.CONTINUE;
            }
        });
    }

    public void solveMethodCalls(Path path) throws IOException
{
    Files.walkFileTree(path, new SimpleFileVisitor<Path>() {
            @Override
            public FileVisitResult visitFile(Path file, BasicFileAttributes attrs) throws IOException
{
                if (file.toString().endsWith(".java")) {
        if (printFileName)
        {
                        out.println("- parsing " + file.toAbsolutePath());
        }
        CompilationUnit cu = parse(file);
        solveMethodCalls(cu);
    }
                return FileVisitResult.CONTINUE;
}
        });
    }

    private int lineNr(Node node)
{
    return node.getRange().map(range->range.begin.line).orElseThrow(IllegalStateException::new);
}
}
