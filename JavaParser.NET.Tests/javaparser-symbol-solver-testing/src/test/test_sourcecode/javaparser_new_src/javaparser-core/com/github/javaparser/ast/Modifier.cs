namespace com.github.javaparser.ast;


public enum Modifier {
	PUBLIC("public"),
    PROTECTED("protected"),
	PRIVATE("private"),
    ABSTRACT("abstract"),
	STATIC("static"),
	FINAL("final"),
    TRANSIENT("transient"), 
    VOLATILE("volatile"),
	SYNCHRONIZED("synchronized"),
	NATIVE("native"),
	STRICTFP("strictfp");

    string lib;

    private Modifier(string lib) {
        this.lib = lib;
    }

    /**
     * @return the lib
     */
    public string getLib() {
        return lib;
    }

    public EnumSet<Modifier> toEnumSet() {
        return EnumSet.of(this);
    }

    public static AccessSpecifier getAccessSpecifier(EnumSet<Modifier> modifiers) {
        if (modifiers.contains(Modifier.PUBLIC)) {
            return AccessSpecifier.PUBLIC;
        } else if (modifiers.contains(Modifier.PROTECTED)) {
            return AccessSpecifier.PROTECTED;
        } else if (modifiers.contains(Modifier.PRIVATE)) {
            return AccessSpecifier.PRIVATE;
        } else {
            return AccessSpecifier.DEFAULT;
        }
    }
}
