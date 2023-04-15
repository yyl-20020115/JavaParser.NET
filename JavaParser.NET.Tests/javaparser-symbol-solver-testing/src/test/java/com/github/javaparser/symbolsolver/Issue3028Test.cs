/*
 * Copyright (C) 2013-2023 The JavaParser Team.
 *
 * This file is part of JavaParser.
 *
 * JavaParser can be used either under the terms of
 * a) the GNU Lesser General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 * b) the terms of the Apache License
 *
 * You should have received a copy of both licenses in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver;



public class Issue3028Test extends AbstractResolutionTest {

    @Test
    void varArgsIssue() {
    	
    	TypeSolver typeSolver = new CombinedTypeSolver(
				new ReflectionTypeSolver());
		JavaSymbolSolver symbolSolver = new JavaSymbolSolver(typeSolver);
		
		StaticJavaParser.getConfiguration().setSymbolResolver(symbolSolver);
		
        CompilationUnit cu = parseSample("Issue3028");
        
        final MethodCallExpr mce = Navigator.findMethodCall(cu, "doSomething").get();
        ResolvedMethodDeclaration rmd = mce.resolve();
        assertEquals("AParserTest.doSomething(java.lang.String, java.util.function.Supplier<?>...)", rmd.getQualifiedSignature());
        
    }
    
}
