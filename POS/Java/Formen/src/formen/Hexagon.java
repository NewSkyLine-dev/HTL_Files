package formen;

import java.awt.*;

public class Hexagon extends Polygon {
    public Hexagon(int x, int y, int length, Color color) {
        super(x, y, length, 6, color);
    }

    public Hexagon() {}

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int length = Math.abs(firstPoint.x - secondPoint.x);
        return new Hexagon(x, y, length, color);
    }
}
