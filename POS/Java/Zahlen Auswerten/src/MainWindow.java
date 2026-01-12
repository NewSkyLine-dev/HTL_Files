import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import java.util.regex.*;

import static java.lang.Double.parseDouble;
import static java.lang.Integer.parseInt;

public class MainWindow extends JFrame {
    private JTable evaluationTable;
    private JList mostCommon;
    private JTextArea textArea;
    private JButton evalButton;
    private JPanel mainPanel;
    private JScrollPane scrollPane;
    private JSlider minSlider, maxSlider;
    private JLabel minLabel, maxLabel;
    private JRadioButton integerBtn, doubleBtn;
    private JCheckBox gesamtsummeCheckBox;
    private final TreeMap<String, Integer> numberCount = new TreeMap<>();
    private int minValue, maxValue;
    private boolean isInteger, sortBySum;

    public MainWindow() throws HeadlessException {
        setTitle("Text Evaluation");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setContentPane(mainPanel);
        pack();

        JMenuBar mb = new JMenuBar();
        JMenuItem newMenuItem = new JMenuItem("Neu");
        newMenuItem.addActionListener(e -> reset());

        mb.add(newMenuItem);
        setJMenuBar(mb);

        String maxLabelText = maxLabel.getText();
        String minLabelText = minLabel.getText();

        minValue = minSlider.getValue();
        maxValue = maxSlider.getValue();

        maxSlider.addChangeListener(e -> {
            maxValue = maxSlider.getValue();
            maxLabel.setText(maxLabelText + ": " + maxValue);
            pack();
        });

        minSlider.addChangeListener(e -> {
            minValue = minSlider.getValue();
            minLabel.setText(minLabelText + ": " + minValue);
            pack();
        });

        integerBtn.addActionListener(e -> {
            isInteger = true;
            doubleBtn.setSelected(false);
        });

        doubleBtn.addActionListener(e -> {
            isInteger = false;
            integerBtn.setSelected(false);
        });

        evalButton.addActionListener(new EvaluationEvent());

        gesamtsummeCheckBox.addActionListener(e -> {
            sortBySum = gesamtsummeCheckBox.isSelected();
            reevaluateMostCommon();
        });
    }

    private void reset() {
        numberCount.clear();
        reevaluateTable();
        reevaluateMostCommon();
    }

    private void createUIComponents() {
        // TODO: place custom component creation code here
        maxSlider = new JSlider();
        minSlider = new JSlider();
    }

    private class EvaluationEvent implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            String text = textArea.getText();
            ArrayList<String> numbers = extractNumbers(text, isInteger);
            updateNumberCount(numbers);
            reevaluateTable();
            reevaluateMostCommon();
        }

        private ArrayList<String> extractNumbers(String text, boolean isInteger) {
            Pattern pattern = isInteger ? Pattern.compile("([0-9]+)") : Pattern.compile("-?\\d+\\.\\d+");
            Matcher matcher = pattern.matcher(text);
            ArrayList<String> numbers = new ArrayList<>();
            while (matcher.find()) {
                String number = matcher.group();
                if (isNumberInRange(number, isInteger)) {
                    numbers.add(number);
                }
            }
            return numbers;
        }

        private boolean isNumberInRange(String number, boolean isInteger) {
            if (isInteger) {
                int intValue = parseInt(number);
                return intValue >= minValue && intValue <= maxValue;
            } else {
                double doubleValue = parseDouble(number);
                return doubleValue >= minValue && doubleValue <= maxValue;
            }
        }

        private void updateNumberCount(ArrayList<String> numbers) {
            for (String number : numbers) {
                numberCount.merge(number, 1, Integer::sum);
            }
        }
    }

    private void reevaluateMostCommon() {
        DefaultListModel<String> listModel = new DefaultListModel<>();

        if (sortBySum) {
            numberCount.entrySet().stream()
                    .sorted((o1, o2) -> {
                        if (isInteger) {
                            return (parseInt(o2.getKey()) * o2.getValue()) - (parseInt(o1.getKey()) * o1.getValue());
                        } else {
                            return (int) ((parseDouble(o2.getKey()) * o2.getValue()) - (parseDouble(o1.getKey()) * o1.getValue()));
                        }
                    })
                    .limit(10)
                    .forEach(entry -> listModel.addElement(entry.getKey() + " (" + entry.getValue() + ")"));
        } else {
            numberCount.entrySet().stream()
                    .sorted((o1, o2) -> o2.getValue() - o1.getValue())
                    .forEach(entry -> listModel.addElement(entry.getKey() + " (" + entry.getValue() + ")"));
        }
        mostCommon.setModel(listModel);
    }

    private void reevaluateTable() {
        String[] columnNames = {"Zahl", "Anzahl"};
        String[][] data = new String[numberCount.size()][2];
        int i = 0;

        for (String key : numberCount.keySet()) {
            data[i][0] = key;
            data[i][1] = numberCount.get(key).toString();
            i++;
        }

        if (isInteger) {
            Arrays.sort(data, Comparator.comparingInt(o -> parseInt(o[0])));
        } else {
            Arrays.sort(data, Comparator.comparingDouble(o -> parseDouble(o[0])));
        }

        evaluationTable.setModel(new DefaultTableModel(data, columnNames));
        scrollPane.setViewportView(evaluationTable);
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            MainWindow mw = new MainWindow();
            mw.setVisible(true);
        });
    }
}
