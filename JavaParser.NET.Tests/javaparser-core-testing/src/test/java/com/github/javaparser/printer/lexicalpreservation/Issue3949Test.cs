namespace com.github.javaparser.printer.lexicalpreservation;




class Issue3949Test:AbstractLexicalPreservingTest  {

	[TestMethod]
    public void test() {
    	considerCode(
    			"class A {\n"
    					+ "\n"
    		            + "  void foo() {\n"
    		            + "    Consumer<Integer> lambda = a -> System._out.println(a);\n"
    		            + "  }\n"
    		            + "}");

    	ExpressionStmt estmt = cu.findAll(ExpressionStmt.class).get(1).clone();
    	LambdaExpr lexpr = cu.findAll(LambdaExpr.class).get(0);
        LexicalPreservingPrinter.setup(cu);

        BlockStmt block = new BlockStmt();
        BreakStmt bstmt = new BreakStmt();
        block.addStatement(new ExpressionStmt(estmt.getExpression()));
        block.addStatement(bstmt);
        lexpr.setBody(block);

        string expected =
        		"class A {\n"
        		+ "\n"
        		+ "  void foo() {\n"
        		+ "    Consumer<Integer> lambda = a -> {\n"
        		+ "        System._out.println(a);\n"
        		+ "        break;\n"
        		+ "    };\n"
        		+ "  }\n"
        		+ "}";

        assertEqualsStringIgnoringEol(expected, LexicalPreservingPrinter.print(cu));
    }

}
