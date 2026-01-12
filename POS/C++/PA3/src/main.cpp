#include <iostream>
#include "letters.h"

int main(int, char **)
{
    /*In der main-Funktion sollen vom Benutzer so lange Wörter abgefragt werden, bis er „Ende" eingibt. Das Wort soll mit der Funktion aus Aufgabe 2 überprüft werden, ob es nur Buchstaben sind. Enthält das Wort ungültige Elemente wird eine Fehlermeldung ausgegeben und das Wort nicht weiterverarbeitet. Für gültige Wörter wird die Funktion count_letters der Buchstaben- Klasse aufgerufen.*/
    Letters letters;
    std::string word;
    while (true)
    {
        std::cout << "Enter a word: ";
        std::cin >> word;
        if (word == "Ende")
        {
            break;
        }
        if (Letters::only_letters(word))
        {
            letters.count_letters(word);
        }
        else
        {
            std::cout << "Invalid word!\n";
        }
    }
    letters.print();
    std::cout << "Sum of letters: " << letters.sum(0) << "\n";
    letters.print_recursive('a');
    return 0;
}
