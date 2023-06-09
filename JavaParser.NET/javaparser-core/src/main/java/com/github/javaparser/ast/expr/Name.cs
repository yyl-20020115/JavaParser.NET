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
 * A name that may consist of multiple identifiers.
 * In other words: it.may.contain.dots.
 * <p>
 * The rightmost identifier is "identifier",
 * The one to the left of it is "qualifier.identifier", etc.
 * <p>
 * You can construct one from a string with the name(...) method.
 *
 * @author Julio Vilmar Gesser
 * @see SimpleName
 */
public class Name:Node implements NodeWithIdentifier<Name> {

    //@NonEmptyProperty
    private string identifier;

    //@OptionalProperty
    private Name qualifier;

    public Name() {
        this(null, null, "empty");
    }

    public Name(/*final*/string identifier) {
        this(null, null, identifier);
    }

    //@AllFieldsConstructor
    public Name(Name qualifier, /*final*/string identifier) {
        this(null, qualifier, identifier);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public Name(TokenRange tokenRange, Name qualifier, string identifier) {
        base(tokenRange);
        setQualifier(qualifier);
        setIdentifier(identifier);
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
    public string getIdentifier() {
        return identifier;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Name setIdentifier(/*final*/string identifier) {
        assertNonEmpty(identifier);
        if (identifier == this.identifier) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.IDENTIFIER, this.identifier, identifier);
        this.identifier = identifier;
        return this;
    }

    /**
     * @return the complete qualified name. Only the identifiers and the dots, so no comments or whitespace.
     */
    public string asString() {
        if (qualifier != null) {
            return qualifier.asString() + "." + identifier;
        }
        return identifier;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Optional<Name> getQualifier() {
        return Optional.ofNullable(qualifier);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Name setQualifier(/*final*/Name qualifier) {
        if (qualifier == this.qualifier) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.QUALIFIER, this.qualifier, qualifier);
        if (this.qualifier != null)
            this.qualifier.setParentNode(null);
        this.qualifier = qualifier;
        setAsParentNodeOf(qualifier);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        if (qualifier != null) {
            if (node == qualifier) {
                removeQualifier();
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public Name removeQualifier() {
        return setQualifier((Name) null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public Name clone() {
        return (Name) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public NameMetaModel getMetaModel() {
        return JavaParserMetaModel.nameMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        if (qualifier != null) {
            if (node == qualifier) {
                setQualifier((Name) replacementNode);
                return true;
            }
        }
        return super.replace(node, replacementNode);
    }

    /**
     * A top level name is a name that is not contained _in a larger Name instance.
     */
    public bool isTopLevel() {
        return !isInternal();
    }

    /**
     * An internal name is a name that constitutes a part of a larger Name instance.
     */
    public bool isInternal() {
        return getParentNode().filter(parent -> parent is Name).isPresent();
    }
}
