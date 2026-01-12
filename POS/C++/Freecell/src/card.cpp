#include <iostream>
#include <iomanip>

#include "card.h"

Card::Card(Suits suite, int value)
{
    this->suite = suite;
    this->value = value;
}

Card *Card::remove_last_card()
{
    if (!this->next)
        return nullptr;

    this->next = next->remove_last_card();
    return this;
}

Card *Card::get_last_card()
{
    if (this->next)
    {
        // Erweiterung
        prev = this; // Saves the previous card

        return this->next->get_last_card();
    }
    else
        return this;
}

void Card::add_card(Card *card)
{
    if (this->next)
        next->add_card(card);
    else
        this->next = card;
}

void Card::print(int x, int y)
{
    std::cout << "\033[" << y << ";" << x << "H┌───┐";
    std::cout << "\033[" << y + 1 << ";" << x << "H│";
    switch (suite)
    {
    case Club:
        std::cout << "\033[34m♣ ";
        break;
    case Spade:
        std::cout << "\033[34m♠ ";
        break;
    case Heart:
        std::cout << "\033[31m♥ ";
        break;
    case Diamond:
        std::cout << "\033[31m♦ ";
        break;
    }
    if (value > 1 && value < 10)
    {
        std::cout << value;
    }
    else
    {
        switch (value)
        {
        case 1:
            std::cout << "A";
            break;
        case 10:
            std::cout << "T";
            break;
        case 11:
            std::cout << "J";
            break;
        case 12:
            std::cout << "Q";
            break;
        case 13:
            std::cout << "K";
            break;
        }
    }
    std::cout << "\033[0m│";
    if (next == nullptr)
    {
        std::cout << "\033[" << y + 2 << ";" << x << "H│";
        switch (suite)
        {
        case Club:
        case Spade:
            std::cout << "\033[34m";
            break;
        case Heart:
        case Diamond:
            std::cout << "\033[31m";
            break;
        }
        if (value > 1 && value < 10)
        {
            std::cout << value;
        }
        else
        {
            switch (value)
            {
            case 1:
                std::cout << "A";
                break;
            case 10:
                std::cout << "T";
                break;
            case 11:
                std::cout << "J";
                break;
            case 12:
                std::cout << "Q";
                break;
            case 13:
                std::cout << "K";
                break;
            }
        }
        switch (suite)
        {
        case Club:
            std::cout << " ♣";
            break;
        case Spade:
            std::cout << " ♠";
            break;
        case Heart:
            std::cout << " ♥";
            break;
        case Diamond:
            std::cout << " ♦";
            break;
        }
        std::cout << "\033[0m│";
        std::cout << "\033[" << y + 3 << ";" << x << "H└───┘";
    }
    else
    {
        next->print(x, y + 2);
    }
}

// Erweiterung
Card::~Card()
{
    delete[] next;
    delete[] prev;
}