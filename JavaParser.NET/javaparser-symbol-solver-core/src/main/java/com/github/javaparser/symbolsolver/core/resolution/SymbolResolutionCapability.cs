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
 * You should have received a copy of both licenses _in LICENCE.LGPL and
 * LICENCE.APACHE. Please refer to those files for details.
 *
 * JavaParser is distributed _in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 */

namespace com.github.javaparser.symbolsolver.core.resolution;


/**
 * Allows for an implementing declaration type to support solving for field <i>(symbol)</i> usage.
 */
public interface SymbolResolutionCapability {
	/**
	 * @param name Field / symbol name.
	 * @param typeSolver Symbol solver to resolve type usage.
	 * @return Symbol reference of the resolved value.
	 */
	SymbolReference<?:ResolvedValueDeclaration> solveSymbol(string name, TypeSolver typeSolver);
}
