public interface Numeric<T extends Number> {
    T zero();
    T add(T... vals);
    T div(T... vals);
    boolean greaterEqual(T val, T from);
    T fromInt(int x);
}
