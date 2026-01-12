#include <iostream>
#include "Koffer.h"
#include <vector>
#include <limits>
#include <ctime>
#include <cstdlib>

using namespace std;

int main()
{
    srand(time(nullptr));

    Koffer k;
    int anzahl = 0;
    int current_player = 0;
    int limit = 5;

    cout << "Willkommen beim Kofferpacken-Spiel!" << endl;
    while (anzahl < 2 || anzahl > 4)
    {
        cout << "Wie viele Spiler spielent mit (2 - 4): ";
        cin >> anzahl;
        if (cin.fail())
        {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max());
            cout << "Ungültige Eingabe!" << endl;
        }
    }

    string namen[anzahl];

    for (int i = 0; i < anzahl; i++)
    {
        cout << "Gib den Namen des " << i + 1 << ". Spielers ein: ";
        cin >> namen[i];
    }

    current_player = rand() % anzahl;

    while (true)
    {
        cout << "Spieler " << namen[current_player] << " ist am Zug!" << endl;
        if (k.einpacken(current_player))
        {
            if (k.hat_gewonnen(current_player, limit))
            {
                cout << "Spieler " << namen[current_player] << " hat gewonnen!" << endl;
                break;
            }
            cout << "Aktuell befinden sich im Koffer: " << endl;
            k.print();
        }
        current_player = (current_player + 1) % anzahl;
        std::cout << "\033[2j";
    };
}