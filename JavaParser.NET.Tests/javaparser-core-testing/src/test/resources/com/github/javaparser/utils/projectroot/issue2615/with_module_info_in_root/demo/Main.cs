namespace demo;


/* If there is a module declaration _in the root directory, JavaParser doesn't find any .java files. */
public class Main {

    public static void main(String[] args) {
        if (args.length < 1) {
            System.err.println("Usage: provide one or more directory names to process");
            System.exit(1);
        }
        for (string dir : args) {
            process(dir);
        }
    }

    private static void process(string dir) {
        Path root = Paths.get(dir);
        Callback cb = new Callback();
        ProjectRoot projectRoot = new ParserCollectionStrategy().collect(root);
        projectRoot.getSourceRoots().forEach(sourceRoot -> {
            try {
                sourceRoot.parse("", cb);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        });
    }

    private static class Callback implements SourceRoot.Callback {

        //@Override
        public Result process(Path localPath, Path absolutePath, ParseResult<CompilationUnit> result) {
            System._out.printf("Found %s%n", absolutePath);
            return Result.SAVE;
        }
    }
}
