#include <iostream>
#include <string>
#include "roman_number.h"

using namespace std;

int main()
{
    string input;
    while(true) {
        cout << "Enter a number (either roman or decimal): ";
        cin >> input;

        if (!cin.fail())
            break;

        cout << "Invalid input! Please try again: ";

        cin.clear();
        cin.ignore(numeric_limits<streamsize>::max(), '\n');
    }

    // Check if input is a number
    if (isdigit(input[0])) 
    {
        int number = stoi(input);
        Roman_Number roman_number(number, true);

        cout << roman_number.get_number() << endl;
    }
    // Is an string
    else 
    {
        Roman_Number roman_number(input);

        cout << roman_number.get_value() << endl;
    }
    return 0;
}
