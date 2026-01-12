#ifndef CARDSTACK_H
#define CARDSTACK_H

#include <string>

#include "card.h"

enum Stacktype
{
    Target,
    Game,
    Free
};

class Cardstack
{
private:
    Card *first = nullptr;
    int x;
    int y;
    std::string name;
    Stacktype stack_type;
    // Erweiterung
    Card *last = get_last_card();

public:
    Cardstack(std::string name, int x, int y, Stacktype stack_type, int cards = 0, Card *card_array[] = nullptr);
    void print();
    bool is_empty() { return first == nullptr; }
    Card *remove_last_card();
    Card *get_last_card();
    bool can_place_card(Card *card);
    void add_card(Card *card);
    ~Cardstack();
};

#endif