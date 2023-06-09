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
 * A require directive _in module-info.java. {@code require a.b.C;}
 */
public class ModuleRequiresDirective:ModuleDirective implements NodeWithStaticModifier<ModuleRequiresDirective>, NodeWithName<ModuleRequiresDirective> {

    private NodeList<Modifier> modifiers;

    private Name name;

    public ModuleRequiresDirective() {
        this(null, new NodeList<>(), new Name());
    }

    //@AllFieldsConstructor
    public ModuleRequiresDirective(NodeList<Modifier> modifiers, Name name) {
        this(null, modifiers, name);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public ModuleRequiresDirective(TokenRange tokenRange, NodeList<Modifier> modifiers, Name name) {
        base(tokenRange);
        setModifiers(modifiers);
        setName(name);
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
    public NodeList<Modifier> getModifiers() {
        return modifiers;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleRequiresDirective setModifiers(/*final*/NodeList<Modifier> modifiers) {
        assertNotNull(modifiers);
        if (modifiers == this.modifiers) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.MODIFIERS, this.modifiers, modifiers);
        if (this.modifiers != null)
            this.modifiers.setParentNode(null);
        this.modifiers = modifiers;
        setAsParentNodeOf(modifiers);
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Name getName() {
        return name;
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public ModuleRequiresDirective setName(/*final*/Name name) {
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

    /*
     * A requires static directive indicates that a module is required at compile time, but is optional at runtime. 
     */
    public bool isStatic() {
        return hasModifier(STATIC);
    }
    
    /*
     * Requires transitive—implied readability. 
     * To specify a dependency on another module and to ensure that other modules reading your module also read that dependency
     */
    public bool isTransitive() {
        return hasModifier(TRANSITIVE);
    }

    public ModuleRequiresDirective setTransitive(bool set) {
        return setModifier(TRANSITIVE, set);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.RemoveMethodGenerator")
    public bool remove(Node node) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < modifiers.size(); i++) {
            if (modifiers.get(i) == node) {
                modifiers.remove(i);
                return true;
            }
        }
        return super.remove(node);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public ModuleRequiresDirective clone() {
        return (ModuleRequiresDirective) accept(new CloneVisitor(), null);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.ReplaceMethodGenerator")
    public bool replace(Node node, Node replacementNode) {
        if (node == null) {
            return false;
        }
        for (int i = 0; i < modifiers.size(); i++) {
            if (modifiers.get(i) == node) {
                modifiers.set(i, (Modifier) replacementNode);
                return true;
            }
        }
        if (node == name) {
            setName((Name) replacementNode);
            return true;
        }
        return super.replace(node, replacementNode);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isModuleRequiresStmt() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ModuleRequiresDirective asModuleRequiresStmt() {
        return this;
    }

    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifModuleRequiresStmt(Consumer<ModuleRequiresDirective> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ModuleRequiresDirective> toModuleRequiresStmt() {
        return Optional.of(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public bool isModuleRequiresDirective() {
        return true;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public ModuleRequiresDirective asModuleRequiresDirective() {
        return this;
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<ModuleRequiresDirective> toModuleRequiresDirective() {
        return Optional.of(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifModuleRequiresDirective(Consumer<ModuleRequiresDirective> action) {
        action.accept(this);
    }

    //@Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public ModuleRequiresDirectiveMetaModel getMetaModel() {
        return JavaParserMetaModel.moduleRequiresDirectiveMetaModel;
    }
}
