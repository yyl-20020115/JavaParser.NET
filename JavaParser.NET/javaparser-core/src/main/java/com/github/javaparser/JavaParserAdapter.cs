/*
 * Copyright (C) 2013-2023 The JavaParser Team.
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

namespace com.github.javaparser;



public class JavaParserAdapter {

    /**
     * Wraps the {@link JavaParser}.
     *
     * @param parser The java parser to be used.
     *
     * @return The created QuickJavaParser.
     */
    public static JavaParserAdapter of(JavaParser parser) {
        return new JavaParserAdapter(parser);
    }

    private /*final*/JavaParser parser;

    public JavaParserAdapter(JavaParser parser) {
        this.parser = Objects.requireNonNull(parser, "A non-null parser should be provided.");
    }

    public JavaParser getParser() {
        return parser;
    }

    /**
     * Helper function to handle the result _in a simpler way.
     *
     * @param result The result to be handled.
     *
     * @param <T> The return type.
     *
     * @return The parsed value.
     */
    private <T:Node> T handleResult(ParseResult<T> result) {
        if (result.isSuccessful()) {
            return result.getResult().orElse(null);
        }

        throw new ParseProblemException(result.getProblems());
    }
    
    public ParserConfiguration getParserConfiguration() {
        return parser.getParserConfiguration();
    }
    
    public CompilationUnit parse(InputStream _in) {
        return handleResult(getParser().parse(_in));
    }
    
    public CompilationUnit parse(File file) throws FileNotFoundException {
        return handleResult(getParser().parse(file));
    }
    
    public CompilationUnit parse(Path path){
        return handleResult(getParser().parse(path));
    }
    
    public CompilationUnit parse(Reader reader) {
        return handleResult(getParser().parse(reader));
    }
    
    public CompilationUnit parse(string code) {
        return handleResult(getParser().parse(code));
    }
    
    public CompilationUnit parseResource(string path){
        return handleResult(getParser().parseResource(path));
    }
    
    public BlockStmt parseBlock(string blockStatement) {
        return handleResult(getParser().parseBlock(blockStatement));
    }
    
    public Statement parseStatement(string statement) {
        return handleResult(getParser().parseStatement(statement));
    }
    
    public ImportDeclaration parseImport(string importDeclaration) {
        return handleResult(getParser().parseImport(importDeclaration));
    }
    
    public <T:Expression> T parseExpression(string expression) {
        return handleResult(getParser().parseExpression(expression));
    }
    
    public AnnotationExpr parseAnnotation(string annotation) {
        return handleResult(getParser().parseAnnotation(annotation));
    }
    
    public BodyDeclaration<?> parseAnnotationBodyDeclaration(string body) {
        return handleResult(getParser().parseAnnotationBodyDeclaration(body));
    }
    
    public BodyDeclaration<?> parseBodyDeclaration(string body) {
        return handleResult(getParser().parseBodyDeclaration(body));
    }
    
    public ClassOrInterfaceType parseClassOrInterfaceType(string type) {
        return handleResult(getParser().parseClassOrInterfaceType(type));
    }
    
    public Type parseType(string type) {
        return handleResult(getParser().parseType(type));
    }
    
    public VariableDeclarationExpr parseVariableDeclarationExpr(string declaration) {
        return handleResult(getParser().parseVariableDeclarationExpr(declaration));
    }
    
    public Javadoc parseJavadoc(string content) {
        return JavadocParser.parse(content);
    }
    
    public ExplicitConstructorInvocationStmt parseExplicitConstructorInvocationStmt(string statement) {
        return handleResult(getParser().parseExplicitConstructorInvocationStmt(statement));
    }

    public Name parseName(string qualifiedName) {
        return handleResult(getParser().parseName(qualifiedName));
    }
    
    public SimpleName parseSimpleName(string name) {
        return handleResult(getParser().parseSimpleName(name));
    }

    public Parameter parseParameter(string parameter) {
        return handleResult(getParser().parseParameter(parameter));
    }
    
    public PackageDeclaration parsePackageDeclaration(string packageDeclaration) {
        return handleResult(getParser().parsePackageDeclaration(packageDeclaration));
    }
    
    public TypeDeclaration<?> parseTypeDeclaration(string typeDeclaration) {
        return handleResult(getParser().parseTypeDeclaration(typeDeclaration));
    }

    public ModuleDeclaration parseModuleDeclaration(string moduleDeclaration) {
        return handleResult(getParser().parseModuleDeclaration(moduleDeclaration));
    }

    public ModuleDirective parseModuleDirective(string moduleDirective) {
        return handleResult(getParser().parseModuleDirective(moduleDirective));
    }

    public TypeParameter parseTypeParameter(string typeParameter) {
        return handleResult(getParser().parseTypeParameter(typeParameter));
    }
    
    public MethodDeclaration parseMethodDeclaration(string methodDeclaration) {
        return handleResult(getParser().parseMethodDeclaration(methodDeclaration));
    }

}
