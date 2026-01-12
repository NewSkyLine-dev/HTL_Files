#include "tower.h"
#include <stdexcept>
#include <iomanip>
#include <iostream>

using namespace std;

Tower::Tower(std::string name, int max, bool filled)
{
    this->name = name;
    this->max = max;
    tower = new int[max];
    for (int i = max - 1, j = 1; i >= 0; i--, j++)
    {
        tower[i] = j * filled;
    }
    if (filled)
        current = max;
}

Tower::~Tower()
{
    delete tower;
}

void Tower::push(int scheibe)
{
    if (current == max)
        throw std::runtime_error("Der Stapel ist schon voll");

    if (current > 0 && tower[current - 1] < scheibe)
        throw std::runtime_error("Auf dem Stapel liegt eine kleiner Scheibe!");

    tower[current] = scheibe;
    current++;
}

int Tower::pop()
{
    if (current == 0)
        throw std::runtime_error("Der Stapel ist schon leer");

    current--;
    int res = tower[current];
    tower[current] = 0;
    return res;
}

int Tower::size()
{
    return current;
}

void Tower::print(int x, int y)
{
    for (int i = max - 1, j = 1; i >= 0; i--, j++)
    {
        if (tower[i] > 0)
        {
            if (tower[i] % 2 == 0)
            {
                cout << "\033[" << y + j << ";" << x + ((max + 1) / 2 - tower[i] / 2) + 1 << "H\033[35m";
                for (int k = 0; k < tower[i]; k++)
                {
                    cout << "█";
                }
            }
            else
            {
                cout << "\033[" << y + j << ";" << x + ((max + 1) / 2 - tower[i] / 2) << "H\033[34m";
                cout << "▐";
                for (int k = 0; k < tower[i] - 1; k++)
                {
                    cout << "█";
                }
                cout << "▌";
            }
            cout << tower[i];
        }
    }
    cout << "\033[" << y + max + 1 << ";" << x + max % 2 << "H\033[31m";
    for (int i = 0; i <= max; i++)
    {
        cout << "▀";
    }
    if (max % 2 == 0)
        cout << "▀";

    cout << "\033[" << y + max + 2 << ";" << x + (max + 1) / 2 - this->name.length() / 2 << "H\033[0m";
    cout << name;
}