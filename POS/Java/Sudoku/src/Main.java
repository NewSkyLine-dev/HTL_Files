import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.Unmarshaller;

import javax.swing.*;
import java.awt.*;
import java.io.File;
import java.sql.SQLException;

public class Main extends JFrame {
    private final SudokuController controller;

    public Main() {
        SudokuModel model = new SudokuModel();
        SudokuView view = new SudokuView();

        setTitle("Sudoku");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(new BorderLayout());

        controller = new SudokuController(model, view);

        model.attach(view);
        add(view, BorderLayout.CENTER);

        JMenuBar menuBar = new JMenuBar();
        JMenu gameMenu = new JMenu("Game");

        JMenuItem newGameItem = new JMenuItem("Neu");
        newGameItem.addActionListener(e -> startNewGame());
        gameMenu.add(newGameItem);

        JMenuItem importGameItem = new JMenuItem("Importieren");
        importGameItem.addActionListener(e -> importGame());
        gameMenu.add(importGameItem);

        JMenuItem hintGameItem = new JMenuItem("Hinweis");
        hintGameItem.addActionListener(e -> hint());
        gameMenu.add(hintGameItem);

        JMenuItem loadFromDBGameItem = new JMenuItem("Laden aus DB");
        loadFromDBGameItem.addActionListener(e -> {
            try {
                loadFromDB();
            } catch (SQLException ex) {
                throw new RuntimeException(ex);
            }
        });
        gameMenu.add(loadFromDBGameItem);

        JMenuItem saveGameItem = new JMenuItem("Speichern");
        saveGameItem.addActionListener(e -> {
            try {
                saveToDB();
            } catch (SQLException ex) {
                JOptionPane.showMessageDialog(this, "Failed to save to database", "Error", JOptionPane.ERROR_MESSAGE);
            }
        });
        gameMenu.add(saveGameItem);

        menuBar.add(gameMenu);
        setJMenuBar(menuBar);

        setSize(600, 600);
        setLocationRelativeTo(null);
    }

    private void saveToDB() throws SQLException {
        controller.saveToDB();
    }

    private void loadFromDB() throws SQLException {
        controller.loadFromDB();
    }

    private void hint() {
        controller.hint();
    }

    private void startNewGame() {
        controller.startGame();
    }

    private void importGame() {
        JFileChooser fileChooser = new JFileChooser();
        int returnValue = fileChooser.showOpenDialog(this);
        if (returnValue == JFileChooser.APPROVE_OPTION) {
            File selectedFile = fileChooser.getSelectedFile();
            loadSudokuFromFile(selectedFile);
        }
    }

    private void loadSudokuFromFile(File file) {
        try {
            JAXBContext context = JAXBContext.newInstance(Sudoku.class);
            Unmarshaller unmarshaller = context.createUnmarshaller();
            Sudoku sudoku = (Sudoku) unmarshaller.unmarshal(file);
            controller.loadSudoku(sudoku);
        } catch (Exception e) {
            JOptionPane.showMessageDialog(this, "Failed to load Sudoku from file.", "Error", JOptionPane.ERROR_MESSAGE);
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            Main main = new Main();
            main.setVisible(true);
        });
    }
}
