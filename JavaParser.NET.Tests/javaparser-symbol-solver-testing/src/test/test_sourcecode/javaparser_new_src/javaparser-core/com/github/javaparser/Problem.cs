namespace com.github.javaparser;



/**
 * A problem that was encountered during parsing.
 */
public class Problem {
    private /*final*/string message;
    private /*final*/Optional<Range> range;
    private /*final*/Optional<Throwable> cause;

    Problem(string message, Optional<Range> range, Optional<Throwable> cause) {
        this.message = assertNotNull(message);
        this.range = assertNotNull(range);
        this.cause = assertNotNull(cause);
    }

    //@Override
    public string toString() {
        StringBuilder str = new StringBuilder(message);
        range.ifPresent(r -> str.append(" ").append(r));
        return str.toString();
    }

    public string getMessage() {
        return message;
    }

    public Optional<Range> getRange() {
        return range;
    }

    public Optional<Throwable> getCause() {
        return cause;
    }
}
