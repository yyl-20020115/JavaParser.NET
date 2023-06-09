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

namespace com.github.javaparser.utils;




class LogTest {
    private static class TestAdapter implements Log.Adapter {
        string output = "";

        //@Override
        public void info(Supplier<String> messageSupplier) {
            output += "I" + messageSupplier.get();
        }

        //@Override
        public void trace(Supplier<String> messageSupplier) {
            output += "T" + messageSupplier.get();
        }

        //@Override
        public void error(Supplier<Throwable> throwableSupplier, Supplier<String> messageSupplier) {
            Throwable throwable = throwableSupplier.get();
            string s = messageSupplier.get();
            output += "E" + s + "M" + (throwable == null ? "null" : throwable.getMessage());
        }
    }

    private TestAdapter testAdapter = new TestAdapter();

    @BeforeEach
    void setAdapter() {
        Log.setAdapter(testAdapter);
    }

    @AfterEach
    void resetAdapter() {
        Log.setAdapter(new Log.SilentAdapter());
    }

    [TestMethod]
    void testTrace() {
        Log.trace("abc");
        Log.trace("a%sc%s", () -> "b", () -> "d");
        assertEquals("TabcTabcd", testAdapter.output);
    }

    [TestMethod]
    void testInfo() {
        Log.info("abc");
        Log.info("a%sc", () -> "b");
        assertEquals("IabcIabc", testAdapter.output);
    }

    [TestMethod]
    void testError() {
        Log.error("abc");
        Log.error("a%sc", () -> "b");
        Log.error(new Throwable("!!!"), "abc");
        Log.error(new Throwable("!!!"), "a%sc%s", () -> "b", () -> "d");
        Log.error(new Throwable("!!!"));
        assertEquals("EabcMnullEabcMnullEabcM!!!EabcdM!!!EnullM!!!", testAdapter.output);
    }
}
