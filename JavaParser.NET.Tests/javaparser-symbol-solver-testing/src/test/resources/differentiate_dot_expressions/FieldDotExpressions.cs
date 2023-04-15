public class FieldDotExpressions {
    public static void main(String[] args) {
        InnerClassFieldContainer.outerField.containerField.containerField.firstContainerMethod();
        InnerClassFieldContainer.InnerClass.innerField.containerField.containerField.secondContainerMethod();
        InnerClassFieldContainer.InnerClass.InnerInnerClass.InnerInnerInnerClass.innerInnerInnerField.thirdContainerMethod();
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

class InnerClassFieldContainer {
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
