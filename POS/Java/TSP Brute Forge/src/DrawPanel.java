import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.io.InputStream;

public class DrawPanel extends JPanel {
    private final ArrayList<City> cities;
    private ArrayList<Integer> bestPath = new ArrayList<>();
    private int currentCityIndex = 0;

    public DrawPanel(ArrayList<City> cities) {
        this.cities = cities;

       Timer timer = new Timer(1000, e -> {
            if (currentCityIndex >= cities.size()) {
                ((Timer) e.getSource()).stop();
            } else {
                currentCityIndex++;
                repaint();
            }
        });
        timer.start();
    }

    private int mapCoordinate(int coordinate, int minInput, int maxInput, int maxOutput) {
        // Normalize the coordinate
        double normalized = (double) (coordinate - minInput) / (maxInput - minInput);
        // Map to the output range
        return (int) (normalized * maxOutput);
    }
    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);

        // Find the minimum and maximum x and y coordinates
        int minX = (int) cities.stream().mapToDouble(City::getX).min().orElse(0);
        int maxX = (int) cities.stream().mapToDouble(City::getX).max().orElse(0);
        int minY = (int) cities.stream().mapToDouble(City::getY).min().orElse(0);
        int maxY = (int) cities.stream().mapToDouble(City::getY).max().orElse(0);

        for (int i = 0; i < currentCityIndex; i++) {
            City city = cities.get(i);
            // Scale the city coordinates relative to the JPanel's size
            int x = mapCoordinate((int) city.getX(), minX, maxX, getWidth() / 2);
            int y = mapCoordinate((int) city.getY(), minY, maxY, getHeight() / 2);

            // Draw line between cities
            if (i > 0) {
                City prevCity = cities.get(i - 1);
                int prevX = mapCoordinate((int) prevCity.getX(), minX, maxX, getWidth() / 2);
                int prevY = mapCoordinate((int) prevCity.getY(), minY, maxY, getHeight() / 2);
                g.drawLine(prevX, prevY, x, y);
            } if (i == currentCityIndex - 1) {
                City firstCity = cities.get(0);
                int firstX = mapCoordinate((int) firstCity.getX(), minX, maxX, getWidth() / 2);
                int firstY = mapCoordinate((int) firstCity.getY(), minY, maxY, getHeight() / 2);
                g.drawLine(firstX, firstY, x, y);
            }

            // Draw the oval using the scaled coordinates
            g.fillOval(x, y, 5, 5);

            // Draw the city name next to the dot
            g.drawString(city.getName(), x + 10, y + 10);
        }
    }

    public static void main(String[] args) {
        InputStream file = Main.class.getResourceAsStream("59 Staedte DE.csv");
        CityMap map = new CityMap();
        map.read(file);

        JFrame frame = new JFrame("Draw Random Dots Example");
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.add(new DrawPanel(map.getCities()));
        frame.setSize(400, 400);
        frame.setVisible(true);
    }
}