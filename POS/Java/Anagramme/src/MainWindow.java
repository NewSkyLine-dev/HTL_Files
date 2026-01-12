import javax.swing.*;
import java.io.IOException;
import java.util.*;

public class MainWindow extends JFrame {
    private JPanel main;
    private JTextField anagramInput;
    private JButton evalButton;
    private JList permutationList;
    private JButton nextButton;
    private final DefaultListModel<String> listModel = new DefaultListModel<>();
    private final Parser parser = new Parser();
    private int currentIndex = 0;
    private final Map<String, Integer> words = new HashMap<>();

    public MainWindow() throws IOException {
        setTitle("Anagramme");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setContentPane(main);

        evalButton.addActionListener(e -> {
            String evalText = anagramInput.getText();
            if (!evalText.isEmpty()) {
                generatePermutations("", evalText);
                updateList();
            }
        });

        nextButton.addActionListener(e -> updateList());
        pack();
    }

    private void generatePermutations(String prefix, String remaining) {
        if (remaining.isEmpty()) {
            Map<String, Integer> charPairs = parser.getCharPairs();
            boolean isForbidden = charPairs.keySet().stream()
                    .filter(key -> charPairs.get(key) == 0)
                    .anyMatch(pair -> prefix.toLowerCase().contains(pair.toLowerCase()));

            if (!isForbidden) {
                charPairs.forEach((key, value) -> {
                    if (prefix.toLowerCase().contains(key.toLowerCase()) && value != 0) {
                        words.merge(prefix, value, Integer::sum);
                    }
                });
            }
        } else {
            for (int i = 0; i < remaining.length(); i++) {
                generatePermutations(prefix + remaining.charAt(i), remaining.substring(0, i) + remaining.substring(i + 1));
            }
        }
    }


    private void updateList() {
        if (currentIndex >= words.size() - 1) {
            currentIndex = 0;
        }

        int endIndex = Math.min(currentIndex + 20, words.size());

        listModel.clear();

        words.entrySet().stream()
                .sorted(Map.Entry.comparingByValue(Comparator.reverseOrder()))
                .forEach(entry -> listModel.addElement(entry.getKey()));

        currentIndex = endIndex;
        permutationList.setModel(listModel);
    }
}
