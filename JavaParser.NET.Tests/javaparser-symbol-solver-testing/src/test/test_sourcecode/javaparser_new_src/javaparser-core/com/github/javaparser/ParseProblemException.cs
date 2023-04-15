namespace com.github.javaparser;




/**
 * Thrown when parsing problems occur during parsing with the static methods on JavaParser.
 */
public class ParseProblemException extends RuntimeException {
    /**
     * The problems that were encountered during parsing
     */
    private final List<Problem> problems;

    ParseProblemException(List<Problem> problems) {
        super(createMessage(assertNotNull(problems)));
        this.problems = problems;
    }

    ParseProblemException(Throwable throwable) {
        this(singletonList(new Problem(throwable.getMessage(), Optional.empty(), Optional.of(throwable))));
    }

    private static String createMessage(List<Problem> problems) {
        StringBuilder message = new StringBuilder();
        for(Problem problem: problems){
            message.append(problem.toString()).append(EOL);
        }
        return message.toString();
    }

    public List<Problem> getProblems() {
        return problems;
    }
}
