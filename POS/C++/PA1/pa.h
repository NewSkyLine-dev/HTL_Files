#pragma once
#include <string>

struct Card
{
    std::string symboles[4] = {"♥", "♦", "♠", "♣"};
    int a = 0;
    int b = 2;
    Card();
};

int random_card_value();
void print_card(int card);
void print_card(Card &card);
void most_valuable_card(Card card[], int number_cards);