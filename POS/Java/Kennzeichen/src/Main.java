import java.util.HashMap;

public class Main {
    public static void main(String[] args) {
        // Make a CSVReader and get the "Kennzeichen.txt" file from the resources folder
        CSVReader csvReader = new CSVReader(Main.class.getResourceAsStream("Kennzeichen.txt"));
        HashMap<String, String> licensePlate = csvReader.getContentHashMap();
        new GUI(licensePlate);
    }
}