#include <iostream>
#include <iomanip>

#include "cardstack.h"

Cardstack::Cardstack(std::string name, int x, int y, Stacktype stack_type, int cards, Card *card_array[])
{
    this->name = name;
    this->x = x;
    this->y = y;
    this->stack_type = stack_type;
    if (cards > 0)
    {
        first = *card_array;
        for (int i = 1; i < cards; i++)
        {
            first->add_card(card_array[i]);
        }
    }
}

void Cardstack::print()
{
    std::cout << "\033[" << y << ";" << x << "H" << name;
    if (first == nullptr)
    {
        std::cout << "\033[" << y + 1 << ";" << x << "H┌┄┄┄┐";
        std::cout << "\033[" << y + 2 << ";" << x << "H┆   ┆";
        std::cout << "\033[" << y + 3 << ";" << x << "H┆   ┆";
        std::cout << "\033[" << y + 4 << ";" << x << "H└┄┄┄┘";
    }
    else
    {
        if (stack_type == Target)
        {
            first->get_last_card()->print(x, y + 1);
        }
        else
        {
            first->print(x, y + 1);
        }
    }
}

Card *Cardstack::remove_last_card()
{
    if (!this->first || stack_type == Target)
    {
        return nullptr;
    }
    Card *ret = this->first->get_last_card();
    this->first = this->first->remove_last_card();
    return ret;
}

Card *Cardstack::get_last_card()
{
    if (this->first == nullptr)
    {
        return nullptr;
    }
    else
    {
        return this->first->get_last_card();
    }
}

bool Cardstack::can_place_card(Card *card) //? Does this work?
{
    if (!card)
    {
        return false;
    }
    if (first)
    {
        if (stack_type == Target &&
            card->get_suite() != first->get_last_card()->get_suite() &&
            card->get_value() < first->get_last_card()->get_value())
        {
            return true;
        }
        else
        {
            if (stack_type == Game)
            {
                if (card->get_value() > first->get_last_card()->get_value())
                {
                    return false;
                }

                if (first->get_suite() <= Club && card->get_suite() > Club)
                {
                    return false;
                }
            }
            return true;
        }
    }
    else
    {
        return !(stack_type == Target && card->get_value() != false);
    }
}

void Cardstack::add_card(Card *card)
{
    if (card)
    {
        if (first)
        {
            first->add_card(card);
        }
        else
        {
            first = card;
        }
    }
}

Cardstack::~Cardstack()
{
    delete[] last;
    delete[] first;
}