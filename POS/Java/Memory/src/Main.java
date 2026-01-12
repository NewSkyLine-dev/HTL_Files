import javax.swing.*;
import java.awt.*;

public class Main {
    public static void main(String[] args) throws HeadlessException {
        /*int totalPlayers;
            int totalPairs;
            StringBuilder totalPlayersText = new StringBuilder();
            StringBuilder totalPairsText = new StringBuilder();

            totalPlayersText.append("How many players: ");
            totalPairsText.append("How many pairs: ");

            do {
                try {
                    totalPlayers = Integer.parseInt(JOptionPane.showInputDialog(totalPlayersText.toString()));
                } catch (NumberFormatException e) {
                    totalPlayers = 0;
                    totalPlayersText.insert(0, "Invalid input. ");
                }
            } while (totalPlayers < 1 || totalPlayers > 4);

            do {
                try {
                    totalPairs = Integer.parseInt(JOptionPane.showInputDialog(totalPairsText.toString()));
                } catch (NumberFormatException e) {
                    totalPairsText.insert(0, "Invalid input. ");
                    totalPairs = 0;
                }
            } while (totalPairs < 1 || totalPairs > 10 || totalPairs % 2 != 0);*/
        // new MemoryGameMenu(totalPlayers, totalPairs);
        SwingUtilities.invokeLater(MemoryGameMenu::new);
    }
}