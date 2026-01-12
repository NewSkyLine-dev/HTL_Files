import java.util.ArrayList;

public class Main {
    public static void main(String[] args) {
        Permutationsgenerator pg = new Permutationsgenerator(4);

        for (ArrayList<Integer> p : pg) {
            System.out.println(p);
        }
    }
}