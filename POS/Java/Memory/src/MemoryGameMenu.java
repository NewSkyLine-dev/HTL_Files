import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.util.*;
import java.util.List;
import java.util.Timer;

public class MemoryGameMenu extends JFrame {
    private int totalPlayers;
    private int totalPairs;
    private final HashMap<String, Integer> players = new HashMap<>();
    private Card lastClickedButton;
    private final JPanel gameLabel = new JPanel();
    private final Timer deltaTime = new Timer();
    private int currentPlayer = 0;
    private final JLabel currentPlayerLabel = new JLabel();
    private int finishedPairs;

    public MemoryGameMenu() throws HeadlessException {
        initUI();
    }

    private void startGame() {
        for (int i = 0; i < totalPlayers; i++) {
            players.put(JOptionPane.showInputDialog("Player " + (i + 1) + " name: "), 0);
        }

        gameLabel.setLayout(new GridLayout(totalPairs / 2, totalPairs / 2 + 1));

        /* Add all cards to the center of the frame */
        File[] iconFiles = new File("data/").listFiles();
        ArrayList<File> iconsList = new ArrayList<>(Arrays.asList(iconFiles));
        Collections.shuffle(iconsList);

        ArrayList<File> usedIcons = new ArrayList<>();
        for (int i = 0; i < totalPairs; ++i) {
            usedIcons.add(iconsList.get(i));
            usedIcons.add(iconsList.get(i)); // Add each icon twice for pairs
        }

        Collections.shuffle(usedIcons);

        for (int i = 0; i < totalPairs * 2; ++i) {
            Card card = new Card(usedIcons.get(i).getName());
            card.addActionListener(new MemoryListener());
            gameLabel.add(card);
        }

        add(gameLabel, BorderLayout.CENTER);
        repaint();
        revalidate();

        updatePlayer();
    }

    private void checkWinner() {
        if (finishedPairs == totalPairs) {
            List<Map.Entry<String, Integer>> sortedPlayers = new ArrayList<>(players.entrySet());

            // Sort players based on their points (descending order)
            sortedPlayers.sort(Map.Entry.comparingByValue(Comparator.reverseOrder()));

            StringBuilder playerInfo = new StringBuilder("<html><body><table>");
            playerInfo.append("<tr><th>Player</th><th>Points</th><th>Rank</th></tr>");

            for (int i = 0; i < sortedPlayers.size(); i++) {
                Map.Entry<String, Integer> playerEntry = sortedPlayers.get(i);
                String playerName = playerEntry.getKey();
                int points = playerEntry.getValue();
                int rank = i + 1;  // Rank is 1-based

                playerInfo.append(String.format("<tr><td>%s</td><td>%d</td><td>%d</td></tr>", playerName, points, rank));
            }

            playerInfo.append("</table></body></html>");

            JOptionPane.showMessageDialog(null, playerInfo.toString(), "Winner", JOptionPane.INFORMATION_MESSAGE);
            System.exit(0);
        }
    }

    private void initUI() {
        setTitle("Memory Game");
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(new BorderLayout());

        JMenuBar menuBar = new JMenuBar();
        JMenu gameMenu = new JMenu("Game");
        JMenu gameOptions = new JMenu("Options");

        gameMenu.setMnemonic('G');
        gameOptions.setMnemonic('O');

        JMenuItem newGame = new JMenuItem("New Game");
        JMenuItem exitGame = new JMenuItem("Exit");

        newGame.setMnemonic('N');
        exitGame.setMnemonic('E');

        newGame.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_N, ActionEvent.CTRL_MASK));
        exitGame.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_E, ActionEvent.CTRL_MASK));

        gameMenu.add(newGame);

        /* Select how many players should be in the game */
        ButtonGroup playerGroup = new ButtonGroup();
        ButtonGroup pairGroup = new ButtonGroup();

        boolean firstSelected = true;

        for (int i = 0; i < 4; i++) {
            JDataRadioButtonMenuItem player = new JDataRadioButtonMenuItem(i + 1 + " Player");
            if (firstSelected) {
                player.setSelected(true);
                firstSelected = false;
                totalPlayers = 1;
            }
            player.setData(String.valueOf(i + 1));
            player.addActionListener(e -> totalPlayers = Integer.parseInt(((JDataRadioButtonMenuItem)e.getSource()).getData()));
            playerGroup.add(player);
            gameOptions.add(player);
        }

        firstSelected = true;

        gameOptions.addSeparator();

        for (int i = 0; i < 4; i++) {
            JDataRadioButtonMenuItem pairs = new JDataRadioButtonMenuItem((i + 1) * 2 + " Pairs");
            if (firstSelected) {
                pairs.setSelected(true);
                firstSelected = false;
                totalPairs = 2;
            }
            pairs.setData(String.valueOf((i + 1) * 2));
            pairs.addActionListener(e -> totalPairs = Integer.parseInt(((JDataRadioButtonMenuItem)e.getSource()).getData()));
            pairGroup.add(pairs);
            gameOptions.add(pairs);
        }

        /*Add the menu bars*/
        menuBar.add(gameMenu);
        menuBar.add(gameOptions);
        setJMenuBar(menuBar);

        newGame.addActionListener(e -> startGame());

        setLocationRelativeTo(null);
        setVisible(true);
    }

    class MemoryListener implements ActionListener {
        @Override
        public void actionPerformed(ActionEvent e) {
            if (lastClickedButton == null) {
                lastClickedButton = (Card) e.getSource();
                lastClickedButton.flip();
                return;
            }

            Card card = (Card) e.getSource();
            card.flip();
            gameLabel.repaint();
            gameLabel.revalidate();

            if (card.getName().equals(lastClickedButton.getName())) {
                lastClickedButton.setEnabled(false);
                card.setEnabled(false);
                lastClickedButton = null;
                players.put((String) players.keySet().toArray()[currentPlayer], players.get(players.keySet().toArray()[currentPlayer]) + 1);
                finishedPairs++;
                checkWinner();
            } else {
                deltaTime.schedule(new TimerTask() {
                    @Override
                    public void run() {
                        lastClickedButton.flip();
                        card.flip();
                        gameLabel.repaint();
                        gameLabel.revalidate();
                        lastClickedButton = null;
                        currentPlayer = (currentPlayer + 1) % totalPlayers;
                        updatePlayer();
                    }
                }, 3000);
            }
            updatePlayer();
        }
    }

    private void updatePlayer() {
        String currentPlayer = (String) players.keySet().toArray()[this.currentPlayer];
        currentPlayerLabel.setText(currentPlayer + ": " + players.get(currentPlayer) + " points");
    }
}