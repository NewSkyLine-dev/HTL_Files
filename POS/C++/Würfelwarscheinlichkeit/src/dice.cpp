#include "dice.h"
#include <cstdlib>
#include <iostream>

using namespace std;

int roll_dice(int sides)
{
    return rand() % sides + 1;
}

int roll_2_dice(int sides1, int sides2)
{
    int result1 = (rand() % sides1) + 1;
    int result2 = (rand() % sides2) + 1;
    return result1 + result2;
}

void print(int result, int count)
{
    cout << result << ": ";
    for (int i = 0; i < count / 100; i++)
    {
        cout << "#";
    }
    for (int i = 0; i < (count % 100) / 10; i++)
    {
        cout << "-";
    }
    for (int i = 0; i < count % 10; i++)
    {
        cout << ".";
    }
    cout << endl;
}