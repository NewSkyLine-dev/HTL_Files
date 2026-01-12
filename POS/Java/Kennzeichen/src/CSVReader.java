import java.io.*;
import java.util.HashMap;

public class CSVReader {
    HashMap<String, String> fileContent = new HashMap<>();

    public CSVReader(InputStream filePath) {
        try {
            readIntoHashMap(filePath);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            System.exit(1);
        }
    }

    public void readIntoHashMap(InputStream filePath) {
        try (BufferedReader br = new BufferedReader(new InputStreamReader(filePath))) {
            String line;
            while ((line = br.readLine()) != null) {
                String[] splitLine = line.split(";");
                fileContent.put(splitLine[0], splitLine[1]);
            }
        } catch (IOException e) {
            System.out.println("Error: " + e.getMessage());
            System.exit(1);
        }
    }

    public HashMap<String, String> getContentHashMap() {
        return fileContent;
    }
}
