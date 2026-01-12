import java.util.Arrays;
import java.util.Iterator;

public class Permutationsgenerator implements Iterator<Integer[]>, Iterable<Integer[]> {
    private int size = 3;
    private boolean hasnext = true;
    private Integer [] permutation = null;


    public Permutationsgenerator(int size) {
        if(size < 3)
        {
            throw new IllegalArgumentException("Größe muss mindestens 3 sein!");
        }
        this.size = size;
    }

    @Override
    public boolean hasNext() {
        return hasnext;
    }

    @Override
    public Integer[] next() {
        if(hasnext == false)
            return null;
        if(permutation == null)
        {
            permutation = new Integer[size];
            for(int i = 0; i < size; i++) {
                permutation[i] = i;
            }
            return permutation.clone();
        }
        else
        {
            permutation = generateNext(permutation);
            if(generateNext(permutation) == null)
            {
                hasnext = false;
            }
            return permutation.clone();
        }
    }

    private Integer [] generateNext(Integer [] old)
    {
        int k = -1;
        int l = -1;
        for(int i = size - 2; i >= 0; i--)
        {
            if(old[i] < old[i + 1])
            {
                k = i;
                break;
            }
        }
        if(k == -1)
        {
            return null;
        }
        for(int i = size - 1; i > k; i--)
        {
            if(old[k] < old[i])
            {
                l = i;
                break;
            }
        }

        Integer [] n = old.clone();
        n[k] = old[l];
        n[l] = old[k];
        Integer [] n2 = n.clone();
        for(int i = k + 1, j = 1; i < size; i++, j++)
        {
            n[i] = n2[size - j];
        }
        return n;
    }

    @Override
    public Iterator<Integer[]> iterator() {
        return new Permutationsgenerator(size);
    }
}
