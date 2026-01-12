#ifndef LETTERS_H
#define LETTERS_H

#include <string>

class Letters
{
public:
    static bool only_letters(std::string word);
    void count_letters(std::string word);
    void print();
    int sum(int index);
    void print_recursive(char letter);

private:
    int letters[26] = {0};
};

#endif // LETTERS_H