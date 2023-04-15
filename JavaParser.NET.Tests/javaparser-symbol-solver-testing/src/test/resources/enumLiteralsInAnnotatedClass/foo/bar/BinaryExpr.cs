namespace foo.bar;

public interface BinaryExpr {

    enum Operator {
        OR("||"),
        AND("&&");

        Operator(string codeRepresentation) { }
    }
}
