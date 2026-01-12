package formen;

import jakarta.xml.bind.JAXBException;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.TreeMap;

public class MainWindow extends JFrame {
    private JPanel formenPanel;
    private JPanel mainPanel;
    private final TreeMap<String, Form> formenKatalog = new TreeMap<>() {
        {
            put("Rechteck", new Rechteck(0, 0, 0, 0, Color.BLACK));
            put("Quadrat", new Quadrat(0, 0, 0, Color.BLACK));
            put("Dreieck", new Dreieck(0, 0, 0, Color.BLACK));
            put("Trapez", new Trapez(0, 0, 0, 0, Color.BLACK));
            put("Raute", new Raute(0, 0, 0, Color.BLACK));
            put("Ellipse", new Ellipse(0, 0, 0, 0, Color.BLACK));
            put("Kreis", new Kreis(0, 0,0, Color.BLACK));
            put("Hexagon", new Hexagon(0, 0, 0, Color.BLACK));
            put("Polygon", new Polygon(0, 0, 0, 0, Color.BLACK));
        }
    };

    public MainWindow() throws HeadlessException {
        setTitle("Formen");
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setContentPane(mainPanel);
        setSize(800, 400);

        JMenuBar mb = new JMenuBar();
        JMenu fileMenu = new JMenu("File");
        JMenu formsMenu = new JMenu("Formen");
        JMenu colorsMenu = new JMenu("Farben");
        JMenuItem saveMenuItem = new JMenuItem("Speichern");
        JMenuItem loadMenuItem = new JMenuItem("Laden");
        JMenuItem clearMenuItem = new JMenuItem("Löschen");

        formsMenu.add(new FormMenuItem("Rechteck", formenKatalog.get("Rechteck"), KeyStroke.getKeyStroke('R'), KeyEvent.VK_R, true));
        formsMenu.add(new FormMenuItem("Quadrat", formenKatalog.get("Quadrat"), KeyStroke.getKeyStroke('Q'), KeyEvent.VK_Q, false));
        formsMenu.add(new FormMenuItem("Dreieck", formenKatalog.get("Dreieck"), KeyStroke.getKeyStroke('D'), KeyEvent.VK_D, false));
        formsMenu.add(new FormMenuItem("Trapez", formenKatalog.get("Trapez"), KeyStroke.getKeyStroke('T'), KeyEvent.VK_T, false));
        formsMenu.add(new FormMenuItem("Raute", formenKatalog.get("Raute"), KeyStroke.getKeyStroke('A'), KeyEvent.VK_A, false));
        formsMenu.add(new FormMenuItem("Ellipse", formenKatalog.get("Ellipse"), KeyStroke.getKeyStroke('E'), KeyEvent.VK_E, false));
        formsMenu.add(new FormMenuItem("Kreis", formenKatalog.get("Kreis"), KeyStroke.getKeyStroke('K'), KeyEvent.VK_K, false));
        formsMenu.add(new FormMenuItem("Hexagon", formenKatalog.get("Hexagon"), KeyStroke.getKeyStroke('H'), KeyEvent.VK_H, false));
        formsMenu.add(new FormMenuItem("Polygon", formenKatalog.get("Polygon"), KeyStroke.getKeyStroke('P'), KeyEvent.VK_P, false));

        colorsMenu.add(new ColorMenuItem("Schwarz", Color.BLACK, KeyStroke.getKeyStroke('S'), KeyEvent.VK_S, true));
        colorsMenu.add(new ColorMenuItem("Rot", Color.RED, KeyStroke.getKeyStroke('R'), KeyEvent.VK_R, false));
        colorsMenu.add(new ColorMenuItem("Grün", Color.GREEN, KeyStroke.getKeyStroke('G'), KeyEvent.VK_G, false));
        colorsMenu.add(new ColorMenuItem("Blau", Color.BLUE, KeyStroke.getKeyStroke('B'), KeyEvent.VK_B, false));

        saveMenuItem.addActionListener(e -> {
            try {
                ((FormenPanel) formenPanel).saveToFile((FormenPanel) formenPanel);
            } catch (JAXBException ex) {
                throw new RuntimeException(ex);
            }
        });
        loadMenuItem.addActionListener(e -> ((FormenPanel) formenPanel).loadFromFile());
        clearMenuItem.addActionListener(e -> ((FormenPanel) formenPanel).clear());

        fileMenu.add(saveMenuItem);
        fileMenu.add(loadMenuItem);
        fileMenu.add(clearMenuItem);

        mb.add(fileMenu);
        mb.add(formsMenu);
        mb.add(colorsMenu);

        setJMenuBar(mb);
    }

    class FormMenuItem extends JRadioButtonMenuItem implements ActionListener {
        static ButtonGroup bg = new ButtonGroup();

        public FormMenuItem(String text, Form form, KeyStroke accelerator, int mnemonic, boolean selected) {
            super(text);

            addActionListener(this);
            setAccelerator(accelerator);
            setMnemonic(mnemonic);
            setSelected(selected);
            if (selected) {
                ((FormenPanel) formenPanel).setCurrentForm(form);
            }
            bg.add(this);
        }

        @Override
        public void actionPerformed(ActionEvent e) {
            ((FormenPanel) formenPanel).setCurrentForm(formenKatalog.get(getText()));
        }
    }

    class ColorMenuItem extends JRadioButtonMenuItem implements ActionListener {
        static ButtonGroup bg = new ButtonGroup();
        private final Color color;

        public ColorMenuItem(String text, Color color, KeyStroke accelerator, int mnemonic, boolean selected) {
            super(text);

            addActionListener(this);
            setAccelerator(accelerator);
            setMnemonic(mnemonic);
            setSelected(selected);
            if (selected) {
                ((FormenPanel) formenPanel).getCurrentForm().setColor(color);
            }
            bg.add(this);

            this.color = color;
        }

        @Override
        public void actionPerformed(ActionEvent e) {
            ((FormenPanel) formenPanel).getCurrentForm().setColor(color);
        }
    }

    public static void main(String[] args) {
        MainWindow m = new MainWindow();
        m.setVisible(true);
    }

    private void createUIComponents() {
        // TODO: place custom component creation code here
        formenPanel = new FormenPanel();
    }
}
