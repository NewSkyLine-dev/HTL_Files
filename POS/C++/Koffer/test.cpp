#include <iostream>
#include <cstdlib>
#include <ctime>

using namespace std;

class Packstueck
{
private:
    int player;
    string stueck;
    Packstueck *naechstes = nullptr;

public:
    Packstueck(int player, string stueck)
    {
        this->player = player;
        this->stueck = stueck;
    }

    ~Packstueck()
    {
        delete naechstes;
    }

    bool einpacken(int player)
    {
        if (player != this->player)
        {
            return false;
        }

        if (naechstes == nullptr)
        {
            naechstes = new Packstueck(player, "");
            return true;
        }

        return naechstes->einpacken(player);
    }

    bool hat_gewonnen()
    {
        return naechstes != nullptr && naechstes->hat_gewonnen();
    }

    void print()
    {
        if (naechstes != nullptr)
        {
            naechstes->print();
        }

        cout << stueck << endl;
    }
};

class Koffer

{
private:
    Packstueck *erstes = nullptr;

public:
    Koffer() {}

    ~Koffer()
    {
        delete erstes;
    }

    bool einpacken(int player, int anzahl)
    {
        if (erstes == nullptr)
        {
            erstes = new Packstueck(player, "");
        }

        bool erfolgreich = true;

        for (int i = 0; i < anzahl; i++)
        {
            cout << "Gib das naechste Packstueck ein: ";
            string stueck;
            cin >> stueck;

            if (!erstes->einpacken(player))
            {
                erfolgreich = false;
                cout << "Falscher Spieler! Runde beendet." << endl;
                break;
            }

            erstes->print();
        }

        return erfolgreich;
    }

    bool hat_gewonnen(int player, int anzahl)
    {
        return erstes != nullptr && erstes->hat_gewonnen();
    }

    void print()
    {
        if (erstes != nullptr)
        {
            erstes->print();
        }
    }
};

/*
int main() {
    srand(time(nullptr));

    cout << "\033[2J"; // Clear console

    int spieler = rand() % 2 + 1; // Zufaelliger Startspieler
    Koffer koffer;
    bool spielende = false;

    while (!spielende) {
        cout << "Spieler " << spieler << " ist an der Reihe." << endl;

        bool erfolgreich = koffer.einpacken(spieler, 1);

        if (erfolgreich) {
            cout << "Erfolgreich eingepackt!" << endl;

            koffer.print();

            if (koffer.hat_gewonnen(spieler, 1)) {
                cout << "Spieler " << spieler << " hat gewonnen!" << endl;
                spielende = true;
                break;
            }

            cout << "Druecke Enter fuer den naechsten Spieler..." << endl;
            cin.ignore();
            cin.get();
            spieler = spieler == 1 ? 2 : 1; // Wechselt den Spieler
        } else {
            cout << "Falsches Packstueck! Runde beendet." << endl;
            cout << "Druecke Enter fuer den naechsten Spieler..." <<
*/