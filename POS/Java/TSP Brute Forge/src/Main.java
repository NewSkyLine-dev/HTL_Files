import javax.swing.*;
import java.io.InputStream;

public class Main {
    public static void main(String[] args) {
        try {
            InputStream file = Main.class.getResourceAsStream("StadtKoordinaten.txt");
            CityMap map = new CityMap();

            map.read(file);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
}