import java.util.LinkedList;

public class PaList<T> {
    private LinkedList<T> list = new LinkedList<>();

    public void add(T n)
    {
        list.add(n);
    }

    public boolean contains(T n)
    {
        return list.contains(n);
    }

    public int size()
    {
        return list.size();
    }

    @Override
    public String toString() {
        return list.toString();
    }

    PaList<T> getEverySecondValue()
    {
        PaList<T> newList = new PaList<>();

        for (int i = 1; i < list.size(); i += 2)
        {
            newList.add(list.get(i));
        }
        return newList;
    }

    public static void main(String[] args) {
        PaList<Integer> intList = new PaList<>();
        intList.add(15);
        intList.add(8);
        intList.add(7);
        intList.add(0);
        intList.add(11);
        intList.add(-4);
        intList.add(70);
        intList.add(-11);
        intList.add(10);
        intList.add(11);
        System.out.println(intList);
        System.out.println(intList.getEverySecondValue()); // 8 0 -4 -11 11
    }
}
