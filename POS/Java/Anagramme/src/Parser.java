import java.io.*;
import java.util.*;

public class Parser {
    Map<String, Integer> CharPairs = new HashMap<>();
    Parser() throws IOException {
        File csvFile = new File(Objects.requireNonNull(getClass().getClassLoader().getResource("Deutsch.csv")).getFile());

        BufferedReader br = new BufferedReader(new FileReader(csvFile));

        for (int i = 0; i < 26; i++) {
            String line = br.readLine();
            String[] values = line.split(";");
            for (int j = 0; j < 26; j++) {
                String[] pair = values[j].split("\\.");
                CharPairs.put((char) (i + 65) + "" + (char) (j + 65), Integer.parseInt(pair[0]) + Integer.parseInt(pair[1]));
            }
        }

        br.close();
    }

    Map<String, Integer> getCharPairs() {
        return CharPairs;
    }

    List<String> getForbiddenValues() {
        // Return the HashMap without the key-value pairs where the value is 0
        return CharPairs.entrySet().stream()
                .filter(entry -> entry.getValue() == 0)
                .map(Map.Entry::getKey)
                .toList();
    }
}
