#ifndef OBJECT_H
#define OBJECT_H

#include <string>

class Object
{
private:
    int number;
    std::string text;
    std::string messages[10];
    bool movable = true;
    int state = 0;

public:
    Object(int number, std::string text);
    ~Object();
    void add_message(int msg_number, std::string msg);
    std::string get();
    void set_movable(bool movable);
    bool is_movable() { return movable; }
    std::string get_name() { return text; }
    int get_number() { return number; }
    int get_state() { return state; }
};

#endif