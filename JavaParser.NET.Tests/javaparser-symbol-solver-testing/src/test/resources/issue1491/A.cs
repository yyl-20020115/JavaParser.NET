public class A {
    public int FUNC() {
        Type cls = null;
        if (cls == null) return 0;
        else {
            B.FUNC3(cls);
            return 1;
        }
    }

    public void FUNC2(Type arg) {
        return;
    }
}

class B {
    public static void FUNC3(Type arg) {
        return;
    }
}
