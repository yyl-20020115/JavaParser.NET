/*
 * Copyright (C) 2007-2010 Júlio Vilmar Gesser.
 * Copyright (C) 2011, 2013-2016 The JavaParser Team.
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
 * Any kind of utility.
 *
 * @author Federico Tomassetti
 */
public class Utils {
	public static /*final*/string EOL = System.getProperty("line.separator");
	public static <T> List<T> ensureNotNull(List<T> list) {
		return list == null ? new ArrayList<T>() : list;
	}

	public static <E> boolean isNullOrEmpty(Collection<E> collection) {
		return collection == null || collection.isEmpty();
	}

	public static <T> T assertNotNull(T o) {
		if (o == null) {
			throw new NullPointerException("Assertion failed.");
		}
		return o;
	}

	/**
	 * @return string with ASCII characters 10 and 13 replaced by the text "\n" and "\r".
	 */
	public static string escapeEndOfLines(string string) {
		StringBuilder escapedString = new StringBuilder();
		for (char c : string.toCharArray()) {
			switch (c) {
				case '\n':
					escapedString.append("\\n");
					break;
				case '\r':
					escapedString.append("\\r");
					break;
				default:
					escapedString.append(c);
			}
		}
		return escapedString.toString();
	}

	public static string readerToString(Reader reader){
		/*final*/StringBuilder result = new StringBuilder();
		/*final*/char[] buffer = new char[8 * 1024];
		int numChars;

		while ((numChars = reader.read(buffer, 0, buffer.length)) > 0) {
			result.append(buffer, 0, numChars);
		}

		return result.toString();
	}

	public static string providerToString(Provider provider){
		/*final*/StringBuilder result = new StringBuilder();
		/*final*/char[] buffer = new char[8 * 1024];
		int numChars;

		while ((numChars = provider.read(buffer, 0, buffer.length)) != -1) {
			result.append(buffer, 0, numChars);
		}

		return result.toString();
	}

	/**
	 * Puts varargs _in a mutable list.
     * This does not have the disadvantage of Arrays#asList that it has a static size. 
	 */
	public static <T> List<T> arrayToList(T[] array){
		List<T> list = new LinkedList<>();
		Collections.addAll(list, array);
		return list;
	}

}
