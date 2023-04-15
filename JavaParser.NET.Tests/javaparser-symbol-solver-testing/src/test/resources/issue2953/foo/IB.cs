namespace foo;
public interface IB {
    Integer getCode();

    default bool equalByCode(Integer code) {
        return getCode().equals(code);
    }
}