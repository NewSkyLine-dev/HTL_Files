import javax.swing.*;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class SudokuController {
    private final SudokuModel model;
    private final SudokuView view;
    private Connection con = null;

    public SudokuController(SudokuModel model, SudokuView view) {
        this.model = model;
        this.view = view;
        attachListeners();
        try {
            connectToDB();
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(null, "Failed to connect to database", "Error", JOptionPane.ERROR_MESSAGE);
        }
    }

    private void attachListeners() {
        JTextField[][] cells = view.getCells();
        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                int finalRow = row;
                int finalCol = col;
                cells[row][col].addMouseListener(new MouseAdapter() {
                    @Override
                    public void mouseClicked(MouseEvent e) {
                        String input = JOptionPane.showInputDialog("Enter a number (1-9):");
                        if (input != null && input.matches("[1-9]")) {
                            int number = Integer.parseInt(input);
                            if (model.isValidPlacement(finalRow, finalCol, number)) {
                                model.setCellValue(finalRow, finalCol, number);
                            } else {
                                JOptionPane.showMessageDialog(null, "Invalid placement. Try again.");
                            }
                        }
                    }
                });
            }
        }
    }

    public void loadSudoku(Sudoku sudoku) {
        model.loadSudoku(sudoku);
    }

    public void startGame() {
        model.startGame();
    }

    public void hint() {
        model.hint();
    }

    private void connectToDB() throws ClassNotFoundException, SQLException {
        Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
        con = DriverManager.getConnection("jdbc:derby:sudokudb");
        // stmt = con.createStatement();
        System.out.println("Connected to database");
    }

    public void saveToDB() throws SQLException {
        String name = JOptionPane.showInputDialog("Enter name for game: ");
        PreparedStatement pstmt = con.prepareStatement("INSERT INTO GAME(Name) VALUES (?)", Statement.RETURN_GENERATED_KEYS);
        pstmt.setString(1, name);
        pstmt.executeUpdate();
        ResultSet rs = pstmt.getGeneratedKeys();
        int gameId = 0;
        if (rs.next()) {
            gameId = rs.getInt(1);
        }
        pstmt = con.prepareStatement("INSERT INTO CELL(GAMEID, ROW, COL, VALUE) VALUES (?, ?, ?, ?)");
        for (int row = 0; row < 9; row++) {
            for (int col = 0; col < 9; col++) {
                pstmt.setInt(1, gameId);
                pstmt.setInt(2, row);
                pstmt.setInt(3, col);
                pstmt.setInt(4, model.getCellValue(row, col));
                pstmt.executeUpdate();
            }
        }
    }

    public void loadFromDB() throws SQLException {
        PreparedStatement ps = con.prepareStatement("SELECT NAME FROM GAME");
        ResultSet names = ps.executeQuery();

        JComboBox<String> jcb = new JComboBox<>();
        while (names.next()) {
            jcb.addItem(names.getString("NAME"));
        }

        int selectedOption = JOptionPane.showConfirmDialog(null, jcb, "Choose an option", JOptionPane.OK_CANCEL_OPTION);
        if (selectedOption == JOptionPane.CANCEL_OPTION) {
            return;
        }

        String selectedName = (String) jcb.getSelectedItem();

        ps = con.prepareStatement("SELECT * FROM GAME JOIN CELL ON GAME.GAMEID = CELL.GAMEID WHERE GAME.NAME = ?");
        ps.setString(1, selectedName);
        ResultSet rs = ps.executeQuery();
        List<Field> cells = new ArrayList<>();
        while (rs.next()) {
            Field cell = new Field(String.valueOf(rs.getInt("VALUE")), rs.getInt("ROW"), rs.getInt("COL"));
            cells.add(cell);
        }
        model.loadSudoku(new Sudoku(cells));
    }
}
