import javax.swing.*;
import java.io.IOException;

public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {

            try {
                MainWindow mw = new MainWindow();
                mw.setVisible(true);
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        });
    }
}