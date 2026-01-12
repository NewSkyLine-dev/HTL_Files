#include <iostream>
#include <ctime>
#include "dice.h"

using namespace std;

int main()
{
    srand(time(NULL));
    int choice;
    std::cout << "Wollen Sie einen oder zwei Würfel werfen? (1/2): ";
    std::cin >> choice;

    if (choice == 1)
    {
        int num_rolls = 0;
        int sides = 0;
        std::cout << "Wie oft Würfeln: ";
        std::cin >> num_rolls;
        std::cout << "Wie viele Seiten hat der Würfel: ";
        std::cin >> sides;

        int results[sides] = {0};
        for (int i = 0; i < num_rolls; i++)
        {
            int result = roll_dice(sides);
            results[result - 1]++;
        }

        for (int i = 0; i < sides; i++)
        {
            std::cout << i + 1 << ": ";
            print(i + 1, results[i]);
        }
    }
    else if (choice == 2)
    {
        int num_rolls = 0;
        int sides1 = 0, sides2 = 0;
        std::cout << "Wie oft Würfeln: ";
        std::cin >> num_rolls;
        std::cout << "Wie viele Seiten hat der erste Würfel: ";
        std::cin >> sides1;
        std::cout << "Wie viele Seiten hat der zweite Würfel: ";
        std::cin >> sides2;

        int results[11] = {0};
        for (int i = 0; i < num_rolls; i++)
        {
            int result = roll_2_dice(sides1, sides2);
            results[result - 2]++;
        }

        for (int i = 2; i <= 12; i++)
        {
            print(i, results[i - 2]);
        }
    }
    else
    {
        std::cout << "Ungültige Wahl. Bitte wählen Sie entweder 1 oder 2." << std::endl;
    }

    return 0;
}