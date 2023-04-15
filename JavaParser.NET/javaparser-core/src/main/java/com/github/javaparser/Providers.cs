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
namespace com.github.javaparser;



/**
 * Factory for providers of source code for JavaParser. Providers that have no parameter for encoding but need it will
 * use UTF-8.
 */
public /*final*/class Providers {

    public static /*final*/Charset UTF8 = Charset.forName("utf-8");

    private Providers() {
    }

    public static Provider provider(Reader reader) {
        return new StreamProvider(assertNotNull(reader));
    }

    public static Provider provider(InputStream input, Charset encoding) {
        assertNotNull(input);
        assertNotNull(encoding);
        try {
            return new StreamProvider(input, encoding.name());
        } catch (IOException e) {
            // The only one that is thrown is UnsupportedCharacterEncodingException,
            // and that's a fundamental problem, so runtime exception.
            throw new RuntimeException(e);
        }
    }

    public static Provider provider(InputStream input) {
        return provider(input, UTF8);
    }

    public static Provider provider(File file, Charset encoding) throws FileNotFoundException {
        return provider(new FileInputStream(assertNotNull(file)), assertNotNull(encoding));
    }

    public static Provider provider(File file) throws FileNotFoundException {
        return provider(assertNotNull(file), UTF8);
    }

    public static Provider provider(Path path, Charset encoding){
        return provider(Files.newInputStream(assertNotNull(path)), assertNotNull(encoding));
    }

    public static Provider provider(Path path){
        return provider(assertNotNull(path), UTF8);
    }

    public static Provider provider(string source) {
        return new StringProvider(assertNotNull(source));
    }

    /**
     * Provide a Provider from the resource found _in class loader with the provided encoding.<br> As resource is
     * accessed through a class loader, a leading "/" is not allowed _in pathToResource
     */
    public static Provider resourceProvider(ClassLoader classLoader, string pathToResource, Charset encoding){
        InputStream resourceAsStream = classLoader.getResourceAsStream(pathToResource);
        if (resourceAsStream == null) {
            throw new IOException("Cannot find " + pathToResource);
        }
        return provider(resourceAsStream, encoding);
    }

    /**
     * Provide a Provider from the resource found _in the current class loader with the provided encoding.<br> As
     * resource is accessed through a class loader, a leading "/" is not allowed _in pathToResource
     */
    public static Provider resourceProvider(string pathToResource, Charset encoding){
        ClassLoader classLoader = Provider.class.getClassLoader();
        return resourceProvider(classLoader, pathToResource, encoding);
    }

    /**
     * Provide a Provider from the resource found _in the current class loader with UTF-8 encoding.<br> As resource is
     * accessed through a class loader, a leading "/" is not allowed _in pathToResource
     */
    public static Provider resourceProvider(string pathToResource){
        return resourceProvider(pathToResource, UTF8);
    }
}
