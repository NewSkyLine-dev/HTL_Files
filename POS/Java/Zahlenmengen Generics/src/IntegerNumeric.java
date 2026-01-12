public class IntegerNumeric implements Numeric<Integer> {
    public Integer zero() {
        return 0;
    }

    public Integer add(Integer... vals) {
        Integer sum = 0;
        for (Integer val : vals) {
            sum += val;
        }
        return sum;
    }

    public Integer div(Integer... vals) {
        Integer sum = 1;
        for (Integer val : vals) {
            sum /= val;
        }
        return sum;
    }

    @Override
    public boolean greaterEqual(Integer val, Integer from) {
        return val >= from;
    }

    @Override
    public Integer fromInt(int x) {
        return x;
    }
}
