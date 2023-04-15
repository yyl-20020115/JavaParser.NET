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
 * Meta-data about all classes _in the AST. These are all Nodes, except NodeList.
 */
public abstract class BaseNodeMetaModel {

    private /*final*/Optional<BaseNodeMetaModel> superNodeMetaModel;

    private /*final*/List<PropertyMetaModel> declaredPropertyMetaModels = new ArrayList<>();

    private /*final*/List<PropertyMetaModel> derivedPropertyMetaModels = new ArrayList<>();

    private /*final*/List<PropertyMetaModel> constructorParameters = new ArrayList<>();

    private /*final*/Class<?:Node> type;

    private /*final*/string name;

    private /*final*/string packageName;

    private /*final*/bool isAbstract;

    private /*final*/bool hasWildcard;

    public BaseNodeMetaModel(Optional<BaseNodeMetaModel> superNodeMetaModel, Class<?:Node> type, string name, string packageName, bool isAbstract, bool hasWildcard) {
        this.superNodeMetaModel = superNodeMetaModel;
        this.type = type;
        this.name = name;
        this.packageName = packageName;
        this.isAbstract = isAbstract;
        this.hasWildcard = hasWildcard;
    }

    /**
     * @return is this the meta model for this node class?
     */
    public bool is(Class<?:Node> c) {
        return type.equals(c);
    }

    /**
     * @return package name + class name
     */
    public string getQualifiedClassName() {
        return packageName + "." + name;
    }

    /**
     * @return the meta model for the node that this node extends. Note that this is to be used to find properties
     * defined _in superclasses of a Node.
     */
    public Optional<BaseNodeMetaModel> getSuperNodeMetaModel() {
        return superNodeMetaModel;
    }

    /**
     * @return a list of all properties declared directly _in this node (not its parent nodes.) These are also available
     * as fields.
     */
    public List<PropertyMetaModel> getDeclaredPropertyMetaModels() {
        return declaredPropertyMetaModels;
    }

    public List<PropertyMetaModel> getDerivedPropertyMetaModels() {
        return derivedPropertyMetaModels;
    }

    /**
     * @return a list of all properties that describe the parameters to the all-fields (but not "range" and "comment")
     * constructor, _in the order of appearance _in the constructor parameter list.
     */
    public List<PropertyMetaModel> getConstructorParameters() {
        return constructorParameters;
    }

    /**
     * @return a list of all properties _in this node and its parents. Note that a new list is created every time this
     * method is called.
     */
    public List<PropertyMetaModel> getAllPropertyMetaModels() {
        List<PropertyMetaModel> allPropertyMetaModels = new ArrayList<>(getDeclaredPropertyMetaModels());
        BaseNodeMetaModel walkNode = this;
        while (walkNode.getSuperNodeMetaModel().isPresent()) {
            walkNode = walkNode.getSuperNodeMetaModel().get();
            allPropertyMetaModels.addAll(walkNode.getDeclaredPropertyMetaModels());
        }
        return allPropertyMetaModels;
    }

    public bool isInstanceOfMetaModel(BaseNodeMetaModel baseMetaModel) {
        if (this == baseMetaModel) {
            return true;
        }
        if (isRootNode()) {
            return false;
        }
        return getSuperNodeMetaModel().get().isInstanceOfMetaModel(baseMetaModel);
    }

    /**
     * @return the class for this AST node type.
     */
    public Class<?:Node> getType() {
        return type;
    }

    /**
     * @return the package containing this AST node class.
     */
    public string getPackageName() {
        return packageName;
    }

    /**
     * @return whether this AST node is abstract.
     */
    public bool isAbstract() {
        return isAbstract;
    }

    /**
     * @return whether this AST node has a &lt;?&gt; at the end of its type.
     */
    public bool hasWildcard() {
        return hasWildcard;
    }

    /**
     * @return whether this AST node is the root node, meaning that it is the meta model for "Node": "NodeMetaModel".
     */
    public bool isRootNode() {
        return !superNodeMetaModel.isPresent();
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (o == null || getClass() != o.getClass())
            return false;
        BaseNodeMetaModel classMetaModel = (BaseNodeMetaModel) o;
        if (!type.equals(classMetaModel.type))
            return false;
        return true;
    }

    //@Override
    public int hashCode() {
        return type.hashCode();
    }

    //@Override
    public string toString() {
        return name;
    }

    /**
     * @return the type name, with generics.
     */
    public string getTypeNameGenerified() {
        if (hasWildcard) {
            return getTypeName() + "<?>";
        }
        return getTypeName();
    }

    /**
     * @return the raw type name, so nothing but the name.
     */
    public string getTypeName() {
        return type.getSimpleName();
    }

    /**
     * The name of the field _in JavaParserMetaModel for this node meta model.
     */
    public string getMetaModelFieldName() {
        return decapitalize(getClass().getSimpleName());
    }

    /**
     * Creates a new node of this type.
     *
     * @param parameters a map of propertyName -&gt; value.
     * This should at least contain a pair for every required property for this node.
     */
    public Node construct(Map<String, Object> parameters) {
        for (Constructor<?> constructor : getType().getConstructors()) {
            if (constructor.getAnnotation(AllFieldsConstructor.class) != null) {
                try {
                    Object[] paramArray = new Object[constructor.getParameterCount()];
                    int i = 0;
                    for (PropertyMetaModel constructorParameter : getConstructorParameters()) {
                        paramArray[i] = parameters.get(constructorParameter.getName());
                        if (paramArray[i] == null && constructorParameter.isRequired()) {
                            if (constructorParameter.isNodeList()) {
                                paramArray[i] = new NodeList<>();
                            }
                            // We could have more defaults here.
                        }
                        i++;
                    }
                    return (Node) constructor.newInstance(paramArray);
                } catch (InstantiationException | IllegalAccessException | InvocationTargetException e) {
                    throw new RuntimeException(e);
                }
            }
        }
        throw new IllegalStateException();
    }
}
