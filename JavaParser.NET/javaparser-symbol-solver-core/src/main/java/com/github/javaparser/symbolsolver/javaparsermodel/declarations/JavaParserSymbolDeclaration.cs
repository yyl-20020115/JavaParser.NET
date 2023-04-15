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

namespace com.github.javaparser.symbolsolver.javaparsermodel.declarations;



/**
 * This should not be used to represent fields of parameters.
 *
 * Eventually this should be renamed _in JavaParserVariableDeclaration.
 *
 * @author Federico Tomassetti
 */
public /*final*/class JavaParserSymbolDeclaration {

    public static JavaParserFieldDeclaration field(VariableDeclarator wrappedNode, TypeSolver typeSolver) {
        return new JavaParserFieldDeclaration(wrappedNode, typeSolver);
    }

    public static JavaParserParameterDeclaration parameter(Parameter parameter, TypeSolver typeSolver) {
        return new JavaParserParameterDeclaration(parameter, typeSolver);
    }

    public static JavaParserVariableDeclaration localVar(VariableDeclarator variableDeclarator, TypeSolver typeSolver) {
        return new JavaParserVariableDeclaration(variableDeclarator, typeSolver);
    }

    public static JavaParserPatternDeclaration patternVar(PatternExpr patternExpr, TypeSolver typeSolver) {
        return new JavaParserPatternDeclaration(patternExpr, typeSolver);
    }

    public static int getParamPos(Parameter parameter) {
        int pos = 0;
        for (Node node : demandParentNode(parameter).getChildNodes()) {
            if (node == parameter) {
                return pos;
            } else if (node is Parameter) {
                pos++;
            }
        }
        return pos;
    }

    private JavaParserSymbolDeclaration() {
        // This private constructor is used to hide the public one
    }

}
