class Koffer {
    private Packstueck erstes;

    public Koffer() {
        this.erstes = null;
    }

    public void packe(Packstueck packstueck) {
        if (erstes == null) {
            erstes = packstueck;
        } else {
            Packstueck aktuelles = erstes;
            while (aktuelles.getNext() != null) {
                aktuelles = aktuelles.getNext();
            }
            aktuelles.getNext(packstueck);
        }
    }

    public Packstueck getErstes() {
        return erstes;
    }

    public static void printKoffer(Koffer koffer) {
        Packstueck aktuelles = koffer.getErstes();
        while (aktuelles != null) {
            System.out.println("-" + aktuelles.getName());
            aktuelles = aktuelles.getNext();
        }
    }
}