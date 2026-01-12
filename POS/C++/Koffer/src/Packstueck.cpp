#include "Packstueck.h"
#include <iostream>

using namespace std;

Packstueck::Packstueck(int player, string stueck)
{
    this->player = player;
    this->stueck = stueck;
}

Packstueck::~Packstueck()
{
    delete naechstes;
}

bool Packstueck::einpacken(int player)
{
    cout << "Was wurde bisher eingepackt: ";
    string p;
    cin >> p;
    if (p != this->stueck)
        return false;

    // If the next item is empty, create a new node and return true
    if (naechstes == nullptr)
    {
        cout << "Gib das nächste Packstück ein: ";
        string stueck;
        cin >> stueck;
        naechstes = new Packstueck(player, stueck);
        return true;
    }
    else
        return naechstes->einpacken(player);
}

bool Packstueck::hat_gewonnen(int player, int anzahl)
{
    if (player == this->player)
        anzahl--;
    if (anzahl == 0)
        return true;
    else if (naechstes == nullptr)
        return false;
    return naechstes->hat_gewonnen(player, anzahl);
}

void Packstueck::print()
{
    // print the value of stueck
    cout << "\033[3" << player + 1 << "m" << stueck << "\033[0m" << endl;

    // if the next pointer is not null, call the print function on the next pointer
    if (naechstes != nullptr)
        naechstes->print();
}