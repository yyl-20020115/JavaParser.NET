namespace com.github.javaparser.ast.nodeTypes;


public interface NodeWithBlockStmt<T> {
    BlockStmt getBody();

    T setBody(BlockStmt block);

    default BlockStmt createBody() {
        BlockStmt block = new BlockStmt();
        setBody(block);
        block.setParentNode((Node) this);

        return block;
    }
}
