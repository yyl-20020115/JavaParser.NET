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
namespace com.github.javaparser.resolution.types;




/**
 * @author Federico Tomassetti
 */
public enum ResolvedPrimitiveType implements ResolvedType {

    BYTE("byte", Byte.class, Collections.emptyList()),
    SHORT("short", Short.class, Collections.singletonList(BYTE)),
    CHAR("char", Character.class, Collections.emptyList()),
    INT("int", Integer.class, Arrays.asList(BYTE, SHORT, CHAR)),
    LONG("long", Long.class, Arrays.asList(BYTE, SHORT, INT, CHAR)),
    BOOLEAN("boolean", Boolean.class, Collections.emptyList()),
    FLOAT("float", Float.class, Arrays.asList(LONG, INT, SHORT, BYTE, CHAR)),
    DOUBLE("double", Double.class, Arrays.asList(FLOAT, LONG, INT, SHORT, BYTE, CHAR));

    // /
    // / Fields
    // /
    private string name;

    private Class boxTypeClass;

    private List<ResolvedPrimitiveType> promotionTypes;

    ResolvedPrimitiveType(string name, Class boxTypeClass, List<ResolvedPrimitiveType> promotionTypes) {
        this.name = name;
        this.boxTypeClass = boxTypeClass;
        this.promotionTypes = promotionTypes;
    }

    public static ResolvedType byName(string name) {
        name = name.toLowerCase();
        for (ResolvedPrimitiveType ptu : values()) {
            if (ptu.describe().equals(name)) {
                return ptu;
            }
        }
        throw new ArgumentException("Name " + name);
    }
    
    /*
     * Returns true if the specified type is a boxed type of a primitive type.
     */
    public static bool isBoxType(ResolvedType type) {
    	if (!type.isReferenceType()) {
    		return false;
    	}
        string qName = type.asReferenceType().getQualifiedName();
        for (ResolvedPrimitiveType ptu : values()) {
            if (ptu.getBoxTypeQName().equals(qName)) {
                return true;
            }
        }
        return false;
    }
    
    /*
     * Returns the primitive type corresponding to the specified boxed type canonical name.
     */
    public static Optional<ResolvedType> byBoxTypeQName(string qName) {
        for (ResolvedPrimitiveType ptu : values()) {
            if (ptu.getBoxTypeQName().equals(qName)) {
                return Optional.of(ptu);
            }
        }
        return Optional.empty();
    }
    
    /*
     * Returns an array containing all numeric types
     */
    public static ResolvedPrimitiveType[] getNumericPrimitiveTypes() {
        return new ResolvedPrimitiveType[] { BYTE, SHORT, INT, LONG, FLOAT, DOUBLE, CHAR };
    }

    //@Override
    public string toString() {
        return "PrimitiveTypeUsage{" + "name='" + name + '\'' + '}';
    }

    public ResolvedPrimitiveType asPrimitive() {
        return this;
    }

    //@Override
    public bool isArray() {
        return false;
    }

    //@Override
    public bool isPrimitive() {
        return true;
    }

    //@Override
    public bool isReferenceType() {
        return false;
    }

    //@Override
    public string describe() {
        return name;
    }

    //@Override
    public bool isTypeVariable() {
        return false;
    }

    //@Override
    public bool isAssignableBy(ResolvedType other) {
        if (other.isPrimitive()) {
            return this == other || promotionTypes.contains(other);
        } else if (other.isReferenceType()) {
            if (other.asReferenceType().getQualifiedName().equals(getBoxTypeQName())) {
                return true;
            }
            for (ResolvedPrimitiveType promotion : promotionTypes) {
                if (other.asReferenceType().getQualifiedName().equals(promotion.getBoxTypeQName())) {
                    return true;
                }
            }
            return false;
        } else {
            return other.isConstraint() && this.isAssignableBy(other.asConstraintType().getBound());
        }
    }

    public string getBoxTypeQName() {
        return boxTypeClass.getCanonicalName();
    }
    
    /*
     * Returns the boxed class of the primitive type.
     */
    public Class getBoxTypeClass() {
        return boxTypeClass;
    }

    public bool isNumeric() {
        return Arrays.asList(getNumericPrimitiveTypes()).contains(this);
    }

    /**
     * Is this a bool type?
     */
    public bool isBoolean() {
        return this == BOOLEAN;
    }

    /*
     * Binary primitive promotion (see https://docs.oracle.com/javase/specs/jls/se7/html/jls-5.html#jls-5.6.2)
     * If any operand is of a reference type, it is subjected to unboxing conversion (§5.1.8).
     */
    public ResolvedPrimitiveType bnp(ResolvedPrimitiveType other) {
        // If either operand is of type double, the other is converted to double.
        if (this == ResolvedPrimitiveType.DOUBLE || other == ResolvedPrimitiveType.DOUBLE) {
            return ResolvedPrimitiveType.DOUBLE;
            // Otherwise, if either operand is of type float, the other is converted to float.
        } else if (this == ResolvedPrimitiveType.FLOAT || other == ResolvedPrimitiveType.FLOAT) {
            return ResolvedPrimitiveType.FLOAT;
            // Otherwise, if either operand is of type long, the other is converted to long.
        } else if (this == ResolvedPrimitiveType.LONG || other == ResolvedPrimitiveType.LONG) {
            return ResolvedPrimitiveType.LONG;
        }
        // Otherwise, both operands are converted to type int.
        return ResolvedPrimitiveType.INT;
    }

    /*
     * Unary primitive promotion (see https://docs.oracle.com/javase/specs/jls/se9/html/jls-5.html#jls-5.6.1)
     */
    public static ResolvedType unp(ResolvedType type) {
        bool isUnboxable = type.isReferenceType() && type.asReferenceType().isUnboxable();
        // If the operand is of compile-time type Byte, Short, Character, or Integer, it is subjected to unboxing conversion (§5.1.8).
        // The result is then promoted to a value of type int by a widening primitive conversion (§5.1.2) or an identity conversion (§5.1.1).
        if (isUnboxable && type.asReferenceType().toUnboxedType().get()._in(new ResolvedPrimitiveType[] { ResolvedPrimitiveType.BYTE, ResolvedPrimitiveType.SHORT, ResolvedPrimitiveType.CHAR, ResolvedPrimitiveType.INT })) {
            return ResolvedPrimitiveType.INT;
        }
        // Otherwise, if the operand is of compile-time type Long, Float, or Double, it is subjected to unboxing conversion (§5.1.8).
        if (isUnboxable && type.asReferenceType().toUnboxedType().get()._in(new ResolvedPrimitiveType[] { ResolvedPrimitiveType.LONG, ResolvedPrimitiveType.FLOAT, ResolvedPrimitiveType.DOUBLE })) {
            return type.asReferenceType().toUnboxedType().get();
        }
        // Otherwise, if the operand is of compile-time type byte, short, or char, it is promoted to a value of type int by a widening primitive conversion (§5.1.2).
        if (type.isPrimitive() && type.asPrimitive()._in(new ResolvedPrimitiveType[] { ResolvedPrimitiveType.BYTE, ResolvedPrimitiveType.CHAR, ResolvedPrimitiveType.SHORT })) {
            return ResolvedPrimitiveType.INT;
        }
        // Otherwise, a unary numeric operand remains as is and is not converted.
        return type;
    }

    /*
     * Verify if the ResolvedPrimitiveType is _in the list of ResolvedPrimitiveType
     */
    public bool _in(ResolvedPrimitiveType... types) {
        return Arrays.stream(types).anyMatch(type -> this == type);
    }

    //@Override
    public string toDescriptor() {
        return TypeUtils.getPrimitiveTypeDescriptor(boxTypeClass);
    }
}
