namespace com.packageName;

public class InnerStaticClassFieldContainer {
    static class InnerClass {
        public static string methodCall() {
            return "CalledMethod";
        }
        static class InnerInnerClass {
            public static /*final*/string MY_INT = "1";
            public static string innerMethodCall() {
                return "CalledInnerInnerClass";
            }
        }
    }
}
