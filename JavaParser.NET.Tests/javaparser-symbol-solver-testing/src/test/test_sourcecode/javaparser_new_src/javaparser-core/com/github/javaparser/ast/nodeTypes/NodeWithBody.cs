namespace com.github.javaparser.ast.nodeTypes;


public interface NodeWithBody<T> {
    public Statement getBody();

    public T setBody(final Statement body);

    public default BlockStmt createBlockStatementAsBody() {
        BlockStmt b = new BlockStmt();
        b.setParentNode((Node) this);
        setBody(b);
        return b;
    }
}
