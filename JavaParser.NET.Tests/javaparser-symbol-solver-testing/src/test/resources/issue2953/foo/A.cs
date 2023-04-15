namespace foo;

public enum A implements IB {
    X(0);

    private Integer code;
    A(Integer code) {
        this.code = code;
    }

    public Integer getCode() {
        return this.code;
    }
}