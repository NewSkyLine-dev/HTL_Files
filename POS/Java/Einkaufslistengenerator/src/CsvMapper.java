import java.io.*;
import java.util.*;

public class CsvMapper {
    private final File csvFile;
    private final HashMap<String, LinkedList<String>> mappedFile = new HashMap<>();

    public HashMap<String, LinkedList<String>> getMappedFile() {
        return mappedFile;
    }

    public CsvMapper(File file) {
        this.csvFile = file;
    }

    public void loadCsv() {
        try (BufferedReader br = new BufferedReader(new FileReader(csvFile))) {
            while (br.readLine() != null) {
                String[] line = br.readLine().split(";");
                String key = line[0].trim();
                String value = line[1];
                if (mappedFile.containsKey(key)) {
                    mappedFile.get(key).add(value);
                } else {
                    LinkedList<String> list = new LinkedList<>();
                    list.add(value);
                    mappedFile.put(key, list);
                }
            }
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}
