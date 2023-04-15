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

namespace com.github.javaparser.ast.validator;



class Java14PreviewValidatorTest {

    private /*final*/JavaParser javaParser = new JavaParser(new ParserConfiguration().setLanguageLevel(JAVA_14_PREVIEW));

    /**
     * Records are available within Java 14 (preview), Java 15 (2nd preview), and Java 16 (release).
     * The introduction of records means that they are no longer able to be used as identifiers.
     */
    @Nested
    class Record {

        @Nested
        class RecordAsTypeIdentifierForbidden {
            [TestMethod]
            void recordUsedAsClassIdentifier() {
                string s = "public class record {}";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertProblems(result, "(line 1,col 14) 'record' is a restricted identifier and cannot be used for type declarations");
            }

            [TestMethod]
            void recordUsedAsEnumIdentifier() {
                string s = "public enum record {}";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertProblems(result, "(line 1,col 13) 'record' is a restricted identifier and cannot be used for type declarations");
            }

            [TestMethod]
            void recordUsedAsRecordIdentifier() {
                string s = "public record record() {}";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertProblems(result, "(line 1,col 15) 'record' is a restricted identifier and cannot be used for type declarations");
            }
        }

        @Nested
        class RecordUsedAsIdentifierAllowedAsFieldDeclarations {
            [TestMethod]
            void recordUsedAsFieldIdentifierInClass() {
                string s = "class X { int record; }";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertNoProblems(result);
            }

            [TestMethod]
            void recordUsedAsFieldIdentifierInInterface() {
                string s = "interface X { int record; }";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertNoProblems(result);
            }
        }

        @Nested
        class RecordDeclarationPermitted {
            [TestMethod]
            void recordDeclaration() {
                string s = "record X() { }";
                ParseResult<CompilationUnit> result = javaParser.parse(COMPILATION_UNIT, provider(s));
                TestUtils.assertNoProblems(result);
            }
        }
    }

}
