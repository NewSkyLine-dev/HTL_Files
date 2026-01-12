#include <stdexcept>

#include "object.h"

using namespace std;

Object::Object(int number, std::string text)
{
    this->number = number;
    this->text = text;
}

Object::~Object()
{
}

void Object::add_message(int msg_number, std::string msg)
{
    if (msg_number >= 10 || msg_number < 0)
    {
        throw invalid_argument("Invalid Message-Number " + to_string(msg_number));
    }
    if (messages[msg_number] == "")
    {
        messages[msg_number] = msg;
    }
    else
    {
        messages[msg_number] += "\n" + msg;
    }
}

std::string Object::get()
{
    return messages[state];
}

void Object::set_movable(bool movable)
{
    this->movable = movable;
}