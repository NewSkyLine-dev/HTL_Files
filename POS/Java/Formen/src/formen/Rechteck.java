package formen;

import jakarta.xml.bind.annotation.XmlAttribute;

import java.awt.*;

public class Rechteck extends Polygon {
    protected int height;
    public Rechteck(int x, int y, int length, int height, Color color) {
        super(x, y, length, 4, color);
        this.height = height;
    }

    public Rechteck() {}

    @XmlAttribute
    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int length = Math.abs(firstPoint.x - secondPoint.x);
        int height = Math.abs(firstPoint.y - secondPoint.y);
        return new Rechteck(x, y, length, height, color);
    }

    @Override
    public void paint(Graphics g) {
        g.drawRect(x, y, length, height);
    }
}
