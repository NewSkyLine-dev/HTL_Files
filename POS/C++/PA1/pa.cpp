#include "pa.h"
#include <iostream>

using namespace std;

int random_card_value()
{
    return rand() % 13 + 1;
}

void print_card(int card)
{
    switch (card) {
        case 1:
            std::cout << "A" << std::endl;
            break;
        case 11:
            std::cout << "J" << std::endl;
            break;
        case 12:
        	std::cout << "D" << std::endl;
            break;
        case 13:
            std::cout << "K" << std::endl;
            break;
        default:
            std::cout << card << std::endl;
            break;
    }
}

void print_card(Card &card)
{
    std::string symbole_number = card.symboles[card.a % 4];
    if (symbole_number == "♥" or symbole_number == "♦") {
        std::cout << "\033[34m" << symbole_number << " ";
        print_card(card.a);
    }
    else {
        std::cout << "\033[31m" << symbole_number << " ";
        print_card(card.a);
    }
    std::cout << "\033[0m";
}

Card::Card()
{
    a = random_card_value();
    std::string symbole_number = symboles[random_card_value() % 4];
    if (random_card_value() % 2 == 0)
    {
        std::cout << "\033[31m" << symbole_number << " ";
        print_card(a);
    }
    else
    {
        std::cout << "\033[34m" << symbole_number << " ";
        print_card(a);
    }
    std::cout << "\033[0m";
}

void most_valuable_card(Card card[], int number_cards)
{
    int index_2 = 1;
    int temp;
    for (int i = 0; i < number_cards; i++)
    {
        if (card[i].a < card[index_2].a) {
            temp = card[index_2].a;
        }
        else {
            index_2++;
        }
    }
    cout << temp << endl;
}