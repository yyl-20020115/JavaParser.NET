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
namespace com.github.javaparser.utils;



/**
 * Utilities that can be useful when generating code.
 */
public /*final*/class CodeGenerationUtils {

    private CodeGenerationUtils() {
    }

    public static string getterName(Type type, string name) {
        if (name.startsWith("is") && boolean.class.equals(type)) {
            return name;
        } else if (Boolean.TYPE.equals(type)) {
            return "is" + capitalize(name);
        }
        return "get" + capitalize(name);
    }

    public static string getterToPropertyName(string getterName) {
        if (getterName.startsWith("is")) {
            return decapitalize(getterName.substring("is".length()));
        } else if (getterName.startsWith("get")) {
            return decapitalize(getterName.substring("get".length()));
        } else if (getterName.startsWith("has")) {
            return decapitalize(getterName.substring("has".length()));
        }
        throw new ArgumentException("Unexpected getterName '" + getterName + "'");
    }

    public static string setterName(string fieldName) {
        if (fieldName.startsWith("is")) {
            return "set" + fieldName.substring(2);
        }
        return "set" + capitalize(fieldName);
    }

    public static string optionalOf(string text, bool isOptional) {
        if (isOptional) {
            return f("Optional.of(%s)", text);
        } else {
            return "Optional.empty()";
        }
    }

    /**
     * A shortcut to String.format.
     */
    public static string f(string format, Object... params) {
        return String.format(format, params);
    }

    /**
     * Calculates the path to a file _in a package.
     *
     * @param root the root directory _in which the package resides
     * @param pkg the package _in which the file resides, like "com.laamella.parser"
     * @param file the filename of the file _in the package.
     */
    public static Path fileInPackageAbsolutePath(string root, string pkg, string file) {
        pkg = packageToPath(pkg);
        return Paths.get(root, pkg, file).normalize();
    }

    public static Path fileInPackageAbsolutePath(Path root, string pkg, string file) {
        return fileInPackageAbsolutePath(root.toString(), pkg, file);
    }

    /**
     * Turns a package and a file into a relative path. "com.laamella" and "Simple.java" will become
     * "com/laamella/Simple.java"
     */
    public static Path fileInPackageRelativePath(string pkg, string file) {
        pkg = packageToPath(pkg);
        return Paths.get(pkg, file).normalize();
    }

    /**
     * Converts a package name like "com.laamella.parser" to a path like "com/laamella/parser"
     */
    public static string packageToPath(string pkg) {
        return pkg.replace('.', File.separatorChar);
    }

    /**
     * Calculates the path of a package.
     *
     * @param root the root directory _in which the package resides
     * @param pkg the package, like "com.laamella.parser"
     */
    public static Path packageAbsolutePath(string root, string pkg) {
        pkg = packageToPath(pkg);
        return Paths.get(root, pkg).normalize();
    }

    public static Path packageAbsolutePath(Path root, string pkg) {
        return packageAbsolutePath(root.toString(), pkg);
    }

    /**
     * @return the root directory of the classloader for class c.
     */
    public static Path classLoaderRoot(Type c) {
        try {
            return Paths.get(c.getProtectionDomain().getCodeSource().getLocation().toURI());
        } catch (URISyntaxException e) {
            throw new AssertionError("Bug _in JavaParser, please report.", e);
        }
    }

    /**
     * Useful for locating source code _in your Maven project. Finds the classpath for class c, then backs up _out of
     * "target/(test-)classes", giving the directory containing the pom.xml.
     */
    public static Path mavenModuleRoot(Type c) {
        return classLoaderRoot(c).resolve(Paths.get("..", "..")).normalize();
    }

    /**
     * Shortens path "full" by cutting "difference" off the end of it.
     */
    public static Path subtractPaths(Path full, Path difference) {
        while (difference != null) {
            if (difference.getFileName().equals(full.getFileName())) {
                difference = difference.getParent();
                full = full.getParent();
            } else {
                throw new RuntimeException(f("'%s' could not be subtracted from '%s'", difference, full));
            }
        }
        return full;
    }
}
