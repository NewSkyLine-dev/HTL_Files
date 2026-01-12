import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlAttribute;

@XmlAccessorType(XmlAccessType.FIELD)
public class Field {
    @XmlAttribute(name="zahl")
    private String number;

    @XmlAttribute(name="positionX")
    private int x;

    @XmlAttribute(name="positionY")
    private int y;

    public Field() {
    }

    public Field(String number, int x, int y) {
        this.number = number;
        this.x = x;
        this.y = y;
    }

    public String getNumber() {
        return number;
    }

    public void setNumber(String number) {
        this.number = number;
    }

    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }
}