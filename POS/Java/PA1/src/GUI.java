import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Objects;

public class GUI extends JFrame {
    private JPanel upperPanel;
    private JPanel middlePanel;
    private JPanel lowerPanel;
    private JTextField textField1;
    private JButton einpackenButton;
    private JTextField textField2;
    private JButton datumPruefenButton;
    private JPanel mainPanel;
    private JLabel outputLabel;
    private JLabel datumOutputLabel;
    private JButton lastClickedTextButton;
    private JButton lastClickedImageButton;
    private JButton button1;
    private JButton button2;
    private JButton button3;
    private JButton button4;
    private JButton button5;
    private JButton button6;
    private JButton button7;
    private JButton button8;

    public GUI() throws HeadlessException {
        middlePanel.setLayout(new GridLayout(2, 4));

        String[] images = new String[] {"Chips", "Cola", "Donut", "Kaese", "Milch", "Pizza", "Pommes", "Schoko"};
        ArrayList<String> imagesList = new ArrayList<>(Arrays.asList(images));
        Collections.shuffle(imagesList);
        for (int i = 0; i < 8; ++i) {
            JButton button = new JButton();
            button.setText(imagesList.get(i));
            button.addActionListener(e -> {
                if (lastClickedImageButton == null) {
                    lastClickedTextButton = button;
                } else {
                    if (Objects.equals(lastClickedImageButton.getName().toLowerCase(), button.getText().toLowerCase())) {
                        button.setText("");
                        button.setEnabled(false);
                        lastClickedImageButton.setIcon(null);
                        lastClickedImageButton.setEnabled(false);
                        lastClickedImageButton = null;
                    }
                }
            });
            middlePanel.add(button);
        }

        Component[] components = lowerPanel.getComponents();
        for (Component component : components) {
            JButton button = (JButton) component;
            button.addActionListener(e -> {
                if (lastClickedTextButton == null) {
                    lastClickedImageButton = button;
                } else {
                    if (Objects.equals(lastClickedTextButton.getText().toLowerCase(), button.getName().toLowerCase())) {
                        button.setIcon(null);
                        button.setEnabled(false);
                        lastClickedTextButton.setText("");
                        lastClickedTextButton.setEnabled(false);
                        lastClickedTextButton = null;
                    }
                }
            });
        }

        einpackenButton.addActionListener(e -> {
            String input = textField1.getText();
            String alreadyPresent = outputLabel.getText();

            input += "<br/>";

            alreadyPresent = alreadyPresent.replace("<html>", "");
            alreadyPresent = alreadyPresent.replace("</html>", "");

            outputLabel.setText("<html>" + alreadyPresent + input + "</html>");
        });

        datumPruefenButton.addActionListener(e -> {
            String[] input = textField2.getText().split("\\.");
            int d = Integer.parseInt(input[0]);
            int m = Integer.parseInt(input[1]);
            int y = Integer.parseInt(input[2]);

            if (Hinweise.isValidDate(d, m, y)) {
                datumOutputLabel.setText("Datum ist gültig");
            } else {
                datumOutputLabel.setText("Datum ist ungültig");
            }
        });
    }

    public static void main(String[] args) {
        JFrame frame = new JFrame("GUI");
        frame.setContentPane(new GUI().mainPanel);
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.pack();
        frame.setVisible(true);
    }
}
