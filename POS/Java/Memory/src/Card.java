import javax.swing.*;
import java.awt.*;

public class Card extends JButton {
    private final String value;
    private boolean isFlipped = false;

    public Card(String value) {
        this.value = value;
        Icon icon = new ImageIcon(getClass().getResource(value));
        String name = value.substring(0, value.lastIndexOf('.'));
        setPreferredSize(new Dimension(100, 100));
        setName(name);
        setIcon(isFlipped ? icon : null);
    }

    public void flip() {
        isFlipped = !isFlipped;
        Icon icon = new ImageIcon(getClass().getResource(value));
        setIcon(isFlipped ? icon : null);
    }
}
