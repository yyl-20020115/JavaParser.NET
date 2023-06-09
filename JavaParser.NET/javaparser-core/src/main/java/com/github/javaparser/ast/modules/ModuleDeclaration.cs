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
namespace com.github.javaparser.ast.modules;



/**
 * A Java 9 Jigsaw module declaration. {@code @Foo module com.github.abc { requires a.B; }}
 */
public class ModuleDeclaration:Node implements NodeWithName<ModuleDeclaration>, NodeWithAnnotations<ModuleDeclaration> {

    private Name name;

    private NodeList<AnnotationExpr> annotations;

    private bool isOpen;

    private NodeList<ModuleDirective> directives;

    public ModuleDeclaration() {
        this(null, new NodeList<>(), new Name(), false, new NodeList<>());
    }

    public ModuleDeclaration(Name name, bool isOpen) {
        this(null, new NodeList<>(), name, isOpen, new NodeList<>());
    }

    //@AllFieldsConstructor
    public ModuleDeclaration(NodeList<AnnotationExpr> annotations, Name name, bool isOpen, NodeList<ModuleDirective> directives) {
        this(null, annotations, name, isOpen, directives);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ModuleDeclaration(TokenRange tokenRange, NodeList<AnnotationExpr> annotations, Name name, bool isOpen, NodeList<ModuleDirective> directives) {
        base(tokenRange);
        setAnnotations(annotations);
        setName(name);
        setOpen(isOpen);
        setDirectives(directives);
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
    public Name getName() {
        return name;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleDeclaration setName(/*final*/Name name) {
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
    public NodeList<AnnotationExpr> getAnnotations() {
        return annotations;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleDeclaration setAnnotations(/*final*/NodeList<AnnotationExpr> annotations) {
        assertNotNull(annotations);
        if (annotations == this.annotations) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.ANNOTATIONS, this.annotations, annotations);
        if (this.annotations != null)
            this.annotations.setParentNode(null);
        this.annotations = annotations;
        setAsParentNodeOf(annotations);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < annotations.size(); i++) {
            if (annotations.get(i) == node) {
                annotations.remove(i);
                return true;
            }
        }
        for (int i = 0; i < directives.size(); i++) {
            if (directives.get(i) == node) {
                directives.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public bool isOpen() {
        return isOpen;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleDeclaration setOpen(/*final*/bool isOpen) {
        if (isOpen == this.isOpen) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.OPEN, this.isOpen, isOpen);
        this.isOpen = isOpen;
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public NodeList<ModuleDirective> getDirectives() {
        return directives;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleDeclaration setDirectives(/*final*/NodeList<ModuleDirective> directives) {
        assertNotNull(directives);
        if (directives == this.directives) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.DIRECTIVES, this.directives, directives);
        if (this.directives != null)
            this.directives.setParentNode(null);
        this.directives = directives;
        setAsParentNodeOf(directives);
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ModuleDeclaration clone() {
        return (ModuleDeclaration) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ModuleDeclarationMetaModel getMetaModel() {
        return JavaParserMetaModel.moduleDeclarationMetaModel;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < annotations.size(); i++) {
            if (annotations.get(i) == node) {
                annotations.set(i, (AnnotationExpr) replacementNode);
                return true;
            }
        }
        for (int i = 0; i < directives.size(); i++) {
            if (directives.get(i) == node) {
                directives.set(i, (ModuleDirective) replacementNode);
                return true;
            }
        }
        if (node == name) {
            setName((Name) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    /**
     * Add a directive to the module, like "exports R.S to T1.U1, T2.U2;"
     */
    public ModuleDeclaration addDirective(string directive) {
        return addDirective(parseModuleDirective(directive));
    }

    public ModuleDeclaration addDirective(ModuleDirective directive) {
        getDirectives().add(directive);
        return this;
    }
}
