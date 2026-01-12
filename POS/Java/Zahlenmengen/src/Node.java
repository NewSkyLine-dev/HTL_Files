public class Node {
    Node left;
    Node right;
    int data;

    public Node(int data) {
        this.data = data;
    }

    void set(int val) {
        if (val < data) {
            if (left == null) {
                left = new Node(val);
            } else {
                left.set(val);
            }
        } else if (val > data) {
            if (right == null) {
                right = new Node(val);
            } else {
                right.set(val);
            }
        }
    }

    boolean get(int val) {
        if (val == this.data) {
            return true;
        } else if (val < this.data) {
            return this.left != null && this.left.get(val);
        } else {
            return this.right != null && this.right.get(val);
        }
    }

    int size() {
        int size = 1;
        if (left != null) {
            size += left.size();
        }
        if (right != null) {
            size += right.size();
        }
        return size;
    }

    Node remove(int value) {
        if (value < data) {
            if (left != null) {
                left = left.remove(value);
            }
        } else if (value > data) {
            if (right != null) {
                right = right.remove(value);
            }
        } else {
            if (left == null && right == null) {
                return null;
            } else if (left == null) {
                return right;
            } else if (right == null) {
                return left;
            }

            data = right.getMin();
            right = right.remove(data);
        }
        return this;
    }

    private int getMin() {
        return this.left == null ? this.data : this.left.getMin();
    }

    private int getMax() {
        return this.right == null ? this.data : this.right.getMax();
    }

    public void clone(Set result) {
        result.set(data);
        if (left != null) {
            left.clone(result);
        }
        if (right != null) {
            right.clone(result);
        }
    }

    void print() {
        if (left != null) {
            left.print();
        }
        System.out.print(data + " ");
        if (right != null) {
            right.print();
        }
    }

    public void intersect(Set s, Set result) {
        if (s.get(data)) {
            result.set(data);
        }
        if (left != null) {
            left.intersect(s, result);
        }
        if (right != null) {
            right.intersect(s, result);
        }
    }

    public void union(Set s, Set result) {
        result.set(data);
        if (left != null) {
            left.union(s, result);
        }
        if (right != null) {
            right.union(s, result);
        }
    }

    public void diff(Set s, Set result) {
        if (!s.get(data)) {
            result.set(data);
        }
        if (left != null) {
            left.diff(s, result);
        }
        if (right != null) {
            right.diff(s, result);
        }
    }

    public void range(int from, int to, Set result) {
        if (from <= data && data <= to) {
            result.set(data);
        }
        if (left != null) {
            left.range(from, to, result);
        }
        if (right != null) {
            right.range(from, to, result);
        }
    }
}
