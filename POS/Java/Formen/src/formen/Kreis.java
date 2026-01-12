package formen;

import java.awt.*;

public class Kreis extends Ellipse {
    public Kreis(int x, int y, int radius, Color color) {
        super(x, y, radius, radius, color);
    }

    public Kreis() {}

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int radius = Math.abs(firstPoint.x - secondPoint.x);
        return new Kreis(x, y, radius, color);
    }
}
