package formen;

import jakarta.xml.bind.annotation.XmlAttribute;

import java.awt.*;

public class Ellipse extends Form {
    protected int width, height;

    public Ellipse(int x, int y, int width, int height, Color color) {
        super(x, y);
        this.width = width;
        this.height = height;
        setColor(color);
    }

    public Ellipse() {}

    @XmlAttribute
    public int getWidth() {
        return width;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    @XmlAttribute
    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    @Override
    public void paint(Graphics g) {
        drawOval(g, x, y, width, height);
    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int width = Math.abs(firstPoint.x - secondPoint.x);
        int height = Math.abs(firstPoint.y - secondPoint.y);
        return new Ellipse(x, y, width, height, color);
    }
}
