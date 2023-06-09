namespace com.github.javaparser.ast.nodeTypes;



public interface NodeWithStatements<T> {
    public List<Statement> getStmts();

    public T setStmts(/*final*/List<Statement> stmts);

    //@SuppressWarnings("unchecked")
    public default T addStatement(Statement statement) {
        getStmts().add(statement);
        statement.setParentNode((Node) this);
        return (T) this;
    }

    //@SuppressWarnings("unchecked")
    public default T addStatement(int index, /*final*/Statement statement) {
        getStmts().add(index, statement);
        statement.setParentNode((Node) this);
        return (T) this;
    }

    public default T addStatement(Expression expr) {
        ExpressionStmt statement = new ExpressionStmt(expr);
        expr.setParentNode(statement);
        return addStatement(statement);
    }

    public default T addStatement(string statement) {
        return addStatement(new NameExpr(statement));
    }

    public default T addStatement(int index, /*final*/Expression expr) {
        Statement stmt = new ExpressionStmt(expr);
        expr.setParentNode(stmt);
        return addStatement(index, stmt);
    }

}
