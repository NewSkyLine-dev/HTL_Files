import java.util.List;

public class SudokuModel {
    private final int[][] grid = new int[9][9];
    private Observer observers = null;

    public void attach(Observer observer) {
        this.observers = observer;
    }

    private void notifyObservers(int row, int col, int value) {
        if (this.observers != null) observers.update(row, col, value);
    }

    public boolean isValidPlacement(int row, int col, int number) {
        return isRowValid(row, number) && isColumnValid(col, number) && isBlockValid(row, col, number);
    }

    private boolean isRowValid(int row, int number) {
        for (int col = 0; col < 9; col++) {
            if (grid[row][col] == number) {
                return false;
            }
        }
        return true;
    }

    private boolean isColumnValid(int col, int number) {
        for (int row = 0; row < 9; row++) {
            if (grid[row][col] == number) {
                return false;
            }
        }
        return true;
    }

    private boolean isBlockValid(int row, int col, int number) {
        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                if (grid[startRow + i][startCol + j] == number) {
                    return false;
                }
            }
        }
        return true;
    }

    public void setCellValue(int row, int col, int value) {
        grid[row][col] = value;
        notifyObservers(row, col, value);
    }

    public int getCellValue(int row, int col) {
        return grid[row][col];
    }

    public void loadSudoku(Sudoku sudoku) {
        List<Field> fields = sudoku.getFields();
        for (Field field : fields) {
            int number = Integer.parseInt(field.getNumber());
            int x = field.getX();
            int y = field.getY();
            grid[y][x] = number;
            notifyObservers(y, x, number);
        }
    }

    public void startGame() {
        for (int i = 0; i < 20; i++) {
            int row = (int) (Math.random() * 9);
            int col = (int) (Math.random() * 9);
            int number = (int) (Math.random() * 9) + 1;
            if (isValidPlacement(row, col, number)) {
                grid[row][col] = number;
                notifyObservers(row, col, number);
            }
        }
    }

    public void hint() {
        int row = (int) (Math.random() * 9);
        int col = (int) (Math.random() * 9);
        for (int number = 1; number <= 9; number++) {
            if (isValidPlacement(row, col, number)) {
                grid[row][col] = number;
                notifyObservers(row, col, number);
                break;
            }
        }
    }
}
