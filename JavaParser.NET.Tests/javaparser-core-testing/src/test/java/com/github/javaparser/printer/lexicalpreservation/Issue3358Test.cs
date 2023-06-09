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

namespace com.github.javaparser.printer.lexicalpreservation;



public class Issue3358Test:AbstractLexicalPreservingTest  {
    
    [TestMethod]
    void testArrayTypeWithBracketAfterTypeWithoutWhitespace() {
        string def = "int[] i";
        considerVariableDeclaration(def);
        expression.asVariableDeclarationExpr().getModifiers().addFirst(Modifier.privateModifier());
        assertTrue(LexicalPreservingPrinter.getOrCreateNodeText(expression).getElements().stream()
                .anyMatch(elem -> elem.expand().equals(Keyword.PRIVATE.asString())));
        assertTrue(LexicalPreservingPrinter.print(expression).equals("private int[] i"));
    }
    
    [TestMethod]
    void testArrayTypeWithWhitespaceBeforeTypeAndBracket() {
        string def = "int [] i";
        considerVariableDeclaration(def);
        expression.asVariableDeclarationExpr().getModifiers().addFirst(Modifier.privateModifier());
        assertTrue(LexicalPreservingPrinter.getOrCreateNodeText(expression).getElements().stream()
                .anyMatch(elem -> elem.expand().equals(Keyword.PRIVATE.asString())));
        assertTrue(LexicalPreservingPrinter.print(expression).equals("private int [] i"));
    }
    
    [TestMethod]
    void testArrayTypeWithWhitespaceBeforeEachToken() {
        string def = "int [ ] i";
        considerVariableDeclaration(def);
        expression.asVariableDeclarationExpr().getModifiers().addFirst(Modifier.privateModifier());
        assertTrue(LexicalPreservingPrinter.getOrCreateNodeText(expression).getElements().stream()
                .anyMatch(elem -> elem.expand().equals(Keyword.PRIVATE.asString())));
        assertTrue(LexicalPreservingPrinter.print(expression).equals("private int [ ] i"));
    }
    
    [TestMethod]
    void testArrayTypeWithMultipleWhitespaces() {
        string def = "int   [   ]   i";
        considerVariableDeclaration(def);
        expression.asVariableDeclarationExpr().getModifiers().addFirst(Modifier.privateModifier());
        assertTrue(LexicalPreservingPrinter.getOrCreateNodeText(expression).getElements().stream()
                .anyMatch(elem -> elem.expand().equals(Keyword.PRIVATE.asString())));
        assertTrue(LexicalPreservingPrinter.print(expression).equals("private int   [   ]   i"));
    }
    
// TODO This syntax {@code int i[]} does not work!
    
}
