/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */
namespace com.github.javaparser.ast.expr;




/**
 * <h1>A lambda expression</h1>
 * <h2>Java 1-7</h2>
 * Does not exist.
 * <h2>Java 8+</h2>
 * {@code (a, b) -> a + b}
 * <br>{@code a -> ...}
 * <br>{@code (Long a) -> { println(a); }}
 * <p>The parameters are on the left side of the -&gt;.
 * If a parameter uses type inference (it has no type specified) then its type is set to {@code UnknownType}.
 * If they are _in ( ), "isEnclosingParameters" is true.
 * <br>The body is to the right of the -&gt;.
 * The body is either a BlockStmt when it is _in { } braces, or an ExpressionStmt when it is not _in braces.
 *
 * @author Raquel Pau
 */
public class LambdaExpr:Expression implements NodeWithParameters<LambdaExpr> {

    private NodeList<Parameter> parameters;

    private bool isEnclosingParameters;

    private Statement body;

    public LambdaExpr() {
        this(null, new NodeList<>(), new ReturnStmt(), false);
    }

    /**
     * Creates a single parameter lambda expression.
     */
    public LambdaExpr(Parameter parameter, BlockStmt body) {
        this(null, new NodeList<>(parameter), body, false);
    }

    /**
     * Creates a zero or multi-parameter lambda expression with its parameters wrapped _in ( ).
     */
    public LambdaExpr(NodeList<Parameter> parameters, BlockStmt body) {
        this(null, parameters, body, true);
    }

    /**
     * Creates a single parameter lambda expression.
     */
    public LambdaExpr(Parameter parameter, Expression body) {
        this(null, new NodeList<>(parameter), new ExpressionStmt(body), false);
    }

    /**
     * Creates a zero or multi-parameter lambda expression with its parameters wrapped _in ( ).
     */
    public LambdaExpr(NodeList<Parameter> parameters, Expression body) {
        this(null, parameters, new ExpressionStmt(body), true);
    }

    //@AllFieldsConstructor
    public LambdaExpr(NodeList<Parameter> parameters, Statement body, bool isEnclosingParameters) {
        this(null, parameters, body, isEnclosingParameters);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public LambdaExpr(TokenRange tokenRange, NodeList<Parameter> parameters, Statement body, bool isEnclosingParameters) {
        base(tokenRange);
        setParameters(parameters);
        setBody(body);
        setEnclosingParameters(isEnclosingParameters);
        customInitialization();
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<Parameter> getParameters() {
        return parameters;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public LambdaExpr setParameters(/*final*/NodeList<Parameter> parameters) {
        assertNotNull(parameters);
        if (parameters == this.parameters) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.PARAMETERS, this.parameters, parameters);
        if (this.parameters != null)
            this.parameters.setParentNode(null);
        this.parameters = parameters;
        setAsParentNodeOf(parameters);
        return this;
    }

    /**
     * @return a BlockStatement or an ExpressionStatement. See class Javadoc.
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Statement getBody() {
        return body;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public LambdaExpr setBody(/*final*/Statement body) {
        assertNotNull(body);
        if (body == this.body) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.BODY, this.body, body);
        if (this.body != null)
            this.body.setParentNode(null);
        this.body = body;
        setAsParentNodeOf(body);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public R accept<R, A>(GenericVisitor<R, A> v, A arg) {
        return v.visit(this, arg);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public void accept<A>(VoidVisitor<A> v, A arg) {
        v.visit(this, arg);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isEnclosingParameters() {
        return isEnclosingParameters;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public LambdaExpr setEnclosingParameters(/*final*/bool isEnclosingParameters) {
        if (isEnclosingParameters == this.isEnclosingParameters) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ENCLOSING_PARAMETERS, this.isEnclosingParameters, isEnclosingParameters);
        this.isEnclosingParameters = isEnclosingParameters;
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < parameters.size(); i++) {
            if (parameters.get(i) == node) {
                parameters.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    /**
     * @return if the body of this lambda is a simple expression, return that expression.
     * Otherwise (when the body is a block) return Optional.empty().
     */
    @DerivedProperty
    public Optional<Expression> getExpressionBody() {
        if (body.isExpressionStmt()) {
            return Optional.of(body.asExpressionStmt().getExpression());
        } else {
            return Optional.empty();
        }
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public LambdaExpr clone() {
        return (LambdaExpr) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public LambdaExprMetaModel getMetaModel() {
        return JavaParserMetaModel.lambdaExprMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == body) {
            setBody((Statement) replacementNode);
            return true;
        }
        for (int i = 0; i < parameters.size(); i++) {
            if (parameters.get(i) == node) {
                parameters.set(i, (Parameter) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isLambdaExpr() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public LambdaExpr asLambdaExpr() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifLambdaExpr(Consumer<LambdaExpr> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<LambdaExpr> toLambdaExpr() {
        return Optional.of(this);
    }

    /*
     * Lambda expressions are always poly expressions
     */
    //@Override
    public bool isPolyExpression() {
        return true;
    }
}
