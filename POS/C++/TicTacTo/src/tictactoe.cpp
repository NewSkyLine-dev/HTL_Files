#include "tictactoe.h"

#include <iomanip>
#include <iostream>
#include <stdexcept>

TicTacToe::TicTacToe(std::string player1, std::string player2)
    : player1_(player1), player2_(player2), currentPlayer_((rand() % 2 == 0) ? player1_ : player2_)
{
}

void TicTacToe::play()
{
    while (true)
    {
        printBoard();
        makeMove();
        std::cout << "\033[0m";
        printBoard();
        if (plays == 9 || checkTie('X') || checkTie('O'))
        {
            break;
        }
    }
    if (!checkTie('X') && !checkTie('O') && plays == 9)
    {
        std::cout << "Unentschieden !" << std::endl;
    }
    if (checkTie('X'))
    {
        std::cout << player1_ << " hat gewonnen" << std::endl;
    }
    else if (checkTie('O'))
    {
        std::cout << player2_ << " hat gewonnen" << std::endl;
    }
}

std::string field_color(char x)
{
    std::string my_string(1, x);
    if (x == 'X')
    {
        return RED + my_string + "\033[0m";
    }
    else if (x == 'O')
    {
        return BLUE + my_string + "\033[0m";
    }
    else
    {
        return my_string;
    }
}

void TicTacToe::printBoard()
{
    size_t row_width = 13;
    std::cout << "\033[2J";
    std::cout << "╔═══════════╗" << std::endl
              << "║ " << RED << player1_ << std::setw((row_width - player1_.size()) - 1) << "\033[0m"
              << "  ║" << std::endl
              << "╠═══╦═══╦═══╣" << std::endl
              << "║ " << field_color(board_[0][0]) << " ║ " << field_color(board_[0][1]) << " ║ "
              << field_color(board_[0][2]) << " ║" << std::endl
              << "╠═══╬═══╬═══╣" << std::endl
              << "║ " << field_color(board_[1][0]) << " ║ " << field_color(board_[1][1]) << " ║ "
              << field_color(board_[1][2]) << " ║" << std::endl
              << "╠═══╬═══╬═══╣" << std::endl
              << "║ " << field_color(board_[2][0]) << " ║ " << field_color(board_[2][1]) << " ║ "
              << field_color(board_[2][2]) << " ║" << std::endl
              << "╠═══╩═══╩═══╣" << std::endl
              << "║ " << BLUE << player2_ << std::setw((row_width - player2_.size()) - 1) << "\033[0m"
              << "  ║" << std::endl
              << "╚═══════════╝" << std::endl;
}

bool TicTacToe::checkTie(char symbole)
{
    for (int i = 0; i < 3; i++)
    {
        // check rows
        if (board_[i][0] == symbole && board_[i][1] == symbole && board_[i][2] == symbole)
        {
            return true;
        }
        // check columns
        if (board_[0][i] == symbole && board_[1][i] == symbole && board_[2][i] == symbole)
        {
            return true;
        }
    }
    // check diagonals
    if (board_[0][0] == symbole && board_[1][1] == symbole && board_[2][2] == symbole)
    {
        return true;
    }
    if (board_[0][2] == symbole && board_[1][1] == symbole && board_[2][0] == symbole)
    {
        return true;
    }
    return false;
}

void TicTacToe::makeMove()
{
    int  input;
    bool bIsPlaying      = false;
    char current_symbole = (currentPlayer_ == player1_) ? symbole_player_one : symbole_player_two;
    while (!bIsPlaying)
    {
        std::cout << currentPlayer_ << " ist dran: ";
        std::cin >> input;
        if (input > 9 || input < 0 || !isdigit(board_[BoardIndex[input - 1].row][BoardIndex[input - 1].col]))
        {
            std::cout << "Falscher Wert als board, nochmal versuchen" << std::endl;
        }
        else
        {
            this->currentField = input;
            bIsPlaying         = true;
            --input;
            plays++;

            // Change the number to a symbole
            board_[BoardIndex[input].row][BoardIndex[input].col] = current_symbole;
        }
    }
    switchPlayer();
    bIsPlaying = false;
}

void TicTacToe::switchPlayer()
{
    this->currentPlayer_ = (currentPlayer_ == player1_) ? player2_ : player1_;
}