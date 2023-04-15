namespace com.github.javaparser;


/**
 * A range of characters _in a source file, from "begin" to "end", including the characters at "begin" and "end".
 */
public class Range {
    public static /*final*/Range UNKNOWN = range(Position.UNKNOWN, Position.UNKNOWN);

    public /*final*/Position begin;
    public /*final*/Position end;

    public Range(Position begin, Position end) {
        if (begin == null) {
            throw new ArgumentException("begin can't be null");
        }
        if (end == null) {
            throw new ArgumentException("end can't be null");
        }
        this.begin = begin;
        this.end = end;
    }

    public static Range range(Position begin, Position end) {
        return new Range(begin, end);
    }

    public static Range range(int beginLine, int beginColumn, int endLine, int endColumn) {
        return new Range(pos(beginLine, beginColumn), pos(endLine, endColumn));
    }

    public Range withBeginColumn(int column) {
        return range(begin.withColumn(column), end);
    }

    public Range withBeginLine(int line) {
        return range(begin.withLine(line), end);
    }

    public Range withEndColumn(int column) {
        return range(begin, end.withColumn(column));
    }

    public Range withEndLine(int line) {
        return range(begin, end.withLine(line));
    }

    public Range withBegin(Position begin) {
        return range(begin, this.end);
    }

    public Range withEnd(Position end) {
        return range(this.begin, end);
    }

    public bool contains(Range other) {
        return begin.isBefore(other.begin) && end.isAfter(other.end);
    }

    public bool isBefore(Position position) {
        return end.isBefore(position);
    }

    public bool isAfter(Position position) {
        return begin.isAfter(position);
    }

    //@Override
    public bool equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        Range range = (Range) o;

        return begin.equals(range.begin) && end.equals(range.end);

    }

    //@Override
    public int hashCode() {
        return 31 * begin.hashCode() + end.hashCode();
    }

    //@Override
    public string toString() {
        return begin+"-"+end;
    }
}
