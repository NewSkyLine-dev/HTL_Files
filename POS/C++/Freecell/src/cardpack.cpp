#include <cstdlib>
#include <ctime>

#include "cardpack.h"

Cardpack::Cardpack()
{
    srand(time(nullptr));
    for (int i = 0; i < 52; i++)
    {
        pack[i] = new Card(static_cast<Suits>(i / 13), i % 13 + 1);
    }
}

Cardpack::~Cardpack()
{
    for (int i = 0; i < 52; i++)
    {
        delete pack[i];
    }
}

void Cardpack::get_random_cards(Card *array[], int count)
{
    for (int i = 0; i < count; i++)
    {
        if (size > 0)
        {
            int c = rand() % size;
            array[i] = pack[c];
            pack[c] = pack[size - 1];
            pack[size - 1] = nullptr;
            size--;
        }
        else
        {
            array[i] = nullptr;
        }
    }
}
