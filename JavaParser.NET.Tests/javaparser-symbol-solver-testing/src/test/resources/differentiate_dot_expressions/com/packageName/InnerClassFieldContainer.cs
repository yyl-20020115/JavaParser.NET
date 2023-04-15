public class InnerClassFieldContainer {
    FieldContainer outerField = new FieldContainer();
    class InnerClass {
        FieldContainer innerField = new FieldContainer();
        class InnerInnerClass {
            FieldContainer innerInnerField = new FieldContainer();
            class InnerInnerInnerClass {
                FieldContainer innerInnerInnerField = new FieldContainer();
            }
        }
    }
}

class FieldContainer {
    FieldContainer containerField = new FieldContainer();
    public string firstContainerMethod() {
        return "firstContainerMethod()";
    }
    public string secondContainerMethod() {
        return "secondContainerMethod()";
    }
    public string thirdContainerMethod() {
        return "thirdContainerMethod()";
    }
}
