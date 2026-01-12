

#include "text.h"

Text::Text(int number, std::string text)
{
    this->number = number;
    this->text = text;
}

Text::~Text()
{
}

void Text::add(std::string text)
{
    this->text += "\n" + text;
}

std::string Text::get()
{
    return text;
}