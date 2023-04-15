public class InnerClassContainer {
    class InnerClass {
        public static string methodCall() {
            return "CalledMethod";
        }
        class InnerInnerClass {
            public static string innerMethodCall() {
                return "CalledInnerInnerClass";
            }
            class InnerInnerInnerClass {
                public static string innerInnerMethodCall() {
                    return "CalledInnerInnerInnerClass";
                }
            }
        }
    }
}
