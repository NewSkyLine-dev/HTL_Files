#ifndef CALCULATOR_H
#define CALCULATOR_H

#include "Node.h"
#include <string>

void calcu(std::string arg_path, Calculator &calc);

class Calculator
{
private:
    void push(double d);
    double pop();

private:
    Node *first = nullptr;

public:
    Calculator();
    ~Calculator();
    double calculate(std::string input);
};

#endif // CALCULATOR_H