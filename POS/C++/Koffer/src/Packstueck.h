#ifndef PACKSTUECK_H
#define PACKSTUECK_H

#include <string>

class Packstueck
{
private:
    int player;
    std::string stueck;
    Packstueck *naechstes = nullptr;

public:
    Packstueck(int player, std::string stueck);
    ~Packstueck();
    bool einpacken(int player);
    bool hat_gewonnen(int player, int anzahl);
    void print();
};

#endif // PACKSTUECK_H