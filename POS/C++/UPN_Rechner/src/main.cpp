#include <iostream>
#include "Calculator.h"

int main(int, char **argv)
{
    std::string line;
    std::cout << "Bitte gib die Rechnungen in der UPN-Schreibweise mit einem = am Ende ein" << std::endl;
    std::cout << "Eine Eingabe von 'Ende' beendet das Programm" << std::endl;
    while (true)
    {
        try
        {
            Calculator calc;
            std::string input = "";
            calcu(argv[1], calc);

            std::cout << "Rechnung: ";
            std::getline(std::cin, input);
            if (input == "Ende")
            {
                break;
            }
            double result = calc.calculate(input);
            std::cout << "Ergebnis: " << result << std::endl;
        }
        catch (const std::logic_error &e)
        {
            std::cerr << e.what() << '\n';
            return 1;
        }
    }
}