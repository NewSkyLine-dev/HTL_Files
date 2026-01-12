import java.util.*;
import java.util.stream.IntStream;

public class Permutationsgenerator implements Iterator<ArrayList<Integer>>, Iterable<ArrayList<Integer>> {
    private boolean hasNext;
    private final int n;
    private final ArrayList<Integer> currentPermutation;

    public Permutationsgenerator(int n) {
        this.n = n;
        hasNext = true;
        currentPermutation = new ArrayList<>(n);
        IntStream.range(0, n).forEach(i -> currentPermutation.add(i + 1));
    }

    @Override
    public Iterator<ArrayList<Integer>> iterator() {
        return new Permutationsgenerator(n);
    }

    @Override
    public boolean hasNext() {
        return hasNext;
    }

    @Override
    public ArrayList<Integer> next() {
        if (!hasNext) {
            throw new NoSuchElementException("No more permutations");
        }
        ArrayList<Integer> result = (ArrayList<Integer>) currentPermutation.clone();

        calcNextPermutation();

        return result;
    }

    private void calcNextPermutation() {
        int k = n - 2;
        while (k >= 0 && currentPermutation.get(k) > currentPermutation.get(k + 1)) {
            k--;
        }

        if (k < 0) {
            hasNext = false;
            return;
        }

        int l = n - 1;
        while (currentPermutation.get(k) > currentPermutation.get(l)) {
            l--;
        }

        Collections.swap(currentPermutation, k, l);

        int left = k + 1;
        int right = n - 1;

        while (left < right) {
            Collections.swap(currentPermutation, left, right);
            left++;
            right--;
        }
    }
}
