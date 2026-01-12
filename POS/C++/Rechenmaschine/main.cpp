#include <iostream>

using namespace std;

int main(int, char **)
{
    int sum = 0;
    int num = 0;
    bool finished = false;
    char calc_operator;
    cout << "Willkommen bei der besten Rechenmaschine!" << endl;
    while (!finished)
    {
        cout << "Ergebnis: " << sum << endl;
        cout << "Bitte geben Sie eine Zahl ein: ";
        cin >> num;
        cout << "Gib bitte eine Rechnoperation ein: ";
        cin >> calc_operator;
        switch (calc_operator)
        {
        case '+':
            sum += num;
            break;
        case '-':
            sum -= num;
            break;
        case '*':
            sum *= num;
            break;
        case '/':
            sum /= num;
            break;
        case '=':
            cout << "Endergebnis: " << sum << endl;
            finished = true;
            break;
        }
    }
}
