package formen;

import jakarta.xml.bind.annotation.XmlAttribute;
import jakarta.xml.bind.annotation.XmlTransient;

import java.awt.*;

public abstract class Form {
    protected int x, y;

    @XmlTransient
    protected Color color;

    public Form(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Form() {}

    @XmlAttribute
    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    @XmlAttribute
    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }

    @XmlAttribute(name = "color")
    public int getColorAsInt()
    {
        return color.getRGB();
    }

    public void setColorAsInt(int color)
    {
        this.color = new Color(color);
    }


    @XmlTransient
    public Color getColor() {
        return color;
    }

    public void setColor(Color color) {
        this.color = color;
    }

    public abstract void paint(Graphics g);
    protected final void drawOval(Graphics g, int x1, int y1, int w, int h)
    {
        g.drawOval(x1, y1, w, h);
    }

    public abstract Form getNewInstance(Point firstPoint, Point secondPoint);
}
