package formen;

import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlElementWrapper;
import jakarta.xml.bind.annotation.XmlElements;
import jakarta.xml.bind.annotation.XmlRootElement;

import java.util.LinkedList;

@XmlRootElement
public class FormenData {
    private LinkedList<Form> formen;

    public FormenData() {}

    public FormenData(LinkedList<Form> formen) {
        this.formen = formen;
    }

    @XmlElementWrapper(name="formen")
    @XmlElements({
            @XmlElement(name="Ellipse", type=Ellipse.class),
            @XmlElement(name="Dreieck", type=Dreieck.class),
            @XmlElement(name="Quadrat", type=Quadrat.class),
            @XmlElement(name="Hexagon", type=Hexagon.class),
            @XmlElement(name="Kreis", type=Kreis.class),
            @XmlElement(name="Rechteck", type=Rechteck.class)
    })
    public LinkedList<Form> getFormen() {
        return formen;
    }

    public void setFormen(LinkedList<Form> formen) {
        this.formen = formen;
    }
}