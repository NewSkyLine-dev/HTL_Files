import javax.swing.*;

public class JDataRadioButtonMenuItem extends JRadioButtonMenuItem {
    String data;

    public JDataRadioButtonMenuItem(String text) {
        super(text);
        this.data = text;
    }

    public JDataRadioButtonMenuItem() {
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }
}
