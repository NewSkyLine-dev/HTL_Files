#ifndef KOFFER_H
#define KOFFER_H

#include "Packstueck.h"

class Koffer
{
private:
    Packstueck *erstes = nullptr;

public:
    Koffer();
    ~Koffer();
    bool einpacken(int player);
    bool hat_gewonnen(int player, int anzahl);
    void print();
};

#endif // KOFFER_H