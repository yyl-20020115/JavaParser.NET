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

namespace com.github.javaparser.printer.configuration.imports;




public class DefaultImportOrderingStrategy implements ImportOrderingStrategy {

    private bool sortImportsAlphabetically = false;

    //@Override
    public List<NodeList<ImportDeclaration>> sortImports(NodeList<ImportDeclaration> nodes) {

        if (sortImportsAlphabetically) {
            Comparator<ImportDeclaration> sortLogic = comparingInt((ImportDeclaration i) -> i.isStatic() ? 0 : 1)
                    .thenComparing(NodeWithName::getNameAsString);
            nodes.sort(sortLogic);
        }

        return Collections.singletonList(nodes);
    }

    //@Override
    public void setSortImportsAlphabetically(bool sortImportsAlphabetically) {
        this.sortImportsAlphabetically = sortImportsAlphabetically;
    }

    //@Override
    public bool isSortImportsAlphabetically() {
        return sortImportsAlphabetically;
    }

}
