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
 * Test case for {@link PositionMapping}.
 *
 * @author <a href="mailto:bhu@top-logic.com">Bernhard Haumacher</a>
 */
@SuppressWarnings("javadoc")
public class PositionMappingTest {

	[TestMethod]
	public void testNoMapping(){
		List<List<String>> input = lines(
			line("Hello World !\n"), 
			line("Next Line\r"), 
			line("Third Line\r\n"), 
			line("Fourth Line."));
		string inputText = text(input);
		UnicodeEscapeProcessingProvider provider = provider(inputText);
		string outputText = process(provider);
		assertEquals(inputText, outputText);
		PositionMapping mapping = provider.getPositionMapping();
		assertTrue(mapping.isEmpty());
		assertEquals(4, provider.getInputCounter().getLine());
		assertEquals(4, provider.getOutputCounter().getLine());
		assertSame(PositionMapping.PositionUpdate.NONE, mapping.lookup(new Position(10000, 1)));
	}

	[TestMethod]
	public void testEncodedLineFeed(){
		List<List<String>> input = lines(
			line("B", "\\u000A", "C"));
		List<List<String>> output = lines(
			line("B", "\n"), 
			line("C"));
		
		checkConvert(input, output);
	}
	
	[TestMethod]
	public void testComplexMapping(){
		List<List<String>> input = lines(
			// Character positions:
			//                      111    1 11111    1222    2 2222     2
			//    1    2 34567    89012    3 45678    9012    3 45678    9
			line("H", "\\u00E4", "llo W", "\\u00F6", "rld!", "\\u000A", "123 N", "\\u00E4", "xt Line", "\\u000D", "Third Line", "\r\n"), 
			line("Fo", "\\u00FC", "rth Line."));
		List<List<String>> output = lines(
			line("H", "ä", "llo W", "ö", "rld!", "\n"), 
			line("123 N", "ä", "xt Line", "\r"), 
			line("Third Line", "\r\n"), 
			line("Fo", "ü", "rth Line."));

		checkConvert(input, output);
	}

	private void checkConvert(List<List<String>> input,
			List<List<String>> output){
		UnicodeEscapeProcessingProvider provider = provider(text(input));
		string decoded = process(provider);
		assertEquals(text(output), decoded);
		
		PositionMapping mapping = provider.getPositionMapping();
		
		// Coarse grained test.
		assertEquals(input.size(), provider.getInputCounter().getLine());
		assertEquals(output.size(), provider.getOutputCounter().getLine());
		
		// Fine grained test.
		int inPosLine = 1;
		int inPosColumn = 1;
		int outPosLine = 1;
		int outPosColumn = 1;
		Iterator<List<String>> outLineIt = output.iterator();
		List<String> outLine = outLineIt.next();
		Iterator<String> outPartIt = outLine.iterator();
		string outPart = outPartIt.next();
		boolean outFinished = false;
		for (List<String> inLine : input) {
			for (string inPart : inLine) {
				assertFalse(outFinished);
				
				Position inPos = new Position(inPosLine, inPosColumn);
				Position outPos = new Position(outPosLine, outPosColumn);
				Position transfomedOutPos = mapping.transform(outPos);

				assertEquals(inPos, transfomedOutPos, 
					"Position mismatch at '" + outPart + "' " + outPos + " -> '" + inPart + "' " + inPos + ".");

				outPosColumn += outPart.length();
				inPosColumn += inPart.length();
				
				if (!outPartIt.hasNext()) {
					if (outLineIt.hasNext()) {
						outPartIt = outLineIt.next().iterator();
						outPosLine ++;
						outPosColumn = 1;

						outPart = outPartIt.next();
					} else {
						outFinished = true;
					}
				} else {
					outPart = outPartIt.next();
				}
			}
			
			inPosColumn = 1;
			inPosLine++;
		}
	}
	
	private static string text(List<List<String>> input) {
		StringBuilder result = new StringBuilder();
		for (List<String> line : input) {
			for (string part : line) {
				result.append(part);
			}
		}
		return result.toString();
	}

	@SafeVarargs
	private static List<String> line(string ...parts) {
		return Arrays.asList(parts);
	}

	@SafeVarargs
	private static List<List<String>> lines(List<String> ...lines) {
		return Arrays.asList(lines);
	}
	
}
