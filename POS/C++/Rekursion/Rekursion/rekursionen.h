#ifndef REKURSIONEN_H
#define REKURSIONEN_H

#include <string>

class Rekursionen
{
public:
    static int summe(int n);
    static void print_array_backwards(int array[], int length);
    static void print_array(int array[], int length); // Erweiterung
    static bool is_palindrom(std::string s);
    static bool binary_search(int array[], int start, int end, int value);
};

#endif