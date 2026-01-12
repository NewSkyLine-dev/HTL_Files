package formen;

import jakarta.xml.bind.annotation.XmlAttribute;

import java.awt.*;

public class Polygon extends Form {
    protected int length, corners;

    @XmlAttribute
    public int getLength() {
        return length;
    }

    public void setLength(int length) {
        this.length = length;
    }

    @XmlAttribute
    public int getCorners() {
        return corners;
    }

    public void setCorners(int corners) {
        this.corners = corners;
    }

    public Polygon(int x, int y, int length, int corners, Color color) {
        super(x, y);
        this.length = length;
        this.corners = corners;
        setColor(color);
    }

    public Polygon() {}

    @Override
    public void paint(Graphics g) {
        int[] xPoints = new int[corners];
        int[] yPoints = new int[corners];
        for (int i = 0; i < corners; i++) {
            xPoints[i] = (int)(x + length * Math.cos(2 * Math.PI * i / corners));
            yPoints[i] = (int)(y + length * Math.sin(2 * Math.PI * i / corners));
        }
        g.drawPolygon(xPoints, yPoints, corners);
    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = (int) firstPoint.getX();
        int y = (int) firstPoint.getY();
        int length = (int) firstPoint.distance(secondPoint);
        return new Polygon(x, y, length, corners, color);
    }
}
