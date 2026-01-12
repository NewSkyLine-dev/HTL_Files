#include <stdexcept>
#include <iostream>

#include "rekursionen.h"

using namespace std;

int Rekursionen::summe(int n)
{
    if (n == 0 || n == 1)
        return n;
    else if (n > 1)
        return n + summe(n - 1);
    else
        return n + summe(n + 1);
}

void Rekursionen::print_array_backwards(int array[], int length)
{
    if (length == 0)
        return;
    std::cout << array[length - 1] << " ";
    print_array_backwards(array, length - 1);
}

void Rekursionen::print_array(int array[], int length)
{
    // Stop recursion, when reached the end of array
    if (length <= 0)
        return;

    // Recursive iteration
    print_array(array, length - 1);

    // Print out the last element of array
    std::cout << array[length - 1] << " ";
}

bool Rekursionen::is_palindrom(std::string s)
{
    // Remove spaces and punctuation and convert to lowercase
    std::string cleaned;
    for (char c : s)
    {
        if (std::isalnum(c))
            cleaned += std::tolower(c);
    }

    // Base case: if the length of the cleaned string is 0 or 1, it is a palindrome
    if (cleaned.size() <= 1)
        return true;

    // Check if first and last characters are the same
    if (cleaned[0] != cleaned[cleaned.size() - 1])
        return false;

    // Recursive case: check if the substring excluding the first and last characters is a palindrome
    return is_palindrom(cleaned.substr(1, cleaned.size() - 2));
}

// Relies on an ordered array
bool Rekursionen::binary_search(int array[], int start, int end, int value)
{
    if (start <= end)
    {
        // Get the middle if the array
        int mid = start + (end - start) / 2;

        // Check if the middle of the array, and the value is the same
        if (array[mid] == value)
            return true;

        // Check if the middle of the array is bigger then the value, if yes, split the other part in half as well
        else if (array[mid] > value)
            return binary_search(array, start, mid - 1, value);

        // If all these cases are not valid, make a recursive search
        else
            return binary_search(array, mid + 1, end, value);
    }
    return false;
}
