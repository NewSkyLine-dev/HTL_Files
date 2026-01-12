#include <iostream>
#include "pa.h"

int main()
{
    while (true)
    {
        try
        {
            bool useBegin = false;
            std::string input;
            std::string name;
            std::cout << "Soll auch nach Namen gesucht werden die mit der Eingabe starten (j / n): ";
            std::cin >> input;
            if (input == "j")
                useBegin = true;
            else
                useBegin = false;
            std::cout << std::endl
                      << "Nach welchem Namen soll gesucht werden: ";
            std::cin >> name;
            std::cout << std::endl;
            Names names(useBegin);
            names.printNamesSorted(name);
        }
        catch (const std::string &e)
        {
            break;
        }
    }
}