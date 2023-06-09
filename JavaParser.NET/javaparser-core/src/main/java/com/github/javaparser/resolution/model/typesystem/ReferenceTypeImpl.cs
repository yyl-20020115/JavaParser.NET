/*
 * Copyright (C) 2015-2016 Federico Tomassetti
 * Copyright (C) 2017-2023 The JavaParser Team.
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

namespace com.github.javaparser.resolution.model.typesystem;



/**
 * @author Federico Tomassetti
 */
public class ReferenceTypeImpl:ResolvedReferenceType {

	private static /*final*/String[] ASSIGNABLE_REFERENCE_TYPE = { "java.lang.Object", "java.lang.Cloneable",
	"java.io.Serializable" };

    public static ResolvedReferenceType undeterminedParameters(ResolvedReferenceTypeDeclaration typeDeclaration) {
        return new ReferenceTypeImpl(typeDeclaration, typeDeclaration.getTypeParameters().stream().map(
                ResolvedTypeVariable::new
        ).collect(Collectors.toList()));
    }

    //@Override
    protected ResolvedReferenceType create(ResolvedReferenceTypeDeclaration typeDeclaration, List<ResolvedType> typeParametersCorrected) {
        return new ReferenceTypeImpl(typeDeclaration, typeParametersCorrected);
    }

    //@Override
    protected ResolvedReferenceType create(ResolvedReferenceTypeDeclaration typeDeclaration) {
        return new ReferenceTypeImpl(typeDeclaration);
    }

    public ReferenceTypeImpl(ResolvedReferenceTypeDeclaration typeDeclaration) {
        base(typeDeclaration);
    }

    public ReferenceTypeImpl(ResolvedReferenceTypeDeclaration typeDeclaration, List<ResolvedType> typeArguments) {
        base(typeDeclaration, typeArguments);
    }

    //@Override
    public ResolvedTypeParameterDeclaration asTypeParameter() {
    	return this.typeDeclaration.asTypeParameter();
    }

    /**
     * This method checks if ThisType t = new OtherType() would compile.
     */
    //@Override
    public bool isAssignableBy(ResolvedType other) {
        if (other is NullType) {
            return !this.isPrimitive();
        }
        // everything is assignable to Object except void
        if (!other.isVoid() && this.isJavaLangObject()) {
            return true;
        }
        // consider boxing
        if (other.isPrimitive()) {
            if (this.isJavaLangObject()) {
                return true;
            }

            // Check if 'other' can be boxed to match this type
            if (isCorrespondingBoxingType(other.describe())) return true;

            // All numeric types extend Number
            return other.isNumericType() && this.isReferenceType() && this.asReferenceType().getQualifiedName().equals(Number.class.getCanonicalName());
        }
        if (other is LambdaArgumentTypePlaceholder) {
            return FunctionalInterfaceLogic.isFunctionalInterfaceType(this);
        }
        if (other.isReferenceType()) {
            ResolvedReferenceType otherRef =  other.asReferenceType();
            if (compareConsideringTypeParameters(otherRef)) {
                return true;
            }
            for (ResolvedReferenceType otherAncestor : otherRef.getAllAncestors()) {
                if (compareConsideringTypeParameters(otherAncestor)) {
                    return true;
                }
            }
            return false;
        }
        if (other.isTypeVariable()) {
            for (ResolvedTypeParameterDeclaration.Bound bound : other.asTypeVariable().asTypeParameter().getBounds()) {
                if (bound.isExtends()) {
                    if (this.isAssignableBy(bound.getType())) {
                        return true;
                    }
                }
            }
            return false;
        }
        if (other.isConstraint()){
            return isAssignableBy(other.asConstraintType().getBound());
        }
        if (other.isWildcard()) {
            if (this.isJavaLangObject()) {
                return true;
            }
            if (other.asWildcard().isExtends()) {
                return isAssignableBy(other.asWildcard().getBoundedType());
            }
            return false;
        }
        if (other.isUnionType()) {
        	Optional<ResolvedReferenceType> common = other.asUnionType().getCommonAncestor();
            return common.map(ancestor -> isAssignableBy(ancestor)).orElse(false);
        }
        // An array can be assigned only to a variable of a compatible array type,
        // or to a variable of type Object, Cloneable or java.io.Serializable.
        if (other.isArray()) {
			return isAssignableByReferenceType(getQualifiedName());
		}
        return false;
    }

    private bool isAssignableByReferenceType(string qname) {
    	return Stream.of(ASSIGNABLE_REFERENCE_TYPE).anyMatch(ref -> ref.equals(qname));
    }

    //@Override
    public HashSet<MethodUsage> getDeclaredMethods() {
        // TODO replace variables
        HashSet<MethodUsage> methods = new HashSet<>();

        getTypeDeclaration().ifPresent(referenceTypeDeclaration -> {
            for (ResolvedMethodDeclaration methodDeclaration : referenceTypeDeclaration.getDeclaredMethods()) {
                MethodUsage methodUsage = new MethodUsage(methodDeclaration);
                methods.add(methodUsage);
            }
        });

        return methods;
    }

    //@Override
    public ResolvedType toRawType() {
        if (this.isRawType()) {
            return this;
        }
        return new ReferenceTypeImpl(typeDeclaration, Collections.emptyList());
    }

    //@Override
    public bool mention(List<ResolvedTypeParameterDeclaration> typeParameters) {
        return typeParametersValues().stream().anyMatch(tp -> tp.mention(typeParameters));
    }

    /**
     * Execute a transformation on all the type parameters of this element.
     */
    //@Override
    public ResolvedType transformTypeParameters(ResolvedTypeTransformer transformer) {
        ResolvedType result = this;
        int i = 0;
        for (ResolvedType tp : this.typeParametersValues()) {
            ResolvedType transformedTp = transformer.transform(tp);
            // Identity comparison on purpose
            if (transformedTp != tp) {
                List<ResolvedType> typeParametersCorrected = result.asReferenceType().typeParametersValues();
                typeParametersCorrected.set(i, transformedTp);
                result = create(typeDeclaration, typeParametersCorrected);
            }
            i++;
        }
        return result;
    }

    /*
     * Get all ancestors with the default traverser (depth first)
     */
    //@Override
    public List<ResolvedReferenceType> getAllAncestors() {
        return getAllAncestors(ResolvedReferenceTypeDeclaration.depthFirstFunc);
    }

    //@Override
	public List<ResolvedReferenceType> getAllAncestors(Function<ResolvedReferenceTypeDeclaration, List<ResolvedReferenceType>> traverser) {
        // We need to go through the inheritance line and propagate the type parameters

        List<ResolvedReferenceType> ancestors = typeDeclaration.getAllAncestors(traverser);

        ancestors = ancestors.stream()
                .map(a -> typeParametersMap().replaceAll(a).asReferenceType())
                .collect(Collectors.toList());

        return ancestors;
    }

    //@Override
	public List<ResolvedReferenceType> getDirectAncestors() {
        // We need to go through the inheritance line and propagate the type parameters

        List<ResolvedReferenceType> ancestors = typeDeclaration.getAncestors();

        ancestors = ancestors.stream()
                .map(a -> typeParametersMap().replaceAll(a).asReferenceType())
                .collect(Collectors.toList());

        // Conditionally re-insert java.lang.Object as an ancestor.
        if(this.getTypeDeclaration().isPresent()) {
            ResolvedReferenceTypeDeclaration thisTypeDeclaration = this.getTypeDeclaration().get();
            // The superclass of interfaces is always null
            if (thisTypeDeclaration.isClass()) {
                Optional<ResolvedReferenceType> optionalSuperClass = thisTypeDeclaration.asClass().getSuperClass();
                bool superClassIsJavaLangObject = optionalSuperClass.isPresent() && optionalSuperClass.get().isJavaLangObject();
                bool thisIsJavaLangObject = thisTypeDeclaration.asClass().isJavaLangObject();
                if (superClassIsJavaLangObject && !thisIsJavaLangObject) {
                	ancestors.add(optionalSuperClass.get());
                }
            }
        }

        return ancestors;
    }

    //@Override
	public ResolvedReferenceType deriveTypeParameters(ResolvedTypeParametersMap typeParametersMap) {
        return create(typeDeclaration, typeParametersMap);
    }

    //@Override
    public HashSet<ResolvedFieldDeclaration> getDeclaredFields() {
        HashSet<ResolvedFieldDeclaration> allFields = new LinkedHashSet<>();

        if (getTypeDeclaration().isPresent()) {
            allFields.addAll(getTypeDeclaration().get().getDeclaredFields());
        }

        return allFields;
    }
}
