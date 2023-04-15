public class InnerClassDotExpressions {
    public static void main(String[] args) {
        InnerClassContainer.InnerClass.methodCall();
        InnerClassContainer.InnerClass.InnerInnerClass.innerMethodCall();
        InnerClassContainer.InnerClass.InnerInnerClass.InnerInnerInnerClass.innerInnerMethodCall();
    }
}

class InnerClassContainer {
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
