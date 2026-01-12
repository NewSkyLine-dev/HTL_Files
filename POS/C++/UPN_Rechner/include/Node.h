#ifndef NODE_H
#define NODE_H

class Calculator;

class Node
{
    friend Calculator;

public:
    explicit Node(double d);
    ~Node();

private:
    double data = 0.0;
    Node *next = nullptr;
};

#endif // NODE_H