import javax.swing.*;
import javax.swing.table.DefaultTableModel;

import java.awt.event.*;
import java.time.format.DateTimeFormatter;
import java.util.*;

public class MainWindow extends JFrame {
    private JTable birthdayTable;
    private JPanel main;
    private final DefaultTableModel model;
    private final GeburtstagManager geburtstagManager = new GeburtstagManager();
    ResourceBundle rb = ResourceBundle.getBundle("DateBirth");

    public MainWindow() {
        setSize(500, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setContentPane(main);
        setTitle(rb.getString("projectName"));

        model = new DefaultTableModel(new Object[]{rb.getString("name"), rb.getString("birthday")}, 0);
        birthdayTable.setModel(model);

        JMenuBar menuBar = new JMenuBar();
        JMenu menu = new JMenu(rb.getString("mbMenu"));
        JMenuItem hinzufuegenItem = new JMenuItem(rb.getString("btnAddBirthday"));
        JMenuItem spracheItem = new JMenuItem(rb.getString("btnChangeLanguage"));

        addWindowListener(new WindowAdapter() {
            @Override
            public void windowOpened(WindowEvent e) {
                try {
                    geburtstagManager.loadGeburtstage();
                    refreshTable();
                } catch (Exception ex) {
                    System.out.println(ex.getMessage());
                }
            }
        });

        addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                try {
                    geburtstagManager.saveGeburtstage();
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        });

        spracheItem.addActionListener(e -> {
            String localeStr = zeigeSprachDialog();
            Locale.setDefault(new Locale(localeStr));
            ResourceBundle.clearCache();
            rb = ResourceBundle.getBundle("DateBirth");

            setTitle(rb.getString("projectName"));
            hinzufuegenItem.setText(rb.getString("btnAddBirthday"));
            menu.setText(rb.getString("mbMenu"));
            model.setColumnIdentifiers(new Object[]{rb.getString("name"), rb.getString("birthday")});
            spracheItem.setText(rb.getString("btnChangeLanguage"));

            repaint();
            revalidate();
        });

        birthdayTable.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_BACK_SPACE) {
                    int selectedRow = birthdayTable.getSelectedRow();
                    if (selectedRow >= 0) {
                        deleteEntry(selectedRow);
                        refreshTable();
                    }
                }
            }
        });

        hinzufuegenItem.addActionListener(e -> {
            DateDialog dialog = new DateDialog();
            dialog.setVisible(true);

            if (dialog.getData() != null) {
                geburtstagManager.getGeburtstage().add(dialog.getData());
                refreshTable();
            }
        });

        menu.add(hinzufuegenItem);
        menu.add(spracheItem);
        menuBar.add(menu);

        setJMenuBar(menuBar);

        pack();
    }

    private void refreshTable() {
        model.setRowCount(0);
        String pattern = rb.getString("dateFormat");
        DateTimeFormatter dateFormat = DateTimeFormatter.ofPattern(pattern);

        geburtstagManager.getBirthdayByDate().forEach((date, geburtstage) -> {
            geburtstage.forEach(geburtstag -> {
                if (geburtstag.equals(geburtstage.get(0))) {
                    model.addRow(new Object[]{geburtstag.getPerson(), date.format(dateFormat)});
                } else {
                    model.addRow(new Object[]{geburtstag.getPerson(), ""});
                }
            });
        });
    }

    private String zeigeSprachDialog() {
        HashMap<String, String> sprachen = new HashMap<>();
        sprachen.put("Deutsch", "de");
        sprachen.put("Englisch", "en");
        sprachen.put("Spanisch", "es");
        JComboBox<String> sprachAuswahl = new JComboBox<>(sprachen.keySet().toArray(new String[0]));

        JOptionPane.showMessageDialog(this, sprachAuswahl, "Sprache wählen", JOptionPane.PLAIN_MESSAGE);

        return sprachen.get(sprachAuswahl.getSelectedItem());
    }

    private void deleteEntry(int selectedRow) {
        geburtstagManager.getGeburtstage().remove(selectedRow);
        model.removeRow(selectedRow);
        refreshTable();
    }
}
