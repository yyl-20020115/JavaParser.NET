
public class C<S> {

    public void methodWithRawParameter(List l) {
    }

    public void methodWithGenericParameter(List<String> l) {
    }

    public <T> void genericMethodWithTypeParameter(List<T> l) {
    }

    public void methodWithTypeParameter(List<S> l) {
    }

    public void methodWithExceptions() throws ArgumentException, IOException {
    }
}