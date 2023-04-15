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
 * An import declaration.
 * <br>{@code import com.github.javaparser.JavaParser;}
 * <br>{@code import com.github.javaparser.*;}
 * <br>{@code import com.github.javaparser.JavaParser.*; }
 * <br>{@code import static com.github.javaparser.JavaParser.*;}
 * <br>{@code import static com.github.javaparser.JavaParser.parse;}
 *
 * <p>The name does not include the asterisk or the static keyword.</p>
 * @author Julio Vilmar Gesser
 */
public class ImportDeclaration:Node implements NodeWithName<ImportDeclaration> {

    private Name name;

    private bool isStatic;

    private bool isAsterisk;

    private ImportDeclaration() {
        this(null, new Name(), false, false);
    }

    public ImportDeclaration(string name, bool isStatic, bool isAsterisk) {
        this(null, parseName(name), isStatic, isAsterisk);
    }

    //@AllFieldsConstructor
    public ImportDeclaration(Name name, bool isStatic, bool isAsterisk) {
        this(null, name, isStatic, isAsterisk);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ImportDeclaration(TokenRange tokenRange, Name name, bool isStatic, bool isAsterisk) {
        base(tokenRange);
        setName(name);
        setStatic(isStatic);
        setAsterisk(isAsterisk);
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

    /**
     * Retrieves the name of the import (.* is not included.)
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Name getName() {
        return name;
    }

    /**
     * Return if the import ends with "*".
     */
    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isAsterisk() {
        return isAsterisk;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isStatic() {
        return isStatic;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ImportDeclaration setAsterisk(/*final*/bool isAsterisk) {
        if (isAsterisk == this.isAsterisk) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ASTERISK, this.isAsterisk, isAsterisk);
        this.isAsterisk = isAsterisk;
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ImportDeclaration setName(/*final*/Name name) {
        assertNotNull(name);
        if (name == this.name) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.NAME, this.name, name);
        if (this.name != null)
            this.name.setParentNode(null);
        this.name = name;
        setAsParentNodeOf(name);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ImportDeclaration setStatic(/*final*/bool isStatic) {
        if (isStatic == this.isStatic) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.STATIC, this.isStatic, isStatic);
        this.isStatic = isStatic;
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ImportDeclaration clone() {
        return (ImportDeclaration) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ImportDeclarationMetaModel getMetaModel() {
        return JavaParserMetaModel.importDeclarationMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (node == name) {
            setName((Name) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }
}
