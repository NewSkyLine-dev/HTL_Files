#include "Calculator.h"
#include <iostream>
#include <sstream>
#include <stdexcept>
#include <string>
#include <fstream>

Calculator::Calculator() { this->first = nullptr; }

Calculator::~Calculator()
{
    Node *first;
    first = this->first;
    if (this->first)
    {
        this->first->~Node();
        delete first;
    }
}

// Es soll eine Textdatei eingelesen werden, die in jeder Zeile eine Rechnung hat und jede Rechnung ausgegeben werden. Der Pfad zur Datei soll als Befehlszeilenargument angegeben werden.
void calcu(std::string arg_path, Calculator &calc)
{
    std::ifstream file(arg_path);
    if (file.is_open())
    {
        std::string line;
        while (std::getline(file, line))
        {
            calc.calculate(line);
            // std::cout << line << std::endl;
        }
        file.close();
    }
    else
    {
        std::cerr << "Error: could not open file \"" << arg_path << "\"" << std::endl;
    }
}
double Calculator::calculate(std::string input)
{
    auto size = [&]()
    {
        size_t size = 0;
        for (Node *current = first; current != nullptr; current = current->next)
        {
            size++;
        }
        return size;
    };
    std::string token;
    auto printStack = [&]()
    {
        std::cout << "Stack: ";
        for (Node *current = first; current != nullptr; current = current->next)
        {
            std::cout << current->data << " ";
        }
        std::cout << std::endl;
    };

    while (!input.empty())
    {
        printStack();
        size_t pos = input.find(' ');
        if (pos != std::string::npos)
        {
            token = input.substr(0, pos);
            input.erase(0, pos + 1);
        }
        else
        {
            token = input;
            input.clear();
        }
        if (token == "+")
        {
            if (size() < 2)
            {
                throw std::logic_error("Not enough operands for addition");
            }
            double b = pop();
            double a = pop();
            double result = a + b;
            push(result);
            std::cout << a << " + " << b << " = " << result << std::endl;
        }
        else if (token == "-")
        {
            if (size() < 2)
            {
                throw std::logic_error("Not enough operands for subtraction");
            }
            double b = pop();
            double a = pop();
            double result = a - b;
            push(result);
            std::cout << a << " - " << b << " = " << result << std::endl;
        }
        else if (token == "*")
        {
            if (size() < 2)
            {
                throw std::logic_error("Not enough operands for multiplication");
            }
            double b = pop();
            double a = pop();
            double result = a * b;
            push(result);
            std::cout << a << " * " << b << " = " << result << std::endl;
        }
        else if (token == "/")
        {
            if (size() < 2)
            {
                throw std::logic_error("Not enough operands for division");
            }
            double b = pop();
            double a = pop();
            double result = a / b;
            push(result);
            std::cout << a << " / " << b << " = " << result << std::endl;
        }
        else if (token == "=")
        {
            if (size() != 1)
            {
                throw std::logic_error("Invalid notation: There should be one operand on the stack");
            }
            double result = pop();
            return result;
        }
        else
        {
            double d = std::stod(token);
            push(d);
        }
    }
    throw std::logic_error("Invalid notation: = symbol not found at the end");
}

void Calculator::push(double d)
{
    Node *current = first;
    Node *n = new Node(d);
    if (!current)
    {
        first = n;
        return;
    }

    while (current->next)
    {
        current = current->next;
    }

    current->next = n;
}

double Calculator::pop()
{
    Node *current = first;
    Node *previous = nullptr;
    if (!current)
    {
        return 0.0;
    }

    while (current->next)
    {
        previous = current;
        current = current->next;
    }

    double d = current->data;
    if (previous)
    {
        previous->next = nullptr;
    }
    else
    {
        first = nullptr;
    }
    delete current;

    return d;
}