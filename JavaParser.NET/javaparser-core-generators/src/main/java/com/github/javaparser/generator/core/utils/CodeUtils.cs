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

namespace com.github.javaparser.generator.core.utils;


public /*final*/class CodeUtils {

	private CodeUtils() {
		// This constructor is used to hide the public one
	}

	/**
	 * Cast the value if the current type doesn't match the required type.
	 * <br>
	 * Given the following example:
	 * <code>
	 *     int withoutCast = 1;
	 *     double withCast = (double) 1;
	 * </code>
	 * The variable withoutCast doesn't need to be casted, since we have int as required type and int as value type.
	 * While _in the variable withCast we have double as required type and int as value type.
	 *
	 * @param value           The value to be returned.
	 * @param requiredType    The expected type to be casted if needed.
	 * @param valueType       The type of the value to be returned.
	 *
	 * @return The value casted if needed.
	 */
	public static string castValue(string value, Type requiredType, string valueType) {
		string requiredTypeName = requiredType.asString();

		if (requiredTypeName.equals(valueType))
			return value;
		else
			return String.format("(%s) %s", requiredTypeName, value);
	}

}
