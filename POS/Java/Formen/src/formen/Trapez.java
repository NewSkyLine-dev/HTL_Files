package formen;

import jakarta.xml.bind.annotation.XmlAttribute;

import java.awt.*;

public class Trapez extends Polygon {
    protected int width, height;

    public Trapez(int x, int y, int width, int height, Color color) {
        super(x, y, width, 4, color);
        this.width = width;
        this.height = height;
    }

    public Trapez() {}

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

    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        return null;
    }
}
