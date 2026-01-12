#ifndef CARDPACK_H
#define CARDPACK_H

#include "card.h"

class Cardpack
{
private:
    Card *pack[52];
    int size = 52;

public:
    Cardpack();
    void get_random_cards(Card *array[], int count);
    // Erweiterung
    ~Cardpack();
};

#endif