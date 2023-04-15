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
namespace com.github.javaparser.serialization;




class JavaParserJsonSerializerTest {
    [TestMethod]
    void test() {
        CompilationUnit cu = parse("class X{java.util.Y y;}");

        string serialized = serialize(cu, false);

        assertEquals("{\"!\":\"com.github.javaparser.ast.CompilationUnit\",\"range\":{\"beginLine\":1,\"beginColumn\":1,\"endLine\":1,\"endColumn\":23},\"tokenRange\":{\"beginToken\":{\"kind\":19,\"text\":\"class\"},\"endToken\":{\"kind\":0,\"text\":\"\"}},\"imports\":[],\"types\":[{\"!\":\"com.github.javaparser.ast.body.ClassOrInterfaceDeclaration\",\"range\":{\"beginLine\":1,\"beginColumn\":1,\"endLine\":1,\"endColumn\":23},\"tokenRange\":{\"beginToken\":{\"kind\":19,\"text\":\"class\"},\"endToken\":{\"kind\":100,\"text\":\"}\"}},\"extendedTypes\":[],\"implementedTypes\":[],\"isInterface\":\"false\",\"typeParameters\":[],\"members\":[{\"!\":\"com.github.javaparser.ast.body.FieldDeclaration\",\"range\":{\"beginLine\":1,\"beginColumn\":9,\"endLine\":1,\"endColumn\":22},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"java\"},\"endToken\":{\"kind\":103,\"text\":\";\"}},\"modifiers\":[],\"variables\":[{\"!\":\"com.github.javaparser.ast.body.VariableDeclarator\",\"range\":{\"beginLine\":1,\"beginColumn\":21,\"endLine\":1,\"endColumn\":21},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"y\"},\"endToken\":{\"kind\":94,\"text\":\"y\"}},\"name\":{\"!\":\"com.github.javaparser.ast.expr.SimpleName\",\"range\":{\"beginLine\":1,\"beginColumn\":21,\"endLine\":1,\"endColumn\":21},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"y\"},\"endToken\":{\"kind\":94,\"text\":\"y\"}},\"identifier\":\"y\"},\"type\":{\"!\":\"com.github.javaparser.ast.type.ClassOrInterfaceType\",\"range\":{\"beginLine\":1,\"beginColumn\":9,\"endLine\":1,\"endColumn\":19},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"java\"},\"endToken\":{\"kind\":94,\"text\":\"Y\"}},\"name\":{\"!\":\"com.github.javaparser.ast.expr.SimpleName\",\"range\":{\"beginLine\":1,\"beginColumn\":19,\"endLine\":1,\"endColumn\":19},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"Y\"},\"endToken\":{\"kind\":94,\"text\":\"Y\"}},\"identifier\":\"Y\"},\"scope\":{\"!\":\"com.github.javaparser.ast.type.ClassOrInterfaceType\",\"range\":{\"beginLine\":1,\"beginColumn\":9,\"endLine\":1,\"endColumn\":17},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"java\"},\"endToken\":{\"kind\":94,\"text\":\"util\"}},\"name\":{\"!\":\"com.github.javaparser.ast.expr.SimpleName\",\"range\":{\"beginLine\":1,\"beginColumn\":14,\"endLine\":1,\"endColumn\":17},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"util\"},\"endToken\":{\"kind\":94,\"text\":\"util\"}},\"identifier\":\"util\"},\"scope\":{\"!\":\"com.github.javaparser.ast.type.ClassOrInterfaceType\",\"range\":{\"beginLine\":1,\"beginColumn\":9,\"endLine\":1,\"endColumn\":12},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"java\"},\"endToken\":{\"kind\":94,\"text\":\"java\"}},\"name\":{\"!\":\"com.github.javaparser.ast.expr.SimpleName\",\"range\":{\"beginLine\":1,\"beginColumn\":9,\"endLine\":1,\"endColumn\":12},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"java\"},\"endToken\":{\"kind\":94,\"text\":\"java\"}},\"identifier\":\"java\"},\"annotations\":[]},\"annotations\":[]},\"annotations\":[]}}],\"annotations\":[]}],\"modifiers\":[],\"name\":{\"!\":\"com.github.javaparser.ast.expr.SimpleName\",\"range\":{\"beginLine\":1,\"beginColumn\":7,\"endLine\":1,\"endColumn\":7},\"tokenRange\":{\"beginToken\":{\"kind\":94,\"text\":\"X\"},\"endToken\":{\"kind\":94,\"text\":\"X\"}},\"identifier\":\"X\"},\"annotations\":[]}]}", serialized);
    }

    static string serialize(Node node, bool prettyPrint) {
        Map<String, ?> config = new HashMap<>();
        if (prettyPrint) {
            config.put(JsonGenerator.PRETTY_PRINTING, null);
        }
        JsonGeneratorFactory generatorFactory = Json.createGeneratorFactory(config);
        JavaParserJsonSerializer serializer = new JavaParserJsonSerializer();
        StringWriter jsonWriter = new StringWriter();
        try (JsonGenerator generator = generatorFactory.createGenerator(jsonWriter)) {
            serializer.serialize(node, generator);
        }
        return jsonWriter.toString();
    }
}
