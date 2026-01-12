import jakarta.xml.bind.*;
import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.io.*;
import java.util.*;


public class MainWindow extends JFrame {
    private JPanel main;
    private JTextArea aufgabe2textArea;
    private JPanel aufgabe3Panel;
    private JButton aufgabe2Button;
    private JTable viewTable;
    // private Map<Character, Integer> charCountMap = new HashMap<>();
    private final CharacterFrequencyList characterFrequency = new CharacterFrequencyList();
    private final Map<String, Integer> lowerCaseCharCount = new HashMap<>();
    private final Map<String, Integer> upperCaseCharCount = new HashMap<>();

    public MainWindow() throws HeadlessException {
        setTitle("3. praktische Arbeit");
        setSize(800, 600);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);

        DefaultTableModel model = (DefaultTableModel) viewTable.getModel();

        model.addColumn("Buchstabe");
        // model.addColumn("Anzahl");
        model.addColumn("Klein");
        model.addColumn("Groß");

        aufgabe2Button.addActionListener(e -> {
            String input = aufgabe2textArea.getText();
            characterFrequency.addText(input);
            countCharacters(input);
            updateTableModel(model);
        });

        JMenuBar mb = new JMenuBar();
        setJMenuBar(mb);

        JMenu menu = new JMenu("File");
        menu.setMnemonic('F');
        mb.add(menu);

        JMenuItem saveItem = new JMenuItem("Speicher");
        saveItem.setMnemonic('S');
        saveItem.setAccelerator(KeyStroke.getKeyStroke("S"));
        menu.add(saveItem);

        JMenuItem loadItem = new JMenuItem("Laden");
        loadItem.setMnemonic('L');
        loadItem.setAccelerator(KeyStroke.getKeyStroke("L"));
        menu.add(loadItem);

        saveItem.addActionListener(e -> {
            JFileChooser fileChooser = new JFileChooser();
            fileChooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
            int returnValue = fileChooser.showSaveDialog(null);

            if (returnValue == JFileChooser.APPROVE_OPTION) {
                File directory = fileChooser.getCurrentDirectory();
                File xmlFile = new File(directory, "characterFrequencyList.xml");
                try {
                    JAXBContext context = JAXBContext.newInstance(CharacterFrequencyList.class);
                    Marshaller marshaller = context.createMarshaller();
                    marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
                    marshaller.marshal(characterFrequency, xmlFile);
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        });

        loadItem.addActionListener(e -> {
            StringBuilder fileContent = new StringBuilder();
            JFileChooser fileChooser = new JFileChooser();
            int returnValue = fileChooser.showOpenDialog(null);
            if (returnValue == JFileChooser.APPROVE_OPTION) {
                File selectedFile = fileChooser.getSelectedFile();
                try {
                    BufferedReader reader = new BufferedReader(new java.io.FileReader(selectedFile));
                    String line;
                    while ((line = reader.readLine()) != null) {
                        fileContent.append(line);
                    }
                    reader.close();
                    aufgabe2textArea.setText(fileContent.toString());
                    countCharacters(fileContent.toString());
                    updateTableModel(model);
                }
                catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        });
    }

    private void countCharacters(String input) {
        input = input.replaceAll("[^a-zA-Z]", "");
        for (String character : input.split("")) {
            incrementCharacterFrequency(character.toLowerCase());
            if (Character.isLowerCase(character.charAt(0))) {
                lowerCaseCharCount.put(character, lowerCaseCharCount.getOrDefault(character, 0) + 1);
            } else {
                upperCaseCharCount.put(character, upperCaseCharCount.getOrDefault(character, 0) + 1);
            }
        }
    }

    private void incrementCharacterFrequency(String character) {
        boolean found = false;
        for (CharacterFrequency cf : characterFrequency.getCharacterFrequencyList()) {
            if (Objects.equals(cf.getCharacter(), character)) {
                cf.setFrequency(cf.getFrequency() + 1);
                found = true;
                break;
            }
        }
        if (!found) {
            characterFrequency.getCharacterFrequencyList().add(new CharacterFrequency(character, 1));
        }
    }

    private void updateTableModel(DefaultTableModel model) {
        model.setRowCount(0);
        // for (Map.Entry<Character, Integer> entry : charCountMap.entrySet()) {
        //     model.addRow(new Object[]{entry.getKey(), entry.getValue()});
        // }
        // for (CharacterFrequency cf : characterFrequencyList.getCharacterFrequencyList()) {
        //     model.addRow(new Object[]{cf.getCharacter(), cf.getFrequency()});
        // }
        String alphabet = "abcdefghijklmnopqrstuvwxyz";
        for (char character : alphabet.toCharArray()) {
            int lowerCaseCount = lowerCaseCharCount.getOrDefault(String.valueOf(character), 0);
            int upperCaseCount = upperCaseCharCount.getOrDefault(String.valueOf(character).toUpperCase(), 0);
            model.addRow(new Object[]{String.valueOf(character).toUpperCase(), lowerCaseCount, upperCaseCount});
        }
    }

    public static void main(String[] args) {
        MainWindow mw = new MainWindow();
        mw.setVisible(true);
    }
}
