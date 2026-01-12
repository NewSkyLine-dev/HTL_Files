import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.Objects;

public class Main extends JFrame {
    private final JButton[][] buttons;
    private final JComboBox<String> player1ComboBox;
    private final JComboBox<String> player2ComboBox;
    private final JLabel currentPlayerLabel;
    private final JButton startButton;
    private boolean player1Turn;
    private final ImageIcon emptyImage = new ImageIcon(Objects.requireNonNull(getClass().getResource("Empty.png")));
    private final ImageIcon crossImage = new ImageIcon(Objects.requireNonNull(getClass().getResource("Cross.png")));
    private final ImageIcon starImage = new ImageIcon(Objects.requireNonNull(getClass().getResource("Star.png")));

    public Main() {
        setTitle("Tic Tac Toe");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(300, 300);
        setLayout(new BorderLayout());

        buttons = new JButton[3][3];
        JPanel gamePanel = new JPanel(new GridLayout(3, 3));

        for (int i = 0; i < 3; i++) {
             for (int j = 0; j < 3; j++) {
                    buttons[i][j] = new JButton();
                    buttons[i][j].setIcon(emptyImage);
                    buttons[i][j].setEnabled(false);
                    buttons[i][j].addActionListener(new ButtonClickListener(i, j));
                    gamePanel.add(buttons[i][j]);
             }
        }

        add(gamePanel, BorderLayout.CENTER);

        JPanel controlPanel = new JPanel(new FlowLayout());

        player1ComboBox = new JComboBox<>(new String[]{"Spieler 1", "Computer"});
        player2ComboBox = new JComboBox<>(new String[]{"Spieler 2", "Computer"});
        currentPlayerLabel = new JLabel("Aktueller Spieler: ...");
        startButton = new JButton("Start");
        startButton.addActionListener(new StartButtonClickListener());

        // Add an ItemListener to player1ComboBox
        player1ComboBox.addItemListener(e -> player2ComboBox.setEnabled(!"Computer".equals(player1ComboBox.getSelectedItem())));

        // Add an ItemListener to player2ComboBox
        player2ComboBox.addItemListener(e -> player1ComboBox.setEnabled(!"Computer".equals(player2ComboBox.getSelectedItem())));

        controlPanel.add(player1ComboBox);
        controlPanel.add(player2ComboBox);
        controlPanel.add(currentPlayerLabel);
        controlPanel.add(startButton);

        add(controlPanel, BorderLayout.SOUTH);

        player1Turn = true;
    }

    private class ButtonClickListener implements ActionListener {
        private final int row;
        private final int col;

        public ButtonClickListener(int row, int col) {
            this.row = row;
            this.col = col;
        }

        @Override
        public void actionPerformed(ActionEvent e) {
            if (buttons[row][col].getIcon() == null) {
                if (player1Turn) {
                    buttons[row][col].setIcon(crossImage);
                } else {
                    buttons[row][col].setIcon(starImage);
                }
                player1Turn = !player1Turn;
                updateCurrentPlayerLabel();
                checkForWinner();
            }
        }
    }

    private class StartButtonClickListener implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            player1Turn = true;
            updateCurrentPlayerLabel();
            for (int i = 0; i < 3; i++) {
                 for (int j = 0; j < 3; j++) {
                        buttons[i][j].setIcon(null);
                        buttons[i][j].setEnabled(true);
                 }
            }
            startButton.setEnabled(false);
            player1Turn = ((Math.random() * 2) + 1) == 1;
            currentPlayerLabel.setText("Aktueller Spieler: " + (player1Turn ? "Spieler 1" : "Spieler 2"));
        }
    }

    private void updateCurrentPlayerLabel() {
        if (player1Turn) {
            currentPlayerLabel.setText("Aktueller Spieler: Spieler 1");
        } else {
            currentPlayerLabel.setText("Aktueller Spieler: Spieler 2");
        }
    }
    private void checkForWinner() {
        String winner = null;

        // Check rows and columns for a win
        for (int i = 0; i < 3; i++) {
            if (checkWinCondition(buttons[i][0], buttons[i][1], buttons[i][2])) {
                winner = !player1Turn ? "Spieler 1" : "Spieler 2";
                break;
            }
            if (checkWinCondition(buttons[0][i], buttons[1][i], buttons[2][i])) {
                winner = !player1Turn ? "Spieler 1" : "Spieler 2";
                break;
            }
        }

        // Check diagonals
        if (winner == null && (checkWinCondition(buttons[0][0], buttons[1][1], buttons[2][2]) ||
                checkWinCondition(buttons[0][2], buttons[1][1], buttons[2][0]))) {
            winner = !player1Turn ? "Spieler 1" : "Spieler 2";
        }

        if (winner != null) {
            JOptionPane.showMessageDialog(this, "Gewonnen: " + winner);
            resetGame();
        } else {
            // Check for a draw
            boolean isDraw = true;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (buttons[i][j].getIcon() == null) {
                        isDraw = false;
                        break;
                    }
                }
            }

            if (isDraw) {
                JOptionPane.showMessageDialog(this, "Unentschieden!");
                resetGame();
            }
        }
    }

    private boolean checkWinCondition(JButton b1, JButton b2, JButton b3) {
        return b1.getIcon() != null && b1.getIcon().equals(b2.getIcon()) && b1.getIcon().equals(b3.getIcon());
    }


    private void resetGame() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                buttons[i][j].setIcon(new ImageIcon(Objects.requireNonNull(getClass().getResource("Empty.png"))));
                buttons[i][j].setEnabled(true);
            }
        }
        currentPlayerLabel.setText("Aktueller Spieler: ...");
        startButton.setEnabled(true);
    }


    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            Main main = new Main();
            main.setMinimumSize(new Dimension(800, 800));
            main.setResizable(false);
            main.setVisible(true);
        });
    }
}