public class FloatNumeric implements Numeric<Float> {

    @Override
    public Float zero() {
        return 0.0f;
    }

    @Override
    public Float add(Float... vals) {
        Float sum = 0.0f;
        for (Float val : vals) {
            sum += val;
        }
        return sum;
    }

    @Override
    public Float div(Float... vals) {
        Float sum = 1.0f;
        for (Float val : vals) {
            sum /= val;
        }
        return sum;
    }

    @Override
    public boolean greaterEqual(Float val, Float from) {
        return val >= from;
    }

    @Override
    public Float fromInt(int x) {
        return (float) x;
    }
}
