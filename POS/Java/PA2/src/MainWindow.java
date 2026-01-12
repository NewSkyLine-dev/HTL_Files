import javax.swing.*;
import java.awt.*;
import java.io.*;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Scanner;

public class MainWindow extends JFrame {
    private JPanel main;
    private JTextField aufgabe3TextField;
    private JButton aufgabe3Button;
    private JPanel aufgabe3Panel;
    private JLabel aufgabe4Label;
    private JTextField aufgabe5Eingabe;
    private JButton aufgabe5Button;
    private JLabel aufgabe5Label;
    private JLabel aufgabe7Label;
    private JButton aufgabe7Button;
    HashMap<String, String> storage = new HashMap<>();
    Timer timer = new Timer(1000, e -> aufgabe4Label.setText(getCurrentTimeStamp()));
    ArrayList<String> aufgabe3Text = new ArrayList<>(); //!

    public MainWindow() throws HeadlessException {
        setTitle("2. praktische Arbeit");
        setSize(800, 500);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(main);
        loadWords();
        startTimer();
        aufgabe4Label.setText(getCurrentTimeStamp());
        aufgabe3Button.getAccessibleContext().setAccessibleName(aufgabe3Button.getText());
        aufgabe5Button.getAccessibleContext().setAccessibleName(aufgabe5Button.getText());
        aufgabe7Button.getAccessibleContext().setAccessibleName(aufgabe7Button.getText());
        aufgabe3TextField.getAccessibleContext().setAccessibleName("Textbaustein eingeben");
        aufgabe3Panel.getAccessibleContext().setAccessibleName("Panel für Textbausteine");
        aufgabe5Eingabe.getAccessibleContext().setAccessibleName("Zu übersetzender Text");
        aufgabe5Label.getAccessibleContext().setAccessibleName("Übersetzter Text");
        aufgabe7Label.getAccessibleContext().setAccessibleName("Label für Textbausteinkombinationen");

        aufgabe3Panel.setLayout(new FlowLayout());

        //Aufgabe 3, 5, 6, 7
        aufgabe3Button.addActionListener(e -> {
            String text = aufgabe3TextField.getText();
            if (!text.isEmpty()) {
                aufgabe3Panel.add(new JLabel(text));
                aufgabe3Panel.revalidate();
                aufgabe3Panel.repaint();
                aufgabe3Text.add(text);
                pack(); //!
            }
        });

        aufgabe5Button.addActionListener(e -> {
            String text = aufgabe5Eingabe.getText();
            if (!text.isEmpty()) {
                String[] splitText = text.split(" ");
                StringBuilder sb = new StringBuilder();
                for (String s : splitText) {
                    if (storage.containsKey(s)) {
                        sb.append(storage.get(s)).append(" ");
                    } else {
                        // Aufgabe 6
                        String notKnownWord = JOptionPane.showInputDialog("Das Wort " + s + " ist nicht bekannt. Bitte geben Sie eine Übersetzung ein.");
                        storage.put(s, notKnownWord);
                        sb.append(storage.get(s)).append(" ");
                        // Aufgabe 5
                        // sb.append(s).append(" ");
                    }
                }
                aufgabe5Label.setText(sb.toString());
                pack(); //!
            }
        });

        aufgabe7Button.addActionListener(e -> {
            Permutationsgenerator pg = new Permutationsgenerator(aufgabe3Text.size());
            StringBuilder sb = new StringBuilder();
            sb.append("<html>");
            for (Integer[] i : pg) {
                for (int j : i) {
                    sb.append(aufgabe3Text.get(j)).append(" ");
                }
                sb.append("<br/>");
            }
            sb.append("</html>");
            aufgabe7Label.setText(sb.toString());
            pack();
        });
    }

    private void loadWords()
    {
        try {
            File myFile = new File("Words.txt");
            Scanner myReader = new Scanner(myFile);
            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();
                String[] splitText = data.split(";");
                storage.put(splitText[0], splitText[1]);
            }
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }

    private void startTimer()
    {
        timer.start();
    }

    public static String getCurrentTimeStamp() {
        SimpleDateFormat sdfDate = new SimpleDateFormat("HH:mm:ss");
        return sdfDate.format(new Date());
    }

    public static void main(String[] args) {
        MainWindow mw = new MainWindow();
        mw.setVisible(true);
    }
}
