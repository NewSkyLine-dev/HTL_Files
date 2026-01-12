package formen;

import jakarta.xml.bind.*;
import jakarta.xml.bind.annotation.XmlTransient;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.util.*;

@XmlTransient
public class FormenPanel extends JPanel {
    private final LinkedList<Form> formen = new LinkedList<>();
    @XmlTransient
    private Form currentForm = null;
    @XmlTransient
    private Point firstPoint = null;
    @XmlTransient
    private Point secondPoint = null;
    @XmlTransient
    private Form previewForm = null;

    public FormenPanel() {
        addMouseListener(new MouseAdapter() {
            @Override
            public void mouseClicked(MouseEvent e) {
                if (firstPoint == null) {
                    firstPoint = e.getPoint();
                    System.out.println("firstPoint: " + firstPoint);
                }
                else {
                    secondPoint = e.getPoint();
                    System.out.println("secondPoint: " + secondPoint);
                    Form newForm = currentForm.getNewInstance(firstPoint, secondPoint);
                    formen.add(newForm);
                    repaint();
                    firstPoint = null;
                    secondPoint = null;
                }
            }
        });

        addMouseMotionListener(new MouseMotionAdapter() {
            @Override
            public void mouseMoved(MouseEvent e) {
                previewForm = currentForm.getNewInstance(e.getPoint(), new Point(e.getPoint().x + 50, e.getPoint().y + 50));
                repaint();
            }
        });
    }

    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
        if (previewForm != null) {
            previewForm.paint(g);
        }
    }

    @Override
    public void paint(Graphics g) {
        super.paint(g);
        for(Form f : formen)
        {
            if(f != null) {
                g.setColor(f.getColor());
                f.paint(g);
            }
        }
    }

    public LinkedList<Form> getFormen() {
        return formen;
    }

    public void saveToFile(FormenPanel fp) throws JAXBException {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
        int returnValue = fileChooser.showSaveDialog(this);

        if (returnValue == JFileChooser.APPROVE_OPTION) {
            File directory = fileChooser.getCurrentDirectory();
            File xmlFile = new File(directory, "formen.xml");

            FormenData data = new FormenData(fp.getFormen());

            JAXBContext cx = JAXBContext.newInstance(FormenData.class);
            Marshaller marshaller = cx.createMarshaller();
            marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
            marshaller.marshal(data, xmlFile);
        }
    }

    public void loadFromFile() {
        JFileChooser fileChooser = new JFileChooser();
        fileChooser.setFileSelectionMode(JFileChooser.FILES_ONLY);
        int returnValue = fileChooser.showOpenDialog(this);

        if (returnValue == JFileChooser.APPROVE_OPTION) {
            File file = fileChooser.getSelectedFile();
            try {
                JAXBContext cx = JAXBContext.newInstance(FormenData.class);
                Unmarshaller unmarshaller = cx.createUnmarshaller();
                FormenData data = (FormenData) unmarshaller.unmarshal(file);
                formen.addAll(data.getFormen());
                repaint();
            } catch (JAXBException e) {
                e.printStackTrace();
            }
        }
    }

    public void clear() {
        formen.clear();
        repaint();
    }

    public void setCurrentForm(Form currentForm) {
        this.currentForm = currentForm;
    }

    public Form getCurrentForm() {
        return currentForm;
    }
}