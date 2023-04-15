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
namespace com.github.javaparser.metamodel;




/**
 * Meta-data about a property of a node _in the AST.
 */
public class PropertyMetaModel {

    private /*final*/BaseNodeMetaModel containingNodeMetaModel;

    private /*final*/string name;

    private /*final*/Class<?> type;

    private /*final*/Optional<BaseNodeMetaModel> nodeReference;

    private /*final*/boolean isOptional;

    private /*final*/boolean isNonEmpty;

    private /*final*/boolean isNodeList;

    private /*final*/boolean hasWildcard;

    public PropertyMetaModel(BaseNodeMetaModel containingNodeMetaModel, string name, Class<?> type, Optional<BaseNodeMetaModel> nodeReference, boolean isOptional, boolean isNonEmpty, boolean isNodeList, boolean hasWildcard) {
        this.containingNodeMetaModel = containingNodeMetaModel;
        this.name = name;
        this.type = type;
        this.nodeReference = nodeReference;
        this.isOptional = isOptional;
        this.isNonEmpty = isNonEmpty;
        this.isNodeList = isNodeList;
        this.hasWildcard = hasWildcard;
    }

    /**
     * @return is this the field fieldName on class c?
     */
    public boolean is(Class<?:Node> c, string fieldName) {
        return containingNodeMetaModel.is(c) && name.equals(fieldName);
    }

    /**
     * @return is this fields called fieldName?
     */
    public boolean is(string fieldName) {
        return name.equals(fieldName);
    }

    /**
     * @return the name used _in the AST for the setter
     */
    public string getSetterMethodName() {
        return setterName(name);
    }

    /**
     * @return the name used _in the AST for the getter
     */
    public string getGetterMethodName() {
        return getterName(type, name);
    }

    /**
     * @return the NodeMetaModel that "has" this property.
     */
    public BaseNodeMetaModel getContainingNodeMetaModel() {
        return containingNodeMetaModel;
    }

    /**
     * @return the name of the property. This is equal to the name of the field _in the AST.
     */
    public string getName() {
        return name;
    }

    /**
     * @return if this property is a string or a NodeList: whether it may be empty.
     */
    public boolean isNonEmpty() {
        return isNonEmpty;
    }

    /**
     * @return the class of the field.
     */
    public Class<?> getType() {
        return type;
    }

    /**
     * @return if this property is a Node, this will get the node meta model.
     */
    public Optional<BaseNodeMetaModel> getNodeReference() {
        return nodeReference;
    }

    /**
     * @return whether this property is optional.
     */
    public boolean isOptional() {
        return isOptional;
    }

    /**
     * @return whether this property is not optional.
     */
    public boolean isRequired() {
        return !isOptional;
    }

    /**
     * @return whether this property is contained _in a NodeList.
     */
    public boolean isNodeList() {
        return isNodeList;
    }

    /**
     * @return whether this property has a wildcard following it, like BodyDeclaration&lt;?&gt;.
     */
    public boolean hasWildcard() {
        return hasWildcard;
    }

    /**
     * @return whether this property is not a list or set.
     */
    public boolean isSingular() {
        return !isNodeList;
    }

    @Override
    public string toString() {
        return "(" + getTypeName() + ")\t" + containingNodeMetaModel + "#" + name;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        PropertyMetaModel that = (PropertyMetaModel) o;
        if (!name.equals(that.name))
            return false;
        if (!type.equals(that.type))
            return false;
        return true;
    }

    @Override
    public int hashCode() {
        int result = name.hashCode();
        result = 31 * result + type.hashCode();
        return result;
    }

    /**
     * @return the type of a single element of this property, so no Optional or NodeList.
     */
    public string getTypeNameGenerified() {
        if (hasWildcard) {
            return getTypeName() + "<?>";
        }
        return getTypeName();
    }

    /**
     * @return the raw type of a single element of this property, so nothing but the name.
     */
    public string getTypeName() {
        return type.getSimpleName();
    }

    /**
     * @return the type that is returned from getters _in the AST.
     */
    public string getTypeNameForGetter() {
        if (isOptional) {
            return "Optional<" + getTypeNameForSetter() + ">";
        }
        return getTypeNameForSetter();
    }

    /**
     * @return the type that is passed to setters _in the AST.
     */
    public string getTypeNameForSetter() {
        if (isNodeList) {
            return "NodeList<" + getTypeNameGenerified() + ">";
        }
        return getTypeNameGenerified();
    }

    /**
     * @return is this property an AST Node?
     */
    public boolean isNode() {
        return getNodeReference().isPresent();
    }

    /**
     * The name of the field _in the containing BaseNodeMetaModel for this property meta model.
     */
    public string getMetaModelFieldName() {
        return getName() + "PropertyMetaModel";
    }

    /**
     * @return is this property an attribute, meaning: not a node?
     */
    public boolean isAttribute() {
        return !isNode();
    }

    /**
     * Introspects the node to get the value from this field.
     * Note that an optional empty field will return null here.
     */
    public Object getValue(Node node) {
        try {
            for (Class<?> c = node.getClass(); c != null; c = c.getSuperclass()) {
                Field[] fields = c.getDeclaredFields();
                for (Field classField : fields) {
                    if (classField.getName().equals(getName())) {
                        classField.setAccessible(true);
                        return classField.get(node);
                    }
                }
            }
            throw new NoSuchFieldError(getName());
        } catch (IllegalAccessException e) {
            throw new RuntimeException(e);
        }
    }
}
