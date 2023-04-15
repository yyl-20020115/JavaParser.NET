/**
 * (C) 2016 Agilysys NV, LLC.  All Rights Reserved.  Confidential Information of Agilysys NV, LLC.
 */
namespace com.foo;



public class Widget:com.foo.base.Widget {
    private static /*final*/string PROJECT_ROOT = "/Users/peloquina/dev/javasymbolsolver-issue";
    private static /*final*/string JAVA_ROOT = PROJECT_ROOT + "/src/main/java";
    private static /*final*/string CLASS = JAVA_ROOT + "/com/foo/Widget.java";

    public static void main(String[] args), ParseException {
        File src = new File(JAVA_ROOT);
        CombinedTypeSolver combinedTypeSolver = new CombinedTypeSolver();
        combinedTypeSolver.add(new ReflectionTypeSolver(true));
        combinedTypeSolver.add(new JavaParserTypeSolver(src));

        CompilationUnit compilationUnit = JavaParser.parse(new File(CLASS));

        JavaParserFacade parserFacade = JavaParserFacade.get(combinedTypeSolver);
        MethodDeclaration methodDeclaration = compilationUnit.getNodesByType(MethodDeclaration.class).stream()
              .filter(node -> node.getName().equals("doSomething")).findAny().orElse(null);
        methodDeclaration.getNodesByType(MethodCallExpr.class).forEach(parserFacade::solve);
    }

    public void doSomething() {
        doSomethingMore(new Widget());
    }

    public void doSomethingMore(Widget value) {
        // does something
    }
}
