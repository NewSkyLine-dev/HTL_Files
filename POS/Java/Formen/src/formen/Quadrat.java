package formen;

import java.awt.*;

public class Quadrat extends Rechteck {
    public Quadrat(int x, int y, int length, Color color) {
        super(x, y, length, length, color);
    }

    public Quadrat() {}

    @Override
    public Form getNewInstance(Point firstPoint, Point secondPoint) {
        int x = Math.min(firstPoint.x, secondPoint.x);
        int y = Math.min(firstPoint.y, secondPoint.y);
        int width = Math.abs(firstPoint.x - secondPoint.x);
        return new Quadrat(x, y, width, color);
    }
}
