public class Main {
    public static void main(String[] args) {
        Zahlenmenge<Integer> s1 = new Zahlenmenge<>();

        s1.set(-9);
        s1.set(-5);
        s1.set(-4);
        s1.set(-3);
        s1.set(0);
        s1.set(2);
        s1.set(4);
        s1.set(10);

        Zahlenmenge<Integer> s2 = new Zahlenmenge<>();

        s2.set(-5);
        s2.set(-3);
        s2.set(0);
        s2.set(1);
        s2.set(2);
        s2.set(7);
        s2.set(9);

        // s1.union(s2).print();
        // s1.intersect(s2).print();
        // s1.range(0, 10).print();
        // s2.range(-10, 0).print();
        // s2.remove(0);
        // s2.print();
        // s1.diff(s2).print();
    }
}