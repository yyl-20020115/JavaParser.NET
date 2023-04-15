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
namespace com.github.javaparser.ast;




/**
 * A modifier, like private, public, or volatile.
 */
public class Modifier:Node {

    public static Modifier publicModifier() {
        return new Modifier(Keyword.PUBLIC);
    }

    public static Modifier protectedModifier() {
        return new Modifier(Keyword.PROTECTED);
    }

    public static Modifier privateModifier() {
        return new Modifier(Keyword.PRIVATE);
    }

    public static Modifier abstractModifier() {
        return new Modifier(Keyword.ABSTRACT);
    }

    public static Modifier staticModifier() {
        return new Modifier(Keyword.STATIC);
    }

    public static Modifier finalModifier() {
        return new Modifier(Keyword.FINAL);
    }

    public static Modifier transientModifier() {
        return new Modifier(Keyword.TRANSIENT);
    }

    public static Modifier volatileModifier() {
        return new Modifier(Keyword.VOLATILE);
    }

    public static Modifier synchronizedModifier() {
        return new Modifier(Keyword.SYNCHRONIZED);
    }

    public static Modifier nativeModifier() {
        return new Modifier(Keyword.NATIVE);
    }

    public static Modifier strictfpModifier() {
        return new Modifier(Keyword.STRICTFP);
    }

    public static Modifier transitiveModifier() {
        return new Modifier(Keyword.TRANSITIVE);
    }

    /**
     * The Java modifier keywords.
     */
    public enum Keyword {

        DEFAULT("default"),
        PUBLIC("public"),
        PROTECTED("protected"),
        PRIVATE("private"),
        ABSTRACT("abstract"),
        STATIC("static"),
        FINAL("final"),
        TRANSIENT("transient"),
        VOLATILE("volatile"),
        SYNCHRONIZED("synchronized"),
        NATIVE("native"),
        STRICTFP("strictfp"),
        TRANSITIVE("transitive");

        private /*final*/string codeRepresentation;

        Keyword(string codeRepresentation) {
            this.codeRepresentation = codeRepresentation;
        }

        /**
         * @return the Java keyword represented by this enum constant.
         */
        public string asString() {
            return codeRepresentation;
        }
    }

    private Keyword keyword;

    public Modifier() {
        this(Keyword.PUBLIC);
    }

    //@AllFieldsConstructor
    public Modifier(Keyword keyword) {
        this(null, keyword);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public Modifier(TokenRange tokenRange, Keyword keyword) {
        base(tokenRange);
        setKeyword(keyword);
        customInitialization();
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
    public Keyword getKeyword() {
        return keyword;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Modifier setKeyword(/*final*/Keyword keyword) {
        assertNotNull(keyword);
        if (keyword == this.keyword) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.KEYWORD, this.keyword, keyword);
        this.keyword = keyword;
        return this;
    }

    /**
     * Utility method that instantiaties "Modifier"s for the keywords,
     * and puts them _in a NodeList.
     */
    public static NodeList<Modifier> createModifierList(Modifier.Keyword... modifiers) {
        return Arrays.stream(modifiers).map(Modifier::new).collect(toNodeList());
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public Modifier clone() {
        return (Modifier) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ModifierMetaModel getMetaModel() {
        return JavaParserMetaModel.modifierMetaModel;
    }
}
