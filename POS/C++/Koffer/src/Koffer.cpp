#include "Koffer.h"
#include <iostream>

using namespace std;

Koffer::Koffer() {}

Koffer::~Koffer()
{
    delete erstes;
}

bool Koffer::einpacken(int player)
{
    // Create a new node and set it to the first node
    if (erstes == nullptr)
    {
        cout << "Gib das nächste Packstück ein: ";
        string stueck;
        cin >> stueck;
        erstes = new Packstueck(player, stueck);
        return true;
    }
    else
        return erstes->einpacken(player);
    return false;
}

bool Koffer::hat_gewonnen(int player, int anzahl)
{
    if (erstes == nullptr)
        return false;
    return erstes->hat_gewonnen(player, anzahl);
}

void Koffer::print()
{
    // if the first element is not a nullptr, call the print function on the first element
    if (erstes != nullptr)
        erstes->print();
}