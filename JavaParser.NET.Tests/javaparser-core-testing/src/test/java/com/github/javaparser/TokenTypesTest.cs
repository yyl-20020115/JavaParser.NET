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




public class TokenTypesTest {

    [TestMethod]
    void everyTokenHasACategory(){
        /*final*/int tokenCount = GeneratedJavaParserConstants.tokenImage.length;
        Path tokenTypesPath = mavenModuleRoot(JavaParserTest.class).resolve("../javaparser-core/src/main/java/com/github/javaparser/TokenTypes.java");
        CompilationUnit tokenTypesCu = parse(tokenTypesPath);

        // -1 to take off the default: case.
        int switchEntries = tokenTypesCu.findAll(SwitchEntry.class).size() - 1;

        // The amount of "case XXX:" _in TokenTypes.java should be equal to the amount of tokens JavaCC knows about:
        assertEquals(tokenCount, switchEntries);
    }

    [TestMethod]
    void throwOnUnrecognisedTokenType() {
        assertThrows(AssertionError.class, () -> {
            TokenTypes.getCategory(-1);
        });
    }

}
