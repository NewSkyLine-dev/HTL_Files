import org.apache.commons.codec.language.Soundex;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.sql.*;
import java.util.ArrayList;
import java.util.Objects;

public class MainWindow extends JFrame{
    private JPanel main;
    private Soundex soundex = new Soundex();
    private JComboBox<String> aufgabe2Geschlecht;
    private JComboBox<Integer> aufgabe2Jahr;
    private JButton aufgabe3Button;
    private JTextField aufgabe4Suchtext;
    private JButton aufgabe4Button;
    private JTable aufgabe3Tabelle;
    //aufgabe4Liste zeigt in Aufgabe 6 die Männernamen an
    private JList aufgabe4Liste;
    //aufgabe6Liste zeigt in Aufgabe 6 die Frauennamen an
    private JList aufgabe6Liste;
    private JButton aufgabe5Button;
    private Connection con = null;
    private Statement stmnt = null;

    public MainWindow() throws HeadlessException {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setTitle("PA4");
        setSize(800,600);
        setContentPane(main);

        addWindowListener(new WindowAdapter() {
            @Override
            public void windowOpened(WindowEvent e) {
                super.windowOpened(e);
                connectToDatabase();

                // UI Setup
                fillGenderComboBox();
                fillYearComboBox();

                aufgabe3Button.addActionListener(ev -> {
                    try {
                        PreparedStatement ps = con.prepareStatement("SELECT NAME, COUNT FROM FIRSTNAME WHERE GENDER = ? AND \"YEAR\" = ?");

                        ps.setString(1, (String) aufgabe2Geschlecht.getSelectedItem());
                        ps.setInt(2, (int) aufgabe2Jahr.getSelectedItem());

                        ResultSet rs = ps.executeQuery();
                        DefaultTableModel model = new DefaultTableModel();
                        model.addColumn("Name");
                        model.addColumn("Anzahl");

                        while (rs.next()) {
                            model.addRow(new Object[]{rs.getString("NAME"), rs.getInt("COUNT")});
                        }
                        aufgabe3Tabelle.setModel(model);
                    } catch (Exception ex) {
                        ex.printStackTrace();
                    }
                });

                aufgabe4Button.addActionListener(ev -> {
                    try {
                        // PreparedStatement ps = con.prepareStatement("SELECT NAME FROM FIRSTNAME WHERE NAME LIKE ?");
                        // ps.setString(1, "%" + aufgabe4Suchtext.getText() + "%");

                        // ResultSet rs = ps.executeQuery();

                        // DefaultListModel<String> model = new DefaultListModel<>();
                        // while (rs.next()) {
                            // model.addElement(rs.getString("NAME"));
                        // }
                        // aufgabe4Liste.setModel(model);

                        PreparedStatement ps = con.prepareStatement("SELECT DISTINCT(NAME), GENDER FROM FIRSTNAME WHERE NAME LIKE ?");
                        ps.setString(1, "%" + aufgabe4Suchtext.getText() + "%");

                        ResultSet rs = ps.executeQuery();

                        DefaultListModel<String> maleModel = new DefaultListModel<>();
                        DefaultListModel<String> femaleModel = new DefaultListModel<>();

                        while (rs.next()) {
                            if (rs.getString("GENDER").equals("M")) {
                                maleModel.addElement(rs.getString("NAME"));
                            } else {
                                femaleModel.addElement(rs.getString("NAME"));
                            }
                        }
                        aufgabe4Liste.setModel(maleModel);
                        aufgabe6Liste.setModel(femaleModel);
                    } catch (Exception ex) {
                        ex.printStackTrace();
                    }
                });

                aufgabe5Button.addActionListener(ev -> {
                    try {
                        // String soundCode = getSoundExCode(aufgabe4Suchtext.getText());
                        // PreparedStatement ps = con.prepareStatement("SELECT NAME FROM FIRSTNAME");

                        // ResultSet rs = ps.executeQuery();
                        // DefaultListModel<String> model = new DefaultListModel<>();

                        // while (rs.next()) {
                        //     if (getSoundExCode(rs.getString("NAME")).equals(soundCode)) {
                        //         model.addElement(rs.getString("NAME"));
                        //     }
                        // }

                        // aufgabe4Liste.setModel(model);

                        ResultSet rs = stmnt.executeQuery("SELECT DISTINCT(NAME), GENDER FROM FIRSTNAME");

                        DefaultListModel<String> maleModel = new DefaultListModel<>();
                        DefaultListModel<String> femaleModel = new DefaultListModel<>();

                        String soundCode = getSoundExCode(aufgabe4Suchtext.getText());

                        while (rs.next()) {
                            if (getSoundExCode(rs.getString("NAME")).equals(soundCode)) {
                                if (Objects.equals(rs.getString("GENDER"), "M")) {
                                    maleModel.addElement(rs.getString("NAME"));
                                } else {
                                    femaleModel.addElement(rs.getString("NAME"));
                                }
                            }
                        }
                        aufgabe4Liste.setModel(maleModel);
                        aufgabe6Liste.setModel(femaleModel);
                    } catch (Exception ex) {
                        ex.printStackTrace();
                    }
                });

                // Aufgabe 7
                aufgabe3Tabelle.addMouseListener(new java.awt.event.MouseAdapter() {
                    @Override
                    public void mouseClicked(java.awt.event.MouseEvent evt) {
                        int row = aufgabe3Tabelle.rowAtPoint(evt.getPoint());

                        String name = (String) aufgabe3Tabelle.getValueAt(row, 0);
                        String soundCode = getSoundExCode(name);

                        try {
                            ResultSet rs = stmnt.executeQuery("SELECT DISTINCT(NAME), GENDER FROM FIRSTNAME");
                            DefaultListModel<String> maleModel = new DefaultListModel<>();
                            DefaultListModel<String> femaleModel = new DefaultListModel<>();

                            while (rs.next()) {
                                if (getSoundExCode(rs.getString("NAME")).equals(soundCode)) {
                                    if (Objects.equals(rs.getString("GENDER"), "M")) {
                                        maleModel.addElement(rs.getString("NAME"));
                                    } else if (Objects.equals(rs.getString("GENDER"), "F")) {
                                        femaleModel.addElement(rs.getString("NAME"));
                                    }
                                }
                            }
                            aufgabe4Liste.setModel(maleModel);
                            aufgabe6Liste.setModel(femaleModel);
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                });
            }


            @Override
            public void windowClosing(WindowEvent e) {
                super.windowClosing(e);
                try {
                    if (stmnt != null) {
                        stmnt.close();
                    }
                    if (con != null) {
                        con.close();
                    }
                } catch (Exception ex) {
                    ex.printStackTrace();
                }
            }
        });
    }

    private void connectToDatabase() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            con = DriverManager.getConnection("jdbc:derby:database");
            stmnt = con.createStatement();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void fillGenderComboBox() {
        ResultSet rs;
        try {
            rs = stmnt.executeQuery("SELECT DISTINCT GENDER FROM FIRSTNAME");
            while (rs.next()) {
                aufgabe2Geschlecht.addItem(rs.getString("GENDER"));
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void fillYearComboBox() {
        ResultSet rs;
        try {
            rs = stmnt.executeQuery("SELECT DISTINCT \"YEAR\" FROM FIRSTNAME ORDER BY \"YEAR\" ASC");
            while (rs.next()) {
                aufgabe2Jahr.addItem(rs.getInt("YEAR"));
            }
            aufgabe2Jahr.addItem(2010);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String getSoundExCode(String name)
    {
        return soundex.soundex(name);
    }

    public static void main(String[] args) {
        MainWindow mw = new MainWindow();
        mw.setVisible(true);
    }

    private void createUIComponents() {
        aufgabe2Jahr = new JComboBox<>();
        aufgabe2Geschlecht = new JComboBox<>();
    }
}
