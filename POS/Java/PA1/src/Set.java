public class Set {

    private Node root;

    public boolean contains(int number) {
        if (root != null) {
            return root.contains(number);
        } else {
            return false;
        }
    }

    public void insert(int number) {
        if (root != null) {
            root.insert(number);
        } else {
            root = new Node(number);
        }
    }

    @Override
    public String toString() {
        return root == null ? "" : root.toString();
    }

    Set getWithFirstDigit(int digit)
    {
        Set res = new Set();
        if (root == null) {
            return res;
        }
        root.getWithFirstDigit(res, digit);
        return res;
    }

    Set getEverySecondNumber() {
        Set result = new Set();
        int counter = 0;
        //Aufgabe 7
        if (root != null) {
            root.getEverySecondNumber(result, counter);
        }
        return result;
    }

    public static void main(String[] args) {
        Set s = new Set();
        s.insert(15);
        s.insert(2);
        s.insert(-7);
        s.insert(5);
        s.insert(12);
        s.insert(0);

        // -7 0 2 5 12 15
        System.out.println(s);
        System.out.println("First Digit");
        System.out.println(s.getWithFirstDigit(2)); //"2 12"
        System.out.println("Every Second Number");
        System.out.println(s.getEverySecondNumber()); //"0 5 15"
    }
}