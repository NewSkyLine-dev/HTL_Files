public class Packstueck {
    private final String name;
    private Packstueck naechstes;

    public Packstueck(String name) {
        this.name = name;
        this.naechstes = null;
    }

    public String getName() {
        return name;
    }

    public Packstueck getNext() {
        return naechstes;
    }

    public void getNext(Packstueck naechstes) {
        this.naechstes = naechstes;
    }
}