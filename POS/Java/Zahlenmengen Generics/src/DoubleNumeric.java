public class DoubleNumeric implements Numeric<Double> {
    @Override
    public Double zero() {
        return 0.0;
    }

    @Override
    public Double add(Double... vals) {
        Double sum = 0.0;
        for (Double val : vals) {
            sum += val;
        }
        return sum;
    }

    @Override
    public Double div(Double... vals) {
        Double sum = 1.0;
        for (Double val : vals) {
            sum /= val;
        }
        return sum;
    }

    @Override
    public boolean greaterEqual(Double val, Double from) {
        return val >= from;
    }

    @Override
    public Double fromInt(int x) {
        return (double) x;
    }
}
