import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.util.*;
import java.util.stream.IntStream;

public class MainWindow extends JFrame {
    private JPanel main;
    private JComboBox charOne;
    private JComboBox charTwo;
    private JComboBox charThree;
    private JComboBox charFour;
    private JTable rightWrongTable;
    private JButton evalButton;
    private final DefaultTableModel rightWrongTableModel = (DefaultTableModel) rightWrongTable.getModel();
    private int numberOfChars = 4;
    private String rangeOfChars = "A-H";
    private int currentRow = 0;

    public MainWindow() {
        /* Construct Variables if needed */
        JMenuBar mb = new JMenuBar();
        JMenuItem newGameItem = new JMenuItem("Neues Spiel");
        JMenu optionsMenuItem = new JMenu("Optionen");
        JMenu charMenuItem = new JMenu("Buchstaben");
        JMenu charCountMenuItem = new JMenu("Anzahl Buchstaben");
        JRadioButtonMenuItem aToHRadioButton = new JRadioButtonMenuItem("A-H");
        JRadioButtonMenuItem hToZRadioButton = new JRadioButtonMenuItem("H-Z");
        JRadioButtonMenuItem fourCharsRadioButton = new JRadioButtonMenuItem("4");
        JRadioButtonMenuItem threeCharsRadioButton = new JRadioButtonMenuItem("3");
        ButtonGroup charBg = new ButtonGroup();
        ButtonGroup charCountBg = new ButtonGroup();

        /* Swing UI initialization */
        setTitle("Memory Game");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(500, 500);
        setContentPane(main);

        /* Logic goes here */
        setJMenuBar(mb);
        mb.add(optionsMenuItem);
        optionsMenuItem.add(charMenuItem);
        optionsMenuItem.add(newGameItem);
        optionsMenuItem.add(charCountMenuItem);

        aToHRadioButton.setSelected(true);
        fourCharsRadioButton.setSelected(true);

        charBg.add(aToHRadioButton);
        charBg.add(hToZRadioButton);

        charCountBg.add(fourCharsRadioButton);
        charCountBg.add(threeCharsRadioButton);

        charMenuItem.add(aToHRadioButton);
        charMenuItem.add(hToZRadioButton);

        rightWrongTable.setTableHeader(null);
        rightWrongTableModel.setColumnCount(4);

        charCountMenuItem.add(fourCharsRadioButton);
        charCountMenuItem.add(threeCharsRadioButton);

        updateComboBox();

        rightWrongTable.getColumnModel().getColumns().asIterator().forEachRemaining(column -> {
            column.setPreferredWidth(50);
            column.setCellRenderer(new CustomCellRenderer());
        });

        fourCharsRadioButton.addChangeListener(e -> {
            if (fourCharsRadioButton.isSelected()) {
                numberOfChars = 4;
            } else {
                numberOfChars = 3;
                charFour.setVisible(false);
            }
        });

        threeCharsRadioButton.addChangeListener(e -> {
            if (threeCharsRadioButton.isSelected()) {
                numberOfChars = 3;
                charFour.setVisible(false);
            } else {
                numberOfChars = 4;
            }
        });

        aToHRadioButton.addChangeListener(e -> {
            if (aToHRadioButton.isSelected()) {
                rangeOfChars = "A-H";
            } else {
                rangeOfChars = "H-Z";
            }
            updateComboBox();
        });

        hToZRadioButton.addChangeListener(e -> {
            if (hToZRadioButton.isSelected()) {
                rangeOfChars = "H-Z";
            } else {
                rangeOfChars = "A-H";
            }
            updateComboBox();
        });

        newGameItem.addActionListener(e -> startGame());
    }

    private void updateComboBox() {
        charOne.removeAllItems();
        charTwo.removeAllItems();
        charThree.removeAllItems();
        charFour.removeAllItems();

        if (rangeOfChars.equals("A-H")) {
            IntStream.range(65, 73).forEach(i -> {
                charOne.addItem((char) i);
                charTwo.addItem((char) i);
                charThree.addItem((char) i);
                charFour.addItem((char) i);
            });
        } else {
            IntStream.range(72, 91).forEach(i -> {
                charOne.addItem((char) i);
                charTwo.addItem((char) i);
                charThree.addItem((char) i);
                charFour.addItem((char) i);
            });
        }
    }

    private void startGame() {
        StringBuilder secretWord = new StringBuilder();
        if (rangeOfChars.equals("A-H")) {
            for (int i = 0; i < numberOfChars; i++) {
                secretWord.append((char) (Math.random() * 8 + 65));
            }
        } else {
            for (int i = 0; i < numberOfChars; i++) {
                secretWord.append((char) (Math.random() * 8 + 72));
            }
        }
        System.out.println(secretWord);
        
        evalButton.addActionListener(e -> gameLoop(secretWord.toString()));
    }

    private void gameLoop(String secretWord) {
        List<Character> guessedWord = new ArrayList<>();

        guessedWord.add((Character) charOne.getSelectedItem());
        guessedWord.add((Character) charTwo.getSelectedItem());
        guessedWord.add((Character) charThree.getSelectedItem());

        if (numberOfChars == 4) {
            guessedWord.add((Character) charFour.getSelectedItem());
        }
        rightWrongTableModel.addRow(new Object[]{});

        if (guessedWord.equals(secretWord.chars().mapToObj(e -> (char) e).toList())) {
            JOptionPane.showMessageDialog(this, "You won!");
            return;
        }

        IntStream.range(0, numberOfChars).forEach(col -> {
            if (secretWord.charAt(col) == guessedWord.get(col)) {
                rightWrongTableModel.setValueAt("B", currentRow, col); // Black: Position (OK), Letter (OK)
            } else if (secretWord.contains(guessedWord.get(col).toString())) {
                rightWrongTableModel.setValueAt("W", currentRow, col); // White: Position (Wrong), Letter (OK)
            } else {
                rightWrongTableModel.setValueAt("", currentRow, col); // Neither position nor letter is correct
            }
        });
        currentRow++;
    }
}
