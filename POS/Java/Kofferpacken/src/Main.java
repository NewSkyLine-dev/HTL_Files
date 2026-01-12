import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        Koffer koffer = new Koffer();
        int fehler = 0;
        int eingepackteStuecke = 0;

        System.out.println("Willkommen beim Kofferpacken-Spiel!");
        System.out.print("Gib die maximale Fehleranzahl ein: ");

        int maxFehler = scanner.nextInt();

        while (fehler < maxFehler) {
            for (int i = 0; i < 20; i++) {
                System.out.println();
            }

            System.out.println("Eine neue Runde beginnt.");
            System.out.println("Noch erlaubte Fehler: " + (maxFehler - fehler));

            if (koffer.getErstes() == null) {
                System.out.println("Der Koffer ist noch leer.");
            } else {
                Packstueck aktuelles = koffer.getErstes();
                while (aktuelles != null) {
                    boolean allCorrect = true;
                    System.out.print("Gib das Packstück '" + aktuelles.getName() + "' ein: ");
                    String input = scanner.next();
                    while (allCorrect) {
                        if (!input.equalsIgnoreCase(aktuelles.getName())) {
                            System.out.println("Falsche Eingabe! Ein Fehler wurde gemacht.");
                            allCorrect = false;
                            fehler++;
                        }
                        else {
                            break;
                        }
                    }
                    aktuelles = aktuelles.getNext();
                }

                if (fehler >= maxFehler) {
                    System.out.println("Zu viele Fehler!");
                    break;
                }

            }
            if (packNext(koffer, scanner)) {
                eingepackteStuecke++;
            }
        }

        System.out.println("Aktueller Inhalt des Koffers:");
        Koffer.printKoffer(koffer);

        System.out.println("Anzahl der erfolgreich eingepackten Packstücke: " + eingepackteStuecke);
    }

    private static boolean packNext(Koffer koffer, Scanner scanner) {
        System.out.print("Geben Sie ein neues Packstück ein (oder 'q' zum Beenden): ");
        String input = scanner.next();
        if (input.equalsIgnoreCase("q")) {
            return false;
        }
        Packstueck neuesPackstueck = new Packstueck(input);
        koffer.packe(neuesPackstueck);
        return true;
    }
}