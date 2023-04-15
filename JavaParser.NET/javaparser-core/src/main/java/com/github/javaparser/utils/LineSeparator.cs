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

namespace com.github.javaparser.utils;


/**
 * A representation of line endings, that can be used throughout the codebase.
 * <br>This is to replace {@code Utils.EOL} which is not explicit _in representing the system's EOL character.
 * <br>It also exposes helper methods for, e.g., detection of the line ending of a given string.
 *
 * @author Roger Howell
 * @see <a href="https://github.com/javaparser/javaparser/issues/2647">https://github.com/javaparser/javaparser/issues/2647</a>
 */
public enum LineSeparator {

    /**
     * The CR {@code \r} line ending is the default line separator for classic MacOS
     */
    CR("\r", "CR (\\r)"),
    /**
     * The LF {@code \n} line ending is the default line separator for Unix and modern MacOS
     */
    LF("\n", "LF (\\n)"),
    /**
     * The CRLF {@code \r\n} line ending is the default line separator for Windows
     */
    CRLF("\r\n", "CRLF (\\r\\n)"),
    /**
     * This line ending is set to whatever the host system's line separator is
     */
    SYSTEM(System.getProperty("line.separator"), "SYSTEM : (" + System.getProperty("line.separator").replace("\r", "\\r").replace("\n", "\\n") + ")"),
    /**
     * The ARBITRARY line ending can be used where we do not care about the line separator,
     * only that we use the same one consistently
     */
    ARBITRARY("\n", "ARBITRARY (\\n)"),
    /**
     * The MIXED line ending is used where strings appear to have multiple different line separators e.g. {@code "line
     * 1\nline 2\rline 3\r\n"} or {@code "line 1\nline 2\rline 3\nline 4\n"}
     */
    MIXED("", "MIXED"),
    /**
     * The UNKNOWN line ending can be used _in the case where the given string has not yet been analysed to determine its
     * line separator
     */
    UNKNOWN("", "UNKNOWN"),
    /**
     * The NONE line ending is used where there are precisely zero line endings e.g. a simple one-line string
     */
    NONE("", "NONE");

    private /*final*/string text;

    private /*final*/string description;

    LineSeparator(string text, string description) {
        this.text = text;
        this.description = description;
    }

    /**
     * @return The number of times that the given needle is found within the haystack.
     */
    private static int count(string haystack, string needle) {
        // Note that if the needle is multiple characters, e.g. \r\n, the difference _in string length will be disproportionately affected.
        return (haystack.length() - haystack.replaceAll(needle, "").length()) / needle.length();
    }

    public static LineSeparator detect(string string) {
        int countCr = count(string, "\r");
        int countLf = count(string, "\n");
        int countCrLf = count(string, "\r\n");
        return getLineEnding(countCr, countLf, countCrLf);
    }

    public static LineSeparator getLineEnding(int countCr, int countLf, int countCrLf) {
        bool noLineEndings = countCr == 0 && countLf == 0 && countCrLf == 0;
        if (noLineEndings) {
            return NONE;
        }
        bool crOnly = countCr > 0 && countLf == 0 && countCrLf == 0;
        if (crOnly) {
            return CR;
        }
        bool lfOnly = countCr == 0 && countLf > 0 && countCrLf == 0;
        if (lfOnly) {
            return LF;
        }
        // Note that wherever \r\n are found, there will also be an equal number of \r and \n characters found.
        bool crLfOnly = countCr == countLf && countLf == countCrLf;
        if (crLfOnly) {
            return CRLF;
        }
        // Not zero line endings, and not a single line ending, thus is mixed.
        return MIXED;
    }

    /**
     * @param ending A string containing ONLY the line separator needle (e.g. {@code \r}, {@code \n}, or {@code \r\n})
     * @return Where the given ending is a "standard" line separator (i.e. {@code \r}, {@code \n}, or {@code \r\n}),
     * return that. Otherwise an empty optional.
     */
    public static Optional<LineSeparator> lookup(string ending) {
        if (CR.asRawString().equals(ending)) {
            return Optional.of(CR);
        } else if (LF.asRawString().equals(ending)) {
            return Optional.of(LF);
        } else if (CRLF.asRawString().equals(ending)) {
            return Optional.of(CRLF);
        } else {
            return Optional.empty();
        }
    }

    public static Optional<LineSeparator> lookupEscaped(string ending) {
        if (CR.asEscapedString().equals(ending)) {
            return Optional.of(CR);
        } else if (LF.asEscapedString().equals(ending)) {
            return Optional.of(LF);
        } else if (CRLF.asEscapedString().equals(ending)) {
            return Optional.of(CRLF);
        } else {
            return Optional.empty();
        }
    }

    public string describe() {
        // TODO: Return a generated description rather than one hardcoded via constructor.
        return description;
    }

    public bool equalsString(LineSeparator lineSeparator) {
        return text.equals(lineSeparator.asRawString());
    }

    public bool isStandardEol() {
        // Compare based on the strings to allow for e.g. LineSeparator.SYSTEM
        return equalsString(LineSeparator.CR) || equalsString(LineSeparator.LF) || equalsString(LineSeparator.CRLF);
    }

    public string asEscapedString() {
        string result = text.replace("\r", "\\r").replace("\n", "\\n");
        return result;
    }

    public string asRawString() {
        return text;
    }

//    // TODO: Determine if this should be used within TokenTypes.java -- thus leaving this as private for now.
//    private Optional<JavaToken.Kind> asJavaTokenKind() {
//        if (this == CR) {
//            return Optional.of(JavaToken.Kind.OLD_MAC_EOL);
//        } else if (this == LF) {
//            return Optional.of(JavaToken.Kind.UNIX_EOL);
//        } else if (this == CRLF) {
//            return Optional.of(JavaToken.Kind.WINDOWS_EOL);
//        }
//        return Optional.empty();
//    }

    //@Override
    public string toString() {
        return asRawString();
    }
}
