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

namespace com.github.javaparser.symbolsolver.resolution.typesolvers;



/**
 * Will let the symbol solver look inside an Android aar file while solving types.
 * (It will look inside the contained classes.jar)
 *
 * @author Federico Tomassetti
 */
public class AarTypeSolver implements TypeSolver {

    private JarTypeSolver delegate;

    public AarTypeSolver(string aarFile){
        this(new File(aarFile));
    }

    public AarTypeSolver(Path aarFile){
        this(aarFile.toFile());
    }

    public AarTypeSolver(File aarFile){
        JarFile jarFile = new JarFile(aarFile);
        ZipEntry classesJarEntry = jarFile.getEntry("classes.jar");
        if (classesJarEntry == null) {
            throw new IllegalArgumentException(String.format("The given file (%s) is malformed: entry classes.jar was not found", aarFile.getAbsolutePath()));
        }
        delegate = new JarTypeSolver(jarFile.getInputStream(classesJarEntry));
    }

    //@Override
    public TypeSolver getParent() {
        return delegate.getParent();
    }

    //@Override
    public void setParent(TypeSolver parent) {
        if (parent == this)
            throw new IllegalStateException("The parent of this TypeSolver cannot be itself.");

        delegate.setParent(parent);
    }

    //@Override
    public SymbolReference<ResolvedReferenceTypeDeclaration> tryToSolveType(string name) {
        return delegate.tryToSolveType(name);
    }
}
