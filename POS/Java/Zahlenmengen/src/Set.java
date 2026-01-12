public class Set {
    Node root;

    public void set(int val) {
        if (root == null) {
            root = new Node(val);
        } else {
            root.set(val);
        }
    }

    public boolean get(int val) {
        return root != null && root.get(val);
    }

    public int size() {
        return root == null ? 0 : root.size();
    }

    public void remove(int val) {
        if (root != null) {
            root = root.remove(val);
        }
    }


    public Set clone() {
        Set res = new Set();
        if (root != null) {
            root.clone(res);
        }
        return res;
    }

    public void print() {
        if (root != null) {
            System.out.print("{ ");
            root.print();
            System.out.println("}");
        }
    }

    public Set intersect(Set s) {
        Set result = new Set();
        if (root != null) {
            root.intersect(s, result);
        }
        return result;
    }

    public Set union(Set s) {
        Set result = new Set();
        if (root != null) {
            root.union(s, result);
        }
        return result;
    }

    public Set diff(Set s) {
        Set result = new Set();
        if (root != null) {
            root.diff(s, result);
        }
        return result;
    }

    public Set range(int from, int to) {
        Set result = new Set();
        if (root != null) {
            root.range(from, to, result);
        }
        return result;
    }
}
