#ifndef TRAVEL_H
#define TRAVEL_H

class Travel
{
private:
    int target;
    int action;
    int condition;

public:
    Travel(int target, int action);
    ~Travel();
    void print(int menu);
    int get_target() { return target; }
    int get_condition() { return condition; }
    int get_action() { return action; }
};

#endif