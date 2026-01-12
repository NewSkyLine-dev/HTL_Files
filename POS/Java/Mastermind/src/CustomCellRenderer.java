import javax.swing.*;
import javax.swing.table.DefaultTableCellRenderer;
import java.awt.*;

public class CustomCellRenderer extends DefaultTableCellRenderer {
    public Component getTableCellRendererComponent(JTable table, Object value, boolean isSelected, boolean hasFocus, int row, int column)
    {
        super.getTableCellRendererComponent(table, value, isSelected, hasFocus, row, column);
        // setHorizontalAlignment(SwingConstants.CENTER);

        if (value != null) {
            String cellValue = value.toString();
            switch (cellValue) {
                case "R" -> setBackground(Color.RED);
                case "B" -> setBackground(Color.BLACK);
                case "W" -> setBackground(Color.LIGHT_GRAY);
                default -> setBackground(Color.WHITE);
            }
        }
        setText("");
        return this;
    }
}
