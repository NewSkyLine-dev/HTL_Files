#ifndef TEXT_H
#define TEXT_H

#include <string>

class Text
{
private:
    int number;
    std::string text;

public:
    Text(int number, std::string text);
    ~Text();
    void add(std::string text);
    std::string get();
};

#endif