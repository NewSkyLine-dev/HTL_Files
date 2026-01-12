import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.HeadlessException;
import java.awt.event.*;
import java.awt.BorderLayout;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class MainWindow extends JFrame {
    private Connection con = null;
    private Statement stmn = null;
    private final JTable userTable = new JTable();
    private User currentUser = null;
    List<Message> shownMessages = new ArrayList<>();

    public MainWindow() throws HeadlessException {
        addWindowListener(new WindowAdapter() {
            @Override
            public void windowOpened(WindowEvent e) {
                try {
                    authenticateUser();
                } catch (SQLException ex) {
                    throw new RuntimeException(ex);
                }
            }

            @Override
            public void windowClosed(WindowEvent e) {
                closeDatabaseResources();
            }
        });
    }

    private void authenticateUser() throws SQLException {
        String username = JOptionPane.showInputDialog("Benutzername:");
        String password = JOptionPane.showInputDialog("Passwort:");
        setupDatabase();
        ValidUser(username, password);
        setupUI();
        loadUserData();
    }

    private void ValidUser(String username, String password) throws SQLException {
        ResultSet rs = stmn.executeQuery("SELECT * FROM benutzer WHERE name = '" + username + "' AND passwort = '" + password + "'");
        if (!rs.next()) {
            stmn.executeUpdate("INSERT INTO benutzer (name, passwort) VALUES ('" + username + "', '" + password + "')");
        }
        currentUser = new User(rs.getInt("benutzerid"), rs.getString("name"));
    }

    private void setupUI() {
        setTitle("Nachrichtensystem");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(400, 300);

        JTabbedPane tabbedPane = new JTabbedPane(JTabbedPane.TOP, JTabbedPane.SCROLL_TAB_LAYOUT);

        JPanel userAdministrationPanel = new JPanel();
        JPanel inboxPanel = new JPanel();
        JPanel outboxPanel = new JPanel();
        JPanel newMessagePanel = new JPanel();

        userAdministrationPanel.setLayout(new BorderLayout());

        JScrollPane userTableScrollPane = new JScrollPane(userTable);

        userAdministrationPanel.add(userTableScrollPane, BorderLayout.CENTER);

        tabbedPane.addChangeListener(e -> {
            DefaultTableModel MessageTableModel = new DefaultTableModel();
            MessageTableModel.addColumn("Nachricht");
            MessageTableModel.addColumn("Datum");
            MessageTableModel.addColumn("Absender");
            MessageTableModel.addColumn("Empfänger");
            if (tabbedPane.getSelectedIndex() == 0) {
                MessageTableModel.setRowCount(0);
                shownMessages = getIncomingMessages();
                fillMessages(inboxPanel, MessageTableModel, shownMessages);
            } else if (tabbedPane.getSelectedIndex() == 1) {
                MessageTableModel.setRowCount(0);
                shownMessages = getOutgoingMessages();
                fillMessages(outboxPanel, MessageTableModel, shownMessages);
            }
        });

        newMessagePanel.setLayout(new BorderLayout());

        JTextField messageField = new JTextField("Empfänger eingeben");
        JTextField receiverField = new JTextField("Nachricht eingeben");
        JButton sendButton = new JButton("Senden");
        sendButton.addActionListener(e -> {
            sendMessage(receiverField.getText(), messageField.getText());
            messageField.setText("");
            receiverField.setText("");
        });

        newMessagePanel.add(messageField, BorderLayout.NORTH);
        newMessagePanel.add(receiverField, BorderLayout.CENTER);
        newMessagePanel.add(sendButton, BorderLayout.SOUTH);

        tabbedPane.addTab("Posteingang", inboxPanel);
        tabbedPane.addTab("Postausgang", outboxPanel);
        tabbedPane.addTab("Neue Nachricht", newMessagePanel);

        add(tabbedPane);
    }

    private void fillMessages(JPanel messagePanel, DefaultTableModel messageTableModel, List<Message> messages) {
        for (Message message : messages) {
            messageTableModel.addRow(new Object[]{
                    message.getMessage(),
                    message.getDate(),
                    message.getSender(),
                    message.getReceiver()
            });
        }
        JTable messageTable = new JTable(messageTableModel);
        JScrollPane outboxTableScrollPane = new JScrollPane(messageTable);

        // Check if message is pressed with enter
        messageTable.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                    showMessage(shownMessages.get(messageTable.getSelectedRow()).getId());
                }
            }
        });

        messagePanel.setLayout(new BorderLayout());
        messagePanel.add(outboxTableScrollPane, BorderLayout.CENTER);
    }

    private void showMessage(int id) {
        try {
            ResultSet rs = stmn.executeQuery("SELECT * FROM nachrichten WHERE nachrichtid = " + id);
            if (rs.next()) {
                JOptionPane.showMessageDialog(this, rs.getString("nachricht"), "Nachricht", JOptionPane.INFORMATION_MESSAGE);
            }
        } catch (SQLException ex) {
            System.out.println(ex.getMessage());
        }
    }

    private void
    setupDatabase() {
        try {
            Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
            con = DriverManager.getConnection("jdbc:derby:data");
            System.setProperty("derby.language.sequence.preallocator", "1");
            stmn = con.createStatement();
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }
    }

    private void loadUserData() throws SQLException {
        DefaultTableModel userTableModel = new DefaultTableModel();
        userTableModel.addColumn("ID");
        userTableModel.addColumn("Username");
        userTableModel.addColumn("Passwort");

        ResultSet rs = stmn.executeQuery("SELECT * FROM benutzer");

        while (rs.next()) {
            userTableModel.addRow(new Object[]{
                    rs.getInt("benutzerid"),
                    rs.getString("name"),
                    rs.getString("passwort")
            });
        }

        userTable.setModel(userTableModel);
    }

    private List<Message> getOutgoingMessages() {
        List<Message> messages = new ArrayList<>();
        try {
            ResultSet rs = stmn.executeQuery("SELECT * FROM nachrichten WHERE absenderid = " + currentUser.getId() + " ORDER BY receivedat DESC");
            getMessages(messages, rs);
        } catch (SQLException ex) {
            System.out.println(ex.getMessage());
        }
        return messages;
    }

    private void getMessages(List<Message> messages, ResultSet rs) throws SQLException {
        while (rs.next()) {
            stmn = con.createStatement();
            ResultSet rsSender = stmn.executeQuery("SELECT name FROM benutzer WHERE benutzerid = " + rs.getString("absenderid"));

            stmn = con.createStatement();
            ResultSet rsReceiver = stmn.executeQuery("SELECT name FROM benutzer WHERE benutzerid = " + rs.getString("empfaengerid"));

            if (rsSender.next() && rsReceiver.next()) {
                messages.add(new Message(rs.getInt("nachrichtid"),
                        rs.getString("nachricht"),
                        rs.getDate("receivedat").toLocalDate(),
                        rsSender.getString("name"),
                        rsReceiver.getString("name")));
            }
        }
    }

    private List<Message> getIncomingMessages() {
        List<Message> messages = new ArrayList<>();
        try {
            ResultSet rs = stmn.executeQuery("SELECT * FROM nachrichten WHERE empfaengerid = " + currentUser.getId() + " ORDER BY receivedat DESC");
            getMessages(messages, rs);
        } catch (SQLException ex) {
            System.out.println(ex.getMessage());
        }
        return messages;
    }

    private void sendMessage(String message, String receiver) {
        try {
            stmn = con.createStatement();
            ResultSet rsReceiver = stmn.executeQuery("SELECT benutzerid FROM benutzer WHERE name = '" + receiver + "'");

            stmn = con.createStatement();
            if (rsReceiver.next()) {
                System.setProperty("derby.language.sequence.preallocator", "1");
                stmn = con.createStatement();
                stmn.executeUpdate("INSERT INTO nachrichten (nachricht, absenderid, empfaengerid, receivedat) VALUES ('" + message + "', " + currentUser.getId() + ", " + rsReceiver.getInt("benutzerid") + ", CURRENT_DATE)");
            } else {
                JOptionPane.showMessageDialog(this, "Empfänger nicht gefunden", "Fehler", JOptionPane.ERROR_MESSAGE);
            }
        } catch (SQLException ex) {
            System.out.println(ex.getMessage());
        }
    }

    private void closeDatabaseResources() {
        try {
            if (stmn != null) stmn.close();
            if (con != null) con.close();
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }
    }
}
