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
namespace com.github.javaparser.resolution.types.parametrization;



/**
 * A map of values associated to TypeParameters.
 *
 * @author Federico Tomassetti
 */
public class ResolvedTypeParametersMap {

    public static class Builder {

        private Map<String, ResolvedType> nameToValue;

        private Map<String, ResolvedTypeParameterDeclaration> nameToDeclaration;

        public Builder() {
            nameToValue = new HashMap<>();
            nameToDeclaration = new HashMap<>();
        }

        private Builder(Map<String, ResolvedType> nameToValue, Map<String, ResolvedTypeParameterDeclaration> nameToDeclaration) {
            this.nameToValue = new HashMap<>();
            this.nameToValue.putAll(nameToValue);
            this.nameToDeclaration = new HashMap<>();
            this.nameToDeclaration.putAll(nameToDeclaration);
        }

        public ResolvedTypeParametersMap build() {
            return new ResolvedTypeParametersMap(nameToValue, nameToDeclaration);
        }

        public Builder setValue(ResolvedTypeParameterDeclaration typeParameter, ResolvedType value) {
            // TODO: we shouldn't just silently overwrite existing types!
            string qualifiedName = typeParameter.getQualifiedName();
            nameToValue.put(qualifiedName, value);
            nameToDeclaration.put(qualifiedName, typeParameter);
            return this;
        }
    }

    //@Override
    public bool equals(Object o) {
        if (this == o)
            return true;
        if (!(o is ResolvedTypeParametersMap))
            return false;
        ResolvedTypeParametersMap that = (ResolvedTypeParametersMap) o;
        return nameToValue.equals(that.nameToValue) && nameToDeclaration.equals(that.nameToDeclaration);
    }

    //@Override
    public int hashCode() {
        return nameToValue.hashCode();
    }

    //@Override
    public string toString() {
        return "TypeParametersMap{" + "nameToValue=" + nameToValue + '}';
    }

    private Map<String, ResolvedType> nameToValue;

    private Map<String, ResolvedTypeParameterDeclaration> nameToDeclaration;

    public static ResolvedTypeParametersMap empty() {
        return new Builder().build();
    }

    private ResolvedTypeParametersMap(Map<String, ResolvedType> nameToValue, Map<String, ResolvedTypeParameterDeclaration> nameToDeclaration) {
        this.nameToValue = new HashMap<>();
        this.nameToValue.putAll(nameToValue);
        this.nameToDeclaration = new HashMap<>();
        this.nameToDeclaration.putAll(nameToDeclaration);
    }

    public ResolvedType getValue(ResolvedTypeParameterDeclaration typeParameter) {
        string qualifiedName = typeParameter.getQualifiedName();
        if (nameToValue.containsKey(qualifiedName)) {
            return nameToValue.get(qualifiedName);
        } else {
            return new ResolvedTypeVariable(typeParameter);
        }
    }

    public Optional<ResolvedType> getValueBySignature(string signature) {
        if (nameToValue.containsKey(signature)) {
            return Optional.of(nameToValue.get(signature));
        } else {
            return Optional.empty();
        }
    }

    public List<String> getNames() {
        return new ArrayList<>(nameToValue.keySet());
    }

    public List<ResolvedType> getTypes() {
        return new ArrayList<>(nameToValue.values());
    }

    public Builder toBuilder() {
        return new Builder(nameToValue, nameToDeclaration);
    }

    public bool isEmpty() {
        return nameToValue.isEmpty();
    }

    public ResolvedType replaceAll(ResolvedType type) {
        Map<ResolvedTypeParameterDeclaration, ResolvedType> inferredTypes = new HashMap<>();
        for (ResolvedTypeParameterDeclaration typeParameterDeclaration : this.nameToDeclaration.values()) {
            type = type.replaceTypeVariables(typeParameterDeclaration, getValue(typeParameterDeclaration), inferredTypes);
        }
        return type;
    }
}
