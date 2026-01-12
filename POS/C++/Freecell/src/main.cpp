#include <iostream>
#include <cstdlib>
#include <ctime>
#include <limits>

#include "cardstack.h"
#include "cardpack.h"

int main(int, char **)
{
    while (true)
    {
        Cardpack pack;
        Cardstack *places[16];
        Card *cards[7];
        places[0] = new Cardstack("  1  ", 50, 1, Target);
        places[1] = new Cardstack("  2  ", 44, 1, Target);
        places[2] = new Cardstack("  3  ", 38, 1, Target);
        places[3] = new Cardstack("  4  ", 32, 1, Target);
        places[4] = new Cardstack("  5  ", 19, 1, Free);
        places[5] = new Cardstack("  6  ", 13, 1, Free);
        places[6] = new Cardstack("  7  ", 7, 1, Free);
        places[7] = new Cardstack("  8  ", 1, 1, Free);
        pack.get_random_cards(cards, 7);
        places[8] = new Cardstack("  9  ", 1, 6, Game, 7, cards);
        pack.get_random_cards(cards, 7);
        places[9] = new Cardstack("  10 ", 8, 6, Game, 7, cards);
        pack.get_random_cards(cards, 7);
        places[10] = new Cardstack("  11 ", 15, 6, Game, 7, cards);
        pack.get_random_cards(cards, 7);
        places[11] = new Cardstack("  12 ", 22, 6, Game, 7, cards);
        pack.get_random_cards(cards, 6);
        places[12] = new Cardstack("  13 ", 29, 6, Game, 6, cards);
        pack.get_random_cards(cards, 6);
        places[13] = new Cardstack("  14 ", 36, 6, Game, 6, cards);
        pack.get_random_cards(cards, 6);
        places[14] = new Cardstack("  15 ", 43, 6, Game, 6, cards);
        pack.get_random_cards(cards, 6);
        places[15] = new Cardstack("  16 ", 50, 6, Game, 6, cards);

        bool finished = false;
        while (!finished)
        {
            std::cout << "\033[2J";
            for (int i = 0; i < 16; i++)
            {
                places[i]->print();
            }
            int source = 0;
            int target = 0;
            std::cout << "\033[1;56HVon Stapel eingeben: ";
            std::cin >> source;
            if (std::cin.fail())
            {
                std::cin.clear();
                std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                std::cout << "\033[4;56HUngültige Eingabe.\033[5;56HTaste drücken um fortzusetzen.";
                std::cin.get();
                std::cin.get();
                continue;
            }
            std::cout << "\033[2;56HZiel Stapel eingeben: ";
            std::cin >> target;
            if (std::cin.fail())
            {
                std::cin.clear();
                std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                std::cout << "\033[4;56HUngültige Eingabe.\033[5;56HTaste drücken um fortzusetzen.";
                std::cin.get();
                std::cin.get();
                continue;
            }
            std::cout << "\033[3;56H" << source << " -> " << target;

            if (source > 0 && source < 17 && target > 0 && target < 17)
            {
                Card *c = places[source - 1]->get_last_card();
                if (c)
                {
                    if (places[target - 1]->can_place_card(c))
                    {
                        Card *c = places[source - 1]->remove_last_card();
                        places[target - 1]->add_card(c);
                    }
                    else
                    {
                        std::cout << "\033[4;56HAuf diesen Stapel kann diese Karte nicht gelegt werden.\033[5;56HTaste drücken um fortzusetzen.";
                        std::cin.get();
                        std::cin.get();
                        continue;
                    }
                }
                else
                {
                    std::cout << "\033[4;56HVon dem Stapel kann keine Karte genommen werden.\033[5;56HTaste drücken um fortzusetzen.";
                    std::cin.get();
                    std::cin.get();
                    continue;
                }
            }
            else
            {
                std::cout << "\033[4;56HUngültige Eingabe.\033[5;56HTaste drücken um fortzusetzen.";
                std::cin.get();
                std::cin.get();
            }

            finished = true;
            for (int i = 4; i < 16; i++)
            {
                if (!places[i]->is_empty())
                {
                    finished = false;
                    break;
                }
            }
        }
        for (int i = 0; i < 16; i++)
        {
            places[i]->print();
        }
        std::cout << "\033[11;1HGratulation! Du hast gewonnen!." << std::endl;
        std::cout << "\033[12;1HNochmal spielen? (j/n): ";
        char c;
        std::cin >> c;
        if (c == 'j')
            pack.~Cardpack();

        else
            break;
    }
}
