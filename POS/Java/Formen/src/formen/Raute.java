package formen;

import jakarta.xml.bind.annotation.XmlAttribute;

import java.awt.*;

public class Raute extends Polygon {
    protected int width;
    public Raute(int x, int y, int width, Color color) {
        super(x, y, width, 4, color);
        this.width = width;
    }

    public Raute() {}

    @XmlAttribute
    public int getWidth() {
        return width;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    @Override
    public void paint(Graphics g) {
        int[] xPoints = {x, x + width / 2, x + width, x + width / 2};
        int[] yPoints = {y + width / 2, y, y + width / 2, y + width};
        g.drawPolygon(xPoints, yPoints, 4);
    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int width = Math.abs(firstPoint.x - secondPoint.x);
        return new Raute(x, y, width, color);
    }
}
