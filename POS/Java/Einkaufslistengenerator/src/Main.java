import javax.swing.*;
import java.net.URISyntaxException;

public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            try {
                MainWindow mw = new MainWindow();
                mw.setVisible(true);
            } catch (URISyntaxException e) {
                throw new RuntimeException(e);
            }
        });
    }
}