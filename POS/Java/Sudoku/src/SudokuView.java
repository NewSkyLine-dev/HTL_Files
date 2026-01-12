import javax.swing.*;
import java.awt.*;

public class SudokuView extends JPanel implements Observer {
    private final JTextField[][] cells = new JTextField[9][9];

    public SudokuView() {
        final JPanel[][] panels = new JPanel[3][3];

        setLayout(new GridLayout(3, 3));

        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                cells[row][col] = new JTextField();
                cells[row][col].setHorizontalAlignment(JTextField.CENTER);
                cells[row][col].setFont(new Font("Arial", Font.BOLD, 20));
                cells[row][col].setBorder(BorderFactory.createLineBorder(Color.BLACK, 1));
                cells[row][col].setEditable(false);
                cells[row][col].setFocusable(false);
            }
        }

        for (int row = 0; row < 3; row++) {
            for (int col = 0; col < 3; col++) {
                panels[row][col] = new JPanel(new GridLayout(3, 3));
                panels[row][col].setBorder(BorderFactory.createLineBorder(Color.BLACK, 1));
                add(panels[row][col]);
            }
        }

        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                panels[row / 3][col / 3].add(cells[row][col]);
            }
        }
    }

    @Override
    public void update(int row, int col, int value) {
        cells[row][col].setText(value == 0 ? "" : String.valueOf(value));
    }

    public JTextField[][] getCells() {
        return cells;
    }
}
