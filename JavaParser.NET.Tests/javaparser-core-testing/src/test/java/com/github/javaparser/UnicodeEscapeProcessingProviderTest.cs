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
 * Test case for {@link UnicodeEscapeProcessingProvider}.
 */
public class UnicodeEscapeProcessingProviderTest {

	[TestMethod]
	void testUnicodeEscape(){
		assertEquals("13" + '\u12aA' + "98", new String(read("13\\u12aA98")));
	}

	[TestMethod]
	void testEscapedUnicodeEscape(){
		assertEquals("13\\\\u12aA98", new String(read("13\\\\u12aA98")));
	}
	
	[TestMethod]
	void testUnicodeEscapeWithMultipleUs(){
		assertEquals("13" + '\u12aA' + "98", new String(read("13\\uuuuuu12aA98")));
	}
	
	[TestMethod]
	void testInputEndingInBackslash(){
		assertEquals("foobar\\", new String(read("foobar\\")));
	}
	
	[TestMethod]
	void testInputEndingInBackslashU(){
		assertEquals("foobar\\u", new String(read("foobar\\u")));
	}
	
	[TestMethod]
	void testInputEndingInBackslashUs(){
		assertEquals("foobar\\uuuuuu", new String(read("foobar\\uuuuuu")));
	}
	
	[TestMethod]
	void testInputEndingInBackslashU1(){
		assertEquals("foobar\\uA", new String(read("foobar\\uA")));
	}
	
	[TestMethod]
	void testInputEndingInBackslashU2(){
		assertEquals("foobar\\uAB", new String(read("foobar\\uAB")));
	}
	
	[TestMethod]
	void testInputEndingInBackslashU3(){
		assertEquals("foobar\\uABC", new String(read("foobar\\uABC")));
	}
	
	[TestMethod]
	void testInputEndingUnicodeEscape(){
		assertEquals("foobar\uABCD", new String(read("foobar\\uABCD")));
	}
	
	[TestMethod]
	void testEmptyInput(){
		assertEquals("", new String(read("")));
	}

	[TestMethod]
	void testBadUnicodeEscape0(){
		assertEquals("13\\ux", new String(read("13\\ux")));
	}
	
	[TestMethod]
	void testBadUnicodeEscape1(){
		assertEquals("13\\u1x", new String(read("13\\u1x")));
	}

	[TestMethod]
	void testBadUnicodeEscape2(){
		assertEquals("13\\u1Ax", new String(read("13\\u1Ax")));
	}

	[TestMethod]
	void testBadUnicodeEscape3(){
		assertEquals("13\\u1ABx", new String(read("13\\u1ABx")));
	}

	[TestMethod]
	void testBadUnicodeEscapeMultipleUs(){
		assertEquals("13\\uuuuuu1ABx", new String(read("13\\uuuuuu1ABx")));
	}

	[TestMethod]
	void testPushBackWithFullBuffer(){
		assertEquals("12345678\\uuxxxxxxxxxxxxxxxxxxxxxxx", new String(read("12345678\\uuxxxxxxxxxxxxxxxxxxxxxxx")));
	}
	
	[TestMethod]
	void testPushBackWithBufferShift(){
		assertEquals("12345678\\uuxx", new String(read("12345678\\uuxx")));
	}
	
	static string read(string source){
		return process(provider(source));
	}

	static UnicodeEscapeProcessingProvider provider(string source) {
		UnicodeEscapeProcessingProvider provider = new UnicodeEscapeProcessingProvider(10, 
				new StringProvider(source));
		return provider;
	}

	static string process(UnicodeEscapeProcessingProvider provider)
			throws IOException {
		StringBuilder result = new StringBuilder();
		char[] buffer = new char[10];
		while (true) {
			int direct = provider.read(buffer, 0, buffer.length);
			if (direct < 0) {
				break;
			}
			result.append(buffer, 0, direct);
		}
		
		provider.close();
	
		return result.toString();
	}
}
