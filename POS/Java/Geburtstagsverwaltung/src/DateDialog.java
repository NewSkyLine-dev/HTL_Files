import com.toedter.calendar.JDateChooser;

import javax.swing.*;
import java.awt.event.*;
import java.text.SimpleDateFormat;
import java.time.LocalDate;

public class DateDialog extends JDialog {
    private JPanel contentPane;
    private JButton buttonOK;
    private JButton buttonCancel;
    private JDateChooser dateChooser;
    private JTextField nameInputField;

    public DateDialog() {
        setContentPane(contentPane);
        setModal(true);
        getRootPane().setDefaultButton(buttonOK);
        pack();

        buttonOK.addActionListener(e -> {
            onOK();
        });

        buttonCancel.addActionListener(e -> {
            onCancel();
        });

        // call onCancel() when cross is clicked
        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);
        addWindowListener(new WindowAdapter() {
            public void windowClosing(WindowEvent e) {
                onCancel();
            }
        });

        // call onCancel() on ESCAPE
        contentPane.registerKeyboardAction(e -> onCancel(), KeyStroke.getKeyStroke(KeyEvent.VK_ESCAPE, 0), JComponent.WHEN_ANCESTOR_OF_FOCUSED_COMPONENT);
    }

    private void onOK() {
        dispose();
    }

    private void onCancel() {
        dispose();
    }

    private void createUIComponents() {
        // TODO: place custom component creation code here
        dateChooser = new JDateChooser();
    }

    public Geburtstag getData() {
        return new Geburtstag(nameInputField.getText(), LocalDate.parse(new SimpleDateFormat("yyyy-MM-dd").format(dateChooser.getDate())));
    }
}
