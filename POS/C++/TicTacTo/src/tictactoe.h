#ifndef TIC_TAC_TOE_H
#define TIC_TAC_TOE_H

#include <string>

/*
╔═══════════╗
║ Name      ║
╠═══╦═══╦═══╣
║ 9 ║ 8 ║ 7 ║
╠═══╬═══╬═══╣
║ 4 ║ 5 ║ 6 ║
╠═══╬═══╬═══╣
║ 1 ║ 2 ║ 3 ║
╠═══╩═══╩═══╣
║ Name      ║
╚═══════════╝
Name ist dran;
*/

#define BLUE "\033[34m"
#define RED "\033[31m"

struct BoardIndex
{
    int row;
    int col;
};

class TicTacToe
{
public:
    TicTacToe(std::string player1, std::string player2);
    void play();

private:
    std::string player1_;
    std::string player2_;
    int         plays        = 0;
    char        board_[3][3] = {{'7', '8', '9'}, {'4', '5', '6'}, {'1', '2', '3'}};
    std::string currentPlayer_;
    void        initializeBoard();
    void        printBoard();
    bool        checkTie(char symbole);
    void        makeMove();
    void        switchPlayer();
    char        symbole_player_one = 'X';
    char        symbole_player_two = 'O';
    int         currentField       = 0;
    BoardIndex  BoardIndex[9]      = {{2, 0}, {2, 1}, {2, 2}, {1, 0}, {1, 1}, {1, 2}, {0, 0}, {0, 1}, {0, 2}};
};

#endif // TIC_TAC_TOE_H