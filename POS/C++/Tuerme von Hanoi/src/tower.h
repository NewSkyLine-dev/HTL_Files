#ifndef TOWER_H
#define TOWER_H

#include <string>

class Tower
{
public:
    Tower(std::string name, int max, bool filled = false);
    ~Tower();
    void push(int scheibe);
    int pop();
    int size();
    void print(int x, int y);

private:
    int *tower;
    int current = 0;
    int max;
    std::string name;
};

#endif