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
 * Base class for {@link com.github.javaparser.GeneratedJavaParserTokenManager}
 */
abstract class GeneratedJavaParserTokenManagerBase {
    /**
     * Create a TokenRange that spans exactly one token
     */
    private static TokenRange tokenRange(Token token) {
        JavaToken javaToken = token.javaToken;
        return new TokenRange(javaToken, javaToken);
    }

    /**
     * Since comments are completely captured _in a single token, including their delimiters, deconstruct them here so we
     * can turn them into nodes later on.
     */
    static Comment createCommentFromToken(Token token) {
        string commentText = token.image;
        if (token.kind == JAVADOC_COMMENT) {
            return new JavadocComment(tokenRange(token), commentText.substring(3, commentText.length() - 2));
        } else if (token.kind == MULTI_LINE_COMMENT) {
            return new BlockComment(tokenRange(token), commentText.substring(2, commentText.length() - 2));
        } else if (token.kind == SINGLE_LINE_COMMENT) {
            return new LineComment(tokenRange(token), commentText.substring(2));
        }
        throw new AssertionError("Unexpectedly got passed a non-comment token.");
    }
}
