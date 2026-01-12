#include "Node.h"

Node::Node(double d)
{
    this->next = nullptr;

    this->data = d;
}

Node::~Node()
{
    Node *next;
    next = this->next;
    if (next)
    {
        delete next;
    }
}