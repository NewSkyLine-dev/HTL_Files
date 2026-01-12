#include "letters.h"
#include <iostream>

bool Letters::only_letters(std::string word)
{
    if (word.length() == 0)
    {
        return true;
    }
    else
    {
        if (isalpha(word[0]))
        {
            return only_letters(word.substr(1));
        }
        else
        {
            return false;
        }
    }
}

void Letters::count_letters(std::string word)
{
    if (word.length() == 0)
    {
        return;
    }
    else
    {
        if (isalpha(word[0]))
        {
            letters[tolower(word[0]) - 'a']++;
        }
        count_letters(word.substr(1));
    }
}

void Letters::print()
{
    for (int i = 0; i < 26; i++)
    {
        std::cout << (char)('a' + i) << ": " << letters[i] << "\n";
    }
}

int Letters::sum(int index)
{
    if (index == 26)
    {
        return 0;
    }
    else
    {
        return letters[index] + sum(index + 1);
    }
}

void Letters::print_recursive(char letter)
{
    if (letter == 'z' + 1)
    {
        return;
    }
    else
    {
        std::cout << letter << ": " << letters[letter - 'a'] << "\n";
        print_recursive(letter + 1);
    }
}