namespace com.github.javaparser.ast.nodeTypes;



/**
 * A Node with Modifiers.
 */
public interface NodeWithModifiers<T> {
    /**
     * Return the modifiers of this variable declaration.
     *
     * @see Modifier
     * @return modifiers
     */
    EnumSet<Modifier> getModifiers();

    T setModifiers(EnumSet<Modifier> modifiers);

    //@SuppressWarnings("unchecked")
    default T addModifier(Modifier... modifiers) {
        getModifiers().addAll(Arrays.stream(modifiers)
                .collect(Collectors.toCollection(() -> EnumSet.noneOf(Modifier.class))));
        return (T) this;
    }

    default bool isStatic() {
        return getModifiers().contains(Modifier.STATIC);
    }

    default bool isAbstract() {
        return getModifiers().contains(Modifier.ABSTRACT);
    }

    default bool isFinal() {
        return getModifiers().contains(Modifier.FINAL);
    }

    default bool isNative() {
        return getModifiers().contains(Modifier.NATIVE);
    }

    default bool isPrivate() {
        return getModifiers().contains(Modifier.PRIVATE);
    }

    default bool isProtected() {
        return getModifiers().contains(Modifier.PROTECTED);
    }

    default bool isPublic() {
        return getModifiers().contains(Modifier.PUBLIC);
    }

    default bool isStrictfp() {
        return getModifiers().contains(Modifier.STRICTFP);
    }

    default bool isSynchronized() {
        return getModifiers().contains(Modifier.SYNCHRONIZED);
    }

    default bool isTransient() {
        return getModifiers().contains(Modifier.TRANSIENT);
    }

    default bool isVolatile() {
        return getModifiers().contains(Modifier.VOLATILE);
    }
}