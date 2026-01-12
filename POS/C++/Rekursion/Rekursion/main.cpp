#include <iostream>
#include <string>

#include "rekursionen.h"

using namespace std;

int main()
{
    std::string a;
    std::cout << "Hello World!\n";
    std::cin >> a;

    std::cout << a;
}

// int main(int, char **)
// {
//     // Summe
//     try
//     {
//         // cout << Rekursionen::summe(15) << endl;
//         // cout << Rekursionen::summe(1) << endl;
//         // cout << Rekursionen::summe(0) << endl;
//         cout << Rekursionen::summe(-5) << endl;
//     }
//     catch (const std::exception &e)
//     {
//         std::cerr << e.what() << '\n';
//     }

//     int arr[] = {1, 2, 3, 4, 5, 6, 7, 8, 9};
//     // Array rückwärts
//     // Rekursionen::print_array_backwards(arr, 9);
//     // cout << endl;

//     // Palindrome (true, false, true, true)
//     // string palindromes[] = {"hannah", "alba", "reittier", ""};
//     // int count = 4;

//     // Palindrome Erweiterung 1 (alle Beispiele sind Palindrome)
//     // string palindromes[] = {"Hannah", "Reliefpfeiler", "Reittier"};
//     // int count = 3;

//     // Palindrome Erweiterung 2 (alle Beispiele sind Palindrome)
//     // string palindromes[] = {"Erika feuert nur untreue Fakire", "Never odd or even"};
//     // int count = 2;

//     // Palindrome Erweiterung 3 (alle Beispiele sind Palindrome)
//     // string palindromes[] = {"Was it a car or a cat I saw?", "Madam, I’m Adam.", "Eine güldne, gute Tugend: Lüge nie!", "Die Liebe ist Sieger; stets rege ist sie bei Leid."};
//     // int count = 4;

//     for (int i = 0; i < count; i++)
//     {
//         try
//         {
//             if (Rekursionen::is_palindrom(palindromes[i]))
//             {
//                 cout << palindromes[i] << " ist ein Palindrom" << endl;
//             }
//             else
//             {
//                 cout << palindromes[i] << " ist kein Palindrom" << endl;
//             }
//         }
//         catch (const std::exception &e)
//         {
//             std::cerr << e.what() << '\n';
//         }
//     }

//     int search[] = {7, 12, 23, 24, 25, 36, 71, 99, 100};
//     // Binäre Suche
//     // cout << Rekursionen::binary_search(search, 0, 8, 7) << endl;   // suche 7
//     // cout << Rekursionen::binary_search(search, 0, 8, 100) << endl; // suche 100
//     // cout << Rekursionen::binary_search(search, 0, 8, -1) << endl;  // suche -1
//     // cout << Rekursionen::binary_search(search, 0, 8, 23) << endl;  // suche 23
//     // cout << Rekursionen::binary_search(search, 0, 8, 70) << endl;  // suche 70
//     // cout << Rekursionen::binary_search(search, 0, 8, 700) << endl; // suche 700

//     // Erweiterung
//     // Rekursionen::print_array(arr, 9);
//     // cout << endl;
// }
