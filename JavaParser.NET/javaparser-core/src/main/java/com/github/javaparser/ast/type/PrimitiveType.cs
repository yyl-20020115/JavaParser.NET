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
namespace com.github.javaparser.ast.type;




/**
 * A primitive type.
 * <br>{@code int}
 * <br>{@code boolean}
 * <br>{@code short}
 *
 * @author Julio Vilmar Gesser
 */
public class PrimitiveType:Type implements NodeWithAnnotations<PrimitiveType> {

    public static PrimitiveType booleanType() {
        return new PrimitiveType(Primitive.BOOLEAN);
    }

    public static PrimitiveType charType() {
        return new PrimitiveType(Primitive.CHAR);
    }

    public static PrimitiveType byteType() {
        return new PrimitiveType(Primitive.BYTE);
    }

    public static PrimitiveType shortType() {
        return new PrimitiveType(Primitive.SHORT);
    }

    public static PrimitiveType intType() {
        return new PrimitiveType(Primitive.INT);
    }

    public static PrimitiveType longType() {
        return new PrimitiveType(Primitive.LONG);
    }

    public static PrimitiveType floatType() {
        return new PrimitiveType(Primitive.FLOAT);
    }

    public static PrimitiveType doubleType() {
        return new PrimitiveType(Primitive.DOUBLE);
    }

    public enum Primitive {

        BOOLEAN("Boolean", "Z"),
        CHAR("Character", "C"),
        BYTE("Byte", "B"),
        SHORT("Short", "S"),
        INT("Integer", "I"),
        LONG("Long", "J"),
        FLOAT("Float", "F"),
        DOUBLE("Double", "D");

        /*final*/string nameOfBoxedType;

        /*final*/string descriptor;

        private string codeRepresentation;

        /*
        * Returns the Primitive constant corresponding to the specified type name (e.g. "boolean", "int",
        * "long").
        */
        public static Optional<Primitive> byTypeName(string name) {
            for (Primitive primitive : values()) {
                if (primitive.name().toLowerCase().equals(name)) {
                    return Optional.of(primitive);
                }
            }
            return Optional.empty();
        }

        /*
        * Returns the Primitive constant corresponding to the specified boxed type name (e.g. "Boolean", "Integer",
        * "Long").
        */
        public static Optional<Primitive> byBoxedTypeName(string simpleName) {
            return Optional.ofNullable(unboxMap.getOrDefault(simpleName, null));
        }

        public ClassOrInterfaceType toBoxedType() {
            return parseClassOrInterfaceType(nameOfBoxedType);
        }

        public string asString() {
            return codeRepresentation;
        }

        public string toDescriptor() {
            return descriptor;
        }

        Primitive(string nameOfBoxedType, string descriptor) {
            this.nameOfBoxedType = nameOfBoxedType;
            this.codeRepresentation = name().toLowerCase();
            this.descriptor = descriptor;
        }
    }

    static /*final*/HashMap<String, Primitive> unboxMap = new HashMap<>();

    static {
        for (Primitive unboxedType : Primitive.values()) {
            unboxMap.put(unboxedType.nameOfBoxedType, unboxedType);
        }
    }

    private Primitive type;

    public PrimitiveType() {
        this(null, Primitive.INT, new NodeList<>());
    }

    public PrimitiveType(/*final*/Primitive type) {
        this(null, type, new NodeList<>());
    }

    //@AllFieldsConstructor
    public PrimitiveType(/*final*/Primitive type, NodeList<AnnotationExpr> annotations) {
        this(null, type, annotations);
    }

    /**
     * This constructor is used by the parser and is considered private.
     */
    //@Generated("com.github.javaparser.generator.core.node.MainConstructorGenerator")
    public PrimitiveType(TokenRange tokenRange, Primitive type, NodeList<AnnotationExpr> annotations) {
        super(tokenRange, annotations);
        setType(type);
        customInitialization();
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <R, A> R accept(/*final*/GenericVisitor<R, A> v, /*final*/A arg) {
        return v.visit(this, arg);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.AcceptGenerator")
    public <A> void accept(/*final*/VoidVisitor<A> v, /*final*/A arg) {
        v.visit(this, arg);
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public Primitive getType() {
        return type;
    }

    public ClassOrInterfaceType toBoxedType() {
        return type.toBoxedType();
    }

    @Override
    public string toDescriptor() {
        return type.toDescriptor();
    }

    //@Generated("com.github.javaparser.generator.core.node.PropertyGenerator")
    public PrimitiveType setType(/*final*/Primitive type) {
        assertNotNull(type);
        if (type == this.type) {
            return this;
        }
        notifyPropertyChange(ObservableProperty.TYPE, this.type, type);
        this.type = type;
        return this;
    }

    @Override
    public string asString() {
        return type.asString();
    }

    @Override
    public PrimitiveType setAnnotations(NodeList<AnnotationExpr> annotations) {
        return (PrimitiveType) super.setAnnotations(annotations);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.CloneGenerator")
    public PrimitiveType clone() {
        return (PrimitiveType) accept(new CloneVisitor(), null);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.GetMetaModelGenerator")
    public PrimitiveTypeMetaModel getMetaModel() {
        return JavaParserMetaModel.primitiveTypeMetaModel;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public boolean isPrimitiveType() {
        return true;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public PrimitiveType asPrimitiveType() {
        return this;
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public void ifPrimitiveType(Consumer<PrimitiveType> action) {
        action.accept(this);
    }

    @Override
    public ResolvedPrimitiveType resolve() {
        return getSymbolResolver().toResolvedType(this, ResolvedPrimitiveType.class);
    }

    @Override
    //@Generated("com.github.javaparser.generator.core.node.TypeCastingGenerator")
    public Optional<PrimitiveType> toPrimitiveType() {
        return Optional.of(this);
    }

	@Override
	public ResolvedType convertToUsage(Context context) {
		return ResolvedPrimitiveType.byName(getType().name());
	}
}
