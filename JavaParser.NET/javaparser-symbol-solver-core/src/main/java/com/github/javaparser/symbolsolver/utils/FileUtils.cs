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

namespace com.github.javaparser.symbolsolver.utils;



public class FileUtils {
    
    /*
     * returns true if the filename exists otherwise return false
     */
    public static bool isValidPath(string filename) {
        File file = new File(filename);
        return file.exists();
    }
    
    /*
     * returns the parent path from the filename as string
     */
    public static string getParentPath(string filename) {
        Utils.assertNotNull(filename);
        int lastIndex = filename.lastIndexOf(File.separator);
        return filename.substring(0, lastIndex);
    }

}
