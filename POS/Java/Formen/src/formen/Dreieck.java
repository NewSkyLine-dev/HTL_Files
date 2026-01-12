package formen;

import java.awt.*;

public class Dreieck extends Polygon {
    public Dreieck(int x, int y, int length, Color color) {
        super(x, y, length, 3, color);
    }

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int length = Math.abs(firstPoint.x - secondPoint.x);
        return new Dreieck(x, y, length, color);
    }
}
