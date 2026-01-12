import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Iterator;

public class PermutationIterator<T> implements Iterator<ArrayList<T>> {
    private final ArrayList<T> elements;
    private final int[] indices;

    public PermutationIterator(ArrayList<T> elements) {
        this.elements = elements;
        this.indices = new int[elements.size()];
        Arrays.fill(indices, 0);
    }

    private void reverse(int start) {
        int i = start;
        int j = elements.size() - 1;
        while (i < j) {
            Collections.swap(elements, i, j);
            i++;
            j--;
        }
    }

    @Override
    public boolean hasNext() {
        return indices[0] < elements.size();
    }

    @Override
    public ArrayList<T> next() {
        ArrayList<T> permutation = new ArrayList<>(elements);

        int i = elements.size() - 1;
        while (i > 0 && indices[i] >= i) {
            indices[i] = 0;
            i--;
        }

        if (i > 0) {
            int j = indices[i]++;
            Collections.swap(elements, i, j);
            reverse(i + 1);
        } else {
            indices[0]++;
        }

        return permutation;
    }

    @Override
    public void remove() {
        throw new UnsupportedOperationException("Remove operation is not supported");
    }
}
