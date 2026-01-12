import java.util.TreeSet;

public class Zahlenmenge<T extends Number> {
    private TreeSet<T> zahlen = new TreeSet<>();
    private Numeric<T> numeric;

    private void defineNumeric() {
        // Check if the first number in the set is an integer
        if (zahlen.first() instanceof Integer) {
            numeric = (Numeric<T>) new IntegerNumeric();
        } else if (zahlen.first() instanceof Double) {
            numeric = (Numeric<T>) new DoubleNumeric();
        } else if (zahlen.first() instanceof Float) {
            numeric = (Numeric<T>) new FloatNumeric();
        } else {
            throw new IllegalArgumentException("Unsupported type");
        }
    }

    public void set(T val) {
        zahlen.add(val);
    }

    public boolean get(T val) {
        return zahlen.contains(val);
    }

    public int size() {
        return zahlen.size();
    }

    public void remove(T val) {
        zahlen.remove(val);
    }

    public Zahlenmenge<T> clone() {
        Zahlenmenge<T> clone = new Zahlenmenge<T>();
        clone.zahlen = (TreeSet<T>) zahlen.clone();
        return clone;
    }

    public void print() {
        System.out.println(zahlen);
    }

    public Zahlenmenge<T> intersect(Zahlenmenge<T> s) {
        Zahlenmenge<T> intersect = new Zahlenmenge<T>();
        for (T val : zahlen) {
            if (s.get(val)) {
                intersect.set(val);
            }
        }
        return intersect;
    }

    public Zahlenmenge<T> union(Zahlenmenge<T> s) {
        Zahlenmenge<T> union = (Zahlenmenge<T>) clone();
        union.zahlen.addAll(s.zahlen);

        return union;
    }

    public Zahlenmenge<T> diff(Zahlenmenge<T> s) {
        Zahlenmenge<T> difference = (Zahlenmenge<T>) clone();
        difference.zahlen.removeAll(s.zahlen);

        return difference;
    }

    public Zahlenmenge<T> range(T from, T to) {
        Zahlenmenge<T> range = (Zahlenmenge<T>) clone();
        range.zahlen = (TreeSet<T>) zahlen.subSet(from, true, to, true);

        return range;
    }

    public T total() {
        defineNumeric();
        T total = numeric.zero();
        for (T val : zahlen) {
            total = numeric.add(total, val);
        }
        return total;
    }

    public T average() {
        defineNumeric();
        return numeric.div(total(), numeric.fromInt(zahlen.size()));
    }

    public T min() {
        return zahlen.first();
    }

    public T max() {
        return zahlen.last();
    }

    public boolean equals(Zahlenmenge<T> s) {
        return zahlen.equals(s.zahlen);
    }

    public T zufallszahl() {
        // liefert zufällig eine Zahl aus der Menge
        int index = (int) (Math.random() * zahlen.size());
        for (int i = 0; zahlen.iterator().hasNext(); i++) {
            if (i == index) {
                return zahlen.iterator().next();
            }
        }
        return null;
    }
}
