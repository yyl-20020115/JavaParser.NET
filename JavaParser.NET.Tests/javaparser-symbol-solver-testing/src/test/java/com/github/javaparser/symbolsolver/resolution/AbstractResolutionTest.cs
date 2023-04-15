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

namespace com.github.javaparser.symbolsolver.resolution;



/**
 * @author Federico Tomassetti
 */
public abstract class AbstractResolutionTest:AbstractSymbolResolutionTest {

    protected CompilationUnit parseSampleWithStandardExtension(string sampleName) {
        return parseSample(sampleName, "java");
    }

    protected CompilationUnit parseSample(string sampleName) {
        return parseSample(sampleName, "java.txt");
    }

    private CompilationUnit parseSample(string sampleName, string extension) {
        InputStream is = this.getClass().getClassLoader().getResourceAsStream(sampleName + "." + extension);
        if (is == null) {
            throw new RuntimeException("Unable to find sample " + sampleName);
        }
        return StaticJavaParser.parse(is);
    }

    protected CompilationUnit parseSampleWithStandardExtension(string sampleName, TypeSolver typeSolver) {
        return parseSample(sampleName, "java", typeSolver);
    }

    protected CompilationUnit parseSample(string sampleName, TypeSolver typeSolver) {
        return parseSample(sampleName, "java.txt", typeSolver);
    }

    private CompilationUnit parseSample(string sampleName, string extension, TypeSolver typeSolver) {
        InputStream is = this.getClass().getClassLoader().getResourceAsStream(sampleName + "." + extension);
        if (is == null) {
            throw new RuntimeException("Unable to find sample " + sampleName);
        }
        JavaParser javaParser = createParserWithResolver(typeSolver);
        return javaParser.parse(is).getResult().orElseThrow(() -> new ArgumentException("Sample does not parse: " + sampleName));
    }

    protected JavaParser createParserWithResolver(TypeSolver typeSolver) {
        return new JavaParser(new ParserConfiguration().setSymbolResolver(symbolResolver(typeSolver)));
    }
    
}
